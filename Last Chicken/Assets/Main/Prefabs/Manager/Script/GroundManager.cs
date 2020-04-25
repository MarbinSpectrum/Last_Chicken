using Custom;
using System.Collections.Generic;
using TerrainEngine2D;
using UnityEngine;

public class GroundManager : MonoBehaviour
{
    public static GroundManager instance;

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    Material fluidSurface;
    float changeTime = 0;
    int surfaceImageNum = 0;
    Texture2D []surfaceTex = new Texture2D[8];


    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public int dirtHp = 0;
    public Sprite dirtMineral;
    public int dirtValue;

    public int stoneHp = 0;
    public Sprite stoneMineral;
    public int stoneValue;

    public int copperHp = 0;
    public Sprite copperMineral;
    public int copperValue;

    public int sandHp = 0;

    public int graniteHp = 0;
    public Sprite graniteMineral;
    public int graniteValue;

    public int ironHp = 0;
    public Sprite ironMineral;
    public int ironValue;

    public int silverHp = 0;
    public Sprite silverMineral;
    public int silverValue;

    public int goldHp = 0;
    public Sprite goldMineral;
    public int goldValue;

    public int mithrilHp = 0;
    public Sprite mithrilMineral;
    public int mithrilValue;

    public int diamondHp = 0;
    public Sprite diamondMineral;
    public int diamondValue;

    public int magnetiteHp = 0;
    public Sprite magnetiteMineral;
    public int magnetiteValue;

    public int titaniumHp = 0;
    public Sprite titaniumMineral;
    public int titaniumValue;

    public int cobaltHp = 0;
    public Sprite cobaltMineral;
    public int cobaltValue;

    public int iceHp = 0;

    public int grassHp = 0;

    public int hearthStoneHp = 0;

    public int[,] groundHp;
    public bool[,] linkArea;

    public List<Vector2Int> linkList = new List<Vector2Int>();
    public List<Vector2Int> linkAreaList = new List<Vector2Int>();

    public int digMask;

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    Texture2D altarData;
    public StageData.GroundLayer[,] altarRect;

    Texture2D fountainData;
    public StageData.GroundLayer[,] fountainRect;

    Texture2D mineAreaData;
    public StageData.GroundLayer[,] mineAreaRect;

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    Texture2D tutorialData;
    public StageData.GroundLayer[,] tutorialRect;
    public StageData.FluidType[,] tutorialFluid;

    Texture2D tutorialBackData;
    public StageData.BackGroundLayer[,] tutorialBackGround;

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    Texture2D stage01Outline;
    public StageData.GroundLayer[,] stage01OutlineRect;

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public StageData.GroundLayer[,] shopMap0101Rect;
    public StageData.FluidType[,] shopMap0101Fluid;
    public Texture2D shopMap0101Data;
    public Vector2 shopMap0101StartPos;
    public int shopMap0101StartDic;
    public Vector2 shopMap0101ShopPos;
    public Vector2 shopMap0101TreasurePos;

    public StageData.GroundLayer[,] smithyMap0101Rect;
    public StageData.FluidType[,] smithyMap0101Fluid;
    public Texture2D smithyMap0101Data;
    public Vector2 smithyMap0101StartPos;
    public int smithyMap0101StartDic;
    public Vector2 smithyMap0101SmithyPos;
    public Vector2 smithyMap0101TreasurePos;
    public Texture2D smithyMap0101BackData;
    public StageData.BackGroundLayer[,] smithyMap0101BackGround;

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public StageData.GroundLayer[,] shopMap0102Rect;
    public StageData.FluidType[,] shopMap0102Fluid;
    public Texture2D shopMap0102Data;
    public Vector2 shopMap0102StartPos;
    public int shopMap0102StartDic;
    public Vector2 shopMap0102ShopPos;
    public Vector2 shopMap0102TreasurePos;

