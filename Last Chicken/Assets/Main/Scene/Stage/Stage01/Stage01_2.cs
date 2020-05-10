using Custom;
using System.Collections.Generic;
using UnityEngine;
using TerrainEngine2D;

public class Stage01_2 : StageData
{
    int[,] maxRect;

    List<RectInt> deleteArea = new List<RectInt>();         //삭제되는 지형
    List<RectInt> verticalArea = new List<RectInt>();       //내려가는 길
    List<RectInt> mineRoadArea = new List<RectInt>();       //가로 길

    int startMineAreaY = 10;   //시작 광산로 Y좌표
    bool outlineflipX;

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

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
        MonsterManager.instance.Init(world, 20);
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
        SetMineRoad();

        groundData = new GroundLayer[world.WorldWidth, world.WorldHeight];
        fluidData = new FluidType[world.WorldWidth, world.WorldHeight];

        //기본 지형백지화
        for (int x = 0; x < world.WorldWidth; x++)
            for (int y = 0; y < world.WorldHeight; y++)
                groundData[x, y] = (GroundLayer)(-1);

        //노이즈값으로 지형을 설정
        for (int x = 0; x < world.WorldWidth; x++)
            for (int y = 0; y < world.WorldHeight; y++)
                if (PerlinNoise(x, y, 15, 15, 1) <= 8)
                    groundData[x, y] = GroundLayer.Dirt;

        //절차적인 맵디자인을 위해 부분적으로 공간을 만듬
        for (int x = 0; x < world.WorldWidth; x++)
            for (int y = 0; y < world.WorldHeight; y++)
                if (Random.Range(0, 100) > 80)
                    groundData[x, y] = (GroundLayer)(-1);

        //광산지형으로 설정된 지형만큼 지형을 덮음
        for (int i = 0; i < mineRoadArea.Count; i++)
            for (int x = mineRoadArea[i].x; x < mineRoadArea[i].x + mineRoadArea[i].width; x++)
                for (int y = mineRoadArea[i].y - mineRoadArea[i].height - 3; y < mineRoadArea[i].y; y++)
                    if (Exception.IndexOutRange(x, y, groundData))
                        groundData[x, y] = GroundLayer.Dirt;

        //세로통로로 설정된 지형만큼 지형을 덮음
        for (int i = 0; i < verticalArea.Count; i++)
            for (int x = verticalArea[i].x - 3; x < verticalArea[i].x + verticalArea[i].width + 3; x++)
                for (int y = verticalArea[i].y - verticalArea[i].height; y < verticalArea[i].y; y++)
                    if (Exception.IndexOutRange(x, y, groundData))
                        groundData[x, y] = GroundLayer.Dirt;

        //지정된 삭제 지역을 삭제
        for (int i = 0; i < deleteArea.Count; i++)
            for (int x = deleteArea[i].x; x < deleteArea[i].x + deleteArea[i].width; x++)
                for (int y = deleteArea[i].y - deleteArea[i].height; y < deleteArea[i].y; y++)
                    if (Exception.IndexOutRange(x, y, groundData))
                        groundData[x, y] = (GroundLayer)(-1);

        ProceduralGeneration(world, groundData, GroundLayer.Dirt);

        //노이즈값으로 지형을 설정
        for (int x = 0; x < world.WorldWidth; x++)
            for (int y = 0; y < world.WorldHeight; y++)
                if (PerlinNoise(x, y, 10, 15, 1) >= 8 && groundData[x, y] == GroundLayer.Dirt)
                    groundData[x, y] = GroundLayer.Stone;

        ProceduralGeneration(world, groundData, GroundLayer.Stone);

        FillArea(150);
        RemoveArea();

        GroundLayer[] Minerals = new GroundLayer[] { GroundLayer.Copper, GroundLayer.Iron, GroundLayer.Silver, GroundLayer.Gold };

