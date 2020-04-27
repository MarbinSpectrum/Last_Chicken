using Custom;
using System.Collections.Generic;
using UnityEngine;
using TerrainEngine2D;

public class ShopMap0101 : EventMap
{
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
            variation.Add(temp);
        }
        int r = Random.Range(0, variation.Count);
        GroundManager.instance.eventMapData = variation[r];
        GroundManager.instance.eventMapBackData = CreateData(GroundManager.instance.eventMapData.width, GroundManager.instance.eventMapData.height);

        switch (r)
        {
            case 0:
                EventMapSetting(ExitDic.왼쪽, -1);
                break;
            case 1:
                EventMapSetting(ExitDic.오른쪽, 1);
                break;
            case 2:
                EventMapSetting(ExitDic.오른쪽, 1);
                break;
            case 3:
                EventMapSetting(ExitDic.왼쪽, -1);
                break;
            case 4:
                EventMapSetting(ExitDic.왼쪽, -1);
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
                    int r = UnityEngine.Random.Range(16, 400);
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

}
