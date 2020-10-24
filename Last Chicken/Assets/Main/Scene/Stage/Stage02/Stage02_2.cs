using Custom;
using System.Collections.Generic;
using UnityEngine;
using TerrainEngine2D;

public class Stage02_2 : StageData
{
    int[,] maxRect;

    public GroundLayer[,] mapRect;
    public FluidType[,] mapFluid;
    public BackGroundLayer[,] mapBackGround;

    bool outlineflipX;

    public List<GameObject> iceMapObject = new List<GameObject>();

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region[Awake]
    public override void Awake()
    {
        base.Awake();
    }
    #endregion

    #region[Update]
    public void Update()
    {
        for (int i = 0; i < iceMapObject.Count; i++)
            iceMapObject[i].SetActive(!CaveManager.inCave);
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
        SetDust();

        GroundManager.instance.Init(world);
        MonsterManager.instance.Init(world, StageManager.instance.stage0202_Monsters.monsterNum, StageManager.instance.stage0202_Monsters.monsterDistance);
    }
    #endregion

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region[배경 설정]
    public override void SetBackGround()
    {
        backGroundData = new BackGroundLayer[world.WorldWidth, world.WorldHeight];
        for (int x = 0; x < world.WorldWidth; x++)
            for (int y = 0; y < world.WorldHeight; y++)
                backGroundData[x, y] = (BackGroundLayer)(-1);

        for (int x = 0; x < world.WorldWidth; x++)
            for (int y = 0; y < world.WorldHeight; y++)
                if (PerlinNoise(x, y, 9, 12, 1) <= 6)
                    backGroundData[x, y] = BackGroundLayer.NormalBackGround;

        for (int x = altarRect.x; x < altarRect.x + altarRect.width; x++)
            for (int y = altarRect.y - altarRect.height; y < altarRect.y; y++)
                if (Exception.IndexOutRange(x, y, backGroundData) && groundData[x, y] == (GroundLayer)(-1))
                    backGroundData[x, y] = BackGroundLayer.DarkAltarBackGround;



        for (int x = fountainRect.x; x < fountainRect.x + fountainRect.width; x++)
            for (int y = fountainRect.y - fountainRect.height; y < fountainRect.y; y++)
                if (Exception.IndexOutRange(x, y, backGroundData) && groundData[x, y] == (GroundLayer)(-1))
                    backGroundData[x, y] = (BackGroundLayer)(-1);
    }
    #endregion

