using Custom;
using TerrainEngine2D;
using UnityEngine;
using System.Collections.Generic;

public class Smithy : AreaScript
{
    public static Smithy instance;

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    [System.NonSerialized] public bool thisUse = false;
    [System.NonSerialized] public bool onArea;
    public static int[] reinforceCost = new int[4] { 2500, 5000, 7500, 10000 };
    float time = 13;
    GameObject uiMouse;

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 

    #region[Awake]
    public override void Awake()
    {
        instance = this;
        bodyCollider = GetComponent<BoxCollider2D>();

        uiMouse = transform.Find("UIMouse").gameObject;
    }
    #endregion

    #region[Update]
    public override void Update()
    {
        onArea = IsAtPlayer(bodyCollider);
        UseArea();
        //SmithySound();
    }
    #endregion

    #region[OnEnable]
    void OnEnable()
    {
        used = false;
        thisUse = false;
        act = false;
    }
    #endregion

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region[UseArea]
    //플레이어가 오브젝트를 사용할 수 있는지 검사
    public override void UseArea()
    {
        if (GameManager.instance.gamePause)
            return;
        if (!used && IsAtPlayer(bodyCollider))
        {
            if (Input.GetMouseButtonDown(1))
            {
                Player.instance.canControl = thisUse;
                thisUse = !thisUse;
            }
            uiMouse.SetActive(!thisUse);
        }
        else
            uiMouse.SetActive(false);
    }
    #endregion

    #region[대장간소리]
    public void SmithySound()
    {
        time += Time.deltaTime;
        if(time > 12)
        {
            time = 0;
            SoundManager.instance.Smithy();
        }
    }
    #endregion

}
