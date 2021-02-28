using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Events;

#region InputEvent Class

/// <summary>
///     Class to store Unity Event data that accepts a float as an input parameter.
///     Can be used to call functions based on current input value.
/// </summary>
// Uses Serializable tag to make class accessable in the inspector.
[Serializable]
public class InputEvent : UnityEvent<float> { }

#endregion

#region Axis Class

/// <summary>
///     Custom class to store information about axial key-bindings.
///     <para>
///         Inputs have positive and negative values to determine the direction of input.
///         Negative inputs can be ignored to create single button inputs.
///     </para>
/// </summary>
[Serializable]
public class InputAxis
{

    /// <summary> 
    ///     A given name for the current input axis. 
    ///     Mainly for presentation purposes. 
    /// </summary>
    public string name;

    // Using header to create section seperation in the Unity inspector.
    [Header("Key Options")]

    // ===== Variables handling positive key. =====

    /// <summary> Internal storage for the string name of the positive input KeyCode. </summary>
    [SerializeField]
    private string _positiveKey = "";

    /// <summary>
    ///     Public getter and Setter interface for _positiveKey.
    ///     Converts input string values to usable KeyCode types.
    /// </summary>
    public string positiveKey
    {
        get { return _positiveKey; }
        set
        {
            positiveKeyCode = TryKeyCodeConversion(value);
            _positiveKey = value;
        }
    }

    // Hiding the variable from being seen in the inspector.
    // Maintains public accessbility modifier.
    /// <summary>
    ///     Storage for the KeyCode conversion of the positive key string.
    ///     Can be used to interface with the Unity input system.
    /// </summary>
    //[HideInInspector]
    public KeyCode positiveKeyCode { get; private set; }

    private bool usePositive = true;

    // ===== Variables handling negative key. =====

    /// <summary> Internal storage for the string name of the negative input KeyCode. </summary>
    [SerializeField]
    private string _negativeKey = "";

    /// <summary>
    ///     Public getter and Setter interface for _negativeKey.
    ///     Converts input string values to usable KeyCode types.
    /// </summary>
    public string negativeKey
    {
        get { return _negativeKey; }
        set
        {
            negativeKeyCode = TryKeyCodeConversion(value);
            _negativeKey = value;
        }
    }

    /// <summary>
    ///     Storage for the KeyCode conversion of the negative key string.
    ///     Can be used to interface with the Unity input system.
    /// </summary>
    //[HideInInspector]
    public KeyCode negativeKeyCode { get; private set; }

    private bool useNegative = true;

    // ===== Variables handling input smoothing. =====

    /// <summary>
    ///     Determines the amount of smoothing for lerping between directional input values.
    ///     Set to 1 to snap.
    /// </summary>
    // Using range to limit size of step between 0 and 1.
    // Also creates handy slider!
    [Range(0, 1)]
    public float stepSize = 0f;

    /// <summary>
    ///     Stores the current float value of the input.
    ///     Used to apply smoothing between axis directions.
    /// </summary>
    private float currentInputValue = 0f;
    private float rawInputValue = 0f;

    // ===== Event Storage. =====

    [Header("Linked Events")]
    /// <summary>
    ///     UnityEvent interface allowing for passing of input float data.
    ///     Link to functions that require the use of inputs.
    /// </summary>
    public InputEvent inputEvent;

    private delegate float InputGenerator(float input);
    private InputGenerator inputGenerator;

    // ===== Functions. =====

    /// <summary>
    ///     Attempts to convert a given string value to a KeyCode.
    ///     Returns a warning if unsuccessful, stating the warning source.
    /// </summary>
    /// <param name="keyCodeString">
    ///     Value to be converted into KeyCode.
    /// </param>
    /// <returns>
    ///     KeyCode conversion of input keyCodeString.
    ///     Value is <c>KeyCode.None</c> if conversion is unsuccessful.
    /// </returns>
    private KeyCode TryKeyCodeConversion(string keyCodeString)
    {
        // Returning the result of parsing from strign to KeyCode if successful.
        if (KeyFormatter.TryParse(keyCodeString, out KeyCode result))
            return result;
        // Throwing a warning to the Unity console when parsing fails.
        else
            Debug.LogWarning(
                $"Warning in {name} input settings:\n" +
                $"Could not convert '{keyCodeString}' to KeyCode. Defaulting to None."
            );
        // If end of function reached, parse has failed.
        // Return an empty KeyCode.
        return KeyCode.None;
    }

