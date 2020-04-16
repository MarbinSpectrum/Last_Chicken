using Custom;
using TerrainEngine2D;
using TerrainEngine2D.Lighting;
using UnityEngine;

public abstract class AreaScript : CustomCollider
{
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public Color lightColor;
    Color defaultColor = new Color(255 / 255f, 180 / 255f, 55 / 255f);

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    [System.NonSerialized] public bool onPlayer = false;
    [System.NonSerialized] public RectInt inRect;
    [System.NonSerialized] public RectInt outRect;

    [System.NonSerialized] public bool used;
    [System.NonSerialized] public BoxCollider2D bodyCollider;
    [System.NonSerialized] protected bool act = false;
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    GameObject areaLight;
    BlockLightSource[] blockLightSource;

    SpriteRenderer[] inSideDarkness;
    SpriteRenderer[] outSideDarkness;

    GameObject particle;

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 

    #region[Awake]
    public virtual void Awake()
    {
        onPlayer = false;

        Transform effect = transform.Find("Effect");

        areaLight = effect.Find("Light").gameObject;
        if (areaLight.transform.childCount > 0)
            blockLightSource = new BlockLightSource[areaLight.transform.childCount];
        for (int i = 0; i < areaLight.transform.childCount; i++)
        {
            blockLightSource[i] = areaLight.transform.GetChild(i).GetChild(0).GetComponent<BlockLightSource>();
            if (blockLightSource[i].transform.name.Equals("light"))
                blockLightSource[i].LightColor = lightColor;
        }

        Transform inDark = effect.Find("InDark");
        if (inDark.childCount > 0)
            inSideDarkness = new SpriteRenderer[inDark.childCount];
        for (int i = 0; i < inDark.childCount; i++)
            inSideDarkness[i] = inDark.GetChild(i).GetComponent<SpriteRenderer>();

        Transform outDark = effect.Find("OutDark");
        if (outDark.childCount > 0)
            outSideDarkness = new SpriteRenderer[outDark.childCount];
        for (int i = 0; i < outDark.childCount; i++)
            outSideDarkness[i] = outDark.GetChild(i).GetComponent<SpriteRenderer>();

        particle = effect.Find("Particle").gameObject;

        bodyCollider = GetComponent<BoxCollider2D>();

    }
    #endregion

    #region[Update]
    public virtual void Update()
    {
        PlayerIn();
        Effect(onPlayer);
    }
    #endregion

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region[UseArea]
    //플레이어가 사용할수있는 상태 인지를 검사
    public virtual void UseArea()
    {
        if (IsAtPlayer(bodyCollider))
        {
            if (Input.GetMouseButtonDown(1))
                Player.instance.canControl = !Player.instance.canControl;
        }
    }
    #endregion

    #region[플레이어가 내부에 있는지 검사]
    public virtual void PlayerIn()
    {
        if (!Player.instance || !StageData.instance || !StageBackGround.instance)
            return;

        if (!onPlayer && AreaCheck.RectIn(Player.instance.transform.position, inRect))
        {
            Debug.Log("Area");
            onPlayer = true;
            PlayerIn(true);
            CameraController.Instance.SetOffset(12);
        }
        else if (onPlayer && !AreaCheck.RectIn(Player.instance.transform.position, outRect))
        {
            onPlayer = false;
            PlayerIn(false);
            CameraController.Instance.SetOffset(0);
        }
    }

    public void PlayerIn(bool b)
    {
        if(b)
        {
            StageBackGround.instance.FadeOut();
            areaLight.SetActive(true);
            Player.instance.playerBlockLightSource.LightColor = lightColor;
        }
        else
        {
            StageBackGround.instance.Fadein();
            areaLight.SetActive(false);
            Player.instance.playerBlockLightSource.LightColor = defaultColor;
        }
    }
    #endregion

    #region[이펙트처리]
    public void Effect(bool on)
    {
        if (particle)
            particle.SetActive(on && !act);

        if (on)
        {
            for (int i = 0; i < inSideDarkness.Length; i++)
            {
                if (inSideDarkness[i].color.a > 0)
                    inSideDarkness[i].color -= new Color(0, 0, 0, Time.deltaTime);
                else
                    inSideDarkness[i].color = new Color(inSideDarkness[i].color.r, inSideDarkness[i].color.g, inSideDarkness[i].color.b, 0);
            }
            for (int i = 0; i < outSideDarkness.Length; i++)
            {
                if (outSideDarkness[i].color.a < 1)
                    outSideDarkness[i].color += new Color(0, 0, 0, Time.deltaTime);
                else
                    outSideDarkness[i].color = new Color(outSideDarkness[i].color.r, outSideDarkness[i].color.g, outSideDarkness[i].color.b, 1);
            }
        }
        else
        {
            for (int i = 0; i < inSideDarkness.Length; i++)
            {
                if (inSideDarkness[i].color.a < 1)
                    inSideDarkness[i].color += new Color(0, 0, 0, Time.deltaTime);
                else
                    inSideDarkness[i].color = new Color(inSideDarkness[i].color.r, inSideDarkness[i].color.g, inSideDarkness[i].color.b, 1);
            }
            for (int i = 0; i < outSideDarkness.Length; i++)
            {
                if (outSideDarkness[i].color.a > 0)
                    outSideDarkness[i].color -= new Color(0, 0, 0, Time.deltaTime);
                else
                    outSideDarkness[i].color = new Color(outSideDarkness[i].color.r, outSideDarkness[i].color.g, outSideDarkness[i].color.b, 0);
            }
        }
    }
    #endregion
}
