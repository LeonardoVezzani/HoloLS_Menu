using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Experimental.UI;



public class MenuControl : MonoBehaviour
{
    [SerializeField] bool initializedMenu = true;
    [SerializeField] bool loadPreviousSettings = true;
    [SerializeField] GameObject buttonElement = null;
    [Space]
    [Header("ID for each settingmenu")]

    int languageID = 0;
    int ttsID = 1;
    int userID = 2;
    int procedureID = 3;
    int scenarioID = 4;
    int videoID = 5;
    [Space]
    [Header("Sub Menus to Populate from XML TBG")]
    [SerializeField] TargetButtonGroup languageTBG =null;
    [SerializeField] TargetButtonGroup textToSpeechTBG = null;
    [SerializeField] UserMenuManager userMM = null;
    [SerializeField] TargetButtonGroup procedureTBG = null;
    [SerializeField] TargetButtonGroup scenarioTBG = null;
    [SerializeField] TargetButtonGroup videoTBG = null;

    [Space]
    [Header("Buttons To Launch Scenes")]
    [SerializeField] BttToMenu tutorialBTN = null;
    [SerializeField] BttToMenu learningBTN = null;
    [SerializeField] BttToMenu trainingBTN = null;
    [SerializeField] BttToMenu remoteBTN = null;
    [SerializeField] BttToMenu autoConnectBTN = null;

    [Space]
    [Header("Buttons To Adjust variables")]
    [SerializeField] BttToMenu eyeCalibrationBTN = null;
    [SerializeField] BttToMenu designEnvironementBTN = null;

    [Space]
    [Header("Buttons with keyboard")]
    [SerializeField] BttToKeyboard ipBtt = null;
    [SerializeField] BttToKeyboard portBtt = null;
    [SerializeField] UserMenuManager userBtt = null;

    public static MenuControl menuInstance;
    public event Action<int> MenuButtonPressed;

    private String[] string_lang = { "italiano", "inglese" };
    private String[] string_tts = { "Azure", "MSC" };
    private String[] string_procedure = { "BLSD", "BLSD PRO", "ACLS", "AirWay Managmeent", "Electric therapy" };
    private String[] string_video = { "on", "off" };
    private String[] string_scenario1 = { "BLSD 1", "BLSD 2", "BLSD 3" };
    private String[] string_scenario2 = { "BLSD PRO 1", "BLSD PRO 2", "BLSD PRO 3", "BLSD PRO 4" };
    private String[] string_scenario3 = { "ACLS 1", "ACLS 2", "ACLS 3", "ACLS 4" };
    private String[] string_scenario4 = { "ET 1", "ET 2" };
    private String[] string_scenario5 = { "AM 1", "AM 2", "AM 3" };
    private String[] string_scenario1_Descr = { "scenario 1 BLSD - blah blah blah", "scenrio 2 BLSD- uga buga bonk", "scneario 3 BLSD- deedly doo" };
    private String[] string_scenario2_Descr = { "scenario 1 BLSD PRO- bibidi", "scenrio 2 BLSD PRO- bobidi", "scneario 3 BLSD PRO- bu", "scenmario 4 BLSD PRO - banana" };
    private String[] string_scenario3_Descr = { "scenario 1 ACLS- bibidi", "scenrio 2 ACLS- bobidi", "scneario 3 ACLS- bu", "scenmario 4 ACLS - banana" };
    private String[] string_scenario4_Descr = { "scenario 1 ET - blah blah blah", "scenrio 2 ET- uga buga bonk" };
    private String[] string_scenario5_Descr = { "scenario 1 AM- bibidi", "scenrio 2 AM- bobidi", "scneario 3 AM- bu" };
    private int[] settings = {1,0,0,3,1,0};

