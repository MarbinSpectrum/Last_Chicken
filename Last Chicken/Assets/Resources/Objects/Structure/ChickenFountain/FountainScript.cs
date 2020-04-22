﻿using Custom;
using TerrainEngine2D;
using UnityEngine;

public class FountainScript : AreaScript
{
    public static FountainScript instance;

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
 
    [System.NonSerialized] public bool thisUse = false;
    [System.NonSerialized] public bool onArea;
    Animator fountainAnimator;

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
    }
    #endregion

    #region[Update]
    public override void Update()
    {
        base.Update();
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
                Player.instance.canControl = true;
                Player.instance.pray = true;
                Player.instance.invincibility = true;
                thisUse = false;
                used = true;
            }
        }
    }
    #endregion

    #region[분수활성화]
    public void ActFountain()
    {
        if (fountainAnimator.GetCurrentAnimatorStateInfo(0).IsName("FountainUsed") && fountainAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && !act)
        {
            act = true;
            Player.instance.canControl = true;
            Player.instance.pray = false;
            Player.instance.invincibility = false;             
            Player.instance.nowHp = Player.instance.maxHp;
            EffectManager.instance.HearthEffect();
        }
    }
    #endregion

    #region[플레이어가 내부에 있는지 검사]
    public override void PlayerIn()
    {
        if (!Player.instance || !StageData.instance || !StageBackGround.instance)
            return;

        if (AreaCheck.RectIn(Player.instance.transform.position, outRect))
        {
            if (Player.instance.rigidbody2D.velocity.y < -2)
                Player.instance.rigidbody2D.velocity = new Vector2(Player.instance.rigidbody2D.velocity.x, -2);
            Player.instance.groundFallTime = 0;
        }

        if (!onPlayer && AreaCheck.RectIn(Player.instance.transform.position, inRect))
        {
            SoundManager.instance.Altar();
            onPlayer = true;
            PlayerIn(true);
            CameraController.Instance.SetOffset(13);
        }
        else if (onPlayer && !AreaCheck.RectIn(Player.instance.transform.position, outRect))
        {
            SoundManager.instance.Stage1();
            onPlayer = false;
            PlayerIn(false);
            CameraController.Instance.SetOffset(0);
        }
    }
    #endregion
}
