using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KeyLogger : MonoBehaviour
{
    private TextMeshProUGUI textMesh;
    private List<KeyLog> keyStream;
    public int maxLength = 5;
    public float logDuration = 5;

    private static KeyCode[] keyCodes = (KeyCode[])Enum.GetValues(typeof(KeyCode));

    private void Start()
    {
        keyStream = new List<KeyLog>();
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            foreach (KeyCode kc in keyCodes)
                if (Input.GetKeyDown(kc))
                {
                    if (keyStream.Count + 1 > maxLength)
                        keyStream.RemoveAt(0);
                    keyStream.Add(new KeyLog(kc.ToString(), Time.time));
                }
            Redraw();
        }
    }

    private void FixedUpdate()
    {
        if (keyStream.Count > 0 && Time.time - keyStream[0].timeOfLog > logDuration)
            keyStream.RemoveAt(0);
        Redraw();
    }

    private void Redraw()
    {
        textMesh.text = "";
        for (int i = keyStream.Count - 1; i >= 0; i--)
            textMesh.text += $">  {keyStream[i].keyName.ToUpper()}\n";
    }
}

public class KeyLog
{
    public string keyName { get; private set; }
    public float timeOfLog { get; private set; }

    public KeyLog(string key, float time)
    {
        keyName = key;
        timeOfLog = time;
    }
}