    private void Awake()
    {
        menuInstance = this;
        if (initializedMenu)
        {
            CreateGroupButtons(languageTBG, string_lang, languageID);
            CreateGroupButtons(textToSpeechTBG, string_tts,ttsID);
            CreateGroupButtons(procedureTBG, string_procedure,procedureID, true);
            CreateGroupButtons(videoTBG, string_video, videoID);
        }
        userMM.OnUserSelected += SelectUser;
        userMM.OnUserDeleted += DeleteUser;
        userMM.OnUserSubmitted += SaveUser;
        userMM.gameObject.GetComponent<TargetButtonGroup>().thisTBGID = userID;


        tutorialBTN.clickToMenu += ButtonClicked;
        learningBTN.clickToMenu += ButtonClicked;
        trainingBTN.clickToMenu += ButtonClicked;
        remoteBTN.clickToMenu += ButtonClicked;
        autoConnectBTN.clickToMenu += ButtonClicked;
        
        eyeCalibrationBTN.clickToMenu += ButtonClicked;
        designEnvironementBTN.clickToMenu += ButtonClicked;

        ipBtt.OnStringSubmitted += KeyboardInput;
        portBtt.OnStringSubmitted += KeyboardInput;
        userBtt.OnStringSubmitted += SaveUser;

    }

    private void Start()
    {
        if (loadPreviousSettings)
        {
            //todo: load scenario menu
            languageTBG.RestoreSavedSettings(string_lang[settings[languageID]]);
            textToSpeechTBG.RestoreSavedSettings(string_tts[settings[ttsID]]);
            procedureTBG.RestoreSavedSettings(string_procedure[settings[procedureID]]);
            scenarioTBG.RestoreSavedSettings(string_scenario1[settings[scenarioID]]);  //todo** :scring_scenario(settings[procedureID])[settings[scenarioID]]
            ChangeSubMenu(scenarioTBG, settings[procedureID]);
            videoTBG.RestoreSavedSettings(string_video[settings[videoID]]);

        }
    }

    public void CreateGroupButtons(TargetButtonGroup _tbg,  string[] _stringArray, int _TBGID, bool _hasSubMenu = false)
    {
        _tbg.thisTBGID = _TBGID;
        int newButtNumb=0;
        foreach(string str in _stringArray)
        {
            DynamicButton nb = NewButton(_tbg, str);
            nb.buttonID = newButtNumb;
            nb.OnClickedDynamicButton += _tbg.ButtonSelected;

            newButtNumb++;
        }
        if (_hasSubMenu) //todo**: might be done better passing an string matrix
        {
            _tbg.OnSelectedSetting += ChangeSubMenu;
        }
        _tbg.OnSelectedSetting += SaveSettings;
    }

    public void ChangeSubMenu(TargetButtonGroup _target, int _buttonID)
    {
        //todo** : CreateSubordinbateGroupButtons(scenarioTBG, string_scenario[_buttonID], 3,  string_scenario_Descr[_buttonID]);

        ClearMenu(scenarioTBG);
        switch (_buttonID)
        {
            case 0:
                CreateSubordinbateGroupButtons(scenarioTBG, string_scenario1, scenarioID,  string_scenario1_Descr);
                break;

            case 1:
                CreateSubordinbateGroupButtons(scenarioTBG, string_scenario2, scenarioID, string_scenario2_Descr);
                break;

            case 2:
                CreateSubordinbateGroupButtons(scenarioTBG, string_scenario3, scenarioID, string_scenario3_Descr);
                break;

            case 3:
                CreateSubordinbateGroupButtons(scenarioTBG, string_scenario4, scenarioID, string_scenario4_Descr);
                break;

            case 4:
                CreateSubordinbateGroupButtons(scenarioTBG, string_scenario5, scenarioID, string_scenario5_Descr);
                break;
        }

    }
   
    public void CreateSubordinbateGroupButtons(TargetButtonGroup _subtbg, string[] _subStringArray, int _subTBGID, String[] _description)
    {
        _subtbg.thisTBGID = _subTBGID;
        int newButtNumb = 0;
        for (int str=0; str< _subStringArray.Length; str++)
        {
            DynamicButton nb = NewButton(_subtbg, _subStringArray[str]);
            nb.scenarioDescription = _description[str];
            nb.buttonID = newButtNumb;
            newButtNumb++;
            nb.OnClickedDynamicButton += _subtbg.ButtonSelected;
        }
        scenarioTBG.OnSelectedSetting += SaveSettings;
    }

