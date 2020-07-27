using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class TitleManager : MonoBehaviour
{
    Image titleName;
    
    private GameObject pressAnyKey;
    private GameObject newGame;
    Button newGameTUTORIAL;
    Button newGameNEW;
    Button newGameRECORDS;
    Button newGameOPTION;
    Button newGameQUIT;
    Button newGameTUTORIAL_Eng;
    Button newGameNEW_Eng;
    Button newGameRECORDS_Eng;
    Button newGameOPTION_Eng;
    Button newGameQUIT_Eng;

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    private GameObject loadGame;
    Button loadGameCONTINUE;
    Button loadGameNEW;
    Button loadGameTUTORIAL;
    Button loadGameRECORDS;
    Button loadGameOPTION;
    Button loadGameQUIT;
    Button loadGameCONTINUE_Eng;
    Button loadGameNEW_Eng;
    Button loadGameTUTORIAL_Eng;
    Button loadGameRECORDS_Eng;
    Button loadGameOPTION_Eng;
    Button loadGameQUIT_Eng;

    private GameObject newGameCheck;
    Button newGameCheckExit;
    Button newGameCheckYes;
    Button newGameCheckNo;

    public List<GameObject> languageData = new List<GameObject>();

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region[Awake]
    void Awake()
    {
        Transform canvas = transform.Find("Canvas");

        Transform menu = canvas.Find("Menu");
        pressAnyKey = canvas.Find("PressAnyKey").gameObject;
        newGame = menu.Find("NewGame").gameObject;
        loadGame = menu.Find("LoadGame").gameObject;

        titleName = menu.Find("TitleName").GetComponent<Image>();

        newGameTUTORIAL = newGame.transform.Find("Btn_한국어").Find("Tutorial").GetComponent<Button>();
        newGameNEW = newGame.transform.Find("Btn_한국어").Find("New").GetComponent<Button>();
        newGameRECORDS = newGame.transform.Find("Btn_한국어").Find("Records").GetComponent<Button>();
        newGameOPTION = newGame.transform.Find("Btn_한국어").Find("Option").GetComponent<Button>();
        newGameQUIT = newGame.transform.Find("Btn_한국어").Find("Quit").GetComponent<Button>();

        loadGameCONTINUE = loadGame.transform.transform.Find("Btn_한국어").Find("Continue").GetComponent<Button>();
        loadGameNEW = loadGame.transform.transform.Find("Btn_한국어").Find("New").GetComponent<Button>();
        loadGameTUTORIAL = loadGame.transform.transform.Find("Btn_한국어").Find("Tutorial").GetComponent<Button>();
        loadGameRECORDS = loadGame.transform.Find("Btn_한국어").Find("Records").GetComponent<Button>();
        loadGameOPTION = loadGame.transform.transform.Find("Btn_한국어").Find("Option").GetComponent<Button>();
        loadGameQUIT = loadGame.transform.transform.Find("Btn_한국어").Find("Quit").GetComponent<Button>();

        newGameTUTORIAL_Eng = newGame.transform.Find("Btn_English").Find("Tutorial").GetComponent<Button>();
        newGameNEW_Eng = newGame.transform.Find("Btn_English").Find("New").GetComponent<Button>();
        newGameRECORDS_Eng = newGame.transform.Find("Btn_English").Find("Records").GetComponent<Button>();
        newGameOPTION_Eng = newGame.transform.Find("Btn_English").Find("Option").GetComponent<Button>();
        newGameQUIT_Eng = newGame.transform.Find("Btn_English").Find("Quit").GetComponent<Button>();

        loadGameCONTINUE_Eng = loadGame.transform.transform.Find("Btn_English").Find("Continue").GetComponent<Button>();
        loadGameNEW_Eng = loadGame.transform.transform.Find("Btn_English").Find("New").GetComponent<Button>();
        loadGameTUTORIAL_Eng = loadGame.transform.transform.Find("Btn_English").Find("Tutorial").GetComponent<Button>();
        loadGameRECORDS_Eng = loadGame.transform.transform.Find("Btn_English").Find("Records").GetComponent<Button>();
        loadGameOPTION_Eng = loadGame.transform.transform.Find("Btn_English").Find("Option").GetComponent<Button>();
        loadGameQUIT_Eng = loadGame.transform.transform.Find("Btn_English").Find("Quit").GetComponent<Button>();

        newGameCheck = loadGame.transform.Find("NewGameCheck").gameObject;
        newGameCheckYes = newGameCheck.transform.Find("Yes").GetComponent<Button>();
        newGameCheckNo = newGameCheck.transform.Find("No").GetComponent<Button>();
        newGameCheckExit = newGameCheck.transform.Find("Exit").GetComponent<Button>();

        #region[한국어]
        newGameTUTORIAL.onClick.AddListener(() =>
        {
            if (SceneController.instance.nowSceneMoving)
                return;
            SoundManager.instance.BtnClick();
            SoundManager.instance.ChickenCoco();
            SceneController.instance.MoveScene("Tutorial");
        });
        newGameNEW.onClick.AddListener(() =>
        {
            if (SceneController.instance.nowSceneMoving)
                return;
            GameManager.instance.ClearData();
            SoundManager.instance.BtnClick();
            SoundManager.instance.ChickenCoco();
            SceneController.instance.MoveScene(GameManager.instance.playData.stageName);
        });
        newGameRECORDS.onClick.AddListener(() =>
        {
            if (SceneController.instance.nowSceneMoving)
                return;
            SoundManager.instance.BtnClick();
            SoundManager.instance.ChickenCoco();
            SceneController.instance.MoveScene("Records");
        });
        newGameOPTION.onClick.AddListener(() =>
        {
            if (SceneController.instance.nowSceneMoving)
                return;
            SoundManager.instance.BtnClick();
            UIManager.instance.ActSettingMenu(true);
        });
        newGameQUIT.onClick.AddListener(() =>
        {
            if (SceneController.instance.nowSceneMoving)
                return;
            SoundManager.instance.BtnClick();
            SoundManager.instance.ChickenCoco();
            SceneController.instance.MoveScene("Quit");
        });

        //////////////////////////////////////////////////////////////

        loadGameCONTINUE.onClick.AddListener(() =>
        {
            if (SceneController.instance.nowSceneMoving)
                return;
            GameManager.instance.LoadData();
            SoundManager.instance.BtnClick();
            SoundManager.instance.ChickenCoco();
            SceneController.instance.MoveScene(GameManager.instance.playData.stageName);
        });
        loadGameNEW.onClick.AddListener(() =>
        {
            SoundManager.instance.BtnClick();
            newGameCheck.SetActive(true);
        });
        loadGameRECORDS.onClick.AddListener(() =>
        {
            if (SceneController.instance.nowSceneMoving)
                return;
            SoundManager.instance.BtnClick();
            SoundManager.instance.ChickenCoco();
            SceneController.instance.MoveScene("Records");
        });
        newGameCheckYes.onClick.AddListener(() =>
        {
            if (SceneController.instance.nowSceneMoving)
                return;
            newGameCheck.SetActive(false);
            GameManager.instance.ClearData();
            SoundManager.instance.BtnClick();
            SoundManager.instance.ChickenCoco();
            SceneController.instance.MoveScene(GameManager.instance.playData.stageName);
        });
        newGameCheckNo.onClick.AddListener(() =>
        {
            SoundManager.instance.BtnClick();
            newGameCheck.SetActive(false);
        });
        newGameCheckExit.onClick.AddListener(() =>
        {
            SoundManager.instance.BtnClick();
            newGameCheck.SetActive(false);
        });
        loadGameTUTORIAL.onClick.AddListener(() =>
        {
            if (SceneController.instance.nowSceneMoving)
                return;
            SoundManager.instance.BtnClick();
            SoundManager.instance.ChickenCoco();
            SceneController.instance.MoveScene("Tutorial");
        });
        loadGameOPTION.onClick.AddListener(() =>
        {
            if (SceneController.instance.nowSceneMoving)
                return;
            SoundManager.instance.BtnClick();
            UIManager.instance.ActSettingMenu(true);
        });
        loadGameQUIT.onClick.AddListener(() =>
        {
            if (SceneController.instance.nowSceneMoving)
                return;
            SoundManager.instance.BtnClick();
            SoundManager.instance.ChickenCoco();
            SceneController.instance.MoveScene("Quit");
        });
        #endregion

        #region[English]
        newGameTUTORIAL_Eng.onClick.AddListener(() =>
        {
            if (SceneController.instance.nowSceneMoving)
                return;
            SoundManager.instance.BtnClick();
            SoundManager.instance.ChickenCoco();
            SceneController.instance.MoveScene("Tutorial");
        });
        newGameNEW_Eng.onClick.AddListener(() =>
        {
            if (SceneController.instance.nowSceneMoving)
                return;
            GameManager.instance.ClearData();
            SoundManager.instance.BtnClick();
            SoundManager.instance.ChickenCoco();
            SceneController.instance.MoveScene(GameManager.instance.playData.stageName);
        });
        newGameRECORDS_Eng.onClick.AddListener(() =>
        {
            if (SceneController.instance.nowSceneMoving)
                return;
            SoundManager.instance.BtnClick();
            SoundManager.instance.ChickenCoco();
            SceneController.instance.MoveScene("Records");
        });
        newGameOPTION_Eng.onClick.AddListener(() =>
        {
            if (SceneController.instance.nowSceneMoving)
                return;
            SoundManager.instance.BtnClick();
            UIManager.instance.ActSettingMenu(true);
        });
        newGameQUIT_Eng.onClick.AddListener(() =>
        {
            if (SceneController.instance.nowSceneMoving)
                return;
            SoundManager.instance.BtnClick();
            SoundManager.instance.ChickenCoco();
            SceneController.instance.MoveScene("Quit");
        });

        //////////////////////////////////////////////////////////////

        loadGameCONTINUE_Eng.onClick.AddListener(() =>
        {
            if (SceneController.instance.nowSceneMoving)
                return;
            GameManager.instance.LoadData();
            SoundManager.instance.BtnClick();
            SoundManager.instance.ChickenCoco();
            SceneController.instance.MoveScene(GameManager.instance.playData.stageName);
        });
        loadGameNEW_Eng.onClick.AddListener(() =>
        {
            SoundManager.instance.BtnClick();
            newGameCheck.SetActive(true);
        });
        loadGameRECORDS_Eng.onClick.AddListener(() =>
        {
            if (SceneController.instance.nowSceneMoving)
                return;
            SoundManager.instance.BtnClick();
            SoundManager.instance.ChickenCoco();
            SceneController.instance.MoveScene("Records");
        });
        loadGameTUTORIAL_Eng.onClick.AddListener(() =>
        {
            if (SceneController.instance.nowSceneMoving)
                return;
            SoundManager.instance.BtnClick();
            SoundManager.instance.ChickenCoco();
            SceneController.instance.MoveScene("Tutorial");
        });
        loadGameOPTION_Eng.onClick.AddListener(() =>
        {
            if (SceneController.instance.nowSceneMoving)
                return;
            SoundManager.instance.BtnClick();
            UIManager.instance.ActSettingMenu(true);
        });
        loadGameQUIT_Eng.onClick.AddListener(() =>
        {
            if (SceneController.instance.nowSceneMoving)
                return;
            SoundManager.instance.BtnClick();
            SoundManager.instance.ChickenCoco();
            SceneController.instance.MoveScene("Quit");
        });
        #endregion
    }
    #endregion

    #region[Update]
    private void Update()
    {
        for (int i = 0; i < languageData.Count; i++)
            if(languageData[i])
                languageData[i].SetActive(languageData[i].transform.name.Contains(GameManager.instance.playData.language.ToString()));

        if (pressAnyKey.activeSelf && Input.anyKey)
        {
            titleName.gameObject.SetActive(true);
            SoundManager.instance.ChickenBark(2);
            pressAnyKey.SetActive(false);
            if (GameManager.instance.playData.stageName != "Stage0101")
                loadGame.SetActive(true);
            else
                newGame.SetActive(true);
        }
    }
    #endregion

}