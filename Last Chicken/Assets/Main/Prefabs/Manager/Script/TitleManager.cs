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


    int selectNum = 0;

    GameObject TUTORIAL_Select;
    GameObject CONTINUE_Select;
    GameObject NEWGAME_Select;
    GameObject RECORDS_Select;
    GameObject OPTION_Select;
    GameObject QUIT_Select;
    GameObject NEWGAME_YES_Select;
    GameObject NEWGAME_NO_Select;

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

        NEWGAME_YES_Select = newGameCheckYes.transform.Find("Select").gameObject;
        NEWGAME_NO_Select = newGameCheckNo.transform.Find("Select").gameObject;

        #region[한국어]
        newGameTUTORIAL.onClick.AddListener(() =>
        {
            Tutorial();
        });
        newGameNEW.onClick.AddListener(() =>
        {
            NewGame(false);
        });
        newGameRECORDS.onClick.AddListener(() =>
        {
            Record();
        });
        newGameOPTION.onClick.AddListener(() =>
        {
            SettingMenu();
        });
        newGameQUIT.onClick.AddListener(() =>
        {
            GameQuit();
        });

        //////////////////////////////////////////////////////////////

        loadGameCONTINUE.onClick.AddListener(() =>
        {
            Contine();
        });
        loadGameNEW.onClick.AddListener(() =>
        {
            NewGame(true);
        });
        loadGameRECORDS.onClick.AddListener(() =>
        {
            Record();
        });
        loadGameTUTORIAL.onClick.AddListener(() =>
        {
            Tutorial();
        });
        loadGameOPTION.onClick.AddListener(() =>
        {
            SettingMenu();
        });
        loadGameQUIT.onClick.AddListener(() =>
        {
            GameQuit();
        });
        #endregion

        #region[English]
        newGameTUTORIAL_Eng.onClick.AddListener(() =>
        {
            Tutorial();
        });
        newGameNEW_Eng.onClick.AddListener(() =>
        {
            NewGame(false);
        });
        newGameRECORDS_Eng.onClick.AddListener(() =>
        {
            Record();
        });
        newGameOPTION_Eng.onClick.AddListener(() =>
        {
            SettingMenu();
        });
        newGameQUIT_Eng.onClick.AddListener(() =>
        {
            GameQuit();
        });

        //////////////////////////////////////////////////////////////

        loadGameCONTINUE_Eng.onClick.AddListener(() =>
        {
            Contine();
        });
        loadGameNEW_Eng.onClick.AddListener(() =>
        {
            NewGame(true);
        });
        loadGameRECORDS_Eng.onClick.AddListener(() =>
        {
            Record();
        });
        loadGameTUTORIAL_Eng.onClick.AddListener(() =>
        {
            Tutorial();
        });
        loadGameOPTION_Eng.onClick.AddListener(() =>
        {
            SettingMenu();
        });
        loadGameQUIT_Eng.onClick.AddListener(() =>
        {
            GameQuit();
        });
        #endregion

        #region[게임기록 새로 작성 여부]
        newGameCheckYes.onClick.AddListener(() =>
        {
            NewGameData();
        });
        newGameCheckNo.onClick.AddListener(() =>
        {
            NewGameWindowExit();
        });
        newGameCheckExit.onClick.AddListener(() =>
        {
            NewGameWindowExit();
        });
        #endregion

    }
    #endregion

    #region[튜토리얼로]
    public void Tutorial()
    {
        if (SceneController.instance.nowSceneMoving)
            return;
        SoundManager.instance.BtnClick();
        SoundManager.instance.ChickenCoco();
        SceneController.instance.MoveScene("Tutorial");
    }
    #endregion

    #region[새게임하기]
    public void NewGame(bool checkNewGame)
    {
        if(checkNewGame && !newGameCheck.activeSelf)
        {
            selectNum = 0;
            SoundManager.instance.BtnClick();
            newGameCheck.SetActive(true);
        }
        else
        {
            if (SceneController.instance.nowSceneMoving)
                return;
            GameManager.instance.ClearData();
            SoundManager.instance.BtnClick();
            SoundManager.instance.ChickenCoco();
            SceneController.instance.MoveScene(GameManager.instance.playData.stageName);
        }
    }
    #endregion

    #region[탐험일지]
    public void Record()
    {
        if (SceneController.instance.nowSceneMoving)
            return;
        SoundManager.instance.BtnClick();
        SoundManager.instance.ChickenCoco();
        SceneController.instance.MoveScene("Records");
    }
    #endregion

    #region[이어하기]
    public void Contine()
    {
        if (SceneController.instance.nowSceneMoving)
            return;
        GameManager.instance.LoadData();
        SoundManager.instance.BtnClick();
        SoundManager.instance.ChickenCoco();
        SceneController.instance.MoveScene(GameManager.instance.playData.stageName);
    }
    #endregion

    #region[설정메뉴]
    public void SettingMenu()
    {
        if (SceneController.instance.nowSceneMoving)
            return;
        SoundManager.instance.BtnClick();
        UIManager.instance.ActSettingMenu(true);
    }
    #endregion

    #region[게임종료]
    public void GameQuit()
    {
        if (SceneController.instance.nowSceneMoving)
            return;
        SoundManager.instance.BtnClick();
        SoundManager.instance.ChickenCoco();
        SceneController.instance.MoveScene("Quit");
    }
    #endregion

    #region[게임기록새로작성]
    public void NewGameData()
    {
        if (SceneController.instance.nowSceneMoving)
            return;
        newGameCheck.SetActive(false);
        GameManager.instance.ClearData();
        SoundManager.instance.BtnClick();
        SoundManager.instance.ChickenCoco();
        SceneController.instance.MoveScene(GameManager.instance.playData.stageName);
    }
    public void NewGameWindowExit()
    {
        if (!newGameCheck.activeSelf)
            return;
        SoundManager.instance.BtnClick();
        newGameCheck.SetActive(false);
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
            {
                loadGame.SetActive(true);
                selectNum = 0;
            }
            else
            {
                newGame.SetActive(true);
                selectNum = 1;
            }
        }

        #region[게임새로하기 체크]
        else if (newGameCheck.activeSelf)
        {
            CONTINUE_Select.SetActive(false);
            NEWGAME_Select.SetActive(false);
            TUTORIAL_Select.SetActive(false);
            RECORDS_Select.SetActive(false);
            OPTION_Select.SetActive(false);
            QUIT_Select.SetActive(false);
            NEWGAME_YES_Select.SetActive(false);
            NEWGAME_NO_Select.SetActive(false);

            if (KeyManager.nowController == GameController.KeyBoard)
                return;

            if (SceneController.instance.nowSceneMoving)
                return;

            if (selectNum == 0 && KeyManager.GetKeyDown(KeyManager.instance.gamePad[GameKeyType.SystemRight]))
            {
                selectNum++;
                selectNum %= 2;
            }
            else if (selectNum == 1 && KeyManager.GetKeyDown(KeyManager.instance.gamePad[GameKeyType.SystemLeft]))
            {
                selectNum++;
                selectNum %= 2;
            }

            if (selectNum == 0)
            {
                NEWGAME_YES_Select.SetActive(true);
                if (KeyManager.GetKeyDown(KeyManager.instance.gamePad[GameKeyType.Select]))
                    NewGameData();
            }
            else if (selectNum == 1)
            {
                NEWGAME_NO_Select.SetActive(true);
                if (KeyManager.GetKeyDown(KeyManager.instance.gamePad[GameKeyType.Select]))
                    NewGameWindowExit();
            }

        }
        #endregion

        #region[새게임 메뉴들]
        else if (newGame.activeSelf)
        {
            if(TUTORIAL_Select == null)
                TUTORIAL_Select = newGame.transform.Find("TutorialSelect").gameObject;
            if (NEWGAME_Select == null)
                NEWGAME_Select = newGame.transform.Find("NewGameSelect").gameObject;
            if (RECORDS_Select == null)
                RECORDS_Select = newGame.transform.Find("RecordSelect").gameObject;
            if (OPTION_Select == null)
                OPTION_Select = newGame.transform.Find("SettingSelect").gameObject;
            if (QUIT_Select == null)
                QUIT_Select = newGame.transform.Find("QUITSelect").gameObject;

            TUTORIAL_Select.SetActive(false);
            NEWGAME_Select.SetActive(false);
            RECORDS_Select.SetActive(false);
            OPTION_Select.SetActive(false);
            QUIT_Select.SetActive(false);

            if (KeyManager.nowController == GameController.KeyBoard)
                return;

            if (UIManager.instance.settingMenu.activeSelf)
                return;

            if (newGameCheck.activeSelf)
                return;

            if (SceneController.instance.nowSceneMoving)
                return;

            if (KeyManager.GetKeyDown(KeyManager.instance.gamePad[GameKeyType.SystemDown]))
            {
                selectNum++;
                selectNum = selectNum > 4 ? 0 : selectNum;
            }
            else if (KeyManager.GetKeyDown(KeyManager.instance.gamePad[GameKeyType.SystemUp]))
            {
                selectNum--;
                selectNum = selectNum < 0 ? 4 : selectNum;
            }

            if (selectNum == 0)
            {
                TUTORIAL_Select.SetActive(true);
                if (KeyManager.GetKeyDown(KeyManager.instance.gamePad[GameKeyType.Select]))
                    Tutorial();
            }
            else if (selectNum == 1)
            {
                NEWGAME_Select.SetActive(true);
                if (KeyManager.GetKeyDown(KeyManager.instance.gamePad[GameKeyType.Select]))
                    NewGame(false);
            }
            else if (selectNum == 2)
            {
                RECORDS_Select.SetActive(true);
                if (KeyManager.GetKeyDown(KeyManager.instance.gamePad[GameKeyType.Select]))
                    Record();
            }
            else if (selectNum == 3)
            {
                OPTION_Select.SetActive(true);
                if (KeyManager.GetKeyDown(KeyManager.instance.gamePad[GameKeyType.Select]))
                    SettingMenu();
            }
            else if (selectNum == 4)
            {
                QUIT_Select.SetActive(true);
                if (KeyManager.GetKeyDown(KeyManager.instance.gamePad[GameKeyType.Select]))
                    GameQuit();
            }
        }
        #endregion

        #region[로드게임 메뉴들]
        else if (loadGame.activeSelf)
        {
            if (CONTINUE_Select == null)
                CONTINUE_Select = loadGame.transform.Find("ContinueSelect").gameObject;
            if (NEWGAME_Select == null)
                NEWGAME_Select = loadGame.transform.Find("NewGameSelect").gameObject;
            if (TUTORIAL_Select == null)
                TUTORIAL_Select = loadGame.transform.Find("TutorialSelect").gameObject;
            if (RECORDS_Select == null)
                RECORDS_Select = loadGame.transform.Find("RecordSelect").gameObject;
            if (OPTION_Select == null)
                OPTION_Select = loadGame.transform.Find("SettingSelect").gameObject;
            if (QUIT_Select == null)
                QUIT_Select = loadGame.transform.Find("QUITSelect").gameObject;

            CONTINUE_Select.SetActive(false);
            NEWGAME_Select.SetActive(false);
            TUTORIAL_Select.SetActive(false);
            RECORDS_Select.SetActive(false);
            OPTION_Select.SetActive(false);
            QUIT_Select.SetActive(false);

            if (KeyManager.nowController == GameController.KeyBoard)
                return;

            if (UIManager.instance.settingMenu.activeSelf)
                return;

            if (newGameCheck.activeSelf)
                return;

            if (SceneController.instance.nowSceneMoving)
                return;

            if (KeyManager.GetKeyDown(KeyManager.instance.gamePad[GameKeyType.SystemDown]))
            {
                selectNum++;
                selectNum = selectNum > 5 ? 0 : selectNum;
            }
            else if (KeyManager.GetKeyDown(KeyManager.instance.gamePad[GameKeyType.SystemUp]))
            {
                selectNum--;
                selectNum = selectNum < 0 ? 5 : selectNum;
            }

            if (selectNum == 0)
            {
                CONTINUE_Select.SetActive(true);
                if (KeyManager.GetKeyDown(KeyManager.instance.gamePad[GameKeyType.Select]))
                    Contine();
            }
            else if (selectNum == 1)
            {
                NEWGAME_Select.SetActive(true);
                if (KeyManager.GetKeyDown(KeyManager.instance.gamePad[GameKeyType.Select]))
                    NewGame(true);
            }
            else if (selectNum == 2)
            {
                TUTORIAL_Select.SetActive(true);
                if (KeyManager.GetKeyDown(KeyManager.instance.gamePad[GameKeyType.Select]))
                    Tutorial();
            }
            else if (selectNum == 3)
            {
                RECORDS_Select.SetActive(true);
                if (KeyManager.GetKeyDown(KeyManager.instance.gamePad[GameKeyType.Select]))
                    Record();
            }
            else if (selectNum == 4)
            {
                OPTION_Select.SetActive(true);
                if (KeyManager.GetKeyDown(KeyManager.instance.gamePad[GameKeyType.Select]))
                    SettingMenu();
            }
            else if (selectNum == 5)
            {
                QUIT_Select.SetActive(true);
                if (KeyManager.GetKeyDown(KeyManager.instance.gamePad[GameKeyType.Select]))
                    GameQuit();
            }

        }
        #endregion


    }
    #endregion

}