    public void ClearMenu(TargetButtonGroup _menuToDestroy)
    {
        foreach (Transform item in _menuToDestroy.transform) {
            GameObject.Destroy(item.gameObject); 
        }
        int startChildren = _menuToDestroy.transform.GetChildCount();
        for (int i = 0; i < startChildren; i++)
        {
            _menuToDestroy.transform.GetChild(0).SetParent(null);
        }
    }

    private DynamicButton NewButton(TargetButtonGroup _dest, string _buttonName)
    {
        GameObject newButton = Instantiate(buttonElement) as GameObject;
       
        newButton.transform.parent = _dest.transform;
        newButton.transform.position = _dest.transform.position  + new Vector3 (0, (_dest.transform.GetChildCount()-1) * (- 0.035f),0);

        DynamicButton newButtDynamicButton = newButton.GetComponent<DynamicButton>();
        TMP_Text _TMP = newButtDynamicButton.buttonElementText;
        _TMP.text = _buttonName;

        newButton.SetActive(true);
        _dest.gameObject.SetActive(false);
        return newButtDynamicButton;
    }

    private void SaveSettings(TargetButtonGroup _TBG, int _setting)
    {
        //todo*: evento verso esterno o playerprefs?
        Debug.Log("Saving Setting : Menu " +_TBG.thisTBGID + " clicked button : " + _setting); 
        settings[_TBG.thisTBGID] = _setting;
    }

    private void KeyboardInput(object? sender, EventArgs arg)
    {
        //todo*: evento verso esterno o playerprefs?
        Debug.Log("Keyboard input string : " + sender.ToString());
    }
    protected void DeleteUser(object sender, EventArgs args)
    {
        //todo*: evento verso esterno o playerprefs?
       
        Debug.Log("User Deleted : " + sender.ToString());
        //PlayerPrefs.SetString("UserName", userName);
        //PlayerPrefs.Save();

        //ProcedureManager.UserName = userName;
        //BLSLogger.Reset();
    }
    protected void SelectUser(object sender, EventArgs args)
    {
        //todo*: evento verso esterno o playerprefs?

        Debug.Log("User Selected : " + sender.ToString());
        //PlayerPrefs.SetString("UserName", userName);
        //PlayerPrefs.Save();

        //ProcedureManager.UserName = userName;
        //BLSLogger.Reset();
    }

    protected void SaveUser(object sender, EventArgs args)
    {
        //todo*: evento verso esterno o playerprefs?
        string userName = sender.ToString();
        if (String.IsNullOrEmpty(userName))
            userName = "N/A";

        Debug.Log("User Created : " + userName);
       //PlayerPrefs.SetString("UserName", userName);
       //PlayerPrefs.Save();

        //ProcedureManager.UserName = userName;
        //BLSLogger.Reset();
    }
    public void ButtonClicked(int _clickedID)
    {
        MenuButtonPressed?.Invoke(_clickedID);
        Debug.Log("button " + _clickedID + "Clicked");
    }

    private void OnDestroy()
    {

        tutorialBTN.clickToMenu -= ButtonClicked;
        learningBTN.clickToMenu -= ButtonClicked;
        trainingBTN.clickToMenu -= ButtonClicked;
        remoteBTN.clickToMenu -= ButtonClicked;
        autoConnectBTN.clickToMenu -= ButtonClicked;

        eyeCalibrationBTN.clickToMenu -= ButtonClicked;
        designEnvironementBTN.clickToMenu -= ButtonClicked;

        ipBtt.OnStringSubmitted -= KeyboardInput;
        portBtt.OnStringSubmitted -= KeyboardInput;
        userBtt.OnStringSubmitted -= SaveUser;

        userMM.OnUserSelected -= SelectUser;
        userMM.OnUserDeleted -= DeleteUser;
        userMM.OnUserSubmitted -= SaveUser;
    }

}