    public StageData.GroundLayer[,] smithyMap0102Rect;
    public StageData.FluidType[,] smithyMap0102Fluid;
    public Texture2D smithyMap0102Data;
    public Vector2 smithyMap0102StartPos;
    public int smithyMap0102StartDic;
    public Vector2 smithyMap0102SmithyPos;
    public Vector2 smithyMap0102TreasurePos;
    public Texture2D smithyMap0102BackData;
    public StageData.BackGroundLayer[,] smithyMap0102BackGround;

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public StageData.GroundLayer[,] shopMap0103Rect;
    public StageData.FluidType[,] shopMap0103Fluid;
    public Texture2D shopMap0103Data;
    public Vector2 shopMap0103StartPos;
    public int shopMap0103StartDic;
    public Vector2 shopMap0103ShopPos;
    public Vector2 shopMap0103TreasurePos;

    public StageData.GroundLayer[,] smithyMap0103Rect;
    public StageData.FluidType[,] smithyMap0103Fluid;
    public Texture2D smithyMap0103Data;
    public Vector2 smithyMap0103StartPos;
    public int smithyMap0103StartDic;
    public Vector2 smithyMap0103SmithyPos;
    public Vector2 smithyMap0103TreasurePos;
    public Texture2D smithyMap0103BackData;
    public StageData.BackGroundLayer[,] smithyMap0103BackGround;

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region[Awake]
    void Awake()
    {
        if (instance == null)
        {
            instance = this;

            fluidSurface = Resources.Load("TerrainData/FluidSurface/FluidSurfaceMesh") as Material;

            for (int i = 0; i < surfaceTex.Length; i++)
                surfaceTex[i] = Resources.Load("TerrainData/FluidSurface/FluidSurface" + i) as Texture2D;


            Texture2D temp;

            temp = Resources.Load("Objects/Item/Mineral/Dirt") as Texture2D;
            dirtMineral = Sprite.Create(temp, new Rect(0, 0, temp.width, temp.height), new Vector2(0.5f, 0.5f));
            temp = Resources.Load("Objects/Item/Mineral/Stone") as Texture2D;
            stoneMineral = Sprite.Create(temp, new Rect(0, 0, temp.width, temp.height), new Vector2(0.5f, 0.5f));
            temp = Resources.Load("Objects/Item/Mineral/Copper") as Texture2D;
            copperMineral = Sprite.Create(temp, new Rect(0, 0, temp.width, temp.height), new Vector2(0.5f, 0.5f));
            temp = Resources.Load("Objects/Item/Mineral/Granite") as Texture2D;
            graniteMineral = Sprite.Create(temp, new Rect(0, 0, temp.width, temp.height), new Vector2(0.5f, 0.5f));
            temp = Resources.Load("Objects/Item/Mineral/Iron") as Texture2D;
            ironMineral = Sprite.Create(temp, new Rect(0, 0, temp.width, temp.height), new Vector2(0.5f, 0.5f));
            temp = Resources.Load("Objects/Item/Mineral/Silver") as Texture2D;
            silverMineral = Sprite.Create(temp, new Rect(0, 0, temp.width, temp.height), new Vector2(0.5f, 0.5f));
            temp = Resources.Load("Objects/Item/Mineral/Gold") as Texture2D;
            goldMineral = Sprite.Create(temp, new Rect(0, 0, temp.width, temp.height), new Vector2(0.5f, 0.5f));
            temp = Resources.Load("Objects/Item/Mineral/Mithril") as Texture2D;
            mithrilMineral = Sprite.Create(temp, new Rect(0, 0, temp.width, temp.height), new Vector2(0.5f, 0.5f));
            temp = Resources.Load("Objects/Item/Mineral/Diamond") as Texture2D;
            diamondMineral = Sprite.Create(temp, new Rect(0, 0, temp.width, temp.height), new Vector2(0.5f, 0.5f));
            temp = Resources.Load("Objects/Item/Mineral/Magnetite") as Texture2D;
            magnetiteMineral = Sprite.Create(temp, new Rect(0, 0, temp.width, temp.height), new Vector2(0.5f, 0.5f));
            temp = Resources.Load("Objects/Item/Mineral/Titanium") as Texture2D;
            titaniumMineral = Sprite.Create(temp, new Rect(0, 0, temp.width, temp.height), new Vector2(0.5f, 0.5f));
            temp = Resources.Load("Objects/Item/Mineral/Cobalt") as Texture2D;
            cobaltMineral = Sprite.Create(temp, new Rect(0, 0, temp.width, temp.height), new Vector2(0.5f, 0.5f));

            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            tutorialData = Resources.Load("TerrainData/TutorialData") as Texture2D;
            tutorialBackData = Resources.Load("TerrainData/TutorialBackData") as Texture2D;
            tutorialRect = new StageData.GroundLayer[tutorialData.width, tutorialData.height];
            tutorialFluid = new StageData.FluidType[tutorialData.width, tutorialData.height];
            tutorialBackGround = new StageData.BackGroundLayer[tutorialBackData.width, tutorialBackData.height];
            for (int i = 0; i < tutorialData.width; i++)
                for (int j = 0; j < tutorialData.height; j++)
                {
                    tutorialRect[i, j] = ColorToGroundData(tutorialData.GetPixel(i, j));
                    tutorialFluid[i, j] = ColorToFluidData(tutorialData.GetPixel(i, j));
                    tutorialBackGround[i, j] = ColorToBackData(tutorialBackData.GetPixel(i, j));
                }

            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            stage01Outline = Resources.Load("TerrainData/StageOutline/StageOutline01") as Texture2D;
            stage01OutlineRect = new StageData.GroundLayer[stage01Outline.width, stage01Outline.height];
            for (int i = 0; i < stage01Outline.width; i++)
                for (int j = 0; j < stage01Outline.height; j++)
                    stage01OutlineRect[i, j] = ColorToGroundData(stage01Outline.GetPixel(i, j));

            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            altarData = Resources.Load("TerrainData/AltarData") as Texture2D;
            altarRect = new StageData.GroundLayer[altarData.width, altarData.height];
            for (int i = 0; i < altarData.width; i++)
                for (int j = 0; j < altarData.height; j++)
                    altarRect[i, j] = ColorToGroundData(altarData.GetPixel(i, j));

            fountainData = Resources.Load("TerrainData/FountainData") as Texture2D;
            fountainRect = new StageData.GroundLayer[fountainData.width, fountainData.height];
            for (int i = 0; i < fountainData.width; i++)
                for (int j = 0; j < fountainData.height; j++)
                    fountainRect[i, j] = ColorToGroundData(fountainData.GetPixel(i, j));

            mineAreaData = Resources.Load("TerrainData/MineAreaData") as Texture2D;
            mineAreaRect = new StageData.GroundLayer[mineAreaData.width, mineAreaData.height];
            for (int i = 0; i < mineAreaData.width; i++)
                for (int j = 0; j < mineAreaData.height; j++)
                    mineAreaRect[i, j] = ColorToGroundData(mineAreaData.GetPixel(i, j));

            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        }
    }
    #endregion

