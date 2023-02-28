using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelControl : MonoBehaviour
{
    [SerializeField] GameObject targetPanel;
    [SerializeField] SubPanelManager thisPanelManager;

    [HideInInspector]
    public PanelControl panelInstance;
    public event Action<int> OnClickedPanel;
    private void Awake()
    {
        panelInstance = this;
    }
    private void OnEnable()
    {
        thisPanelManager.OnMakeRoom += MakeRoom;
    }
    
    public void TogglePanel()
    {
        targetPanel.SetActive(!targetPanel.active);
        OnClickedPanel?.Invoke(this.GetInstanceID());
    }

    private void MakeRoom(int _openPanel)
    {
        if (_openPanel != this.GetInstanceID())
        {
            targetPanel.SetActive(false);
        }
    }
    private void OnDestroy()
    {
        thisPanelManager.OnMakeRoom -= MakeRoom;
    }
}

