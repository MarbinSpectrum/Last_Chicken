using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TerrainEngine2D;

public class TutorialAltarEvent : CustomCollider
{
    BoxCollider2D enterCollider;
    BoxCollider2D altarCollider;
    BoxCollider2D digCollider;

    Transform cameraPos;

    GameObject wallObject;
    GameObject followChicken;
    GameObject chickenMessage;
    bool[] messageFlag = new bool[2];
    string[] message = new string[] { "마지막 남은 닭", "그 닭을 지키면서 지하에 있는 나의 신전으로 와라." };
    string[] message_eng = new string[] { "The Last Chicken", "Protect the chicken and come to \n my temple in the basement." };
    GameObject uiMouse;
    GameObject followGetItem;
    GameObject followPlayerY;
    SpriteRenderer white;

    Rigidbody2D chickenRigid;

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    bool chickenDisableFlag = false;
    bool enterFlag = false;
    bool altarFlag = false;
    bool chickenFlag = false;
    float chickenDownTime = 0;
    bool getChickenFlag = false;

    bool digFlag = false;

    public List<GameObject> languageData = new List<GameObject>();

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 

    #region[Awake]
    public void Awake()
    {
        enterCollider = transform.Find("Collider").Find("EnterCollider").GetComponent<BoxCollider2D>();
        altarCollider = transform.Find("Collider").Find("AltarCollider").GetComponent<BoxCollider2D>();
        digCollider = transform.Find("Collider").Find("DigCollider").GetComponent<BoxCollider2D>();
        cameraPos = transform.Find("CameraPos");
        wallObject = transform.Find("Wall").gameObject;
        uiMouse = transform.Find("UIMouse").gameObject;
        followChicken = transform.Find("FollowChicken").gameObject;
        chickenMessage = transform.Find("ChickenMessage").Find("Text_" + GameManager.instance.playData.language.ToString()).gameObject;
        followGetItem = transform.Find("FollowUI").gameObject;
        followPlayerY = transform.Find("FollowPlayerY").gameObject;
        white = transform.Find("White").GetComponent<SpriteRenderer>();
    }
    #endregion

    #region[Update]
    public void Update()
    {
        for (int i = 0; i < languageData.Count; i++)
            if (languageData[i])
                languageData[i].SetActive(languageData[i].transform.name.Contains(GameManager.instance.playData.language.ToString()));
        TutorialTrigger();
    }
    #endregion

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 

    #region[튜토리얼 트리거]
    private void TutorialTrigger()
    {
        #region[치킨 비활성화]
        if (!chickenDisableFlag)
        {
            if(Chicken.instance)
            {
                Chicken.instance.gameObject.SetActive(false);
                chickenDisableFlag = true;
                followChicken.transform.parent = transform.parent.parent;
                followChicken.transform.GetChild(0).gameObject.SetActive(false);
                followChicken.GetComponent<Follow>().followUI = Chicken.instance.gameObject;
                followChicken.SetActive(true);
            }
        }
        #endregion

        #region[제단이 있는곳 들어가는 이벤트]
        else if (!enterFlag)
        {
            if (IsAtPlayer(enterCollider))
            {
                enterFlag = true;
                CameraController.Instance.objectToFollow = cameraPos;
                wallObject.SetActive(true);
                SoundManager.instance.StopBGM_Sound();
            }

            if (ItemManager.instance.HasItemCheck("Bell"))
            {
                followGetItem.GetComponent<Follow>().followUI = UIManager.instance.itemImg[0].gameObject;
                followGetItem.SetActive(true);
            }
        }
        #endregion

        #region[제단 이벤트]
        else if (!altarFlag) 
        {
            if (IsAtPlayer(altarCollider))
            {
                uiMouse.SetActive(true);
                if (Input.GetKeyDown(KeyManager.instance.keyBoard[GameKeyType.Up]) || KeyManager.GetKeyDown(KeyManager.instance.gamePad[GameKeyType.Up]))
                {
                    Player.instance.canControl = false;
                    Player.instance.pray = true;
                    altarFlag = true;
                    SoundManager.instance.Altar(true);
                    Chicken.instance.gameObject.SetActive(true);
                    Chicken.instance.orderPos = Chicken.instance.transform.position;
                    Chicken.instance.orderTime = 0;
                    chickenRigid = Chicken.instance.GetComponent<Rigidbody2D>();
                    uiMouse.SetActive(false);
                }
            }
            else
            {
                uiMouse.SetActive(false);
            }
        }
        #endregion

        #region[닭 등장 이벤트]
        else if (!chickenFlag)
        {
            Chicken.instance.pattenType = Chicken.Pattern.대기;
            chickenRigid.velocity = new Vector2(0, -0.8f);
            chickenDownTime += Time.deltaTime;
            if(chickenDownTime > 5 && !messageFlag[0])
            {
                messageFlag[0] = true;
                chickenMessage.SetActive(true);
                if (GameManager.instance.playData.language == Language.한국어)
                    chickenMessage.GetComponent<Text>().text = message[0];
                else if (GameManager.instance.playData.language == Language.English)
                    chickenMessage.GetComponent<Text>().text = message_eng[0];
            }

            if (chickenDownTime > 9 && !messageFlag[1])
            {
                messageFlag[1] = true;
                chickenMessage.SetActive(true);
                if (GameManager.instance.playData.language == Language.한국어)
                    chickenMessage.GetComponent<Text>().text = message[1];
                else if (GameManager.instance.playData.language == Language.English)
                    chickenMessage.GetComponent<Text>().text = message_eng[1];
                white.color -= new Color(0, 0, 0, Time.deltaTime / 10f);
            }

            if (chickenDownTime > 14)
            {
                chickenFlag = true;
                Player.instance.canControl = true;
                Player.instance.pray = false;
                followChicken.transform.GetChild(0).gameObject.SetActive(true);
                SoundManager.instance.StopBGM_Sound(true);
            }

            if (chickenDownTime < 5)
                white.color += new Color(0, 0, 0, Time.deltaTime / 5f);
            else if (chickenDownTime > 10)
                white.color -= new Color(0, 0, 0, Time.deltaTime / 4f);
        }
        #endregion

        #region[닭 줍기 이벤트]
        else if (!getChickenFlag)
        {
            if(Player.instance.getChicken)
            {
                followChicken.SetActive(false);
                followGetItem.SetActive(false);
                getChickenFlag = true;
                wallObject.transform.GetChild(1).gameObject.SetActive(false);
                Player.instance.notFallDamage = true;
                cameraPos.transform.position += new Vector3(5, 0,0);
            }
        }
        #endregion

        #region[카메라 조절]
        else if (!digFlag)
        {
            if (IsAtPlayer(digCollider))
            {
                digFlag = true;
                CameraController.Instance.objectToFollow = followPlayerY.transform;
                wallObject.transform.GetChild(2).gameObject.SetActive(true);
                GroundManager.instance.InitDigMask();
            }
        }
        else
        {
            followPlayerY.transform.position = new Vector3(followPlayerY.transform.position.x, Player.instance.transform.position.y, followPlayerY.transform.position.z);
        }
        #endregion
        
    }
    #endregion
}
