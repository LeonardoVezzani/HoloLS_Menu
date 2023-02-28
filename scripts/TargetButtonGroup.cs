using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TargetButtonGroup : MonoBehaviour
{
    [SerializeField] public TMP_Text siblingButtonText = null;
    [HideInInspector] public TargetButtonGroup thisTBG;
    [HideInInspector] public int thisTBGID = 0;
    public event Action<TargetButtonGroup, int> OnSelectedSetting;
    public event Action<string> OnSelectedChangeButtonName;

    private void Awake()
    {
        thisTBG = this;
    }
    public void ButtonSelected(DynamicButton _button)
    {
        OnSelectedSetting?.Invoke(thisTBG, _button.buttonID);
        OnSelectedChangeButtonName?.Invoke(thisTBG.transform.parent.name);
        siblingButtonText.text = _button.buttonElementText.text;
    }
    public void RestoreSavedSettings(string _buttonName)
    {
        siblingButtonText.text = _buttonName;
    }
}
