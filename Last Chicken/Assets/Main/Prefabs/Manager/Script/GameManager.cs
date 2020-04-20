using System;
using System.IO;
using TerrainEngine2D;
using UnityEngine;

public class GameManager : TerrainGenerator
{
    public static GameManager instance;

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    [System.NonSerialized] public PlayData playData;

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    [System.NonSerialized] public bool gamePause;

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    [Header("게임속도")]
    public float gameSpeed = 1;

    [Header("스테이지진행시간")]
    public float stageTime = 0;
    [System.NonSerialized] public bool playStage;

    [System.NonSerialized] public float countDown = 10;
    float clockFlag = 0;
    [System.NonSerialized] public float maxCountDown = 10;

    [System.NonSerialized] public bool gameOver = false;
    [System.NonSerialized] public float gameOverTime = 2;

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    [Header("플레이어 금화")]
    public int playerMoney;

    [HideInInspector]
    public string activeItem;
    [HideInInspector]
    public float activeItemCool;
    [HideInInspector]
    public int activeItemNum;

    [HideInInspector]
    public bool[] passiveSlotAct = new bool[5];
    [HideInInspector]
    public string[] passiveItem = new string[5];
    [HideInInspector]
    public int[] passiveItemNum = new int[5];
    [HideInInspector]
    public int passivePointer = 0;