    #region[지형 설정]
    public override void SetGround()
    {
        groundData = new GroundLayer[world.WorldWidth, world.WorldHeight];
        fluidData = new FluidType[world.WorldWidth, world.WorldHeight];

        //기본 지형백지화
        for (int x = 0; x < world.WorldWidth; x++)
            for (int y = 0; y < world.WorldHeight; y++)
                groundData[x, y] = (GroundLayer)(-1);

        //눈생성
        for (int x = 0; x < world.WorldWidth; x++)
            for (int y = 0; y < world.WorldHeight; y++)
                if (y < world.WorldHeight - 20 && y > 20)
                    if (PerlinNoise(x, y, 15, 25, 1) <= 8)
                        groundData[x, y] = GroundLayer.Dirt;

        //돌생성
        for (int x = 0; x < world.WorldWidth; x++)
            for (int y = 0; y < world.WorldHeight; y++)
                if (y < world.WorldHeight - 20 && y > 20)
                    if (groundData[x, y] == GroundLayer.Dirt)
                        if (PerlinNoise(x, y, 15, 15, 1) <= 2)
                            groundData[x, y] = GroundLayer.Stone;

        FillArea(150);
        RemoveArea();

        GroundLayer[] Minerals = new GroundLayer[] { GroundLayer.Iron, GroundLayer.Silver, GroundLayer.Gold, GroundLayer.Mithril };

        for (int n = 0; n < Minerals.Length; n++)
        {
            for (int x = 0; x < world.WorldWidth; x++)
                for (int y = 0; y < world.WorldHeight; y++)
                {
                    if(n != 3)
                    {
                        if (Random.Range(0, 1500) <= 1498)
                            continue;
                    }
                    else
                    {
                        if (Random.Range(0, 1700) <= 1698)
                            continue;
                    }

                    if (groundData[x, y] == GroundLayer.Dirt)
                        groundData[x, y] = Minerals[n];

                    int randomType = Random.Range(0, 4);

                    for (int i = 0; i < 36; i++)
                    {
                        int ax = x + i % 6;
                        int ay = y + i / 6;
                        if (Exception.IndexOutRange(ax, ay, groundData))
                            if (groundData[ax, ay] == GroundLayer.Dirt && MineralType[randomType, i % 6, i / 6] == 1)
                                groundData[ax, ay] = Minerals[n];
                    }
                }
            // ProceduralGeneration(world, groundData, Minerals[n]);
        }

        //얼음생성
        for (int y = 35; y < world.WorldHeight - 35; y += Random.Range(10, 15))
        {
            int x = Random.Range(0, world.WorldWidth);
            int w = Random.Range(50, 100);
            int h = Random.Range(1, 3);
            for (int a = x - w / 2; a < x + w / 2; a++)
                for (int b = y; b < y + h; b++)
                    if (Exception.IndexOutRange(a, b, groundData))
                        if (groundData[a, b] == (GroundLayer)(-1))
                            groundData[a, b] = GroundLayer.Ice;
        }
        for (int x = 0; x < world.WorldWidth; x++)
            for (int y = 0; y < world.WorldHeight; y++)
                if (y < world.WorldHeight - 20 && y > 20)
                    if (groundData[x, y] == (GroundLayer)(-1))
                        if (PerlinNoise(x, y, 10, 20, 1.5f) <= 10)
                            groundData[x, y] = GroundLayer.Ice;

        ProceduralGeneration(world, groundData, GroundLayer.Ice, 4);

        ProceduralGeneration(world, groundData, GroundLayer.Dirt);

        outlineflipX = Random.Range(0, 100) > 50;
        PlayerMap.instance.exitArrow = outlineflipX;

        //for (int y = 0; y < world.WorldHeight; y++)
        //    for (int x = 0; x < world.WorldWidth; x++)
        //    {
        //        int fy = (int)(y * ((float)GroundManager.instance.stage01OutlineRect.GetLength(1) / (float)world.WorldHeight));
        //        int fx = outlineflipX ? x : world.WorldWidth - x - 1;
        //        if (GroundManager.instance.stage01OutlineRect[fx, fy] == GroundLayer.UnBreakable)
        //            groundData[x, y] = GroundLayer.UnBreakable;
        //        else if (GroundManager.instance.stage01OutlineRect[fx, fy] == GroundLayer.Dirt)
        //            groundData[x, y] = (GroundLayer)(-1);
        //    }

        for (int x = 0; x < world.WorldWidth; x++)
            for (int y = 0; y < world.WorldHeight; y++)
                if ((y <= world.WorldHeight - 15 && y >= world.WorldHeight - 20) || (y <= 30 && y >= 25))
                    if (groundData[x, y] != GroundLayer.UnBreakable)
                        groundData[x, y] = GroundLayer.Dirt;

        RemoveArea();

    }
    #endregion

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region[설치가능 지형 검사]
    void CheckArea()
    {
        //설치 가능한 지점을 설정
        maxRect = new int[world.WorldWidth, world.WorldHeight];
        for (int i = 15; i < world.WorldWidth - 15; i++)
            for (int j = 30; j < world.WorldHeight - 30; j++)
                if (groundData[i, j] == GroundLayer.Dirt)
                    maxRect[i, j] = 1;

        //cave 지점은 설치 불가능
        for (int i = 0; i < CaveManager.instance.objectPool.Count; i++)
        {
            Vector3 pos = new Vector3(CaveManager.instance.objectPool[i].transform.position.x, CaveManager.instance.objectPool[i].transform.position.y, 60);
            for (int x = (int)pos.x - (int)pos.z; x < pos.x + pos.z; x++)
                for (int y = (int)pos.y - (int)pos.z; y < (int)pos.y + (int)pos.z; y++)
                    if (Exception.IndexOutRange(x, y, maxRect))
                        maxRect[x, y] = 0;
        }

        //해당 위치에서 만들수있는 가장 큰 정사각형을 구해줌
        for (int i = 1; i < world.WorldWidth; i++)
            for (int j = 1; j < world.WorldHeight; j++)
            {
                if (maxRect[i, j] != 0)
                {
                    int minValue = Mathf.Min(Mathf.Min(maxRect[i - 1, j], maxRect[i, j - 1]), maxRect[i - 1, j - 1]);
                    maxRect[i, j] = minValue + 1;
                }
            }
    }
    #endregion

