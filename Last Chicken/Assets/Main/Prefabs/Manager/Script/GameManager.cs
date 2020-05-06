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
    [System.NonSerialized] public float gameOverdelayTime;
    [System.NonSerialized] public float gameOverTime = 2;

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    [Header("플레이어 금화")]
    public int playerMoney;

    [HideInInspector]
    public int selectNum = 1;

    public string[] itemSlot = new string[6];
    [HideInInspector]
    public bool[] slotAct = new bool[6];
    [HideInInspector]
    public int[] itemNum = new int[6];
    [HideInInspector]
    public float[] itemCool = new float[6];

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

        for (int i = 0; i < 6; i++)
        {
            slotAct[i] = playData.slotAct[i];
            itemSlot[i] = playData.itemSlot[i];
            itemNum[i] = playData.itemNum[i];
            itemCool[i] = 1000;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////

        playerMoney = playData.playerMoney;

        ///////////////////////////////////////////////////////////////////////////////////////////////////

        playData.pickLevel = 0;

        ///////////////////////////////////////////////////////////////////////////////////////////////////

        BuffManager.loadEnd = false;

        ///////////////////////////////////////////////////////////////////////////////////////////////////

        playData.shopVIP = false;
        playData.randomDice = 100;

        ///////////////////////////////////////////////////////////////////////////////////////////////////

        playData.seed = UnityEngine.Random.Range(0, 10000);

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


        ///////////////////////////////////////////////////////////////////////////////////////////////////

        for (int i = 0; i < 6; i++)
        {
            slotAct[i] = playData.slotAct[i];
            itemSlot[i] = playData.itemSlot[i];
            itemNum[i] = playData.itemNum[i];
            itemCool[i] = 1000;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////

        playerMoney = playData.playerMoney;

        ///////////////////////////////////////////////////////////////////////////////////////////////////

        BuffManager.loadEnd = false;

        ///////////////////////////////////////////////////////////////////////////////////////////////////

        UnityEngine.Random.InitState(playData.seed);

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

        for (int i = 0; i < 6; i++)
        {
            playData.slotAct[i] = slotAct[i];
            playData.itemSlot[i] = itemSlot[i];
            playData.itemNum[i] = itemNum[i];
        }

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
        for (int i = 0; i < 6; i++)
        {
            if (itemNum[i] > 99)
                itemNum[i] = 99;

            itemCool[i] += Time.deltaTime;
        }

        if (playerMoney > 999999)
            playerMoney = 999999;

        UnityEngine.Random.InitState(playData.seed);

    }
    #endregion

    #region[인게임인지 검사]
    public bool InGame()
    {
        return !SceneController.instance.nowScene.Equals("Title") && !SceneController.instance.nowScene.Equals("Prologue") && !SceneController.instance.nowScene.Equals("Demo");
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
            if (!SceneController.instance.nowScene.Equals("Tutorial") && Player.instance.nowHp > 0)
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

                    if (clockFlag < 0 && countDown >= 0)
                    {
                        clockFlag = 1;
                        SoundManager.instance.Ticking();
                    }

                    if (countDown < 0)
                    {
                        gameOverdelayTime += Time.deltaTime;
                        if (gameOverdelayTime < 3)
                        {
                            if (Chicken.instance)
                                Chicken.instance.deleteChickenImg.SetActive(true);
                            if (Player.instance)
                                Player.instance.canControl = false;
                            SoundManager.instance.StopBGM_Sound();
                        }
                        else
                        {
                            gameOverdelayTime = 0;
                            SetGameOver();
                            return;
                        }
                    }
                }
            }
            #endregion

            #region[체력이 적어서 게임오버가 되는경우]
            if (Player.instance.nowHp <= 0)
            {
                gameOverdelayTime += Time.deltaTime;
                bool medkitFlag = (Player.instance.nowHp > -1000 && ItemManager.instance.HasItemCheck("Medkit"));

                if (gameOverdelayTime < (medkitFlag ? 3 : 3))
                {
                    Player.instance.canControl = false;
                    Player.instance.notDamage = true;
                    SoundManager.instance.PlayBGM_Sound(false);

                    #region[구급상자로 소생]
                    if (gameOverdelayTime > 2.75f && medkitFlag)
                    {
                        ItemManager.instance.CostItem("Medkit");
                        Player.instance.nowHp = Player.instance.maxHp;
                        Player.instance.canControl = true;
                        Player.instance.notDamage = false;
                        Player.instance.stunTime = 80;
                        Player.instance.animator.SetBool("Dead", false);
                        EffectManager.instance.HearthEffect();
                        EffectManager.instance.DamageEffect();
                        EffectManager.instance.NowItem(ItemManager.instance.itemData[ItemManager.FindData("Medkit")].itemImg);
                        SoundManager.instance.PlayBGM_Sound(true);
                        gameOverdelayTime = 0;
                    }
                    #endregion
                }
                else
                {
                    gameOverdelayTime = 0;
                    SetGameOver();
                    return;
                }
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
        switch (SceneController.instance.nowScene)
        {
            case "Tutorial":
                if (Player.instance.transform.position.x <= 230 || Player.instance.transform.position.y >= 70)
                    return;
                playData.firstGame = false;
                SceneController.instance.MoveScene("Title");
                break;
            case "Stage0101":
            case "Stage0102":
            case "Stage0103":
                if (Player.instance.transform.position.y >= -5)
                    return;
                int size = SceneController.instance.nowScene.Length;
                string temp = "";
                for (int i = 0; i < 4; i++)
                    temp += SceneController.instance.nowScene[size - 4 + i];

                string nextArea = "";

                if (ItemManager.instance.HasItemCheck("Hammer"))
                    nextArea = "SmithyMap";

                if(nextArea.Equals(""))
                {
                    int r = UnityEngine.Random.Range(0, 100);
                    if (0 <= r && r < 10)
                        nextArea = "SmithyMap";
                    else if (10 <= r && r < 35)
                        nextArea = "FountainMap";
                    else if (35 <= r && r < 60)
                        nextArea = "AltarMap";
                    else if (60 <= r && r < 100)
                        nextArea = "ShopMap";
                }

                playData.stageName = nextArea + temp;
                SceneController.instance.MoveScene(playData.stageName);
                SaveData();
                break;
            case "ShopMap0101":
            case "SmithyMap0101":
            case "AltarMap0101":
            case "FountainMap0101":
                if (Player.instance.transform.position.y >= -5)
                    return;
                playData.stageName = "Stage0102";
                SceneController.instance.MoveScene(playData.stageName);
                SaveData();
                break;
            case "ShopMap0102":
            case "SmithyMap0102":
            case "AltarMap0102":
            case "FountainMap0102":
                if (Player.instance.transform.position.y >= -5)
                    return;
                playData.stageName = "Stage0103";
                SceneController.instance.MoveScene(playData.stageName);
                SaveData();
                break;
            case "ShopMap0103":
            case "SmithyMap0103":
            case "AltarMap0103":
            case "FountainMap0103":
                if (Player.instance.transform.position.y >= -5)
                    return;
                playData.stageName = "Demo";
                SceneController.instance.MoveScene(playData.stageName);
                SaveData();
                break;
        }
    }
    #endregion

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    
}