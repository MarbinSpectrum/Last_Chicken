using System;
using System.Collections.Generic;
using TerrainEngine2D;
using UnityEngine;
using Custom;
public class StageData : TerrainGenerator
{
    public static StageData instance;

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    //Layer types
    public enum Layers { BackGround, Ground }
    //Block types
    public enum BackGroundLayer { NormalBackGround, AltarBackGround, DarkAltarBackGround }
    public enum GroundLayer {Empty = -1, Dirt, Stone, Copper, Sand, Granite, Iron, Silver, Gold, Mithril, Diamond, Magnetite, Titanium, Cobalt, Ice, UnBreakable, Grass, HearthStone,End }
    public enum FluidType { Air, Water, Poison, Lava }

    public BackGroundLayer[,] backGroundData;
    public GroundLayer[,] groundData;
    public FluidType[,] fluidData;

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public static int[,] Dic = new int[,] { { 1, 0 }, { 0, 1 }, { -1, 0 }, { 0, -1 } };
    public static int[,] Dic8 = new int[,] { { 1, 0 }, { 0, 1 }, { -1, 0 }, { 0, -1 }, { 1, 1 }, { 1, 1 }, { -1, 1 }, { -1, -1 } };

    protected int[,,] MineralType = new int[,,]
    {
        {
            { 0,0,0,0,0,0 },
            { 0,0,1,1,1,0 },
            { 0,1,1,1,1,0 },
            { 0,1,1,1,1,0 },
            { 0,1,1,1,0,0 },
            { 0,0,0,0,0,0 }
        },
        {
            { 1,1,0,0,0,0 },
            { 0,1,1,1,1,0 },
            { 0,0,1,1,1,0 },
            { 0,0,0,1,1,0 },
            { 0,0,0,0,1,1 },
            { 0,0,0,0,0,1 }
        },
        {
            { 0,0,0,0,0,0 },
            { 0,1,1,1,0,0 },
            { 0,1,1,1,1,0 },
            { 0,1,1,1,1,0 },
            { 0,0,1,0,0,0 },
            { 0,0,0,0,0,0 }
        },
        {
            { 0,0,0,0,1,0 },
            { 0,0,1,1,1,0 },
            { 0,1,1,1,1,0 },
            { 1,1,1,1,0,0 },
            { 1,1,1,0,0,0 },
            { 1,1,0,0,0,0 }
        },
    };

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    [NonSerialized] public string[] groundString;

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    [NonSerialized] public RectInt altarRect;
    [NonSerialized] public RectInt fountainRect;

    [NonSerialized] public bool[,] fluidOutline;

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region[Awake]
    public virtual void Awake()
    {
        instance = this;
        UnityEngine.Random.InitState(GameManager.instance.playData.seed);

        #region[Enum 배열화]
        List<string> groundlist = new List<string>();
        foreach (GroundLayer p in Enum.GetValues(typeof(GroundLayer)))
            groundlist.Add(p.ToString());
        groundString = groundlist.ToArray();
        #endregion


    }
    #endregion

    #region[GenerateData]
    public override void GenerateData()
    {
        base.GenerateData();
    }
    #endregion

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region[지형생성]
    public virtual void SetGround()
    {
        Debug.Log("지형생성");
    }
    #endregion

    #region[배경생성]
    public virtual void SetBackGround()
    {
        Debug.Log("지형생성");
    }
    #endregion

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region[액체가장자리 무적처리]
    public virtual void SetFluidOutline()
    {
        fluidOutline = new bool[world.WorldWidth, world.WorldHeight];

        for (int x = 0; x < world.WorldWidth; x++)
            for (int y = 0; y < world.WorldHeight; y++)
            {
                bool flag = false;
                for(int i = 0; i < 4; i++)
                {
                    int ax = x + Dic[i, 0];
                    int ay = y + Dic[i, 1];
                    if(Custom.Exception.IndexOutRange(ax,ay,fluidData) && fluidData[ax,ay] != FluidType.Air)
                        flag = true;
                }
                fluidOutline[x, y] = flag;
            }
    }
    #endregion

