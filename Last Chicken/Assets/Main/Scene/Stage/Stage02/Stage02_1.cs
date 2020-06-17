﻿using Custom;
using System.Collections.Generic;
using UnityEngine;
using TerrainEngine2D;

public class Stage02_1 : StageData
{
    int[,] maxRect;

    public GroundLayer[,] mapRect;
    public FluidType[,] mapFluid;
    public BackGroundLayer[,] mapBackGround;

    Vector2Int iglooPos;

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
        Texture2D temp;
        for (int i = 0; i < 5; i++)
        {
            temp = Resources.Load("TerrainData/Stage02/Variation" + i) as Texture2D;
            variation.Add(temp);
        }
        int r = Random.Range(0, variation.Count);
        temp = variation[r];

        mapRect = new GroundLayer[temp.width, temp.height];
        mapFluid = new FluidType[temp.width, temp.height];

        for (int i = 0; i < temp.width; i++)
            for (int j = 0; j < temp.height; j++)
                mapRect[i, j] = GroundManager.instance.ColorToGroundData(temp.GetPixel(i, j));

        for (int i = 0; i < temp.width; i++)
            for (int j = 0; j < temp.height; j++)
                mapFluid[i, j] = GroundManager.instance.ColorToFluidData(temp.GetPixel(i, j));

        for (int i = 0; i < temp.width; i++)
            for (int j = 0; j < temp.height; j++)
                if (temp.GetPixel(i, j) == new Color(0, 0, 0))
                    iglooPos = new Vector2Int(i, j);
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
        MonsterManager.instance.Init(world, StageManager.instance.stage0201_Monsters.monsterNum, StageManager.instance.stage0201_Monsters.monsterDistance);
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

        for (int i = 0; i < mapRect.GetLength(0); i++)
            for (int j = 0; j < mapRect.GetLength(1); j++)
                groundData[i, j] = mapRect[i, j];

        for (int i = 0; i < mapFluid.GetLength(0); i++)
            for (int j = 0; j < mapFluid.GetLength(1); j++)
                fluidData[i, j] = mapFluid[i, j];
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

    #region[오브젝트 설치]
    void SetObject()
    {

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

        #region[이글루]
        if (iglooPos != new Vector2Int(0, 0))
            ObjectManager.instance.Igloo(iglooPos);
        #endregion

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

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
}
