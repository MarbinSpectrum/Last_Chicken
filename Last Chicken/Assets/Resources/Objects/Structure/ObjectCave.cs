using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TerrainEngine2D;
using TerrainEngine2D.Lighting;

public class ObjectCave : CustomCollider
{
    public BoxCollider2D check;
    public GameObject uiMouse;
    public GameObject foothold;
    public GameObject targetObject;
    public Image fade;
    public Transform target;
    public List<GameObject> languageData = new List<GameObject>();
    public BlockLightSource playerBlockLightSource;
    public SpriteRenderer stageDoor;
    public SpriteRenderer stageFoothold;
    public float minX,maxX;
    public AreaScript areaScript;
    public Color onColor = Color.white;
    public Color offColor = Color.white;
    bool act = false;
    bool fadeAct = false;

    #region[Update]
    void Update()
    {
        PlayerCheck();
        Language();
        updateFade();
        FollowPlayer();
        StageLight();
        StageSprite();
        //light.SetActive(!act && TreasureBoxScirpt.useTreasureBoxRadar);
    }
    #endregion

    #region[OnEnable]
    private void OnEnable()
    {
    
    }
    #endregion

    #region[ObjectInit]
    public void ObjectInit()
    {
        areaScript.Init();
    }
    #endregion

    #region[StageLight]
    public void StageLight()
    {
        Color color = new Color(255 / 255f, 180 / 255f, 55 / 255f);
        if (SceneController.instance.nowScene.Contains("Stage01"))
            color = new Color(255 / 255f, 180 / 255f, 55 / 255f);
        else if (SceneController.instance.nowScene.Contains("Stage02") || SceneController.instance.nowScene.Contains("Igloo"))
            color = new Color(204 / 255f, 204 / 255f, 255 / 255f);
        playerBlockLightSource.LightColor = color;
    }
    #endregion

    #region[StageSprite]
    public void StageSprite()
    {
        int stageNum = 0;
        if (SceneController.instance.nowScene.Contains("Stage01"))
            stageNum = 0;
        else if (SceneController.instance.nowScene.Contains("Stage02") || SceneController.instance.nowScene.Contains("Igloo"))
            stageNum = 1;
        stageDoor.sprite = CaveManager.instance.stageDoor_Spr[stageNum];
        stageFoothold.sprite = CaveManager.instance.stageDoorFoothold_Spr[stageNum];
    }
    #endregion

    #region[언어]
    void Language()
    {
        for (int i = 0; i < languageData.Count; i++)
            if (languageData[i])
                languageData[i].SetActive(languageData[i].transform.name.Contains(GameManager.instance.playData.language.ToString()));
    }
    #endregion

    #region[updateFade]
    void updateFade()
    {
        if(fadeAct)
        {
            fade.color += new Color(0, 0, 0, Time.deltaTime);
            fade.color = new Color(0, 0, 0, Mathf.Min(1, fade.color.a));
        }
        else
        {
            fade.color -= new Color(0, 0, 0, Time.deltaTime);
            fade.color = new Color(0, 0, 0, Mathf.Max(0, fade.color.a));
        }
    }
    #endregion

    #region[FollowPlayer]
    void FollowPlayer()
    {
        if (act)
        {
            float nMax = maxX + transform.position.x;
            float nMin = minX + transform.position.x;
            if (Player.instance.transform.position.x < nMin)
                target.position = new Vector3(nMin, target.position.y, target.position.z);
            else if (Player.instance.transform.position.x > nMax)
                target.position = new Vector3(nMax, target.position.y, target.position.z);
            else
                target.position = new Vector3(Player.instance.transform.position.x, target.position.y, target.position.z);
        }
    }
    #endregion

    #region[PlayerCheck]
    void PlayerCheck()
    {
        if (GameManager.instance.gamePause || !Player.instance)
            return;
        bool playerIn = IsAtPlayer(check) && Player.instance.grounded;
        uiMouse.SetActive(playerIn);
        if (playerIn && Player.instance.canControl)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                act = !act;
                fadeAct = true;
                Player.instance.invincibility = true;
                Player.instance.canControl = false;
                StartCoroutine(ObjectSetting(act));
            }
        }

        foothold.SetActive(!targetObject.activeSelf);
        stageDoor.color = !targetObject.activeSelf ? onColor : offColor;
    }
    #endregion

    #region[ObjectSetting]
    private IEnumerator ObjectSetting(bool act)
    {
        yield return new WaitForSeconds(1f);
        CameraController.Instance.SetOffset(act ? 13 : 0);
        CameraController.Instance.objectToFollow = act ? target : Player.instance.transform;
        CameraController.Instance.edgeCheck = !act;
        yield return new WaitForSeconds(1f);
        CaveManager.inCave = act;
        if (act && !Player.instance.getChicken)
            Chicken.instance.gameObject.SetActive(false);
        else if (!act && !Player.instance.getChicken)
            Chicken.instance.gameObject.SetActive(true);

        if(act)
        {
            GroundManager.instance.digMask = 0;
            CaveManager.instance.CaveEnter(gameObject);
            SoundManager.instance.StopBGM_Sound();
        }
        else
        {
            GroundManager.instance.InitDigMask();
            CaveManager.instance.CaveExit();
            SoundManager.instance.PlayBGM_Sound(true);

        }

        fadeAct = false;
        Player.instance.invincibility = false;
        Player.instance.canControl = true;

        targetObject.SetActive(act);
        ObjectManager.instance.gameObject.SetActive(!act);
        MonsterManager.instance.gameObject.SetActive(!act);
        ItemManager.instance.CaveObjectAct(act);
        ItemManager.instance.FieldObjectAct(!act);
        ItemManager.instance.MineralObjectAct(!act);

        World world = World.Instance;
        world.SetLighting(!act);
        for (int i = 0; i < 2; i++)
        {
            Color layerColor = world.GetBlockLayer(i).Material.color;
            world.GetBlockLayer(i).Material.color = new Color(layerColor.r, layerColor.g, layerColor.b, act ? 0 : 1);
        }
        CustomFluidChunk.fluidAct = !act;
        Chunk.actChunk = !act;


        //if (!act)
        //{
        //    yield return new WaitForSeconds(0.2f);
        //   gameObject.SetActive(false);
        //}
    }
    #endregion

}
