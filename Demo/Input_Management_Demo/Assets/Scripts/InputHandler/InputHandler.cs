using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputHandler : MonoBehaviour
{
    /// <summary>
    ///     Centralised storage for all input settings.
    ///     Accessable through the Inspector window.
    /// </summary>
    [Tooltip("Use the + and - icons to add/remove an input group.")]
    public InputGroup[] inputs;

    /// <summary>
    /// 
    /// </summary>
    [Tooltip("Functions to be called whenever control settings are updated.")]
    public UnityEvent updateEvent;

    /// <summary>
    /// 
    /// </summary>
    private List<int> activeIndexes;

    /// <summary>
    ///     Start function called on program load.
    ///     Updating inputs whenever the program starts.
    /// </summary>
    private void Start()
    {
        activeIndexes = new List<int>();
        UpdateInputs();
    }

    /// <summary>
    ///     Update function called every frame.
    ///     Checks for inputs and handles them accordingly.
    /// </summary>
    private void Update()
    {
        // Checking for any input on the keyboard to save on unnessesary processing.

        if (Input.anyKeyDown)
        {
            // Iterating over each input group index.
            // Checking if each input group is being activated and is not currently stored.
            // Storing active inputs.

            for (int i = 0; i < inputs.Length; i++)
                if (inputs[i].TryGetInput() && !activeIndexes.Contains(i))
                    activeIndexes.Add(i);
        }

        // Handling active inputs.

        for (int i = activeIndexes.Count - 1; i >= 0; i--)
        {
            // Updating the state of the input group.
            // Invokes connected functions.

            inputs[activeIndexes[i]].HandleInput();

            // Checking if input is still active after update.
            // If not, removing the index from the active list.

            if (!inputs[activeIndexes[i]].isActive)
                activeIndexes.RemoveAt(i);
        }
    }

    /// <summary>
    ///     Blanket fire update for each input.
    ///     Used in-editor to save changed made during play mode.
    /// </summary>
    public void UpdateInputs()
    {
        // Iterating over each input group to update settings.

        foreach (InputGroup i in inputs)
            i.UpdateSettings();

        updateEvent?.Invoke();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="groupName"></param>
    /// <param name="keyName"></param>
    /// <param name="key"></param>
    public void ChangeInput(string groupName, string keyName, KeyCode key)
    {
        foreach (InputGroup g in inputs)
            if (g.name == groupName)
                g.ChangeKey(keyName, key);

        updateEvent?.Invoke();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="groupName"></param>
    /// <param name="keyName"></param>
    /// <param name="key"></param>
    public void ChangeInput(string groupName, string keyName, string key)
    {
        foreach (InputGroup g in inputs)
            if (g.name == groupName)
                g.ChangeKey(keyName, key);

        updateEvent?.Invoke();
    }
}
