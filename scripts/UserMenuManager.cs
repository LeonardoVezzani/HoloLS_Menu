using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using Microsoft.MixedReality.Toolkit.Experimental.UI;

public class UserMenuManager :  BttToKeyboard
{
    [SerializeField] GameObject userButtonElement;
    bool keyboardActive;
    public event EventHandler OnUserSubmitted;
    public event EventHandler OnUserSelected;
    public event EventHandler OnUserDeleted;


    public override void InsertNewString(object? sender, EventArgs arg)
    {

        NonNativeKeyboard nnKeyboard = (NonNativeKeyboard)sender;

        GameObject newButton = Instantiate(userButtonElement) as GameObject;

        newButton.transform.parent = this.transform;
        newButton.transform.position = this.transform.position + new Vector3(0, (this.transform.GetChildCount() - 1) * (-0.035f), 0);

        UserButtonManager newUBM = newButton.GetComponent<UserButtonManager>();
        newUBM.userButton.GetComponent<DynamicButton>().buttonElementText.text = nnKeyboard.InputField.text;
        newButton.SetActive(true);

        newUBM.userButton.OnClickedDynamicButton += SelectUser;
        newUBM.userDeleteButton.gameObject.GetComponent<DeleteUserButton>().OnDeletedUser += DeleteUser;

        OnUserSubmitted?.Invoke(newButton.gameObject, arg);
        OnUserSelected?.Invoke(sender, arg);
    }

    public void SelectUser(DynamicButton selectedUser)
    {
        //Debug.Log("selected User: " + selectedUser.name);
        OnUserSelected?.Invoke(this, EventArgs.Empty);
    }

    public void DeleteUser(int destroyedUser)
    {
        //Debug.Log("Destroyed user " + destroyedUser);
        OnUserDeleted?.Invoke(this, EventArgs.Empty);
    }
}
