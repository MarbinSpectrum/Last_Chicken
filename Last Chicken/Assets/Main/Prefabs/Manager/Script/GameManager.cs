using System;
using System.IO;
using TerrainEngine2D;
using UnityEngine;
using System.Collections.Generic;

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
        ItemRecord();

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
        //삭제하지않을데이터들
        int screenWidth = playData.ScreenWidth;
        int screenHeight = playData.ScreenHeight;
        float seVolume = playData.SE_Volume;
        float bgmVolume = playData.BGM_Volume;
        bool fullScreen = playData.fullScreen;
        bool firstGame = playData.firstGame;
        PlayData.Language language = playData.language;
        List<bool> monsterRecordTemp = new List<bool>();
        for (int i = 0; i < playData.monsterRecords.Length; i++)
            monsterRecordTemp.Add(playData.monsterRecords[i]);
        List<bool> itemRecordTemp = new List<bool>();
        for (int i = 0; i < playData.itemRecords.Length; i++)
            itemRecordTemp.Add(playData.itemRecords[i]);

        //데이터 삭제
        playData = new PlayData();

        //삭제하지않을데이터입력
        playData.ScreenWidth = screenWidth;
        playData.ScreenHeight = screenHeight;
        playData.SE_Volume = seVolume;
        playData.BGM_Volume = bgmVolume;
        playData.fullScreen = fullScreen;
        playData.firstGame = firstGame;
        playData.language = language;
        for (int i = 0; i < playData.monsterRecords.Length; i++)
            playData.monsterRecords[i] = monsterRecordTemp[i];
        for (int i = 0; i < playData.itemRecords.Length; i++)
            playData.itemRecords[i] = itemRecordTemp[i];

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

    #region[아이템 기록]
    public void ItemRecord()
    {
        for(int i = 0; i < itemSlot.Length; i++)
            if(slotAct[i])
            {
                int itemNum = ItemManager.FindData(itemSlot[i]);
                if(itemNum != -1)
                    playData.itemRecords[itemNum] = true;
            }
    }
    #endregion

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
        return !SceneController.instance.nowScene.Equals("Records") && !SceneController.instance.nowScene.Equals("FirstStart") && !SceneController.instance.nowScene.Equals("Title") && !SceneController.instance.nowScene.Equals("Prologue") && !SceneController.instance.nowScene.Equals("Demo");
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
                        bool charmFlag = ItemManager.instance.HasItemCheck("Charm");
                        if(!charmFlag)
                        {
                            if (gameOverdelayTime < 3)
                            {
                                if (Chicken.instance)
                                {
                                    Chicken.instance.deleteChickenAni.SetInteger("State", 0);
                                    Chicken.instance.deleteChickenImg.SetActive(true);
                                }
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

                        #region[부적으로 닭 부활]
                        else
                        {
                            if (gameOverdelayTime < 5)
                            {
                                if (Chicken.instance)
                                {
                                    Chicken.instance.spriteRenderer.enabled = false;
                                    Chicken.instance.deleteChickenAni.SetInteger("State", 0);
                                    Chicken.instance.deleteChickenImg.SetActive(true);
                                }
                                if (Player.instance)
                                {
                                    Player.instance.canControl = false;
                                    Player.instance.invincibility = true;
                                }
                                SoundManager.instance.StopBGM_Sound();
                            }
                            else if (gameOverdelayTime < 8)
                            {
                                if (Chicken.instance)
                                {       
                                    Chicken.instance.gameObject.SetActive(true);
                                    Chicken.instance.deleteChickenAni.SetInteger("State", 1);
                                    Chicken.instance.transform.position = new Vector3(Player.instance.transform.position.x, Player.instance.transform.position.y + 2, Chicken.instance.transform.position.z);
                                }
                            }
                            else
                            {
                                ItemManager.instance.CostItem("Charm");
                                if (Player.instance)
                                {
                                    Player.instance.invincibility = false;
                                    Player.instance.canControl = true;
                                }
                                SoundManager.instance.PlayBGM_Sound(true);
                                gameOverdelayTime = 0;
                                countDown = 5;
                            }
                        }
                        #endregion
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
                        Player.instance.stunTime = 0.7f;
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

        string stageName = SceneController.instance.nowScene;
        if(stageName.Equals("IglooMap"))
            stageName = "Stage0201";
        else
        {
            stageName = stageName.Substring(0, stageName.Length - 2);
            stageName += "01";
        }
        ClearData();
        playData.stageName = stageName;
        File.WriteAllText(Application.dataPath + "/Resources/PlayData.json", JsonUtility.ToJson(playData, true));
        if (gameOverTime < 0)
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
        int size;
        string temp;
        int stageNum;
        int stageSubNum;
        if (gameOver || !Player.instance)
            return;
        switch (SceneController.instance.nowScene)
        {
            case "Tutorial":
                if (Player.instance.transform.position.x <= 230 || Player.instance.transform.position.y >= 70)
                    return;
                playData.firstGame = false;
                SceneController.instance.MoveScene("Stage0101");
                break;
            case "Stage0101":
            case "Stage0102":
            case "Stage0103":
            case "Stage0201":
            case "Stage0202":
            case "Stage0203":
                if (Player.instance.transform.position.y >= -5)
                    return;
                size = SceneController.instance.nowScene.Length;
                temp = "";
                for (int i = 0; i < 4; i++)
                    temp += SceneController.instance.nowScene[size - 4 + i];
                stageNum = temp[1] - '0';
                stageSubNum = temp[3] - '0';
                stageSubNum++;
                if(stageSubNum > 3)
                {
                    stageSubNum = 1;
                    stageNum++;
                }
                temp = "";
                temp += "0";
                temp += stageNum.ToString();
                temp += "0";
                temp += stageSubNum.ToString();
                if(stageNum >= 3)
                    playData.stageName = "Demo";
                else
                    playData.stageName = "Stage" + temp;
                SceneController.instance.MoveScene(playData.stageName);
                SaveData();
                break;
        }
    }
    #endregion

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    
}