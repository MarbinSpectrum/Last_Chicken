using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TerrainEngine2D;

public class TutorialAltarEvent : CustomCollider
{
    BoxCollider2D enterCollider;
    BoxCollider2D altarCollider;
    BoxCollider2D digCollider;

    Transform cameraPos;

    GameObject wallObject;
    GameObject followChicken;
    GameObject uiMouse;
    GameObject followGetItem;
    GameObject followPlayerY;

    Rigidbody2D chickenRigid;

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    bool chickenDisableFlag = false;
    bool enterFlag = false;
    bool altarFlag = false;
    bool chickenFlag = false;
    float chickenDownTime = 0;
    bool getChickenFlag = false;

    bool digFlag = false;

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
        followGetItem = transform.Find("FollowUI").gameObject;
        followPlayerY = transform.Find("FollowPlayerY").gameObject;
    }
    #endregion

    #region[Update]
    public void Update()
    {
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
        }
        #endregion

        #region[제단 이벤트]
        else if (!altarFlag)
        {
            if (IsAtPlayer(altarCollider))
            {
                uiMouse.SetActive(true);
                if (Input.GetMouseButtonDown(1))
                {
                    Player.instance.canControl = false;
                    Player.instance.pray = true;
                    altarFlag = true;
                    SoundManager.instance.Altar(true);
                    Chicken.instance.gameObject.SetActive(true);
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
            if (chickenDownTime > 14)
            {
                chickenFlag = true;
                Player.instance.canControl = true;
                Player.instance.pray = false;
                followGetItem.GetComponent<Follow>().followUI = UIManager.instance.itemImg[0].gameObject;
                followChicken.transform.GetChild(0).gameObject.SetActive(true);
                followGetItem.SetActive(true);
                SoundManager.instance.StopBGM_Sound();
            }
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