    #region[설치가 가능한지 체크]
    bool CanAddArea(int x, int y, int w, int h)
    {
        int minLength = Mathf.Min(w, h);
        int maxLength = Mathf.Max(w, h);
        int checkLength = maxLength - minLength;

        if (!Exception.IndexOutRange(x, y, maxRect) || (Exception.IndexOutRange(x, y, maxRect) && maxRect[x, y] < minLength))
            return false;

        if (w > h)
            for (int k = x - checkLength; k <= x; k += minLength)
                if (!Exception.IndexOutRange(k, y, maxRect) || (Exception.IndexOutRange(k, y, maxRect) && maxRect[k, y] < minLength))
                    return false;

        if (w <= h)
            for (int k = y - checkLength; k <= y; k += minLength)
                if (!Exception.IndexOutRange(x, k, maxRect) || (Exception.IndexOutRange(x, k, maxRect) && maxRect[x, k] < minLength))
                    return false;

        return true;

    }

    #endregion

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region[오브젝트 설치]
    void SetObject()
    {
        #region[보물배치]
        List<Vector2Int> treasureList = new List<Vector2Int>();
        for (int y = (int)(world.WorldHeight * 0.25f); y < world.WorldHeight - 1; y++)
        {
            for (int x = 0; x < world.WorldWidth - 1; x++)
            {
                bool treasureFlag = true;
                for (int ax = x; ax < x + 5; ax++)
                {
                    for (int ay = y; ay < y + 3; ay++)
                    {
                        if (Exception.IndexOutRange(ax, ay, groundData) && groundData[ax, ay] != GroundLayer.Dirt)
                        {
                            ax = 10000;
                            ay = 10000;
                            treasureFlag = false;
                            break;
                        }
                        else if (!Exception.IndexOutRange(ax, ay, groundData))
                        {
                            ax = 10000;
                            ay = 10000;
                            treasureFlag = false;
                            break;
                        }
                    }
                }

                if (treasureFlag)
                    treasureList.Add(new Vector2Int(x, y));

            }
        }
        if (treasureList.Count > 0)
        {
            Vector2Int pos = treasureList[Random.Range(0, treasureList.Count)];
            ObjectManager.instance.TreasureBox(pos + new Vector2Int(3, 2));
        }
        #endregion

        #region[냉기구멍배치]
        List<Vector2Int> iceHolePos = new List<Vector2Int>();
        for (int n = 0; n < 100; n++)
        {
            if (Random.Range(0, 100) <= StageManager.instance.stage0202_TrapValue)
                continue;
            List<Vector2Int> iceHoleList = new List<Vector2Int>();
            for (int y = 40; y < world.WorldHeight - 40; y++)
            {
                for (int x = 30; x < world.WorldWidth - 30; x++)
                {
                    bool iceHoleFlag = true;
                    for (int i = x - 1; i <= x + 1; i++)
                        for (int j = y - 1; j <= y + 1; j++)
                        {
                            if (Exception.IndexOutRange(i, j, groundData) && groundData[i, j] == (GroundLayer)(-1))
                                iceHoleFlag = false;
                            else if (!Exception.IndexOutRange(i, j, groundData))
                                iceHoleFlag = false;

                            if (Exception.IndexOutRange(i, j, backGroundData) && backGroundData[i, j] == (BackGroundLayer)(-1))
                                iceHoleFlag = false;
                            else if (!Exception.IndexOutRange(i, j, backGroundData))
                                iceHoleFlag = false;

                            if (!iceHoleFlag)
                            {
                                i = 10000;
                                j = 10000;
                            }
                        }

                    if (!iceHoleFlag)
                        continue;

                    for (int i = 0; i < iceHolePos.Count; i++)
                        if (Vector2.Distance(new Vector2Int(x, y), iceHolePos[i]) < 4)
                        {
                            iceHoleFlag = false;
                            break;
                        }

                    if (!iceHoleFlag)
                        continue;

                    if (iceHoleFlag)
                        iceHoleList.Add(new Vector2Int(x, y));

                }
            }
            if (iceHoleList.Count > 0)
            {
                Vector2Int pos = iceHoleList[Random.Range(0, iceHoleList.Count)];
                iceHolePos.Add(pos);
                ObjectManager.instance.IceHole(pos + new Vector2Int(1, 1));
            }
        }
        #endregion

        #region[상자배치]
        List<Vector2Int> woodBoxpos = new List<Vector2Int>();
        for (int n = 0; n < 50; n++)
        {
            if (Random.Range(0, 100) <= StageManager.instance.stage0202_WoodBoxValue)
                continue;

            List<Vector2Int> woodBoxList = new List<Vector2Int>();
            for (int y = 40; y < world.WorldHeight - 40; y++)
            {
                for (int x = 10; x < world.WorldWidth - 10; x++)
                {
                    bool boxFlag = true;
                    for (int i = 0; i < 2; i++)
                        for (int j = 0; j < 2; j++)
                        {
                            int ax = x + i;
                            int ay = y + j;
                            if (j == 0 && Exception.IndexOutRange(ax, ay, groundData) && groundData[ax, ay] == (GroundLayer)(-1))
                                boxFlag = false;
                            else if (j == 1 && Exception.IndexOutRange(ax, ay, groundData) && groundData[ax, ay] != (GroundLayer)(-1))
                                boxFlag = false;
                            else if (!Exception.IndexOutRange(ax, ay, groundData))
                                boxFlag = false;
                        }


                    for(int i = 0; i < woodBoxpos.Count; i++)
                        if (Vector2.Distance(new Vector2Int(x, y),woodBoxpos[i]) < 5)
                            boxFlag = false;

                    if (boxFlag)
                        woodBoxList.Add(new Vector2Int(x, y));

                }
            }
            if (woodBoxList.Count > 0)
            {
                Vector2Int pos = woodBoxList[Random.Range(0, woodBoxList.Count)];
                woodBoxpos.Add(pos);
                ObjectManager.instance.WoodBox(pos + new Vector2Int(1, 1));
            }
        }
        #endregion

        #region[고드름배치]
        List<Vector2Int> falliciclePos = new List<Vector2Int>();
        for (int n = 0; n < 100; n++)
        {
            if (Random.Range(0, 100) <= StageManager.instance.stage0202_TrapValue)
                continue;

            List<Vector2Int> fallicicleList = new List<Vector2Int>();
            for (int y = 40; y < world.WorldHeight - 40; y++)
            {
                for (int x = 10; x < world.WorldWidth - 10; x++)
                {
                    bool fallicicleFlag = true;
                    for (int i = -1; i <= 1; i++)
                        for (int j = -1; j <= 0; j++)
                        {
                            int ax = x + i;
                            int ay = y + j;

                            if (j == 0 && Exception.IndexOutRange(ax, ay, groundData) && groundData[ax, ay] == (GroundLayer)(-1))
                                fallicicleFlag = false;
                            else if (j == -1 && Exception.IndexOutRange(ax, ay, groundData) && groundData[ax, ay] != (GroundLayer)(-1))
                                fallicicleFlag = false;
                            else if (!Exception.IndexOutRange(ax, ay, groundData))
                                fallicicleFlag = false;
                        }


                    for (int i = 0; i < falliciclePos.Count; i++)
                        if (Vector2.Distance(new Vector2Int(x, y), falliciclePos[i]) < 2)
                            fallicicleFlag = false;

                    if (fallicicleFlag)
                        fallicicleList.Add(new Vector2Int(x,y));

                }
            }
            if (fallicicleList.Count > 0)
            {
                Vector2Int pos = fallicicleList[Random.Range(0, fallicicleList.Count)];
                falliciclePos.Add(pos);
                ObjectManager.instance.Icicle(pos + new Vector2(0, -1f), Random.Range(0, 3));
            }
        }

        List<Vector2Int> bottomIciclePos = new List<Vector2Int>();
        for (int n = 0; n < 100; n++)
        {
            if (Random.Range(0, 100) <= StageManager.instance.stage0201_TrapValue)
                continue;

            List<Vector2Int> bottomIcicleList = new List<Vector2Int>();
            for (int y = 40; y < world.WorldHeight - 40; y++)
            {
                for (int x = 10; x < world.WorldWidth - 10; x++)
                {
                    bool bottomIcicleFlag = true;
                    for (int i = -1; i <= 1; i++)
                        for (int j = -1; j <= 0; j++)
                        {
                            int ax = x + i;
                            int ay = y + j;

                            if (j == -1 && Exception.IndexOutRange(ax, ay, groundData) && groundData[ax, ay] == (GroundLayer)(-1))
                                bottomIcicleFlag = false;
                            else if (j == 0 && Exception.IndexOutRange(ax, ay, groundData) && groundData[ax, ay] != (GroundLayer)(-1))
                                bottomIcicleFlag = false;
                            else if (!Exception.IndexOutRange(ax, ay, groundData))
                                bottomIcicleFlag = false;
                        }


                    for (int i = 0; i < bottomIciclePos.Count; i++)
                        if (Vector2.Distance(new Vector2Int(x, y), bottomIciclePos[i]) < 2)
                            bottomIcicleFlag = false;

                    if (bottomIcicleFlag)
                        bottomIcicleList.Add(new Vector2Int(x, y));

                }
            }
            if (bottomIcicleList.Count > 0)
            {
                Vector2Int pos = bottomIcicleList[Random.Range(0, bottomIcicleList.Count)];
                bottomIciclePos.Add(pos);
                ObjectManager.instance.Icicle(pos + new Vector2(0, 1f), Random.Range(3, 6));
            }
        }
        #endregion

        SetCave();
    }
    #endregion

