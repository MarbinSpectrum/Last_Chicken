using Custom;
using System.Collections.Generic;
using UnityEngine;
using TerrainEngine2D;

public class ShopMap0101 : StageData
{
    public GameObject leftWall;
    public GameObject rightWall;
    bool treasureFlip = false;

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region[Awake]
    public override void Awake()
    {
        base.Awake();

        List<Texture2D> variation = new List<Texture2D>();
        for (int i = 0; i < 5; i++)
        {
            Texture2D temp = Resources.Load("TerrainData/ShopMap0101/Variation" + i) as Texture2D;
            if (temp)
                variation.Add(temp);
        }

        if (variation.Count <= 0)
            return;

        int r = Random.Range(0, variation.Count);
        GroundManager.instance.shopMap0101Data = variation[r];
        int activeWall = 0;
        switch (r)
        {
            case 0:
                GroundManager.instance.shopMap0101StartDic = -1;
                treasureFlip = true;
                activeWall = 0;
                break;
            case 1:
                GroundManager.instance.shopMap0101StartDic = +1;
                treasureFlip = true;
                activeWall = 1;
                break;
            case 2:
                GroundManager.instance.shopMap0101StartDic = +1;
                treasureFlip = true;
                activeWall = 1;
                break;
            case 3:
                GroundManager.instance.shopMap0101StartDic = -1;
                treasureFlip = true;
                activeWall = 0;
                break;
            case 4:
                GroundManager.instance.shopMap0101StartDic = +1;
                treasureFlip = false;
                activeWall = 0;
                break;
        }
        bool filpX = Random.Range(0, 100) > 50;
        Texture2D mapData = new Texture2D(GroundManager.instance.shopMap0101Data.width, GroundManager.instance.shopMap0101Data.height);
        for (int y = 0; y < mapData.height; y++)
            for (int x = 0; x < mapData.width; x++)
                mapData.SetPixel(filpX ? mapData.width - x - 1 : x, y, GroundManager.instance.shopMap0101Data.GetPixel(x, y));

        if (filpX)
        {
            if (activeWall == 0)
                activeWall = 1;
            else if (activeWall == 1)
                activeWall = 0;

            treasureFlip = !treasureFlip;
            if (GroundManager.instance.shopMap0101StartDic == 1)
                GroundManager.instance.shopMap0101StartDic = -1;
            else if (GroundManager.instance.shopMap0101StartDic == -1)
                GroundManager.instance.shopMap0101StartDic = +1;
        }

        GroundManager.instance.shopMap0101Data = mapData;

        GroundManager.instance.shopMap0101Rect = new GroundLayer[GroundManager.instance.shopMap0101Data.width, GroundManager.instance.shopMap0101Data.height];
        GroundManager.instance.shopMap0101Fluid = new FluidType[GroundManager.instance.shopMap0101Data.width, GroundManager.instance.shopMap0101Data.height];

        GroundManager.instance.shopMap0101ShopPos = Vector2.zero;
        GroundManager.instance.shopMap0101TreasurePos = Vector2.zero;

        for (int i = 0; i < GroundManager.instance.shopMap0101Data.width; i++)
            for (int j = 0; j < GroundManager.instance.shopMap0101Data.height; j++)
                if (GroundManager.instance.shopMap0101Data.GetPixel(i, j) == new Color(0.2f, 0, 0))
                    GroundManager.instance.shopMap0101StartPos = new Vector2(i, j);
                else if (GroundManager.instance.shopMap0101Data.GetPixel(i, j) == new Color(0.2f, 0.2f, 0))
                    GroundManager.instance.shopMap0101ShopPos = new Vector2(i, j);
                else if (GroundManager.instance.shopMap0101Data.GetPixel(i, j) == new Color(0.2f, 0.2f, 0.2f))
                    GroundManager.instance.shopMap0101TreasurePos = new Vector2(i, j);

        for (int i = 0; i < GroundManager.instance.shopMap0101Data.width; i++)
            for (int j = 0; j < GroundManager.instance.shopMap0101Data.height; j++)
                GroundManager.instance.shopMap0101Rect[i, j] = GroundManager.instance.ColorToGroundData(GroundManager.instance.shopMap0101Data.GetPixel(i, j));

        for (int i = 0; i < GroundManager.instance.shopMap0101Data.width; i++)
            for (int j = 0; j < GroundManager.instance.shopMap0101Data.height; j++)
                GroundManager.instance.shopMap0101Fluid[i, j] = GroundManager.instance.ColorToFluidData(GroundManager.instance.shopMap0101Data.GetPixel(i, j));

        switch (activeWall)
        {
            case 0:
                leftWall.SetActive(false);
                rightWall.SetActive(true);
                break;
            case 1:
                leftWall.SetActive(true);
                rightWall.SetActive(false);
                break;
            case 2:
                leftWall.SetActive(true);
                rightWall.SetActive(true);
                break;
        }
    }
    #endregion

