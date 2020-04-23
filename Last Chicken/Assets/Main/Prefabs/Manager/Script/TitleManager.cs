using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    
    private GameObject pressAnyKey;
    private GameObject newGame;
    Button newGameTUTORIAL;
    Button newGameNEW;
    Button newGameOPTION;
    Button newGameQUIT;

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    private GameObject loadGame;
    Button loadGameCONTINUE;
    Button loadGameNEW;
    Button loadGameTUTORIAL;
    Button loadGameOPTION;
    Button loadGameQUIT;

    private GameObject newGameCheck;
    Button newGameCheckExit;
    Button newGameCheckYes;
    Button newGameCheckNo;

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

        newGameTUTORIAL = newGame.transform.Find("Tutorial").GetComponent<Button>();
        newGameNEW = newGame.transform.Find("New").GetComponent<Button>();
        newGameOPTION = newGame.transform.Find("Option").GetComponent<Button>();
        newGameQUIT = newGame.transform.Find("Quit").GetComponent<Button>();

        loadGameCONTINUE = loadGame.transform.Find("Continue").GetComponent<Button>();
        loadGameNEW = loadGame.transform.Find("New").GetComponent<Button>();
        loadGameTUTORIAL = loadGame.transform.Find("Tutorial").GetComponent<Button>();
        loadGameOPTION = loadGame.transform.Find("Option").GetComponent<Button>();
        loadGameQUIT = loadGame.transform.Find("Quit").GetComponent<Button>();

        newGameCheck = loadGame.transform.Find("NewGameCheck").gameObject;
        newGameCheckYes = newGameCheck.transform.Find("Yes").GetComponent<Button>();
        newGameCheckNo = newGameCheck.transform.Find("No").GetComponent<Button>();
        newGameCheckExit = newGameCheck.transform.Find("Exit").GetComponent<Button>();

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

    }
    #endregion

    #region[Update]
    private void Update()
    {
        if (pressAnyKey.activeSelf && Input.anyKey)
        {
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