    #region[자잘한 공간 채우기]
    void FillArea(int limit = 100)
    {
        LumpChange((GroundLayer)(-1), GroundLayer.Dirt, limit);
    }

    #endregion

    #region[자잘한 지형 지우기]
    void RemoveArea(int limit = 100)
    {
        List<GroundLayer> list = new List<GroundLayer>();
        list.Add(GroundLayer.Dirt);
        list.Add(GroundLayer.Stone);
        list.Add(GroundLayer.Copper);
        list.Add(GroundLayer.Sand);
        list.Add(GroundLayer.Granite);
        list.Add(GroundLayer.Iron);
        list.Add(GroundLayer.Silver);
        list.Add(GroundLayer.Gold);
        list.Add(GroundLayer.Mithril);
        list.Add(GroundLayer.Diamond);
        list.Add(GroundLayer.Magnetite);
        list.Add(GroundLayer.Titanium);
        list.Add(GroundLayer.Cobalt);
        list.Add(GroundLayer.Ice);
        list.Add(GroundLayer.Grass);
        LumpChange(list, (GroundLayer)(-1), limit);
    }
    #endregion

    #region[덩어리 변경]
    void LumpChange(GroundLayer groundList, GroundLayer ground, int limit)
    {
        List<GroundLayer> list = new List<GroundLayer>();
        list.Add(groundList);
        LumpChange(list, ground, limit);
    }