        for (int n = 0; n < Minerals.Length; n++)
        {
            for (int x = 0; x < world.WorldWidth; x++)
                for (int y = 0; y < world.WorldHeight; y++)
                {
                    if (Random.Range(0, 1000) <= 998)
                        continue;

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

        outlineflipX = Random.Range(0, 100) > 50;

        for (int y = 0; y < world.WorldHeight; y++)
            for (int x = 0; x < world.WorldWidth; x++)
            {
                int fx = outlineflipX ? x : world.WorldWidth - x - 1;
                if (GroundManager.instance.stage01OutlineRect[fx, y] == GroundLayer.UnBreakable)
                    groundData[x, y] = GroundLayer.UnBreakable;
                else if (GroundManager.instance.stage01OutlineRect[fx, y] == GroundLayer.Dirt)
                    groundData[x, y] = (GroundLayer)(-1);
            }
    }
    #endregion

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region[설치가능 지형 검사]
    void CheckArea()
    {
        //설치 가능한 지점을 설정
        maxRect = new int[world.WorldWidth, world.WorldHeight];
        for (int i = 0; i < world.WorldWidth; i++)
            for (int j = 0; j < world.WorldHeight; j++)
                maxRect[i, j] = 1;

        //삭제된 지점은 설치 불가능
        for (int i = 0; i < deleteArea.Count; i++)
            for (int x = deleteArea[i].x; x < deleteArea[i].x + deleteArea[i].width; x++)
                for (int y = deleteArea[i].y - deleteArea[i].height; y < deleteArea[i].y; y++)
                    if (Exception.IndexOutRange(x, y, groundData))
                        maxRect[x, y] = 0;

        //광산로는 설치 불가능
        for (int i = 0; i < mineRoadArea.Count; i++)
            for (int x = mineRoadArea[i].x; x < mineRoadArea[i].x + mineRoadArea[i].width; x++)
                for (int y = mineRoadArea[i].y - mineRoadArea[i].height; y < mineRoadArea[i].y - mineRoadArea[i].height + 25; y++)
                    if (Exception.IndexOutRange(x, y, groundData))
                        maxRect[x, y] = 0;


        //제단에는 설치 불가능
        for (int i = 0; i < altarRect.width; i++)
            for (int j = 0; j < altarRect.height; j++)
                if (Exception.IndexOutRange(altarRect.x + i, altarRect.y - altarRect.height + j, maxRect))
                    maxRect[altarRect.x + i, altarRect.y - altarRect.height + j] = 0;
        if (altarRect.width > 0)
        {
            for (int i = 0; i < world.WorldWidth; i++)
                for (int j = 0; j < world.WorldHeight; j++)
                    if (Vector2.Distance(new Vector2(i, j), new Vector2(altarRect.x + altarRect.width / 2f, altarRect.y - altarRect.height / 2f)) < 50)
                        maxRect[i, j] = 0;
        }

        //분수에는 설치 불가능
        for (int i = 0; i < fountainRect.width; i++)
            for (int j = 0; j < fountainRect.height; j++)
                if (Exception.IndexOutRange(fountainRect.x + i, fountainRect.y - fountainRect.height + j, maxRect))
                    maxRect[fountainRect.x + i, fountainRect.y - fountainRect.height + j] = 0;
        if (fountainRect.width > 0)
        {
            for (int i = 0; i < world.WorldWidth; i++)
                for (int j = 0; j < world.WorldHeight; j++)
                    if (Vector2.Distance(new Vector2(i, j), new Vector2(fountainRect.x + fountainRect.width / 2f, fountainRect.y - fountainRect.height / 2f)) < 50)
                        maxRect[i, j] = 0;
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

    #region[광산로 세팅]
    void SetMineRoad()
    {
        Vector2Int HorizontalPathSize = new Vector2Int((int)(world.WorldWidth * 0.75f), 7); //가로통로 크기

        int verticalPathWidth = 6;  //새로경로 너비

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //시작지점 광산로
        RectInt mineRoad = new RectInt(0, world.WorldHeight - startMineAreaY, world.WorldWidth, HorizontalPathSize.y);
        mineRoadArea.Add(mineRoad);

        //중간지점 광산로
        int addH = 40;
        while (world.WorldHeight - addH > 40)
        {
            mineRoad = new RectInt(Random.Range(0, world.WorldWidth - HorizontalPathSize.x), world.WorldHeight - addH, HorizontalPathSize.x, HorizontalPathSize.y);
            mineRoadArea.Add(mineRoad);
            addH += Random.Range(30, 50);
        }

        //마지막지점 광산로
        mineRoad = new RectInt(0, 30, world.WorldWidth, HorizontalPathSize.y);
        mineRoadArea.Add(mineRoad);

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //광산로 지점을 다 지우는 지점으로 설정
        for (int i = 0; i < mineRoadArea.Count; i++)
            deleteArea.Add(mineRoadArea[i]);

        //시작지점을 지우는 지점으로 설정
        deleteArea.Add(new RectInt((world.WorldWidth - 20) / 2, world.WorldHeight, 20, HorizontalPathSize.y + startMineAreaY));

        //광산로와 광산로를 이어주기 위해서 중간에 지우는 지점으로 설정
        RectInt deleteRoad;
        for (int i = 0; i < mineRoadArea.Count - 1; i++)
        {
            int minX = Mathf.Max(mineRoadArea[i].x, mineRoadArea[i + 1].x);
            int maxX = Mathf.Min(mineRoadArea[i].x + mineRoadArea[i].width, mineRoadArea[i + 1].x + mineRoadArea[i + 1].width);
            deleteRoad = new RectInt(Random.Range(minX, maxX - verticalPathWidth), mineRoadArea[i].y + 5, verticalPathWidth, Mathf.Abs(mineRoadArea[i].y - mineRoadArea[i + 1].y) + 5);
            //시작지점에 중간 지우는 지점이 있으면 곤란하니 제거
            while (i == 0 && deleteRoad.x - 10 <= world.WorldWidth / 2 && world.WorldWidth / 2 <= deleteRoad.x + 30)
                deleteRoad = new RectInt(Random.Range(minX, maxX - verticalPathWidth), mineRoadArea[i].y + 5, verticalPathWidth, Mathf.Abs(mineRoadArea[i].y - mineRoadArea[i + 1].y) + 5);
            deleteArea.Add(deleteRoad);

            //새로경로에 추가
            verticalArea.Add(deleteRoad);
        }
    }
    #endregion

    #region[오브젝트 설치]
    void SetObject()
    {
        #region[길에 따른 오브젝트배치]
        for (int i = 0; i < mineRoadArea.Count; i++)
        {
            #region[길배치]
            int num = 2;
            for (int j = 0; j < num; j++)
                ObjectManager.instance.MineWoodBoard(new Vector2(mineRoadArea[i].x + j * 59, mineRoadArea[i].y - mineRoadArea[i].height - 1));
            #endregion

            #region[오브젝트 배치]
            int addX = 10;
            for (int j = 0; j < 15; j++)
            {
                Vector2 objectPos = new Vector2(mineRoadArea[i].x + addX, mineRoadArea[i].y - mineRoadArea[i].height + 1);
                if (Random.Range(0, 100) <= StageManager.instance.stage0102_ObjectValue ||
                    !Exception.IndexOutRange(mineRoadArea[i].x + addX, mineRoadArea[i].y - mineRoadArea[i].height - 1, groundData) ||
                    (Exception.IndexOutRange(mineRoadArea[i].x + addX, mineRoadArea[i].y - mineRoadArea[i].height - 1, groundData) &&
                    groundData[mineRoadArea[i].x + addX, mineRoadArea[i].y - mineRoadArea[i].height - 1] == (GroundLayer)(-1)))
                {
                    addX += Random.Range(10, 15);
                    continue;
                }

                int randomObject = Random.Range(0, 7);
                switch (randomObject)
                {
                    case 0:
                        ObjectManager.instance.Ladder(objectPos);
                        break;
                    case 1:
                        ObjectManager.instance.MineBox(objectPos);
                        break;
                    case 2:
                        ObjectManager.instance.MineCart(objectPos);
                        break;
                    case 3:
                        ObjectManager.instance.Stone(objectPos);
                        break;
                    case 4:
                        ObjectManager.instance.Wagon(objectPos);
                        break;
                    case 5:
                        ObjectManager.instance.WoodBoard(objectPos);
                        break;
                    case 6:
                        ObjectManager.instance.Shovel(objectPos);
                        break;
                }
                addX += Random.Range(10, 15);
            }
            #endregion

            #region[나무상자 배치]
            addX = 10;
            for (int j = 0; j < 15; j++)
            {
                if (i == 0)
                    continue;
                Vector2 objectPos = new Vector2(mineRoadArea[i].x + addX, mineRoadArea[i].y - mineRoadArea[i].height);
                if (Random.Range(0, 100) <= StageManager.instance.stage0102_WoodBoxValue ||
                    !Exception.IndexOutRange(mineRoadArea[i].x + addX, mineRoadArea[i].y - mineRoadArea[i].height - 1, groundData) ||
                    (Exception.IndexOutRange(mineRoadArea[i].x + addX, mineRoadArea[i].y - mineRoadArea[i].height - 1, groundData) &&
                    groundData[mineRoadArea[i].x + addX, mineRoadArea[i].y - mineRoadArea[i].height - 1] == (GroundLayer)(-1)))
                {
                    addX += Random.Range(10, 15);
                    continue;
                }
                ObjectManager.instance.WoodBox(objectPos);
                addX += Random.Range(10, 15);
            }
            #endregion

            #region[석순 배치]
            addX = 10;
            for (int j = 0; j < 15; j++)
            {
                if (i < Mathf.Abs(mineRoadArea.Count * 0.5f))
                    continue;

                int randomObject = Random.Range(0, 6);

                Vector2 objectPos = new Vector2(mineRoadArea[i].x + addX, mineRoadArea[i].y - mineRoadArea[i].height);
                Vector2Int tempPos = new Vector2Int((int)objectPos.x, (int)objectPos.y);

                addX += Random.Range(10, 15);

                if (Random.Range(0, 100) <= StageManager.instance.stage0102_TrapValue)
                    continue;

                switch (randomObject)
                {
                    case 0:
                    case 1:
                    case 2:
                        if ((Exception.IndexOutRange(tempPos.x, tempPos.y + 3, groundData) && groundData[tempPos.x, tempPos.y + 3] == (GroundLayer)(-1)) &&
        (Exception.IndexOutRange(tempPos.x + 1, tempPos.y + 3, groundData) && groundData[tempPos.x + 1, tempPos.y + 3] == (GroundLayer)(-1)))
                            ObjectManager.instance.Stalagmite(objectPos + new Vector2(0, 5.5f), randomObject);
                        break;
                    case 3:
                    case 4:
                    case 5:
                        if ((Exception.IndexOutRange(tempPos.x, tempPos.y, groundData) && groundData[tempPos.x, tempPos.y] == (GroundLayer)(-1)) &&
    (Exception.IndexOutRange(tempPos.x + 1, tempPos.y, groundData) && groundData[tempPos.x + 1, tempPos.y] == (GroundLayer)(-1)))
                            ObjectManager.instance.Stalagmite(objectPos + new Vector2(0, 1.5f), randomObject);
                        break;
                }
            }
            #endregion
        }
        #endregion

        #region[지뢰설치]
        List<Vector2> minePos = new List<Vector2>();

        for (int x = 0; x < world.WorldWidth; x++)
            for (int y = 50; y < world.WorldHeight - 20; y++)
                if (groundData[x, y] == GroundLayer.Dirt && Random.Range(0, 100) > 98)
                {
                    bool flag = true;
                    for (int k = 0; k < minePos.Count; k++)
                    {
                        if (Vector2.Distance(minePos[k], new Vector2(x, y)) < 5)
                        {
                            flag = false;
                            break;
                        }
                    }
                    if (flag)
                    {
                        minePos.Add(new Vector2(x, y));
                        ObjectManager.instance.LandMine(new Vector2(x, y));
                    }
                }
        #endregion

        #region[보물배치]
        if (Random.Range(0, 100) > 80)
        {
            List<Vector2Int> treasureList = new List<Vector2Int>();
            for (int y = world.WorldHeight / 2; y < world.WorldHeight - 1; y++)
            {
                for (int x = 0; x < world.WorldWidth - 1; x++)
                {
                    bool treasureFlag = true;
                    for (int i = 0; i < 15; i++)
                    {
                        int ax = x + i % 5;
                        int ay = y / 5;
                        if (Exception.IndexOutRange(ax, ay, groundData) && groundData[ax, ay] != GroundLayer.Dirt)
                            treasureFlag = false;
                        else if (!Exception.IndexOutRange(ax, ay, groundData))
                            treasureFlag = false;
                    }

                    if (treasureFlag)
                        treasureList.Add(new Vector2Int(x, world.WorldHeight - 1 - y));

                }
            }
            if (treasureList.Count > 0)
            {
                Vector2Int pos = treasureList[Random.Range(0, treasureList.Count)];
                ObjectManager.instance.TreasureBox(pos + new Vector2Int(3, 2));
            }
        }
        #endregion

        ObjectManager.instance.Sign(new Vector2(66 + (outlineflipX ? 0 : -52), 26), false);
        ObjectManager.instance.Sign(new Vector2(82 + (outlineflipX ? 0 : -52), 26), true);

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

        int[,] dic = new int[4, 2] { { +1, 0 }, { -1, 0 }, { 0, +1 }, { 0, -1 } };

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
                            int ax = emp.x + dic[k, 0];
                            int ay = emp.y + dic[k, 1];
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
                            int ax = emp.x + dic[k, 0];
                            int ay = emp.y + dic[k, 1];
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

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
}
