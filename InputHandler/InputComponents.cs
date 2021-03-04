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

#region InputGroup Class

/// <summary>
///     Custom class to store information about axial key-bindings.
///     <para>
///         Inputs have positive and negative values to determine the direction of input.
///         Negative inputs can be ignored to create single button inputs.
///     </para>
/// </summary>
[Serializable]
public class InputGroup
{

    // ===== Public Variables. =====

    /// <summary> 
    ///     A given name for the current input axis. 
    ///     Mainly for presentation purposes. 
    /// </summary>
    [Tooltip("The name of this input group.")]
    public string name;

    // Using header to create section seperation in the Unity inspector.
    [Header("Key Options")]

    /// <summary>
    ///     
    /// </summary>
    public Key[] keys;

    [Header("Input Options")]

    /// <summary>
    ///     Determines the amount of smoothing for lerping between directional input values.
    ///     Set to 1 to snap.
    /// </summary>
    // Using range to limit size of step between 0 and 1.
    // Also creates handy slider!
    [Range(0.0001f, 1)]
    [Tooltip("Determines the amount of input smoothing.\nSet to 1 to disable smoothing.")]
    public float stepSize = 0.1f;

    /// <summary>
    /// 
    /// </summary>
    public bool canHoldInput;

    /// <summary>
    /// 
    /// </summary>
    public bool isActive { 
        get { return hasInput || currentInputValue != 0; }
    }

    [Header("Linked Events")]

    /// <summary>
    ///     UnityEvent interface allowing for passing of input float data.
    ///     Link to functions that require the use of inputs.
    /// </summary>
    public InputEvent inputEvent;

    // ===== Private Variables. =====

    /// <summary>
    /// 
    /// </summary>
    private float snapThreshold;

    /// <summary>
    ///     Stores the current float value of the input.
    ///     Used to apply smoothing between axis directions.
    /// </summary>
    private float currentInputValue = 0f;

    /// <summary>
    /// 
    /// </summary>
    private int rawInputValue = 0;

    /// <summary>
    /// 
    /// </summary>
    private bool hasInput = false;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="current"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    private delegate float InputGenerator(float current, float input);

    /// <summary>
    /// 
    /// </summary>
    private InputGenerator inputGenerator;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    private delegate bool InputChecker(KeyCode key);

    /// <summary>
    /// 
    /// </summary>
    private InputChecker inputChecker;

    // ===== Update Functions. =====

    /// <summary>
    ///     Performs conversion of each stored string key in the current instance to KeyCode.
    ///     Used for in-editor changes.
    /// </summary>
    public void UpdateSettings()
    {
        // Determining the function used to generate an output value.

        inputGenerator =
            stepSize == 1 ?
                (InputGenerator)GetSnappedInput
                : (InputGenerator)GetLerpedInput;

        //

        inputChecker =
            canHoldInput ?
                (InputChecker)Input.GetKey
                : (InputChecker)Input.GetKeyDown;

        //

        snapThreshold = stepSize * 0.5f;

        // Resetting the positive and negative key values to activate the 
        // setter string>KeyCode conversion.

        foreach (Key k in keys)
            k.UpdateKey();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="keyName"></param>
    /// <param name="key"></param>
    public void ChangeKey(string keyName, KeyCode key)
    {
        foreach (Key k in keys)
            if (k.name == keyName)
                k.ChangeKey(key);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="keyName"></param>
    /// <param name="key"></param>
    public void ChangeKey(string keyName, string key)
    {
        foreach (Key k in keys)
            if (k.name == keyName)
                k.ChangeKey(key);
    }

    // ===== Input Handling. =====

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public bool TryGetInput()
    {
        // 

        int output = 0;
        bool input = false;

        // 

        foreach (Key k in keys)
            if (k.useKey && inputChecker(k.keyCode))
            {
                input = true;
                output += (int)k.value;
            }

        // 

        hasInput = input;
        rawInputValue = output;

        // 

        return hasInput;
    }

    /// <summary>
    /// 
    /// </summary>
    public void HandleInput()
    {
        // 
        
        TryGetInput();

        // 
        
        inputEvent?.Invoke(
            inputGenerator(
                currentInputValue, 
                rawInputValue
            )
        );
    }

    // ===== Input Generators. =====

    /// <summary>
    /// 
    /// </summary>
    /// <param name="current"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    private float GetLerpedInput(float current, float input)
    {
        // 

        float inputLerp = Mathf.Lerp(
            current,
            input,
            stepSize
        );

        // 

        if (Mathf.Abs(inputLerp) < snapThreshold)
            currentInputValue = 0;
        else
            currentInputValue = inputLerp;

        // 

        return currentInputValue;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="current"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    private float GetSnappedInput(float current, float input)
    {
        // 

        return input;
    }
}

#endregion

#region Key Class

[Serializable]
public class Key
{
    // ===== Public Variables. =====

    /// <summary>
    ///     The display name of the key.
    /// </summary>
    public string name;

    /// <summary> 
    ///     Internal storage for the string name of the negative input KeyCode. 
    /// </summary>
    [SerializeField]
    [Tooltip("The name of a KeyCode.")]
    private string key = "";

    /// <summary>
    ///     Storage for the KeyCode conversion of the key string.
    ///     Can be used to interface with the Unity input system.
    /// </summary>
    public KeyCode keyCode { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    public bool useKey { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    [Tooltip("The output value returned by the key.")]
    public Output value = Output.Positive;

    // ===== Public Functions. =====

    /// <summary>
    /// 
    /// </summary>
    public void UpdateKey()
    {
        keyCode = TryKeyCodeConversion(key);
        useKey = UseKey();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="targetKey"></param>
    public void ChangeKey(KeyCode targetKey)
    {
        key = targetKey.ToString();
        keyCode = targetKey;
        useKey = UseKey();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="targetKey"></param>
    public void ChangeKey(string targetKey)
    {
        key = targetKey;
        keyCode = TryKeyCodeConversion(targetKey);
        useKey = UseKey();
    }

    // ===== Private Functions. =====

    /// <summary>
    ///     Attempts to convert a given string value to a KeyCode.
    ///     Returns a warning if unsuccessful.
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
    /// 
    /// </summary>
    /// <returns></returns>
    private bool UseKey() => keyCode != KeyCode.None;
}

/// <summary>
/// 
/// </summary>
[Serializable]
public enum Output
{
    Positive = 1,
    Negative = -1
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
