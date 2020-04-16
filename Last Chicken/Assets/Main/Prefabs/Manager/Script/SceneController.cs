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
            EffectManager.instance.PoolOff();
            ObjectManager.instance.PoolOff();
            MonsterManager.instance.PoolOff();
            ItemManager.instance.PoolOff();
        }

        GameSceneSet(false);

        switch (s)
        {
            case "Title":
                UIManager.instance.goTitle = false;
                SoundManager.instance.Title();
                break;
            case "Test":
                if (stageBackGround)
                    stageBackGround.sprite = null;
                GameSceneSet();
                UIManager.instance.showStageNameText.text = "테스트 맵";
                break;
            case "Stage0101":
                if (stageBackGround)
                    stageBackGround.sprite = StageManager.instance.stage0101_BackGround;
                GameSceneSet();
                UIManager.instance.showStageNameText.text = StageManager.instance.stage0101_Name;
                SoundManager.instance.Stage1();
                break;
            case "ShopMap0101":
                if (stageBackGround)
                    stageBackGround.sprite = StageManager.instance.stage0101_BackGround;
                UIManager.instance.showStageNameText.text = "";
                GameSceneSet();
                SoundManager.instance.Stage1();
                break;
            case "Stage0102":
                if (stageBackGround)
                    stageBackGround.sprite = StageManager.instance.stage0102_BackGround;
                GameSceneSet();
                UIManager.instance.showStageNameText.text = StageManager.instance.stage0102_Name;
                SoundManager.instance.Stage1();
                break;
            case "ShopMap0102":
                if (stageBackGround)
                    stageBackGround.sprite = StageManager.instance.stage0102_BackGround;
                UIManager.instance.showStageNameText.text = "";
                GameSceneSet();
                SoundManager.instance.Stage1();
                break;
            case "Stage0103":
                if (stageBackGround)
                    stageBackGround.sprite = StageManager.instance.stage0103_BackGround;
                GameSceneSet();
                UIManager.instance.showStageNameText.text = StageManager.instance.stage0103_Name;
                SoundManager.instance.Stage1();
                break;
            case "ShopMap0103":
                if (stageBackGround)
                    stageBackGround.sprite = StageManager.instance.stage0103_BackGround;
                UIManager.instance.showStageNameText.text = "";
                GameSceneSet();
                SoundManager.instance.Stage1();
                break;
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
            BuffManager.loadEnd = false;
            BuffManager.instance.NextStageBuffRemove();
        }

        if (b)
        {
            stageBackGround.color = Color.white;
            switch(nowScene)
            {
                case "Test":
                    Player.instance.canAttack = true;
                    break;
                case "Stage0101":
                    Player.instance.canAttack = true;
                    Player.instance.transform.position = new Vector3(World.Instance.WorldWidth / 2, World.Instance.WorldHeight + 30, Player.instance.transform.position.z);
                    break;
                case "ShopMap0101":
                    Player.instance.canAttack = true;
                    Player.instance.transform.position = new Vector3(GroundManager.instance.shopMap0101StartPos.x, GroundManager.instance.shopMap0101StartPos.y, Player.instance.transform.position.z);
                    Player.instance.transform.localScale = new Vector3(Mathf.Abs(Player.instance.transform.localScale.x) * GroundManager.instance.shopMap0101StartDic, Player.instance.transform.localScale.y, Player.instance.transform.localScale.x);
                    break;
                case "Stage0102":
                    Player.instance.canAttack = true;
                    Player.instance.transform.position = new Vector3(World.Instance.WorldWidth / 2, World.Instance.WorldHeight + 30, Player.instance.transform.position.z);
                    break;
                case "ShopMap0102":
                    Player.instance.canAttack = true;
                    Player.instance.transform.position = new Vector3(GroundManager.instance.shopMap0102StartPos.x, GroundManager.instance.shopMap0102StartPos.y, Player.instance.transform.position.z);
                    Player.instance.transform.localScale = new Vector3(Mathf.Abs(Player.instance.transform.localScale.x) * GroundManager.instance.shopMap0102StartDic, Player.instance.transform.localScale.y, Player.instance.transform.localScale.x);
                    break;
                case "Stage0103":
                    Player.instance.canAttack = true;
                    Player.instance.transform.position = new Vector3(World.Instance.WorldWidth / 2, World.Instance.WorldHeight + 30, Player.instance.transform.position.z);
                    break;
                case "ShopMap0103":
                    Player.instance.canAttack = true;
                    Player.instance.transform.position = new Vector3(GroundManager.instance.shopMap0103StartPos.x, GroundManager.instance.shopMap0103StartPos.y, Player.instance.transform.position.z);
                    Player.instance.transform.localScale = new Vector3(Mathf.Abs(Player.instance.transform.localScale.x) * GroundManager.instance.shopMap0103StartDic, Player.instance.transform.localScale.y, Player.instance.transform.localScale.x);
                    break;
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