    #region[GenerateData]
    public override void GenerateData()
    {
        base.GenerateData();

        SetGround();
        SetBackGround();
        SetFluidOutline();
        SetObject();
        GenerateBackGround();
        GenerateGround();

        GroundManager.instance.Init(world);
    }
    #endregion

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region[배경 설정]
    void SetBackGround()
    {
        backGroundData = new BackGroundLayer[world.WorldWidth, world.WorldHeight];
        for (int x = 0; x < world.WorldWidth; x++)
            for (int y = 0; y < world.WorldHeight; y++)
                backGroundData[x, y] = (BackGroundLayer)(-1);

        for (int x = 0; x < world.WorldWidth; x++)
            for (int y = 0; y < world.WorldHeight; y++)
                backGroundData[x, y] = BackGroundLayer.NormalBackGround;
    }
    #endregion

    #region[지형 설정]
    public override void SetGround()
    {
        groundData = new GroundLayer[world.WorldWidth, world.WorldHeight];
        fluidData = new FluidType[world.WorldWidth, world.WorldHeight];

        for (int i = 0; i < GroundManager.instance.shopMap0101Rect.GetLength(0); i++)
            for (int j = 0; j < GroundManager.instance.shopMap0101Rect.GetLength(1); j++)
                groundData[i,j] = GroundManager.instance.shopMap0101Rect[i, j];

        for (int i = 0; i < GroundManager.instance.shopMap0101Rect.GetLength(0); i++)
            for (int j = 0; j < GroundManager.instance.shopMap0101Rect.GetLength(1); j++)
                fluidData[i, j] = GroundManager.instance.shopMap0101Fluid[i, j];
    }
    #endregion

    #region[배경생성]
    public override void GenerateBackGround()
    {
        #region[스테이지배경 생성]
        for (int x = 0; x < world.WorldWidth; x++)
            for (int y = 0; y < world.WorldHeight; y++)
                if (backGroundData[x, y] == BackGroundLayer.NormalBackGround)
                {
                    AddBlock(x, y, (byte)Layers.BackGround, (byte)backGroundData[x, y]);
                    SetBlockVariation(x, y, (byte)Layers.BackGround, (byte)UnityEngine.Random.Range(16, 32));
                }

        for (int x = 0; x < world.WorldWidth; x++)
            for (int y = 0; y < world.WorldHeight; y++)
                if (backGroundData[x, y] == BackGroundLayer.NormalBackGround && GetBlockVariation(x, y, (byte)Layers.BackGround) >= 16)
                {
                    int r = UnityEngine.Random.Range(16, 100);
                    #region[돌생성타입1]
                    if (r == 32)
                        SetStone(x, y, 0, 4);
                    #endregion

                    #region[돌생성타입2]
                    else if (r == 33)
                        SetStone(x, y, 4, 2);
                    #endregion

                    #region[돌생성타입3]
                    else if (r == 34)
                        SetStone(x, y, 6, 1);
                    #endregion

                    #region[돌생성타입4]
                    else if (r == 35)
                        SetStone(x, y, 7, 4);
                    #endregion

                    #region[돌생성타입5]
                    else if (r == 36)
                        SetStone(x, y, 11, 1);
                    #endregion

                    #region[돌생성타입6]
                    else if (r == 37)
                        SetStone(x, y, 12, 2);
                    #endregion

                    #region[돌생성타입7]
                    else if (r == 38)
                        SetStone(x, y, 14, 1);
                    #endregion

                    #region[돌생성타입8]
                    else if (r == 39)
                        SetStone(x, y, 15, 1);
                    #endregion

                    #region[기타]
                    else
                        SetBlockVariation(x, y, (byte)Layers.BackGround, (byte)UnityEngine.Random.Range(16, 32));
                    #endregion
                }

        #region[돌생성함수]
        void SetStone(int x, int y, int a, int n)
        {
            bool check = true;
            for (int i = 0; i < n; i++)
            {
                int ax = x + i % 2;
                int ay = y - i / 2;
                if (Custom.Exception.IndexOutRange(ax, ay, backGroundData))
                    check = check &&
                        (GetBlockVariation(ax, ay, (byte)Layers.BackGround) >= 16)
                        && backGroundData[ax, ay] == BackGroundLayer.NormalBackGround;
                else
                    check = false;
            }
            if (check)
            {
                for (int i = 0; i < n; i++)
                {
                    int ax = x + i % 2;
                    int ay = y - i / 2;
                    SetBlockVariation(ax, ay, (byte)Layers.BackGround, (byte)(i + a));
                }
            }
            else
                SetBlockVariation(x, y, (byte)Layers.BackGround, (byte)UnityEngine.Random.Range(16, 32));
        }
        #endregion

        #endregion

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

    #region[오브젝트 설치]
    void SetObject()
    {
        if (GroundManager.instance.shopMap0101ShopPos != Vector2.zero)
            ObjectManager.instance.Shop(GroundManager.instance.shopMap0101ShopPos);

        if (GroundManager.instance.shopMap0101TreasurePos != Vector2.zero)
            ObjectManager.instance.TreasureBox(GroundManager.instance.shopMap0101TreasurePos, treasureFlip);

    }
    #endregion

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

}
