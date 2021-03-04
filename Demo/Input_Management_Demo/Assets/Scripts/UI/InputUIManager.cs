using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InputUIManager : MonoBehaviour
{
    /// <summary> 
    ///     
    /// </summary>
    public InputHandler inputHandler;

    [Space]

    /// <summary> 
    ///     
    /// </summary>
    public RectTransform contentContainer;

    [Header("UI Prefabs")]

    /// <summary> 
    ///     
    /// </summary>
    public GameObject title;

    /// <summary> 
    ///     
    /// </summary>
    public GameObject control;

    // =====

    /// <summary> 
    ///     
    /// </summary>
    private float currentHeight;

    /// <summary> 
    ///     
    /// </summary>
    private bool isListening;

    /// <summary> 
    ///     
    /// </summary>
    private InputUIComponent targetComponent;

    /// <summary> 
    ///     
    /// </summary>
    private static KeyCode[] keyCodes = (KeyCode[])Enum.GetValues(typeof(KeyCode));

    // =====

    /// <summary>
    /// 
    /// </summary>
    private void Start()
    {
        currentHeight = contentContainer.rect.height / 2;
        StartCoroutine(InitialiseUI());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private IEnumerator InitialiseUI()
    {
        // 

        yield return new WaitForSeconds(0.1f);

        // 

        foreach (InputGroup ig in inputHandler.inputs)
        {
            CreateTitle(ig.name);
            for(int i = 0; i < ig.keys.Length; i++)
                CreateControl(ig.name, ref ig.keys[i]);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="uiObj"></param>
    /// <returns></returns>
    private RectTransform CreateUI(GameObject uiObj)
    {
        // 

        RectTransform obj = Instantiate(uiObj, contentContainer.transform).GetComponent<RectTransform>();

        // 

        currentHeight -= obj.rect.height / 2;
        obj.anchoredPosition = new Vector2(0, currentHeight);
        currentHeight -= obj.rect.height / 2;

        // 

        return obj;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="groupName"></param>
    private void CreateTitle(string groupName)
    {
        // 

        CreateUI(title).GetComponent<TextMeshProUGUI>().text = groupName;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="groupName"></param>
    /// <param name="key"></param>
    private void CreateControl(string groupName, ref Key key)
    {
        // 

        CreateUI(control).GetComponent<InputUIComponent>().Initialise(
            groupName, key, this
        );
    }

    /// <summary>
    /// 
    /// </summary>
    private void Update()
    {
        // 

        if (isListening && targetComponent != null && Input.anyKeyDown)
        {
            // 

            foreach(KeyCode kc in keyCodes)
                if (Input.GetKeyDown(kc))
                {
                    // 

                    targetComponent.SetNewKey(kc);

                    // 

                    targetComponent = null;
                    isListening = false;
                }
        }
    }

    // =====

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    public void StartListening(ref InputUIComponent sender)
    {
        // 

        if (!isListening)
        {
            // 

            targetComponent = sender;
            isListening = true;
        }
    }
}
