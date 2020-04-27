using Custom;
using System.Collections.Generic;
using UnityEngine;
using TerrainEngine2D;

public class EventMap : StageData
{
    public GameObject leftWall;
    public GameObject rightWall;
    public enum ExitDic { 없음, 왼쪽, 오른쪽, 양쪽 }
    protected bool flipX_EventMap;

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

    #region[배경 설정]
    public override void SetBackGround()
    {
        backGroundData = new BackGroundLayer[world.WorldWidth, world.WorldHeight];
        for (int x = 0; x < world.WorldWidth; x++)
            for (int y = 0; y < world.WorldHeight; y++)
                backGroundData[x, y] = GroundManager.instance.eventBackGround[x, y];
    }
    #endregion

    #region[지형 설정]
    public override void SetGround()
    {
        groundData = new GroundLayer[world.WorldWidth, world.WorldHeight];
        fluidData = new FluidType[world.WorldWidth, world.WorldHeight];

        for (int i = 0; i < GroundManager.instance.eventMapRect.GetLength(0); i++)
            for (int j = 0; j < GroundManager.instance.eventMapRect.GetLength(1); j++)
                groundData[i, j] = GroundManager.instance.eventMapRect[i, j];

        for (int i = 0; i < GroundManager.instance.eventMapRect.GetLength(0); i++)
            for (int j = 0; j < GroundManager.instance.eventMapRect.GetLength(1); j++)
                fluidData[i, j] = GroundManager.instance.eventMapFluid[i, j];
    }
    #endregion

    #region[이벤트맵 세팅]
    public void EventMapSetting(ExitDic activeWall,int dic)
    {
        if (GroundManager.instance.eventMapData == null || GroundManager.instance.eventMapBackData == null)
            return;
        GroundManager.instance.eventMapStartDic = dic;
        flipX_EventMap = UnityEngine.Random.Range(0, 100) > 50;
        Texture2D mapData = new Texture2D(GroundManager.instance.eventMapData.width, GroundManager.instance.eventMapData.height);
        Texture2D mapBackData = new Texture2D(GroundManager.instance.eventMapBackData.width, GroundManager.instance.eventMapBackData.height);

        for (int y = 0; y < mapData.height; y++)
            for (int x = 0; x < mapData.width; x++)
            {
                mapData.SetPixel(flipX_EventMap ? mapData.width - x - 1 : x, y, GroundManager.instance.eventMapData.GetPixel(x, y));
                mapBackData.SetPixel(flipX_EventMap ? mapBackData.width - x - 1 : x, y, GroundManager.instance.eventMapBackData.GetPixel(x, y));
            }

        GroundManager.instance.eventMapData = mapData;

        GroundManager.instance.eventMapRect = new GroundLayer[GroundManager.instance.eventMapData.width, GroundManager.instance.eventMapData.height];
        GroundManager.instance.eventMapFluid = new FluidType[GroundManager.instance.eventMapData.width, GroundManager.instance.eventMapData.height];
        GroundManager.instance.eventBackGround = new BackGroundLayer[GroundManager.instance.eventMapBackData.width, GroundManager.instance.eventMapBackData.height];

        GroundManager.instance.eventMapMainObjectPos = Vector2.zero;
        GroundManager.instance.eventMapTreasurePos.Clear();

        for (int i = 0; i < GroundManager.instance.eventMapData.width; i++)
            for (int j = 0; j < GroundManager.instance.eventMapData.height; j++)
                if (GroundManager.instance.eventMapData.GetPixel(i, j) == new Color(0.2f, 0, 0))
                    GroundManager.instance.eventMapStartPos = new Vector2(i, j);
                else if (GroundManager.instance.eventMapData.GetPixel(i, j) == new Color(0.2f, 0.2f, 0))
                    GroundManager.instance.eventMapMainObjectPos = new Vector2(i, j);
                else if (GroundManager.instance.eventMapData.GetPixel(i, j) == new Color(0.2f, 0.2f, 0.2f))
                    GroundManager.instance.eventMapTreasurePos.Add(new Vector2(i, j));

        for (int i = 0; i < GroundManager.instance.eventMapData.width; i++)
            for (int j = 0; j < GroundManager.instance.eventMapData.height; j++)
                GroundManager.instance.eventMapRect[i, j] = GroundManager.instance.ColorToGroundData(GroundManager.instance.eventMapData.GetPixel(i, j));

        for (int i = 0; i < GroundManager.instance.eventMapData.width; i++)
            for (int j = 0; j < GroundManager.instance.eventMapData.height; j++)
                GroundManager.instance.eventMapFluid[i, j] = GroundManager.instance.ColorToFluidData(GroundManager.instance.eventMapData.GetPixel(i, j));

        for (int i = 0; i < GroundManager.instance.eventMapBackData.width; i++)
            for (int j = 0; j < GroundManager.instance.eventMapBackData.height; j++)
                GroundManager.instance.eventBackGround[i, j] = GroundManager.instance.ColorToBackData(GroundManager.instance.eventMapBackData.GetPixel(i, j));

        if (flipX_EventMap)
        {
            GroundManager.instance.eventMapStartDic = -GroundManager.instance.eventMapStartDic;
            switch (activeWall)
            {
                case ExitDic.왼쪽:
                    activeWall = ExitDic.오른쪽;
                    break;
                case ExitDic.오른쪽:
                    activeWall = ExitDic.왼쪽;
                    break;
            }
        }

        switch (activeWall)
        {
            case ExitDic.왼쪽:
                leftWall.SetActive(false);
                rightWall.SetActive(true);
                break;
            case ExitDic.오른쪽:
                leftWall.SetActive(true);
                rightWall.SetActive(false);
                break;
            case ExitDic.없음:
                leftWall.SetActive(true);
                rightWall.SetActive(true);
                break;
            case ExitDic.양쪽:
                leftWall.SetActive(false);
                rightWall.SetActive(false);
                break;
        }
    }
    #endregion

    #region[지형 정보생성]
    public Texture2D CreateData(int w,int h, Color c)
    {
        Texture2D temp = new Texture2D(w, h);
        for (int x = 0; x < w; x++)
            for (int y = 0; y < h; y++)
                temp.SetPixel(x, y, c);
        return temp;
    }

    public Texture2D CreateData(int w, int h)
    {
        return CreateData(w,h,Color.black);
    }
    #endregion

    #region[오브젝트 설치]
    public virtual void SetObject()
    {
        if (GroundManager.instance.eventMapMainObjectPos != Vector2.zero)
            ObjectManager.instance.Shop(GroundManager.instance.eventMapMainObjectPos);

        for (int i = 0; i < GroundManager.instance.eventMapTreasurePos.Count; i++)
            ObjectManager.instance.TreasureBox(GroundManager.instance.eventMapTreasurePos[i]);

    }
    #endregion
}
