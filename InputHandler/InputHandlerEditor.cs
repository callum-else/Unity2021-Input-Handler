using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(InputHandler))]
public class InputHandlerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (Application.isPlaying)
        {
            InputHandler ipHandler = (InputHandler)target;
            if (GUILayout.Button("Update Input Settings"))
                ipHandler.UpdateInputs();
        }
    }
}