    #region[Start]
    private void Start()
    {

    }
    #endregion

    #region[Update]
    private void Update()
    {
        SurfaceFluid();
    }
    #endregion

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region[SurfaceFluid]
    public void SurfaceFluid()
    {
        if(!GameManager.instance.InGame())
        {
            changeTime = 0;
            surfaceImageNum = 0;
        }
        else
        {
            if (changeTime > 0.1f)
            {
                changeTime = 0;
                surfaceImageNum++;
            }
            changeTime += Time.deltaTime;
            if (fluidSurface)
                fluidSurface.SetTexture("_MainTex", surfaceTex[surfaceImageNum % surfaceTex.Length]);
        }
    }
    #endregion

    #region[초기설정]
    public void Init(World world)
    {
        linkArea = new bool[world.WorldWidth, world.WorldHeight];
        groundHp = new int[world.WorldWidth, world.WorldHeight];
        linkArea.Initialize();
        linkList.Clear();
        linkAreaList.Clear();
        LinkArea(world.WorldWidth / 2, world.WorldHeight - 1);
        SetGroundHp(world);
    }
    #endregion

    #region[지형 데미지 마스크 설정]
    public void InitDigMask()
    {
        digMask = 0;
        AllDigMask();
        digMask = digMask - (1 << (int)StageData.GroundLayer.UnBreakable);
    }

    public void AllDigMask()
    {
        for (int i = 0; i < 17; i++)
            digMask = digMask | (1 << i);
    }
    #endregion

    #region[땅 체력설정]
    public void SetGroundHp(World world)
    {
        for (int x = 0; x < world.WorldWidth; x++)
            for (int y = 0; y < world.WorldHeight; y++)
                groundHp[x, y] = GetBlockMaxHp(x, y);
    }
    #endregion

