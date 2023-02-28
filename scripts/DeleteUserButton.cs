using System;
using System.Collections.Generic;
using UnityEngine;

public class DeleteUserButton : MonoBehaviour
{
    public event Action<int> OnDeletedUser;
   public void DltUserButton()
    {
        int bttID = this.transform.parent.gameObject.GetComponent<UserButtonManager>().userButton.buttonID;
        OnDeletedUser?.Invoke(bttID);
        GameObject.Destroy(this.transform.parent.gameObject);
    }
}