    void LumpChange(List<GroundLayer> groundList, GroundLayer ground, int limit)
    {
        int[,] fillRect = new int[world.WorldWidth, world.WorldHeight];

        for (int i = 0; i < world.WorldWidth; i++)
            for (int j = 0; j < world.WorldHeight; j++)
                for (int k = 0; k < groundList.Count; k++)
                    if (fillRect[i, j] == 0)
                        fillRect[i, j] = (groundData[i, j] == groundList[k]) ? 1 : 0;

        Queue<Vector2Int> fillQuque = new Queue<Vector2Int>();

        for (int i = 0; i < world.WorldWidth; i++)
            for (int j = 0; j < world.WorldHeight; j++)
            {
                int count = 0;
                if (fillRect[i, j] == 1)
                {
                    fillQuque.Enqueue(new Vector2Int(i, j));
                    fillRect[i, j] = -1;
                    count++;
                    while (fillQuque.Count > 0)
                    {
                        Vector2Int emp = fillQuque.Dequeue();
                        for (int k = 0; k < 4; k++)
                        {
                            int ax = emp.x + Dic[k, 0];
                            int ay = emp.y + Dic[k, 1];
                            if (Exception.IndexOutRange(ax, ay, fillRect) && fillRect[ax, ay] == 1)
                            {
                                fillQuque.Enqueue(new Vector2Int(ax, ay));
                                fillRect[ax, ay] = -1;
                                count++;
                            }

                        }
                    }
                    if (count >= limit)
                        continue;

                    fillQuque.Enqueue(new Vector2Int(i, j));
                    fillRect[i, j] = -2;
                    groundData[i, j] = ground;

                    while (fillQuque.Count > 0)
                    {
                        Vector2Int emp = fillQuque.Dequeue();
                        for (int k = 0; k < 4; k++)
                        {
                            int ax = emp.x + Dic[k, 0];
                            int ay = emp.y + Dic[k, 1];
                            if (Exception.IndexOutRange(ax, ay, fillRect) && fillRect[ax, ay] == -1)
                            {
                                fillQuque.Enqueue(new Vector2Int(ax, ay));
                                fillRect[ax, ay] = -2;
                                groundData[ax, ay] = ground;
                            }

                        }
                    }
                }
            }
    }
    #endregion

