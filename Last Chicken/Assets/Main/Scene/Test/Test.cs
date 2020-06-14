using Custom;
using UnityEngine;

public class Test : StageData
{
    #region[GenerateData]
    public override void GenerateData()
    {
        base.GenerateData();

        backGroundData = new BackGroundLayer[world.WorldWidth, world.WorldHeight];
        groundData = new GroundLayer[world.WorldWidth, world.WorldHeight];
        fluidData = new FluidType[world.WorldWidth, world.WorldHeight];

        SetGround();
        SetBackGround();
        SetFluidOutline();
        GenerateBackGround();
        GenerateGround();
        ObjectManager.instance.MineCart(new Vector2(30, 30));
        ObjectManager.instance.WoodBox(new Vector2(50, 30));
        ObjectManager.instance.WoodBox(new Vector2(100, 30));
        ObjectManager.instance.WoodBox(new Vector2(150, 30));
        //for (int x = 0; x < world.WorldWidth; x++)
        //    ObjectManager.instance.WoodBox(new Vector2(x, 30));
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
        for (int x = 0; x < world.WorldWidth; x++)
            for (int y = 0; y < world.WorldHeight; y++)
                backGroundData[x, y] = (BackGroundLayer)(-1);

        for (int x = 0; x < world.WorldWidth; x++)
            for (int y = 0; y < world.WorldHeight; y++)
                if (x < world.WorldWidth * 0.3f)
                    backGroundData[x, y] = BackGroundLayer.NormalBackGround;
                else if (x < world.WorldWidth * 0.6f)
                    backGroundData[x, y] = BackGroundLayer.AltarBackGround;
                else
                    backGroundData[x, y] = BackGroundLayer.DarkAltarBackGround;
    }
    #endregion

    #region[지형 설정]
    public override void SetGround()
    {
        for (int x = 0; x < world.WorldWidth; x++)
            for (int y = 0; y < world.WorldHeight; y++)
                groundData[x, y] = (GroundLayer)(-1);

        for (int x = 0; x < world.WorldWidth; x++)
        {
            for (int y = 0; y < world.WorldHeight; y++)
            {
                if (y <= 7 && y != 6 && y != 4 && y != 2)
                {
                    for (int i = 0; i < 17; i++)
                        if (x < world.WorldWidth * (i + 1) / 17f)
                        {
                            groundData[x, y] = (GroundLayer)(i);
                            break;
                        }

                }
            }
        }

        for (int x = 1; x < world.WorldWidth - 1; x++)
            for (int y = 0; y < world.WorldHeight; y++)
                if (y == 6)
                    fluidData[x, y] = FluidType.Water;
                else if (y == 4)
                    fluidData[x, y] = FluidType.Poison;
                else if (y == 2)
                    fluidData[x, y] = FluidType.Lava;
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
}