    #region[블록의 최대 체력 구하기]
    public int GetBlockMaxHp(int x, int y)
    {
        int hp = 0;
        switch (StageData.instance.groundData[x, y])
        {
            case StageData.GroundLayer.Dirt:
                hp = dirtHp; break;
            case StageData.GroundLayer.Stone:
                hp = stoneHp; break;
            case StageData.GroundLayer.Copper:
                hp = copperHp; break;
            case StageData.GroundLayer.Sand:
                hp = sandHp; break;
            case StageData.GroundLayer.Granite:
                hp = graniteHp; break;
            case StageData.GroundLayer.Iron:
                hp = ironHp; break;
            case StageData.GroundLayer.Silver:
                hp = silverHp; break;
            case StageData.GroundLayer.Gold:
                hp = goldHp; break;
            case StageData.GroundLayer.Mithril:
                hp = mithrilHp; break;
            case StageData.GroundLayer.Diamond:
                hp = diamondHp; break;
            case StageData.GroundLayer.Magnetite:
                hp = magnetiteHp; break;
            case StageData.GroundLayer.Titanium:
                hp = titaniumHp; break;
            case StageData.GroundLayer.Cobalt:
                hp = cobaltHp; break;
            case StageData.GroundLayer.Ice:
                hp = iceHp; break;
            case StageData.GroundLayer.Grass:
                hp = grassHp; break;
            case StageData.GroundLayer.HearthStone:
                hp = hearthStoneHp; break;
        }
        return hp;
    }
    #endregion

    #region[플레이어와 연결된 땅에 추가]
    public void LinkArea(int x, int y)
    {
        if (!Exception.IndexOutRange(x, y, linkArea) || linkArea[x, y])
            return;
        linkList.Add(new Vector2Int(x, y));
        linkAreaList.Add(new Vector2Int(x, y));
        int[,] offset = { { 0, 1 }, { 0, -1 }, { -1, 0 }, { 1, 0 } };
        while (linkList.Count > 0)
        {
            Vector2Int emp = linkList[0];
            linkList.RemoveAt(0);
            linkArea[emp.x, emp.y] = true;
            for (int i = 0; i < 4; i++)
            {
                int ax = emp.x + offset[i, 0];
                int ay = emp.y + offset[i, 1];
                if (Exception.IndexOutRange(ax, ay, linkArea) && !linkArea[ax, ay] && StageData.instance.GetBlock(ax, ay) == (StageData.GroundLayer)(-1))
                {
                    linkList.Add(new Vector2Int(ax, ay));
                    linkArea[ax, ay] = true;
                }
            }
        }
    }

    public void LinkArea(Vector2Int pos)
    {
        LinkArea(pos.x, pos.y);
    }
    #endregion

    #region[지형정보 변환]

    /*
        Color(0.2f, 0, 0) == 시작지점
        Color(0.2f, 0.2f, 0) == 상점
        Color(0.2f, 0.2f, 0.2f) == 보물상자
        Color(0, 0, 1) == Water
        Color(0, 1, 0) == Poison
        Color(0, 1, 1) == Lava
    */

    public Color GrounddataToColor(StageData.GroundLayer data)
    {
        switch (data)
        {
            case StageData.GroundLayer.Dirt: return new Color(1, 0, 0); //Dirt
            case StageData.GroundLayer.Stone: return new Color(1, 0, 0.25f); //Stone
            case StageData.GroundLayer.Copper: return new Color(1, 0, 0.5f); //Copper
            case StageData.GroundLayer.Sand: return new Color(1, 0, 0.75f); //Sand
            case StageData.GroundLayer.Granite: return new Color(1, 0, 1); //Granite
            case StageData.GroundLayer.Iron: return new Color(1, 0.25f, 0); //Iron
            case StageData.GroundLayer.Silver: return new Color(1, 0.25f, 0.25f); //Silver
            case StageData.GroundLayer.Gold: return new Color(1, 0.25f, 0.75f); //Gold
            case StageData.GroundLayer.Mithril: return new Color(1, 0.25f, 1); //Mithril
            case StageData.GroundLayer.Diamond: return new Color(1, 0.5f, 0); //Diamond
            case StageData.GroundLayer.Magnetite: return new Color(1, 0.5f, 0.25f); //Magnetite
            case StageData.GroundLayer.Titanium: return new Color(1, 0.5f, 0.5f); //Titanum
            case StageData.GroundLayer.Cobalt: return new Color(1, 0.5f, 0.75f); //Cobalt
            case StageData.GroundLayer.Ice: return new Color(1, 0.75f, 0); //Ice
            case StageData.GroundLayer.UnBreakable: return new Color(1, 0.75f, 0.25f); //NonBreak
            case StageData.GroundLayer.Grass: return new Color(1, 0.75f, 0.5f); //Grass
            case StageData.GroundLayer.HearthStone: return new Color(1, 0.75f, 0.75f); //HeathStone
            default:
                return new Color(1, 1, 1); //Empty
        }
    }

