using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public InputAxis[] inputs;

    private void Start()
    {
        UpdateInputs();
    }

    public void UpdateInputs()
    {
        foreach (InputAxis i in inputs)
            i.UpdateSettings();
    }

    private void Update()
    {
        foreach (InputAxis i in inputs)
            i.CheckInput();
    }
}
