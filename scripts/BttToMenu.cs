using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BttToMenu : MonoBehaviour
{

    [HideInInspector]public BttToMenu buttonToMenu;
    public event Action<int> clickToMenu;

    private void Awake()
    {
        buttonToMenu = this;
    }
    public void ButtonClicked()
    {
        clickToMenu?.Invoke(buttonToMenu.GetInstanceID());
    }
}