    public static float getItemDelay = 0;

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region[Awake]
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Application.targetFrameRate = 300;
            LoadData();
            Screen.SetResolution(playData.ScreenWidth, playData.ScreenHeight, playData.fullScreen);
        }
    }
    #endregion

    #region[Start]
    private void Start()
    {

    }
    #endregion

    #region[Update]
    public void Update()
    {
        if (gamePause)
            Time.timeScale = 0;
        else
            Time.timeScale = gameSpeed;

        getItemDelay -= Time.deltaTime;

        if (playStage)
            stageTime += Time.deltaTime;

        SetValue();
        GameOverCheck();
        GameOver();
        StageClear();
    }
    #endregion

    #region[OnApplicationQuit]
    private void OnApplicationQuit()
    {
        File.WriteAllText(Application.dataPath + "/Resources/PlayData.json", JsonUtility.ToJson(playData, true));
    }
    #endregion

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region[데이터 클리어]
    public void ClearData()
    {
        int screenWidth = playData.ScreenWidth;
        int screenHeight = playData.ScreenHeight;
        float seVolume = playData.SE_Volume;
        float bgmVolume = playData.BGM_Volume;
        bool fullScreen = playData.fullScreen;
        bool firstGame = playData.firstGame;
        playData = new PlayData();
        playData.ScreenWidth = screenWidth;
        playData.ScreenHeight = screenHeight;
        playData.SE_Volume = seVolume;
        playData.BGM_Volume = bgmVolume;
        playData.fullScreen = fullScreen;
        playData.firstGame = firstGame;

        ///////////////////////////////////////////////////////////////////////////////////////////////////

        activeItem = playData.playerActiveItem;
        activeItemNum = playData.playerActiveItemNum;
        activeItemCool = 1000;

        ///////////////////////////////////////////////////////////////////////////////////////////////////

        for (int i = 0; i < 5; i++)
        {
            passiveSlotAct[i] = playData.playerPassiveSlotAct[i];
            passiveItem[i] = playData.playerPassiveItem[i];
            passiveItemNum[i] = playData.playerPassiveItemNum[i];
        }
        passivePointer = playData.playerPassivePointer;

        ///////////////////////////////////////////////////////////////////////////////////////////////////

        playerMoney = playData.playerMoney;

        ///////////////////////////////////////////////////////////////////////////////////////////////////

        BuffManager.loadEnd = false;

        ///////////////////////////////////////////////////////////////////////////////////////////////////

        File.WriteAllText(Application.dataPath + "/Resources/PlayData.json", JsonUtility.ToJson(playData, true));
    }

    #endregion

    #region[데이터 로드]
    public void LoadData()
    {
        playData = new PlayData();
        string data = File.ReadAllText(Application.dataPath + "/Resources/PlayData.json");
        if (data != null)
            playData = JsonUtility.FromJson<PlayData>(data.ToString());

        activeItem = playData.playerActiveItem;
        activeItemNum = playData.playerActiveItemNum;
        activeItemCool = 1000;

        ///////////////////////////////////////////////////////////////////////////////////////////////////

        for (int i = 0; i < 5; i++)
        {
            passiveSlotAct[i] = playData.playerPassiveSlotAct[i];
            passiveItem[i] = playData.playerPassiveItem[i];
            passiveItemNum[i] = playData.playerPassiveItemNum[i];
        }
        passivePointer = playData.playerPassivePointer;

        ///////////////////////////////////////////////////////////////////////////////////////////////////

        playerMoney = playData.playerMoney;

        ///////////////////////////////////////////////////////////////////////////////////////////////////

        BuffManager.loadEnd = false;

    }
    #endregion

    #region[데이터 저장]
    public void SaveData()
    {
        if (Player.instance)
        {
            playData.playerNowHp = Player.instance.nowHp;
            playData.playerMaxHp = Player.instance.maxHp;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////

        playData.playerActiveItem = activeItem;
        playData.playerActiveItemNum = activeItemNum;

        ///////////////////////////////////////////////////////////////////////////////////////////////////

        for (int i = 0; i < 5; i++)
        {
            playData.playerPassiveSlotAct[i] = passiveSlotAct[i];
            playData.playerPassiveItem[i] = passiveItem[i];
            playData.playerPassiveItemNum[i] = passiveItemNum[i];
        }
        playData.playerPassivePointer = passivePointer;

        ///////////////////////////////////////////////////////////////////////////////////////////////////

        playData.playerMoney = playerMoney;

        ///////////////////////////////////////////////////////////////////////////////////////////////////

        if (ShopScript.instance)
        {
            playData.shopVIP = ShopScript.instance.shopVIP;
            playData.randomDice = ShopScript.instance.randomDice;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////

        if (BuffManager.instance)
        {
            for (int i = 0; i < BuffManager.buffName.Length; i++)
            {
                playData.playerBuffItemHas[i] = BuffManager.instance.nowBuffList[BuffManager.buffName[i]].hasBuff;
                playData.playerBuffItemNum[i] = BuffManager.instance.nowBuffList[BuffManager.buffName[i]].hasNum;
                playData.playerBuffItemTime[i] = BuffManager.instance.nowBuffList[BuffManager.buffName[i]].time;
            }
        }

        File.WriteAllText(Application.dataPath + "/Resources/PlayData.json", JsonUtility.ToJson(playData, true));
    }
    #endregion

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region[수치 설정]
    public void SetValue()
    {
        if (activeItemNum > 99)
            activeItemNum = 99;
        for(int i = 0; i < 5; i++)
        if (passiveItemNum[i] > 99)
                passiveItemNum[i] = 99;

        if (playerMoney > 999999)
            playerMoney = 999999;

        activeItemCool += Time.deltaTime;
    }
    #endregion

    #region[인게임인지 검사]
    public bool InGame()
    {
        return !SceneController.instance.nowScene.Equals("Title") && !SceneController.instance.nowScene.Equals("Prologue");
    }
    #endregion

    #region[게임오버조건 체크]
    void GameOverCheck()
    {
        if (gameOver)
            return;
     
        if (Player.instance)
        {
            #region[닭을 놓쳐서 게임오버가 되는경우]
            if (!SceneController.instance.nowScene.Equals("Tutorial"))
            {
                if (Player.instance.getChicken)
                {
                    countDown = maxCountDown;
                    clockFlag = 1;
                }
                else
                {
                    countDown -= Time.deltaTime;
                    clockFlag -= Time.deltaTime;

                    if (clockFlag < 0)
                    {
                        clockFlag = 1;
                        SoundManager.instance.Ticking();
                    }

                    if (countDown < 0)
                    {
                        SetGameOver();
                        return;
                    }
                }
            }
            #endregion

            #region[체력이 적어서 게임오버가 되는경우]
            if (Player.instance.nowHp <= 0)
            {
                SetGameOver();
                return;
            }
            #endregion
        }
    }
    #endregion

    #region[게임오버]

    void SetGameOver()
    {
        gameOver = true;
        gameOverTime = 2;
    }

    void GameOver()
    {
        if (!gameOver)
            return;
        gameOverTime -= Time.deltaTime;
        if (Player.instance)
            Player.instance.gameObject.SetActive(false);

        if (Chicken.instance)
            Chicken.instance.gameObject.SetActive(false);

        if (UIManager.instance)
            UIManager.instance.gameOver.SetActive(true);

        SoundManager.instance.StopBGM_Sound();

        ClearData();

        if(gameOverTime < 0)
        {
            gameOver = false;
            gameOverTime = 2;
            SceneController.instance.MoveScene("Title");
        }
    }
    #endregion

    #region[스테이지 클리어]
    public void StageClear()
    {
        if (gameOver || !Player.instance)
            return;

        if (Player.instance.transform.position.y < -5)
        {
            switch(SceneController.instance.nowScene)
            {
                case "Tutorial":
                    playData.firstGame = false;
                    SceneController.instance.MoveScene("Title");
                    break;
                case "Stage0101":
                    playData.stageName = "ShopMap0101";
                    SaveData();
                    SceneController.instance.MoveScene(playData.stageName);
                    break;
                case "ShopMap0101":
                    playData.stageName = "Stage0102";
                    SaveData();
                    SceneController.instance.MoveScene(playData.stageName);
                    break;
                case "Stage0102":
                    playData.stageName = "ShopMap0102";
                    SaveData();
                    SceneController.instance.MoveScene(playData.stageName);
                    break;
                case "ShopMap0102":
                    playData.stageName = "Stage0103";
                    SaveData();
                    SceneController.instance.MoveScene(playData.stageName);
                    break;
                case "Stage0103":
                    playData.stageName = "ShopMap0103";
                    SaveData();
                    SceneController.instance.MoveScene(playData.stageName);
                    break;
                case "ShopMap0103":
                    playData.stageName = "Stage0103";
                    SaveData();
                    SceneController.instance.MoveScene(playData.stageName);
                    break;
            }
        }
    }
    #endregion

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    
}