    #region[배경생성]
    public virtual void GenerateBackGround()
    {
        for (int x = 0; x < world.WorldWidth; x++)
            for (int y = 0; y < world.WorldHeight; y++)
                if (backGroundData[x, y] == BackGroundLayer.NormalBackGround)
                {
                    AddBlock(x, y, (byte)Layers.BackGround, (byte)backGroundData[x, y]);
                    int num = ((x % 12) + y * 12) % 144;
                    SetBlockVariation(x, y, (byte)Layers.BackGround, (byte)(num));
                }

        for (int x = 0; x < world.WorldWidth; x++)
            for (int y = 0; y < world.WorldHeight; y++)
                if (backGroundData[x, y] == BackGroundLayer.AltarBackGround)
                {
                    AddBlock(x, y, (byte)Layers.BackGround, (byte)backGroundData[x, y]);
                    int num = ((x % 12) + y * 12) % 144;
                    SetBlockVariation(x, y, (byte)Layers.BackGround, (byte)(num));
                }

        for (int x = 0; x < world.WorldWidth; x++)
            for (int y = 0; y < world.WorldHeight; y++)
                if (backGroundData[x, y] == BackGroundLayer.DarkAltarBackGround)
                {
                    AddBlock(x, y, (byte)Layers.BackGround, (byte)backGroundData[x, y]);
                    int num = ((x % 12) + y * 12) % 144;
                    SetBlockVariation(x, y, (byte)Layers.BackGround, (byte)(num));
                }
    }
    #endregion

    #region[지형생성]
    public virtual void GenerateGround()
    {
        for (int x = 0; x < world.WorldWidth; x++)
            for (int y = 0; y < world.WorldHeight; y++)
                if (groundData[x, y] != (GroundLayer)(-1))
                    AddBlock(x, y, (byte)Layers.Ground, (byte)groundData[x, y]);

        for (int x = 0; x < world.WorldWidth; x++)
            for (int y = 0; y < world.WorldHeight; y++)
                if (!IsBlockAt(x, y, world.FluidLayer) && fluidData[x, y] != 0)
                    GeneratePool(x, y, fluidDynamics.MaxWeight, (byte)fluidData[x, y], y, new Vector2Int(x, y));
    }
    #endregion

    #region[지형 데이터]
    public GroundLayer GetBlock(Vector2Int pos)
    {
        return GetBlock(pos.x, pos.y);
    }

    public GroundLayer GetBlock(int x, int y)
    {
        if(x < 0 || x >= world.WorldWidth || y < 0 || y >= world.WorldHeight)
            return (GroundLayer)(-1);

        if (IsBlockAt(x, y, (byte)Layers.Ground))
        {
            BlockInfo blockInfo = world.GetBlockLayer((int)Layers.Ground).GetBlockInfo(x, y);
            string emp = blockInfo.Name;
            for (int i = 0; i < groundString.Length; i++)
                if (groundString[i].Equals(emp))
                    return (GroundLayer)(i);
            return (GroundLayer)(-1);

        }
        return (GroundLayer)(-1);
    }

    public void SetBlock(int x, int y, BackGroundLayer background)
    {
        RemoveAllBlocks(x, y);
        AddBlock(x, y, (byte)Layers.BackGround, (byte)background);
        backGroundData[x, y] = background;
        World.Instance.UpdateBitmask(x, y, 3, 3, (byte)Layers.BackGround);
        World.Instance.chunkLoader.UpdateChunk(x, y);
    }

    public void SetBlock(int x, int y, GroundLayer ground)
    {
        World.Instance.RemoveBlock(x, y, (byte)Layers.Ground);
        AddBlock(x, y, (byte)Layers.Ground, (byte)ground);
        groundData[x, y] = ground;
        World.Instance.UpdateBitmask(x, y, 1, 1, (byte)Layers.Ground);
        World.Instance.chunkLoader.UpdateChunk(x, y);
    }

