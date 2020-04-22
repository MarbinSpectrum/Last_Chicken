using Custom;
using System.Collections.Generic;
using UnityEngine;
using TerrainEngine2D;

public class ShopMap0102 : StageData
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
            Texture2D temp = Resources.Load("TerrainData/ShopMap0102/Variation" + i) as Texture2D;
            if (temp)
                variation.Add(temp);
        }

        if (variation.Count <= 0)
            return;

        int r = Random.Range(0, variation.Count);
        GroundManager.instance.shopMap0102Data = variation[r];
        int activeWall = 0;
        switch (r)
        {
            case 0:
                GroundManager.instance.shopMap0102StartDic = -1;
                treasureFlip = true;
                activeWall = 0;
                break;
            case 1:
                GroundManager.instance.shopMap0102StartDic = +1;
                treasureFlip = true;
                activeWall = 1;
                break;
            case 2:
                GroundManager.instance.shopMap0102StartDic = +1;
                treasureFlip = true;
                activeWall = 1;
                break;
            case 3:
                GroundManager.instance.shopMap0102StartDic = +1;
                treasureFlip = true;
                activeWall = 1;
                break;
            case 4:
                GroundManager.instance.shopMap0102StartDic = +1;
                treasureFlip = true;
                activeWall = 0;
                break;
        }
        bool filpX = Random.Range(0, 100) > 50;
        Texture2D mapData = new Texture2D(GroundManager.instance.shopMap0102Data.width, GroundManager.instance.shopMap0102Data.height);
        for (int y = 0; y < mapData.height; y++)
            for (int x = 0; x < mapData.width; x++)
                mapData.SetPixel(filpX ? mapData.width - x - 1 : x, y, GroundManager.instance.shopMap0102Data.GetPixel(x, y));

        if (filpX)
        {
            if (activeWall == 0)
                activeWall = 1;
            else if (activeWall == 1)
                activeWall = 0;

            treasureFlip = !treasureFlip;
            if (GroundManager.instance.shopMap0102StartDic == 1)
                GroundManager.instance.shopMap0102StartDic = -1;
            else if (GroundManager.instance.shopMap0102StartDic == -1)
                GroundManager.instance.shopMap0102StartDic = +1;
        }

        GroundManager.instance.shopMap0102Data = mapData;

        GroundManager.instance.shopMap0102Rect = new GroundLayer[GroundManager.instance.shopMap0102Data.width, GroundManager.instance.shopMap0102Data.height];
        GroundManager.instance.shopMap0102Fluid = new FluidType[GroundManager.instance.shopMap0102Data.width, GroundManager.instance.shopMap0102Data.height];

        GroundManager.instance.shopMap0102ShopPos = Vector2.zero;
        GroundManager.instance.shopMap0102TreasurePos = Vector2.zero;

        for (int i = 0; i < GroundManager.instance.shopMap0102Data.width; i++)
            for (int j = 0; j < GroundManager.instance.shopMap0102Data.height; j++)
                if (GroundManager.instance.shopMap0102Data.GetPixel(i, j) == new Color(0.2f, 0, 0))
                    GroundManager.instance.shopMap0102StartPos = new Vector2(i, j);
                else if (GroundManager.instance.shopMap0102Data.GetPixel(i, j) == new Color(0.2f, 0.2f, 0))
                    GroundManager.instance.shopMap0102ShopPos = new Vector2(i, j);
                else if (GroundManager.instance.shopMap0102Data.GetPixel(i, j) == new Color(0.2f, 0.2f, 0.2f))
                    GroundManager.instance.shopMap0102TreasurePos = new Vector2(i, j);

        for (int i = 0; i < GroundManager.instance.shopMap0102Data.width; i++)
            for (int j = 0; j < GroundManager.instance.shopMap0102Data.height; j++)
                GroundManager.instance.shopMap0102Rect[i, j] = GroundManager.instance.ColorToGroundData(GroundManager.instance.shopMap0102Data.GetPixel(i, j));

        for (int i = 0; i < GroundManager.instance.shopMap0102Data.width; i++)
            for (int j = 0; j < GroundManager.instance.shopMap0102Data.height; j++)
                GroundManager.instance.shopMap0102Fluid[i, j] = GroundManager.instance.ColorToFluidData(GroundManager.instance.shopMap0102Data.GetPixel(i, j));

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

        for (int i = 0; i < GroundManager.instance.shopMap0102Rect.GetLength(0); i++)
            for (int j = 0; j < GroundManager.instance.shopMap0102Rect.GetLength(1); j++)
                groundData[i,j] = GroundManager.instance.shopMap0102Rect[i, j];

        for (int i = 0; i < GroundManager.instance.shopMap0102Rect.GetLength(0); i++)
            for (int j = 0; j < GroundManager.instance.shopMap0102Rect.GetLength(1); j++)
                fluidData[i, j] = GroundManager.instance.shopMap0102Fluid[i, j];
    }
    #endregion

    #region[오브젝트 설치]
    void SetObject()
    {
        if (GroundManager.instance.shopMap0102ShopPos != Vector2.zero)
            ObjectManager.instance.Shop(GroundManager.instance.shopMap0102ShopPos);

        if (GroundManager.instance.shopMap0102TreasurePos != Vector2.zero)
            ObjectManager.instance.TreasureBox(GroundManager.instance.shopMap0102TreasurePos, treasureFlip);

    }
    #endregion

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

}
