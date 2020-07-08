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
    public static int[] reinforceCost = new int[4] { 1200, 2400, 4800, 9600 };

    GameObject uiMouse;

    public List<GameObject> languageData = new List<GameObject>();
    public List<GameObject> actObj = new List<GameObject>();
    public List<GameObject> unActObj = new List<GameObject>();

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
        for (int i = 0; i < languageData.Count; i++)
            if (languageData[i])
                languageData[i].SetActive(languageData[i].transform.name.Contains(GameManager.instance.playData.language.ToString()));

        onArea = IsAtPlayer(bodyCollider);
        UseArea();

        for (int i = 0; i < actObj.Count; i++)
            if (actObj[i])
                actObj[i].SetActive(used);

        for (int i = 0; i < unActObj.Count; i++)
            if (unActObj[i])
                unActObj[i].SetActive(!used);
    }
    #endregion

    #region[OnEnable]
    void OnEnable()
    {
        instance = this;
    }
    #endregion

    #region[Init]
    public override void Init()
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
            if (/*Input.GetMouseButtonDown(1) || */Input.GetKeyDown(KeyCode.W))
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

}
