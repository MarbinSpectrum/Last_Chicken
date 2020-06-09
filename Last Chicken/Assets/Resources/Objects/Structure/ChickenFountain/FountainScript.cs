using Custom;
using TerrainEngine2D;
using UnityEngine;
using System.Collections.Generic;

public class FountainScript : AreaScript
{
    public static FountainScript instance;

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
 
    [System.NonSerialized] public bool thisUse = false;
    [System.NonSerialized] public bool onArea;

    Animator fountainAnimator;
    GameObject uiMouse;

    public List<GameObject> languageData = new List<GameObject>();

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 

    #region[Awake]
    public override void Awake()
    {
        base.Awake();
        instance = this;
        act = false;
        Transform structrue = transform.Find("Structure");
        Transform fountain = structrue.Find("Fountain");

        fountainAnimator = fountain.GetComponent<Animator>();

        uiMouse = transform.Find("UIMouse").gameObject;
    }
    #endregion

    #region[Update]
    public override void Update()
    {
        base.Update();

        for (int i = 0; i < languageData.Count; i++)
            if (languageData[i])
                languageData[i].SetActive(languageData[i].transform.name.Contains(GameManager.instance.playData.language.ToString()));

        onArea = IsAtPlayer(bodyCollider);
        int outWidth = GroundManager.instance.altarRect.GetLength(0);
        int outHeight = Mathf.FloorToInt(GroundManager.instance.altarRect.GetLength(1) * 0.7f);
        int Inwidth = Mathf.FloorToInt(outWidth * 0.7f);
        int Inheight = 14;
        inRect = new RectInt((int)(transform.position.x) - Inwidth / 2, (int)(transform.position.y) - 6 + Inheight, Inwidth, Inheight);
        outRect = new RectInt((int)(transform.position.x) - outWidth / 2, (int)(transform.position.y) - 7 + outHeight, outWidth, outHeight);

        UseArea();
        ActFountain();
        fountainAnimator.SetBool("used", used);
        fountainAnimator.SetBool("usedEnd", usedEnd);
        if (act)
            areaLight.SetActive(false);
    }
    #endregion

    #region[OnEnable]
    void OnEnable()
    {

    }
    #endregion

    #region[Init]
    public override void Init()
    {
        used = false;
        thisUse = false;
        act = false;
        onPlayer = false;
        usedEnd = false;
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
                Player.instance.canControl = false;
                Player.instance.pray = true;
                Player.instance.invincibility = true;
                thisUse = false;
                used = true;
            }
            uiMouse.SetActive(!thisUse);
        }
        else
            uiMouse.SetActive(false);
    }
    #endregion

    #region[분수활성화]
    public void ActFountain()
    {
        if (fountainAnimator.GetCurrentAnimatorStateInfo(0).IsName("FountainUsed") && fountainAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && !act)
        {
            Player.instance.canControl = true;
            Player.instance.pray = false;
            Player.instance.invincibility = false;             
            Player.instance.nowHp = Player.instance.maxHp;
            EffectManager.instance.HearthEffect();

            PlayerIn(false);
            act = true;
        }
        if (fountainAnimator.GetCurrentAnimatorStateInfo(0).IsName("FountainUsed") && fountainAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.95f && !usedEnd)
            usedEnd = true;
    }
    #endregion

    #region[플레이어가 내부에 있는지 검사]
    public override void PlayerIn()
    {
        if (!Player.instance || !StageData.instance || !StageBackGround.instance)
            return;

        if (!act && AreaCheck.RectIn(Player.instance.transform.position, outRect))
        {
            if (Player.instance.rigidbody2D.velocity.y < -2)
                Player.instance.rigidbody2D.velocity = new Vector2(Player.instance.rigidbody2D.velocity.x, -2);
            Player.instance.groundFallTime = 0;
        }

        if (!onPlayer && AreaCheck.RectIn(Player.instance.transform.position, inRect))
        {
            onPlayer = true;
            PlayerIn(true);
        }
        else if (onPlayer && !AreaCheck.RectIn(Player.instance.transform.position, outRect))
        {
            onPlayer = false;
            PlayerIn(false);
        }
    }


    public override void PlayerIn(bool b)
    {
        if (act)
            return;
        if (b)
        {
            SoundManager.instance.Altar(true);
            StageBackGround.instance.FadeOut();
            areaLight.SetActive(true);
        }
        else
        {
            SoundManager.instance.StopBGM_Sound(true);
            StageBackGround.instance.Fadein();
            areaLight.SetActive(false);
        }
    }
    #endregion
}
