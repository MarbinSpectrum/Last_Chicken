using System.Collections;
using UnityEngine;
using TerrainEngine2D;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    [System.NonSerialized] public SpriteRenderer stageBackGround;

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    [System.NonSerialized] public string nowScene;
    [System.NonSerialized] public bool nowSceneMoving = false;

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    Animator fadeAnimator;

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region[Awake]
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            SceneManager.activeSceneChanged += OnSceneChanged;

            fadeAnimator = transform.Find("Canvas").Find("Fade").GetComponent<Animator>();
        }
    }
    #endregion

    #region[Start]
    void Start()
    {
        nowScene = SceneManager.GetActiveScene().name;
        SceneSetting(nowScene, true);
    }
    #endregion

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region[씬 이동완료시 작동되는 함수]
    void OnSceneChanged(Scene previousScene, Scene changedScene)
    {
        nowSceneMoving = false;
        nowScene = changedScene.name;
        SceneSetting(nowScene);
    }
    #endregion

    #region[씬 세팅]
    void SceneSetting(string s, bool startScene = false)
    {
        fadeAnimator.SetTrigger("FadeIn");
        if (StageBackGround.instance)
            stageBackGround = StageBackGround.instance.spriteRenderer;

        if (!startScene)
        {
            EffectManager.instance.gameObject.SetActive(true);
            ObjectManager.instance.gameObject.SetActive(true);
            MonsterManager.instance.gameObject.SetActive(true);
            ItemManager.instance.gameObject.SetActive(true);
            CaveManager.instance.gameObject.SetActive(true);

            EffectManager.instance.PoolOff();
            ObjectManager.instance.PoolOff();
            MonsterManager.instance.PoolOff();
            ItemManager.instance.PoolOff();
            CaveManager.instance.PoolOff();
        }

        World world = World.Instance;
        if (world)
        {
            if (!nowScene.Equals("Tutorial") && !nowScene.Contains("Stage02"))
                world.SetLighting(true);
            for (int i = 0; i < 2; i++)
            {
                Color layerColor = world.GetBlockLayer(i).Material.color;
                world.GetBlockLayer(i).Material.color = new Color(layerColor.r, layerColor.g, layerColor.b, 1);
            }
        }
        CustomFluidChunk.fluidAct = true;
        Chunk.actChunk = true;

        CaveManager.inCave = false;
        ItemManager.instance.fieldObject.Clear();
        ItemManager.instance.caveObject.Clear();
        BuffManager.instance.BuffRemove();

        GameSceneSet(false);

        switch (s)
        {
            #region[Title]
            case "Title":
                UIManager.instance.goTitle = false;
                SoundManager.instance.Title();
                break;
            #endregion

            ////////////////////////////// 인게임맵 ///////////////////////////////////////////

            #region[Tutorial]
            case "Tutorial":
                if (stageBackGround)
                    stageBackGround.sprite = StageManager.instance.tutorial_BackGround;
                GameSceneSet();
                if(GameManager.instance.playData.language == PlayData.Language.한국어)
                    UIManager.instance.showStageNameText.text = StageManager.instance.tutorial_Name;
                else if (GameManager.instance.playData.language == PlayData.Language.English)
                    UIManager.instance.showStageNameText.text = StageManager.instance.tutorial_Name_Eng;
                SoundManager.instance.Tutorial();
                break;
            #endregion

            #region[Stage0101]
            case "Stage0101":
                if (stageBackGround)
                    stageBackGround.sprite = StageManager.instance.stage0101_BackGround;
                GameSceneSet();
                if (GameManager.instance.playData.language == PlayData.Language.한국어)
                    UIManager.instance.showStageNameText.text = StageManager.instance.stage0101_Name;
                else if (GameManager.instance.playData.language == PlayData.Language.English)
                    UIManager.instance.showStageNameText.text = StageManager.instance.stage0101_Name_Eng;
                SoundManager.instance.Stage1();
                break;
            #endregion

            #region[Stage0102]
            case "Stage0102":
                if (stageBackGround)
                    stageBackGround.sprite = StageManager.instance.stage0102_BackGround;
                GameSceneSet();
                if (GameManager.instance.playData.language == PlayData.Language.한국어)
                    UIManager.instance.showStageNameText.text = StageManager.instance.stage0102_Name;
                else if (GameManager.instance.playData.language == PlayData.Language.English)
                    UIManager.instance.showStageNameText.text = StageManager.instance.stage0102_Name_Eng;
                SoundManager.instance.Stage1();
                break;
            #endregion

            #region[Stage0103]
            case "Stage0103":
                if (stageBackGround)
                    stageBackGround.sprite = StageManager.instance.stage0103_BackGround;
                GameSceneSet();
                if (GameManager.instance.playData.language == PlayData.Language.한국어)
                    UIManager.instance.showStageNameText.text = StageManager.instance.stage0103_Name;
                else if (GameManager.instance.playData.language == PlayData.Language.English)
                    UIManager.instance.showStageNameText.text = StageManager.instance.stage0103_Name_Eng;
                SoundManager.instance.Stage1();
                break;
            #endregion

            #region[Stage0201]
            case "Stage0201":
                if (stageBackGround)
                    stageBackGround.sprite = StageManager.instance.stage0201_BackGround;
                GameSceneSet();
                if (GameManager.instance.playData.language == PlayData.Language.한국어)
                    UIManager.instance.showStageNameText.text = StageManager.instance.stage0201_Name;
                else if (GameManager.instance.playData.language == PlayData.Language.English)
                    UIManager.instance.showStageNameText.text = StageManager.instance.stage0201_Name_Eng;
                SoundManager.instance.Stage2();
                break;
            #endregion

            #region[Stage0202]
            case "Stage0202":
                if (stageBackGround)
                    stageBackGround.sprite = StageManager.instance.stage0202_BackGround;
                GameSceneSet();
                if (GameManager.instance.playData.language == PlayData.Language.한국어)
                    UIManager.instance.showStageNameText.text = StageManager.instance.stage0202_Name;
                else if (GameManager.instance.playData.language == PlayData.Language.English)
                    UIManager.instance.showStageNameText.text = StageManager.instance.stage0202_Name_Eng;
                SoundManager.instance.Stage2();
                break;
            #endregion

            #region[Stage0203]
            case "Stage0203":
                if (stageBackGround)
                    stageBackGround.sprite = StageManager.instance.stage0203_BackGround;
                GameSceneSet();
                if (GameManager.instance.playData.language == PlayData.Language.한국어)
                    UIManager.instance.showStageNameText.text = StageManager.instance.stage0203_Name;
                else if (GameManager.instance.playData.language == PlayData.Language.English)
                    UIManager.instance.showStageNameText.text = StageManager.instance.stage0203_Name_Eng;
                SoundManager.instance.Stage2();
                break;
            #endregion

            #region[IglooMap]
            case "IglooMap":
                if (stageBackGround)
                    stageBackGround.sprite = StageManager.instance.Igloo_BackGround;
                GameSceneSet();
                if (GameManager.instance.playData.language == PlayData.Language.한국어)
                    UIManager.instance.showStageNameText.text = StageManager.instance.Igloo_Name;
                else if (GameManager.instance.playData.language == PlayData.Language.English)
                    UIManager.instance.showStageNameText.text = StageManager.instance.Igloo_Name_Eng;
                SoundManager.instance.Stage2();
                break;
            #endregion

            ////////////////////////////// 이벤트맵 ///////////////////////////////////////////

            #region[Test]
            case "Test":
                if (stageBackGround)
                    stageBackGround.sprite = null;
                GameSceneSet();
                UIManager.instance.showStageNameText.text = "테스트";
                break;
            #endregion

        }
    }

    void GameSceneSet(bool b = true)
    {
        UIManager.instance.showStageName.SetActive(b);
        UIManager.instance.gameOver.SetActive(false);
        UIManager.instance.playerItem.SetActive(b);
        UIManager.instance.showChickenPos.SetActive(false);

        GameManager.instance.playStage = b;
        GameManager.instance.stageTime = 0;
        GameManager.instance.gameOver = false;
        GameManager.instance.countDown = GameManager.instance.maxCountDown;

        if (b)
        {
            ItemManager.instance.ReSpawnItemList.Clear();
            BuffManager.loadEnd = false;
            stageBackGround.color = Color.white;
            switch(nowScene)
            {
                #region[Test]
                case "Test":
                    Player.instance.canAttack = true;
                    GroundManager.instance.InitDigMask();
                    break;
                #endregion

                #region[Tutorial]
                case "Tutorial":
                    Player.instance.canAttack = true;
                    Player.instance.notDamage = true;

                    for(int i = 0; i < 6; i++)
                    {
                        GameManager.instance.itemSlot[i] = "";
                        GameManager.instance.itemNum[i] = 0;
                        GameManager.instance.itemCool[i] = 1000;
                    }
                    GameManager.instance.slotAct[0] = true;
                    GameManager.instance.slotAct[1] = true;
                    GameManager.instance.slotAct[2] = true;

                    Player.instance.getChicken = false;
                    Player.instance.transform.position = new Vector3(22, 115, Player.instance.transform.position.z);
                    GroundManager.instance.digMask = 0;
                    GroundManager.instance.digMask = GroundManager.instance.digMask | (int)(Mathf.Pow(2, (int)StageData.GroundLayer.Stone));
                    break;
                #endregion

                #region[EventMap]
                case "ShopMap0101":
                case "ShopMap0102":
                case "ShopMap0103":
                case "SmithyMap0101":
                case "SmithyMap0102":
                case "SmithyMap0103":
                case "AltarMap0101":
                case "AltarMap0102":
                case "AltarMap0103":
                case "FountainMap0101":
                case "FountainMap0102":
                case "FountainMap0103":
                    BuffManager.instance.BuffRemove();
                    Player.instance.canAttack = true;
                    Player.instance.transform.position = new Vector3(GroundManager.instance.eventMapStartPos.x, GroundManager.instance.eventMapStartPos.y, Player.instance.transform.position.z);
                    Player.instance.transform.localScale = new Vector3(Mathf.Abs(Player.instance.transform.localScale.x) * GroundManager.instance.eventMapStartDic, Player.instance.transform.localScale.y, Player.instance.transform.localScale.x);
                    GroundManager.instance.digMask = 0;
                    break;
                #endregion

                #region[InGame]
                case "Stage0101":
                case "Stage0102":
                case "Stage0103":
                case "Stage0201":
                case "Stage0202":
                case "Stage0203":
                case "IglooMap":
                    Player.instance.canAttack = true;
                    Player.instance.transform.position = new Vector3(World.Instance.WorldWidth / 2, World.Instance.WorldHeight + 30, Player.instance.transform.position.z);
                    GroundManager.instance.InitDigMask();
                    GameManager.instance.playData.seed = Random.Range(0, 10000);
                    break;
                default:
                    Debug.LogError("씬설정을 추가하자");
                    break;
                    #endregion
            }
        }
    }
    #endregion

    #region[씬 이동]
    public void MoveScene(string s)
    {
        if (nowSceneMoving)
            return;
        nowSceneMoving = true;
        fadeAnimator.SetTrigger("FadeOut");
        StartCoroutine(LoadScene(s));
    }

    IEnumerator LoadScene(string s)
    {
        yield return new WaitForSeconds(1f);
        while (fadeAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
        {
            yield return new WaitForSeconds(0.1f);
        }
        SceneManager.LoadScene(s);
    }

    #endregion

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////  

    #region[이벤트 맵 검사]
    public bool CheckEventMap()
    {
        if (nowScene.Contains("Shop"))
            return true;

        return false;
    }
    #endregion
}