using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class DynamicButton : MonoBehaviour 
{

    [SerializeField] TMP_Text scenarioInfoText = null;
    [SerializeField]public  TMP_Text buttonElementText = null;
    [SerializeField] public String scenarioDescription = null;
    [HideInInspector] public int buttonID = -1;

    [HideInInspector] public DynamicButton thisDynamicButton;  
    public event Action<DynamicButton> OnClickedDynamicButton;

    private void Awake()
    {
        thisDynamicButton = this;
    }
    private void DynamicEventManager()
    {
        OnClickedDynamicButton?.Invoke(thisDynamicButton);
        if (!(scenarioDescription == null || scenarioDescription == ""))
        {
              scenarioInfoText.text=scenarioDescription;
        }
    }

}
