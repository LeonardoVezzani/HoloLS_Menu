using System;
using System.Collections.Generic;
using UnityEngine;

public class SubPanelManager : MonoBehaviour
{

    [SerializeField] PanelControl menu1 = null;
    [SerializeField] PanelControl menu2 = null;
    [SerializeField] PanelControl menu3 = null;
    [SerializeField] PanelControl menu4 = null;

    [HideInInspector] public SubPanelManager thisInstance;
    public event Action<int> OnMakeRoom;

    private void Awake()
    {
        thisInstance = this;
    }
    private void Start()
    {
        if (menu1 != null)
            menu1.panelInstance.OnClickedPanel += MakeRoom;
        if (menu2 != null)
            menu2.panelInstance.OnClickedPanel += MakeRoom;
        if (menu3 != null)
            menu3.panelInstance.OnClickedPanel += MakeRoom;
        if (menu4 != null)
            menu4.panelInstance.OnClickedPanel += MakeRoom;
    }

    public void MakeRoom(int _menuOpened)
    {
        OnMakeRoom?.Invoke(_menuOpened);
    }
    private void OnDestroy()
    {
        if (menu1 != null)
            menu1.panelInstance.OnClickedPanel -= MakeRoom;
        if (menu2 != null)
            menu2.panelInstance.OnClickedPanel -= MakeRoom;
        if (menu3 != null)
            menu3.panelInstance.OnClickedPanel -= MakeRoom;
        if (menu4 != null)
            menu4.panelInstance.OnClickedPanel -= MakeRoom;
    }

}
