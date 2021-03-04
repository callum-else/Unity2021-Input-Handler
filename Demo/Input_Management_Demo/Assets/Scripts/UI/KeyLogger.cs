using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// 
/// </summary>
public class KeyLogger : MonoBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    private TextMeshProUGUI textMesh;
    
    /// <summary>
    /// 
    /// </summary>
    private List<KeyLog> keyStream;

    /// <summary>
    /// 
    /// </summary>
    private static KeyCode[] keyCodes = (KeyCode[])Enum.GetValues(typeof(KeyCode));

    // =====

    /// <summary>
    /// 
    /// </summary>
    public int maxLength = 5;
    
    /// <summary>
    /// 
    /// </summary>
    public float logDuration = 5;

    // =====

    /// <summary>
    /// 
    /// </summary>
    private void Start()
    {
        // 

        keyStream = new List<KeyLog>();

        // 

        textMesh = GetComponent<TextMeshProUGUI>();
    }

    /// <summary>
    /// 
    /// </summary>
    private void Update()
    {
        // 

        if (Input.anyKeyDown)
        {
            // 

            foreach (KeyCode kc in keyCodes)
                if (Input.GetKeyDown(kc))
                {
                    // 

                    if (keyStream.Count + 1 > maxLength)
                        keyStream.RemoveAt(0);
                    keyStream.Add(new KeyLog(kc.ToString(), Time.time));
                }

            // 

            Redraw();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void FixedUpdate()
    {
        // 

        if (keyStream.Count > 0 && Time.time - keyStream[0].timeOfLog > logDuration)
        {
            keyStream.RemoveAt(0);
            Redraw();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void Redraw()
    {
        // 

        textMesh.text = "";

        // 

        for (int i = keyStream.Count - 1; i >= 0; i--)
            textMesh.text += $">  {keyStream[i].keyName.ToUpper()}\n";
    }
}

/// <summary>
/// 
/// </summary>
public class KeyLog
{
    /// <summary>
    /// 
    /// </summary>
    public string keyName { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    public float timeOfLog { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="time"></param>
    public KeyLog(string key, float time)
    {
        keyName = key;
        timeOfLog = time;
    }
}
