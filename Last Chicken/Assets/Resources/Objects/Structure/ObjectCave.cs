using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TerrainEngine2D;

public class ObjectCave : CustomCollider
{
    public BoxCollider2D check;
    public GameObject uiMouse;
    public GameObject targetObject;
    public Image fade;
    public Transform target;
    public List<GameObject> languageData = new List<GameObject>();
    bool act = false;
    bool fadeAct = false;
    public float minX,maxX;


    void Update()
    {
        PlayerCheck();
        Language();
        updateFade();
        FollowPlayer();
    }

    void Language()
    {
        for (int i = 0; i < languageData.Count; i++)
            if (languageData[i])
                languageData[i].SetActive(languageData[i].transform.name.Contains(GameManager.instance.playData.language.ToString()));
    }

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

    void PlayerCheck()
    {
        if (GameManager.instance.gamePause || !Player.instance)
            return;
        bool playerIn = IsAtPlayer(check) && Player.instance.grounded;
        uiMouse.SetActive(playerIn);

        if (playerIn && Player.instance.canControl)
        {
            if (Input.GetMouseButtonDown(1))
            {
                act = !act;
                fadeAct = true;
                Player.instance.invincibility = true;
                Player.instance.canControl = false;
                StartCoroutine(ObjectSetting(act));
            }
        }
    }

    private IEnumerator ObjectSetting(bool act)
    {
        yield return new WaitForSeconds(2f);
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
        CameraController.Instance.SetOffset(act ? 13 : 0);
        targetObject.SetActive(act);
        ObjectManager.instance.gameObject.SetActive(!act);
        MonsterManager.instance.gameObject.SetActive(!act);
        ItemManager.instance.CaveObjectAct(act);
        ItemManager.instance.FieldObjectAct(!act);

        World world = World.Instance;
        world.SetLighting(!act);
        for (int i = 0; i < 2; i++)
        {
            Color layerColor = world.GetBlockLayer(i).Material.color;
            world.GetBlockLayer(i).Material.color = new Color(layerColor.r, layerColor.g, layerColor.b, act ? 0 : 1);
        }
        CameraController.Instance.objectToFollow = act ? target : Player.instance.transform;
        CameraController.Instance.edgeCheck = !act;
        CustomFluidChunk.fluidAct = !act;
        Chunk.actChunk = !act;


        if (!act)
        {
            yield return new WaitForSeconds(0.2f);
           gameObject.SetActive(false);
        }
    }
}
