using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TerrainEngine2D;

public class TutorialAltarEvent : CustomCollider
{
    BoxCollider2D enterCollider;
    BoxCollider2D altarCollider;

    Transform cameraPos;

    GameObject wallObject;

    GameObject uiMouse;

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    bool enterFlag = false;
    bool altarFlag = false;
    bool chickenFlag = false;

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 

    #region[Awake]
    void Awake()
    {
        enterCollider = transform.Find("Collider").Find("EnterCollider").GetComponent<BoxCollider2D>();
        altarCollider = transform.Find("Collider").Find("AltarCollider").GetComponent<BoxCollider2D>();
        cameraPos = transform.Find("CameraPos");
        wallObject = transform.Find("Wall").gameObject;
        uiMouse = transform.Find("UIMouse").gameObject;
    }
    #endregion

    #region[Update]
    void Update()
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
        #region[제단이 있는곳 들어가는 이벤트]
        if (!enterFlag)
        {
            if (IsAtPlayer(enterCollider))
            {
                enterFlag = true;
                CameraController.Instance.objectToFollow = cameraPos;
                wallObject.SetActive(true);
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
                    uiMouse.SetActive(false);
                }
            }
            else
            {
                uiMouse.SetActive(false);
            }
        }
        #endregion

        #region[닭 이벤트]
        else if (!chickenFlag)
        {

        }
        #endregion
    }
    #endregion
}