    public void RemoveBlock(int x, int y)
    {
        World.Instance.RemoveBlock(x, y, (byte)Layers.Ground);
        groundData[x, y] = (GroundLayer)(-1);
    }

    public void RemoveBlock(Vector2Int pos)
    {
        World.Instance.RemoveBlock(pos.x, pos.y, (byte)Layers.Ground);
    }
    #endregion

    #region[블록 바리레이션]
    public int GetBlockVariation(int x, int y, byte layer)
    {
        if (IsBlockAt(x, y, layer))
        {
            BlockLayer blockLayer = world.GetBlockLayer(layer);
            return blockLayer.GetVariation(x, y);
        }
        return 0;
    }

    public void SetBlockVariation(int x, int y, byte layer, byte n)
    {
        if (IsBlockAt(x, y, layer))
        {
            BlockLayer blockLayer = world.GetBlockLayer(layer);
            blockLayer.SetVariation(x, y, n);

            World.Instance.UpdateBitmask(x, y, 1, 1, layer);
            World.Instance.chunkLoader.UpdateChunk(x, y);
        }
    }
    #endregion

    #region[절차적지형]
    public void ProceduralGeneration<T>(T[,] array, T a, int num = 4)
    {
        ProceduralGeneration<T>(world, array, a, num);
    }
    public void ProceduralGeneration<T>(World world,T[,] array, T a, int num = 4)
    {
        while (num-- > 0)
        {
            T[,] emp = new T[world.WorldWidth, world.WorldHeight];
            T[,] oldData = new T[world.WorldWidth, world.WorldHeight];
            for (int i = 0; i < world.WorldWidth; i++)
                for (int j = 0; j < world.WorldHeight; j++)
                    oldData[i, j] = array[i, j];

            for (int i = 0; i < world.WorldWidth; i++)
            {
                for (int j = 0; j < world.WorldHeight; j++)
                {
                    int c = 0;
                    int ax = 0;
                    int ay = 0;
                    for (int k = 0; k < 9; k++)
                    {
                        ax = i + (k / 3) - 1;
                        ay = j + (k % 3) - 1;
                        if (ax >= 0 && ax < world.WorldWidth && ay >= 0 && ay < world.WorldHeight)
                            if (array[ax, ay].Equals(a))
                                c++;
                    }

                    if (c >= 5)
                        emp[i, j] = a;
                    else
                        emp[i, j] = oldData[i, j];
                }
            }
            for (int i = 0; i < world.WorldWidth; i++)
                for (int j = 0; j < world.WorldHeight; j++)
                    array[i, j] = emp[i, j];
        }

    }
    #endregion

    #region[제단 발동시 배경변경]
    public void AltarBackGroundSwap(bool act)
    {
        if(act)
        {
            for (int x = 0; x < world.WorldWidth; x++)
                for (int y = 0; y < world.WorldHeight; y++)
                    if (backGroundData[x, y] == BackGroundLayer.DarkAltarBackGround)
                        SetBlock(x, y, BackGroundLayer.AltarBackGround);
        }
        else
        {
            for (int x = 0; x < world.WorldWidth; x++)
                for (int y = 0; y < world.WorldHeight; y++)
                    if (backGroundData[x, y] == BackGroundLayer.AltarBackGround)
                    {
                        backGroundData[x, y] = BackGroundLayer.DarkAltarBackGround;
                        //AddBlock(x, y, (byte)Layers.BackGround, (byte)backGroundData[x, y]);
                        SetBlock(x, y, backGroundData[x, y]);
                        int num = ((x % 12) + y * 12) % 144;
                        SetBlockVariation(x, y, (byte)Layers.BackGround, (byte)(num));
                    }
        }
    }
    #endregion

    #region[먼지생성]
    public virtual void SetDust()
    {
        for(int i = 0; i < 1000; i++)
        {
            float x = UnityEngine.Random.Range(0, (float)groundData.GetLength(0));
            float y = UnityEngine.Random.Range(0, (float)groundData.GetLength(1));
            EffectManager.instance.Dust(new Vector2(x, y));
        }
    }
    #endregion
}