    public StageData.GroundLayer ColorToGroundData(Color color)
    {
        float ar, ag, ab;
        color = new Color(Mathf.Ceil(color.r * 100) / 100f, Mathf.Ceil(color.g * 100) / 100f, Mathf.Ceil(color.b * 100) / 100f);
        ar = (color.r * 100 - (color.r * 100 % 5)) / 100f;
        ag = (color.g * 100 - (color.g * 100 % 5)) / 100f;
        ab = (color.b * 100 - (color.b * 100 % 5)) / 100f;

        color = new Color(ar, ag, ab);

        if (color == new Color(1, 0, 0))//Dirt
            return StageData.GroundLayer.Dirt;
        if (color == new Color(1, 0, 0.25f)) //Stone
            return StageData.GroundLayer.Stone;
        if (color == new Color(1, 0, 0.5f)) //Copper
            return StageData.GroundLayer.Copper;
        if (color == new Color(1, 0, 0.75f)) //Sand
            return StageData.GroundLayer.Sand;
        if (color == new Color(1, 0, 1)) //Granite
            return StageData.GroundLayer.Granite;
        if (color == new Color(1, 0.25f, 0)) //Iron
            return StageData.GroundLayer.Iron;
        if (color == new Color(1, 0.25f, 0.25f)) //Silver
            return StageData.GroundLayer.Silver;
        if (color == new Color(1, 0.25f, 0.75f)) //Gold
            return StageData.GroundLayer.Gold;
        if (color == new Color(1, 0.25f, 1)) //Mithril
            return StageData.GroundLayer.Mithril;
        if (color == new Color(1, 0.5f, 0)) //Diamond
            return StageData.GroundLayer.Diamond;
        if (color == new Color(1, 0.5f, 0.25f)) //Magnetite
            return StageData.GroundLayer.Magnetite;
        if (color == new Color(1, 0.5f, 0.5f)) //Titanum
            return StageData.GroundLayer.Titanium;
        if (color == new Color(1, 0.5f, 0.75f)) //Cobalt
            return StageData.GroundLayer.Cobalt;
        if (color == new Color(1, 0.75f, 0)) //Ice
            return StageData.GroundLayer.Ice;
        if (color == new Color(1, 0.75f, 0.25f)) //NonBreak
            return StageData.GroundLayer.UnBreakable;
        if (color == new Color(1, 0.75f, 0.5f)) //Grass
            return StageData.GroundLayer.Grass;
        if (color == new Color(1, 0.75f, 0.75f)) //HeathStone
            return StageData.GroundLayer.HearthStone;
        if (color == new Color(1, 1, 1)) //Empty
            return (StageData.GroundLayer)(-1);
        return (StageData.GroundLayer)(-1);
    }

    public StageData.FluidType ColorToFluidData(Color color)
    {
        if (color == new Color(0, 0, 1))
            return StageData.FluidType.Water;
        if (color == new Color(0, 1, 0))
            return StageData.FluidType.Poison;
        if (color == new Color(0, 1, 1)) 
            return StageData.FluidType.Lava;
        return StageData.FluidType.Air;
    }

    public StageData.BackGroundLayer ColorToBackData(Color color)
    {
        if (color == new Color(0, 0, 0))
            return StageData.BackGroundLayer.NormalBackGround;
        if (color == new Color(0, 1, 0))
            return StageData.BackGroundLayer.AltarBackGround;
        if (color == new Color(0, 1, 1))
            return StageData.BackGroundLayer.DarkAltarBackGround;
        return (StageData.BackGroundLayer)(-1);
    }
    #endregion
}