using Custom;
using TerrainEngine2D;
using UnityEngine;

public class FountainScript : AreaScript
{
    public static FountainScript instance;

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
 
    [System.NonSerialized] public bool thisUse = false;
    [System.NonSerialized] public bool onArea;

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 

    #region[Awake]
    public override void Awake()
    {
        base.Awake();
        instance = this;
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
        inRect = new RectInt((int)(transform.position.x) - Inwidth / 2, (int)(transform.position.y) - 7 + Inheight, Inwidth, Inheight);
        outRect = new RectInt((int)(transform.position.x) - outWidth / 2, (int)(transform.position.y) - 7 + outHeight, outWidth, outHeight);

        UseArea();

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
        if (IsAtPlayer(bodyCollider))
        {
            if (Input.GetMouseButtonDown(1))
            {
                Player.instance.canControl = thisUse;
                Player.instance.pray = !thisUse;
                thisUse = !thisUse;
                if (!used)
                {
                    Player.instance.nowHp = Player.instance.maxHp;
                    used = true;
                    EffectManager.instance.HearthEffect();
                }
            }
        }
    }
    #endregion

    #region[플레이어가 분수 내부에 있는지 검사]
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