    /// <summary>
    ///     Performs conversion of each stored string key in the current instance to KeyCode.
    ///     Used for in-editor changes.
    /// </summary>
    public void UpdateSettings()
    {
        positiveKey = _positiveKey;
        usePositive = positiveKeyCode != KeyCode.None;

        negativeKey = _negativeKey;
        useNegative = negativeKeyCode != KeyCode.None;

        inputGenerator = 
            stepSize == 1 ? 
                (InputGenerator)GetSnappedInput
                : (InputGenerator)GetLerpedInput;
    }

    public bool TryGetInput(out float input)
    {
        input = 
            (usePositive && Input.GetKey(positiveKeyCode)) ? 1 
            : (useNegative && Input.GetKey(negativeKeyCode)) ? -1 : 0;

        return input != 0;
    }

    public void CheckInput()
    {
        if (TryGetInput(out rawInputValue) || currentInputValue != 0)
            inputEvent?.Invoke(inputGenerator(rawInputValue));
    }

    private float GetLerpedInput(float input)
    {
        float inputLerp = Mathf.Lerp(
            currentInputValue,
            input,
            stepSize
        );

        if (Mathf.Abs(inputLerp) < 0.1f)
            currentInputValue = 0;
        else
            currentInputValue = inputLerp;

        return currentInputValue;
    }

    private float GetSnappedInput(float input)
    {
        currentInputValue = input;
        return currentInputValue;
    }
}

#endregion

#region KeyFormatter Class

/// <summary>
///     Static struct for handling conversion between strings and KeyCodes.
/// </summary>
struct KeyFormatter
{
    /// <summary>
    ///     Regex object used in formatting strings to KeyCode format.
    /// </summary>
    // Identifies the first letter of a word OR entire word excluding the first letter.
    // Uses custom grouping to identify each match group by identifier.
    private static Regex keyCodeFormatter = new Regex(@"(?<Letter>\b(?=\w).)|(?<Word>\w+)");

    // ===== Functions. =====

    /// <summary>
    ///     Formats a given string to be usable in a string>KeyCode parse operation.
    /// </summary>
    /// <param name="keyCodeString">
    ///     Value to be formatted into KeyCode.
    /// </param>
    /// <returns>
    ///     Formatted KeyCode string.
    /// </returns>
    private static string FormatKeyCodeString(string keyCodeString)
    {
        // Checking for Regex format matches in the given string.
        if (keyCodeFormatter.IsMatch(keyCodeString))
        {
            // Stores all found matches from the Regex match operation.
            MatchCollection matches = keyCodeFormatter.Matches(keyCodeString);

            // Formatting the output string based on matched Regex group.
            string output = "";
            for (int i = 0; i < matches.Count; i++)
            {
                if (matches[i].Groups["Letter"].Success)
                    output += matches[i].Value.ToUpper();
                else if (matches[i].Groups["Word"].Success)
                    output += matches[i].Value.ToLower();
            }

            // Returning the output.
            return output;
        }
        else
            // Returning the original value if no matches were found.
            return keyCodeString;
    }

    /// <summary>
    ///     Attempts to convert a given string to a KeyCode type.
    /// </summary>
    /// <param name="keyCodeString">
    ///     String to be parsed as a KeyCode type.
    /// </param>
    /// <param name="key">
    ///     KeyCode object to store the parsed key.
    /// </param>
    /// <returns>
    ///     Boolean based on parse success.
    ///     Parsed KeyCode returned as out parameter.
    /// </returns>
    public static bool TryParse(string keyCodeString, out KeyCode key)
    {
        // Using System.Enum.TryParse to convert from string to KeyCode.
        return Enum.TryParse(FormatKeyCodeString(keyCodeString), out key);
    }
}

#endregion
