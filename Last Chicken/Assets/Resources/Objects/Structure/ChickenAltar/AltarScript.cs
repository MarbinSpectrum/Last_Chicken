using Custom;
using TerrainEngine2D;
using UnityEngine;
using System.Collections.Generic;

public class AltarScript : AreaScript
{
    public static AltarScript instance;

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    [System.NonSerialized] public bool thisUse = false;
    [System.NonSerialized] public List<int> buffList = new List<int>();
    [System.NonSerialized] public bool onArea;
    Animator altarAnimator;
    GameObject uiMouse;
    bool upTrigger;

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
        Transform altar = structrue.Find("Altar");

        altarAnimator = altar.GetComponent<Animator>();

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
        ActAltar();
        altarAnimator.SetBool("used", used);
        altarAnimator.SetBool("usedEnd", usedEnd);
        if (act)
            areaLight.SetActive(false);
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
        SetBuff();
        used = false;
        usedEnd = false;
        thisUse = false;
        act = false;
        onPlayer = false;
        SetAltarBackImg();
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
        if (Input.GetAxisRaw("Vertical") == 0)
            upTrigger = false;
        if (!used && IsAtPlayer(bodyCollider))
        {
            if (Input.GetKeyDown(KeyCode.W) || (Input.GetAxisRaw("Vertical") > 0 && !upTrigger))
            {
                upTrigger = true;
                if (Player.instance.canControl && !thisUse)
                {
                    Player.instance.canControl = false;
                    Player.instance.pray = true;
                    thisUse = true;
                }
                else if (thisUse)
                {
                    Player.instance.canControl = true;
                    Player.instance.pray = false;
                    thisUse = false;
                }
            }
            uiMouse.SetActive(!thisUse);
        }
        else
            uiMouse.SetActive(false);
    }
    #endregion

    #region[제단활성화]
    public void ActAltar()
    {
        if(altarAnimator.GetCurrentAnimatorStateInfo(0).IsName("AltarUsed") && altarAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && !act)
        {
            Player.instance.canControl = true;
            Player.instance.pray = false;
            Player.instance.invincibility = false;

            StageData.instance.AltarBackGroundSwap(true);
            AltarBackGroundImg.ChangeImgs(true);

            PlayerIn(false);

            act = true;
        }
        if (altarAnimator.GetCurrentAnimatorStateInfo(0).IsName("AltarUsed") && altarAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.1f && altarAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.9f)
            EffectManager.instance.Vibration(10, altarAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime * 2);
        if (altarAnimator.GetCurrentAnimatorStateInfo(0).IsName("AltarUsed") && altarAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.95f && !usedEnd)
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

    #region[제단의 버프 설정]
    public void SetBuff()
    {
        buffList.Clear();
        for (int i = 0; i < 3; i++)
        {
            int buffIndex = 0;
            do
            {
                buffIndex = Random.Range(0, BuffManager.buffName.Length);


            } while (buffList.Contains(buffIndex));

            buffList.Add(buffIndex);

            for (int j = 0; j < 2; j++)
            {
                UIManager.instance.caseSpriteglow[i, j].GlowBrightness = BuffManager.instance.buffData[BuffManager.FindData(BuffManager.buffName[buffIndex])].buffGlow;
                UIManager.instance.caseSpriteglow[i, j].GlowColor = BuffManager.instance.buffData[BuffManager.FindData(BuffManager.buffName[buffIndex])].buffColor;
                UIManager.instance.caseSpriteRenderer[i, j].sprite = BuffManager.instance.buffData[BuffManager.FindData(BuffManager.buffName[buffIndex])].BuffImg;
                UIManager.instance.caseImage[i].sprite = BuffManager.instance.buffData[BuffManager.FindData(BuffManager.buffName[buffIndex])].BuffImg;
            }
        }
    }
    #endregion

    #region[제단 이미지 세팅]
    void SetAltarBackImg()
    {
        World world = World.Instance;
        if (!World.Instance)
            return;

        //bool[,] useBlock = new bool[world.WorldWidth, world.WorldHeight];

        //for (int x = 0; x < world.WorldWidth; x++)
        //    for (int y = 0; y < world.WorldHeight; y++)
        //        if(StageData.instance.backGroundData != null)
        //            useBlock[x, y] = (StageData.instance.backGroundData[x, y] == StageData.BackGroundLayer.DarkAltarBackGround);


        //for (int x = 0; x < world.WorldWidth; x++)
        //    for (int y = 0; y < world.WorldHeight; y++)
        //        if(useBlock[x, y])
        //        {
        //            if(Random.Range(0,100) > 98)
        //            {
        //                int r = Random.Range(0, 8);
        //                bool check = true;
        //                switch (r)
        //                {
        //                    case 0:
        //                    case 1:
        //                    case 4:
        //                    case 5:
        //                        for (int i = 0; i < 4; i++)
        //                            if (!Exception.IndexOutRange(x + i % 2, y - i / 2, useBlock) || !useBlock[x + i % 2, y - i / 2])
        //                                check = false;
        //                        if (!check)
        //                            break;
        //                        for (int i = 0; i < 4; i++)
        //                            useBlock[x + i % 2, y - i / 2] = false;
        //                        ObjectManager.instance.AltarBackgroundImgs(new Vector2(x, y), r);
        //                        break;
        //                    case 2:
        //                    case 3:
        //                    case 6:
        //                        for (int i = 0; i < 6; i++)
        //                            if (!Exception.IndexOutRange(x + i % 3, y - i / 3, useBlock) || !useBlock[x + i % 3, y - i / 3])
        //                                check = false;
        //                        if (!check)
        //                            break;
        //                        for (int i = 0; i < 6; i++)
        //                            useBlock[x + i % 3, y - i / 3] = false;
        //                        ObjectManager.instance.AltarBackgroundImgs(new Vector2(x, y), r);
        //                        break;
        //                    case 7:
        //                        for (int i = 0; i < 70; i++)
        //                            if (!Exception.IndexOutRange(x + i % 10, y - i / 10, useBlock) || !useBlock[x + i % 10, y - i / 10])
        //                                check = false;
        //                        if (!check)
        //                            break;
        //                        for (int i = 0; i < 70; i++)
        //                            useBlock[x + i % 10, y - i / 10] = false;
        //                        ObjectManager.instance.AltarBackgroundImgs(new Vector2(x, y), r);
        //                        break;
        //                }

        //            }
        //        }

        AltarBackGroundImg.ChangeImgs(false);
    }
    #endregion
}
