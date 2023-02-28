using System;
using System.Collections;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Experimental.UI;
using TMPro;
public class BttToKeyboard : MonoBehaviour
{
    [SerializeField] protected TMP_Text buttonLabel;
    [SerializeField] Transform keyboardTarget;
    bool keyboardActive;
    public event EventHandler OnStringSubmitted;
    public void EnableKeyboard()
    {
        GameObject keyboardTargetGO = new GameObject("KeyboardTarget");
        keyboardTarget = keyboardTargetGO.transform;
        keyboardTarget.SetParent(Camera.main.transform);
        keyboardTarget.localPosition = new Vector3(0f, 0f, 2f);

        keyboardActive = true;

        NonNativeKeyboard.Instance.PresentKeyboard();
        NonNativeKeyboard.Instance.RepositionKeyboard(keyboardTarget, null, 0.05f);

        NonNativeKeyboard.Instance.OnTextSubmitted += InsertNewString;
        NonNativeKeyboard.Instance.OnClosed += (sender, args) =>
        {
            NonNativeKeyboard.Instance.OnTextSubmitted -= InsertNewString;
            keyboardActive = false;
        };
    }
    public virtual void InsertNewString(object? sender, EventArgs arg)
    {
        NonNativeKeyboard nnKeyboard = (NonNativeKeyboard)sender;
        buttonLabel.text = nnKeyboard.InputField.text;
        OnStringSubmitted?.Invoke(sender, arg);
    }
}
