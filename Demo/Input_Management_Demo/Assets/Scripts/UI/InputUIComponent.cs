using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// 
/// </summary>
public class InputUIComponent : MonoBehaviour
{
    [Header("Required Components")]
    [Tooltip("")]
    /// <summary> 
    ///     
    /// </summary>
    public TextMeshProUGUI keyTitle;

    [Tooltip("")]
    /// <summary> 
    ///     
    /// </summary>
    public TextMeshProUGUI keyText;

    // =====

    /// <summary>
    /// 
    /// </summary>
    private InputUIInfo inputInfo;

    /// <summary>
    /// 
    /// </summary>
    private Button editButton;

    /// <summary>
    /// 
    /// </summary>
    private InputUIComponent thisComponent;

    /// <summary>
    /// 
    /// </summary>
    private InputUIManager uiManager;

    /// <summary>
    /// 
    /// </summary>
    private Key keyRef;

    // =====

    /// <summary>
    /// 
    /// </summary>
    /// <param name="groupName"></param>
    /// <param name="key"></param>
    /// <param name="inputUI"></param>
    public void Initialise(string groupName, Key key, InputUIManager uiManager)
    {
        // 

        keyRef = key;

        this.uiManager = uiManager;

        inputInfo = new InputUIInfo(
            groupName, 
            key.name, 
            key.keyCode.ToString()
        );

        // 

        editButton = GetComponentInChildren<Button>();
        thisComponent = this;

        // 

        keyTitle.text = inputInfo.keyTitle;
        keyText.text = inputInfo.keyName;
    }

    /// <summary>
    /// 
    /// </summary>
    public void ListenForInput()
    {
        // 

        keyText.text = "Press Any Key";
        uiManager.StartListening(ref thisComponent);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="keyCode"></param>
    public void SetNewKey(KeyCode keyCode)
    {
        // 

        keyRef.ChangeKey(keyCode);

        // 

        inputInfo.keyName = keyCode.ToString();
        keyText.text = inputInfo.keyName;
    }
}

/// <summary>
/// 
/// </summary>
[System.Serializable]
public class InputUIInfo
{
    /// <summary> 
    ///     
    /// </summary>
    public string groupName;

    /// <summary> 
    ///     
    /// </summary>
    public string keyTitle;

    /// <summary> 
    ///     
    /// </summary>
    public string keyName;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="groupName"></param>
    /// <param name="keyTitle"></param>
    /// <param name="keyName"></param>
    public InputUIInfo(string groupName, string keyTitle, string keyName)
    {
        // 

        this.groupName = groupName;
        this.keyTitle = keyTitle;
        this.keyName = keyName;
    }
}