    #region[동굴생성]
    public void SetCave()
    {
        List<Vector2Int> v = new List<Vector2Int>();
        Vector2 cavePos;

        int[] A = new int[] { 1, 2, 3, 4 };

        for (int i = 0; i < 100; i++)
        {
            int a = Random.Range(0, 4);
            int b = Random.Range(0, 4);
            int temp = A[a];
            A[a] = A[b];
            A[b] = temp;
        }

        for (int k = 0; k < 3; k++)
        {
            CheckArea();
            v.Clear();
            for (int i = 0; i < world.WorldWidth; i++)
                for (int j = 0; j < world.WorldHeight; j++)
                    if (CanAddArea(i, j, 6, 6))
                        v.Add(new Vector2Int(i, j - 2));
            if (v.Count > 0)
            {
                cavePos = v[Random.Range(0, v.Count)];
                switch (A[k])
                {

                    case 1:
                        CaveManager.instance.ShopCave(cavePos, cavePos.x < world.WorldWidth / 2 ? 0 : 1);
                        break;
                    case 2:
                        CaveManager.instance.FountainCave(cavePos, cavePos.x < world.WorldWidth / 2 ? 0 : 1);
                        break;
                    case 3:
                        CaveManager.instance.SmithyCave(cavePos, cavePos.x < world.WorldWidth / 2 ? 0 : 1);
                        break;
                    case 4:
                        CaveManager.instance.AltarCave(cavePos, cavePos.x < world.WorldWidth / 2 ? 0 : 1);
                        break;
                }
                //if (Random.Range(0, 100) > 50)
                for (int i = (int)(cavePos.x - 3); i < (int)(cavePos.x + 3); i++)
                    for (int j = (int)(cavePos.y - 3); j < (int)(cavePos.y + 4); j++)
                        if (Exception.IndexOutRange(i, j, groundData))
                            groundData[i, j] = (GroundLayer)(-1);
            }
        }
    }
    #endregion

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
}
