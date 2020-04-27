using Custom;
using System.Collections.Generic;
using UnityEngine;
using TerrainEngine2D;

public class SmithyMap01_3 : StageData
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

        GroundManager.instance.smithyMap0103Data = Resources.Load("TerrainData/Smithy/Smithy01") as Texture2D;
        GroundManager.instance.smithyMap0103BackData = Resources.Load("TerrainData/Smithy/SmithyBackData01") as Texture2D;

        int activeWall = 0;
        GroundManager.instance.smithyMap0103StartDic = 1;
        treasureFlip = true;
        activeWall = 1;

        bool filpX = Random.Range(0, 100) > 50;
        Texture2D mapData = new Texture2D(GroundManager.instance.smithyMap0103Data.width, GroundManager.instance.smithyMap0103Data.height);
        Texture2D mapBackData = new Texture2D(GroundManager.instance.smithyMap0103BackData.width, GroundManager.instance.smithyMap0103BackData.height);
        for (int y = 0; y < mapData.height; y++)
            for (int x = 0; x < mapData.width; x++)
            {
                mapData.SetPixel(filpX ? mapData.width - x - 1 : x, y, GroundManager.instance.smithyMap0103Data.GetPixel(x, y));
                mapBackData.SetPixel(filpX ? mapBackData.width - x - 1 : x, y, GroundManager.instance.smithyMap0103BackData.GetPixel(x, y));
            }

        if (filpX)
        {
            activeWall = activeWall == 0 ? 1 : 0;

            treasureFlip = !treasureFlip;
            GroundManager.instance.smithyMap0103StartDic = -GroundManager.instance.smithyMap0103StartDic;
        }

        GroundManager.instance.smithyMap0103Data = mapData;

        GroundManager.instance.smithyMap0103Rect = new GroundLayer[GroundManager.instance.smithyMap0103Data.width, GroundManager.instance.smithyMap0103Data.height];
        GroundManager.instance.smithyMap0103Fluid = new FluidType[GroundManager.instance.smithyMap0103Data.width, GroundManager.instance.smithyMap0103Data.height];
        GroundManager.instance.smithyMap0103BackGround = new BackGroundLayer[GroundManager.instance.smithyMap0103BackData.width, GroundManager.instance.smithyMap0103BackData.height];

        GroundManager.instance.smithyMap0103SmithyPos = Vector2.zero;
        GroundManager.instance.smithyMap0103TreasurePos = Vector2.zero;

        for (int i = 0; i < GroundManager.instance.smithyMap0103Data.width; i++)
            for (int j = 0; j < GroundManager.instance.smithyMap0103Data.height; j++)
                if (GroundManager.instance.smithyMap0103Data.GetPixel(i, j) == new Color(0.2f, 0, 0))
                    GroundManager.instance.smithyMap0103StartPos = new Vector2(i, j);
                else if (GroundManager.instance.smithyMap0103Data.GetPixel(i, j) == new Color(0.2f, 0.2f, 0))
                    GroundManager.instance.smithyMap0103SmithyPos = new Vector2(i, j);
                else if (GroundManager.instance.smithyMap0103Data.GetPixel(i, j) == new Color(0.2f, 0.2f, 0.2f))
                    GroundManager.instance.smithyMap0103TreasurePos = new Vector2(i, j);

        for (int i = 0; i < GroundManager.instance.smithyMap0103Data.width; i++)
            for (int j = 0; j < GroundManager.instance.smithyMap0103Data.height; j++)
                GroundManager.instance.smithyMap0103Rect[i, j] = GroundManager.instance.ColorToGroundData(GroundManager.instance.smithyMap0103Data.GetPixel(i, j));

        for (int i = 0; i < GroundManager.instance.smithyMap0103Data.width; i++)
            for (int j = 0; j < GroundManager.instance.smithyMap0103Data.height; j++)
                GroundManager.instance.smithyMap0103Fluid[i, j] = GroundManager.instance.ColorToFluidData(GroundManager.instance.smithyMap0103Data.GetPixel(i, j));

        for (int i = 0; i < GroundManager.instance.smithyMap0103BackData.width; i++)
            for (int j = 0; j < GroundManager.instance.smithyMap0103BackData.height; j++)
                GroundManager.instance.smithyMap0103BackGround[i, j] = GroundManager.instance.ColorToBackData(GroundManager.instance.smithyMap0103BackData.GetPixel(i, j));

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
        //CameraController.Instance.SetOffset(12);
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
                backGroundData[x, y] = GroundManager.instance.smithyMap0103BackGround[x, y];
    }
    #endregion

    #region[지형 설정]

    public override void SetGround()
    {
        groundData = new GroundLayer[world.WorldWidth, world.WorldHeight];
        fluidData = new FluidType[world.WorldWidth, world.WorldHeight];

        for (int i = 0; i < GroundManager.instance.smithyMap0103Rect.GetLength(0); i++)
            for (int j = 0; j < GroundManager.instance.smithyMap0103Rect.GetLength(1); j++)
                groundData[i,j] = GroundManager.instance.smithyMap0103Rect[i, j];

        for (int i = 0; i < GroundManager.instance.smithyMap0103Rect.GetLength(0); i++)
            for (int j = 0; j < GroundManager.instance.smithyMap0103Rect.GetLength(1); j++)
                fluidData[i, j] = GroundManager.instance.smithyMap0103Fluid[i, j];
    }
    #endregion

    #region[오브젝트 설치]
    void SetObject()
    {
        if (GroundManager.instance.smithyMap0103SmithyPos != Vector2.zero)
            ObjectManager.instance.Smithy(GroundManager.instance.smithyMap0103SmithyPos);

        if (GroundManager.instance.smithyMap0103TreasurePos != Vector2.zero)
            ObjectManager.instance.TreasureBox(GroundManager.instance.smithyMap0103TreasurePos, treasureFlip);

    }
    #endregion

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

}
