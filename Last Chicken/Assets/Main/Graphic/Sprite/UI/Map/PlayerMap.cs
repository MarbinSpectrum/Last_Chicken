using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TerrainEngine2D;
using Custom;

public class PlayerMap : MonoBehaviour
{
    public static PlayerMap instance;

    public static bool useMineralMap;
    public static bool useTreasureMap;

    #region[지형 색상]
    [Header("흙")]
    public Color Dirt_Color;
    [Header("돌")]
    public Color Stone_Color;
    [Header("구리")]
    public Color Copper_Color;
    [Header("모래")]
    public Color Sand_Color;
    [Header("화강암")]
    public Color Granite_Color;
    [Header("철")]
    public Color Iron_Color;
    [Header("은")]
    public Color Silver_Color;
    [Header("금")]
    public Color Gold_Color;
    [Header("미스릴")]
    public Color Mithril_Color;
    [Header("다이아몬드")]
    public Color Diamond_Color;
    [Header("자철석")]
    public Color Magnetite_Color;
    [Header("티타늄")]
    public Color Titanium_Color;
    [Header("코발트")]
    public Color Cobalt_Color;
    [Header("얼음")]
    public Color Ice_Color;
    [Header("경계블록")]
    public Color UnBreakable_Color;
    [Header("잔디")]
    public Color Grass_Color;
    [Header("하트스톤")]
    public Color HearthStone_Color;
    [Header("빈곳")]
    public Color Empty_Color;
    #endregion

    //여백색상
    public Color default_Color;

    [Header("-----------------------------------------------------")]

    [HideInInspector]public bool thisUse;

    [Range(0.01f,1)]
    public float size;

    public Vector2Int offset;

    public GameObject map;

    public Texture2D maptexture;
    public Image mapImg;

    public Texture2D mineral_texture;
    public Image mineral_Img;
    public Transform mineralUI;
    Transform[] mineral_Data;
    Image[] mineralColor;
    Text[] mineralText;

    [HideInInspector]public bool exitArrow;

    World world;

    public Texture2D treasure_texture;
    public Image treasure_Img;

    #region[Awake]
    private void Awake()
    {
        instance = this;
        mineralColor = new Image[mineralUI.childCount];
        mineralText = new Text[mineralUI.childCount];
        mineral_Data = new Transform[mineralUI.childCount];
        for (int i = 0; i < mineralUI.childCount; i++)
        {
            mineral_Data[i] = mineralUI.GetChild(i).transform;
            mineralColor[i] = mineral_Data[i].Find("Image").GetComponent<Image>();
            mineralText[i] = mineral_Data[i].Find("Text").GetComponent<Text>();
        }
        for (StageData.GroundLayer mineral = StageData.GroundLayer.Dirt; mineral != StageData.GroundLayer.End; mineral++)
        {
            int m = (int)mineral;
            mineralColor[m].color = GroundDataToColor(mineral);
            mineralText[m].text = mineral.ToString();
        }
    }
    #endregion

    #region[Update]
    void Update()
    {
        if(!GameManager.instance.InGame())
        {
            thisUse = false;
            map.SetActive(false);
            return;
        }

        mineral_Img.gameObject.SetActive(useMineralMap);
        treasure_Img.gameObject.SetActive(useTreasureMap);
        
        if (world == null && World.Instance)
        {
            world = World.Instance;
            DrawMap();
        }

        UpdatePlayerPos();
        UpdateTreasurePos();

        if (Input.GetKeyDown(KeyCode.M))
        {
            if (Player.instance.grounded && Player.instance.canControl && !thisUse)
            {
                Player.instance.canControl = false;
                map.SetActive(true);
                thisUse = true;
                DrawPlayerPos();
            }
            else if (thisUse)
            {
                Player.instance.canControl = true;
                map.SetActive(false);
                thisUse = false;
            }
        }
    }
    #endregion

    #region[초기맵을 그리는작업]
    public void DrawMap()
    {
        if (mapImg && maptexture && mineral_texture  && StageData.instance)
        {
            if (!CaveManager.inCave)
            {
                int worldW = StageData.instance.groundData.GetLength(0);
                int worldH = StageData.instance.groundData.GetLength(1);
                float value = maptexture.height / Mathf.Max((float)worldH, (float)worldW) * size;

                for (StageData.GroundLayer mineral = StageData.GroundLayer.Dirt; mineral != StageData.GroundLayer.End; mineral++)
                {
                    int m = (int)mineral;
                    if (mineral_Data[m])
                        mineral_Data[m].gameObject.SetActive(false);
                }

                treasurePos.Clear();
                treasureCheck.Clear();

                for (int i = 0; i < ObjectManager.instance.treasurePos.Count; i++)
                {
                    treasurePos.Add(ObjectManager.instance.treasurePos[i].transform.position);
                    treasureCheck.Add(true);
                }


                for (int i = 0; i < maptexture.width; i++)
                    for (int j = 0; j < maptexture.height; j++)
                        maptexture.SetPixel(i, j, Empty_Color);
                for (int i = 0; i < mineral_texture.width; i++)
                    for (int j = 0; j < mineral_texture.height; j++)
                        mineral_texture.SetPixel(i, j, new Color(0,0,0,0));
                for (int i = 0; i < mineral_texture.width; i++)
                    for (int j = 0; j < mineral_texture.height; j++)
                        treasure_texture.SetPixel(i, j, new Color(0, 0, 0, 0));

                for (int i = 0; i < worldW * value; i++)
                    for (int j = 0; j < worldH * value; j++)
                        DrawMapPixel(i, j);

                for (int i = 0; i < treasurePos.Count; i++)
                    if (ObjectManager.instance.treasurePos[i].activeSelf)
                        DrawTreasurePos(i);
                DrawPlayerPos();

                maptexture.Apply(true);
                mineral_texture.Apply(true);
                treasure_texture.Apply(true);
            }
        }
    }
    #endregion

    #region[DrawMapPixel]
    //픽셀좌표를 받음
    public void DrawMapPixel(int x,int y)
    {
        int worldW = StageData.instance.groundData.GetLength(0);
        int worldH = StageData.instance.groundData.GetLength(1);
        float value = maptexture.height / Mathf.Max((float)worldH, (float)worldW) * size;
        //픽셀좌표를 블록좌표로
        int ax = (int)(x / value);
        int ay = (int)(y / value);

        if (Exception.IndexOutRange(ax, ay, StageData.instance.groundData))
        {
            //중심으로 이동
            int setPixelX = x + (int)((maptexture.width - worldW * value) / 2) + offset.x;
            int setPixelY = y + (int)((maptexture.height - worldH * value) / 2) + offset.y;

            //공백지형이 아니면
            if (StageData.instance.groundData[ax, ay] != StageData.GroundLayer.Empty)
            {
                //상하좌우에 빈공간이 존재하는 지형인지 검사
                int[,] dic = new int[4, 2] { { 1, 0 }, { -1, 0 }, { 0, 1 }, { 0, -1 } };
                bool check = false;
                for (int d = 0; d < 4; d++)
                    if (Exception.IndexOutRange(ax + dic[d, 0], ay + dic[d, 1], StageData.instance.groundData))
                        if (StageData.instance.groundData[ax + dic[d, 0], ay + dic[d, 1]] == StageData.GroundLayer.Empty)
                            check = true;

                if (!check)
                    maptexture.SetPixel(setPixelX, setPixelY, Empty_Color);
                else
                    maptexture.SetPixel(setPixelX, setPixelY, Color.white);
            }
            else
                maptexture.SetPixel(setPixelX, setPixelY, new Color(0,0,0.5f,0.7f));
        }
        else
            maptexture.SetPixel(x, y, Empty_Color);

        if (Exception.IndexOutRange(ax, ay, StageData.instance.groundData))
        {
            //중심으로 이동
            int setPixelX = x + (int)((mineral_texture.width - worldW * value) / 2) + offset.x;
            int setPixelY = y + (int)((mineral_texture.height - worldH * value) / 2) + offset.y;

            if (StageData.instance.groundData[ax, ay] == StageData.GroundLayer.Empty || 
                StageData.instance.groundData[ax, ay] == StageData.GroundLayer.Dirt ||
                StageData.instance.groundData[ax, ay] == StageData.GroundLayer.Sand ||
                StageData.instance.groundData[ax, ay] == StageData.GroundLayer.Stone ||
                StageData.instance.groundData[ax, ay] == StageData.GroundLayer.Ice ||
                StageData.instance.groundData[ax, ay] == StageData.GroundLayer.Grass ||
                StageData.instance.groundData[ax, ay] == StageData.GroundLayer.UnBreakable)
                mineral_texture.SetPixel(setPixelX, setPixelY, new Color(0, 0, 0, 0));
            else
            {
                mineral_texture.SetPixel(setPixelX, setPixelY, GroundDataToColor(StageData.instance.groundData[ax, ay]));
                int m = (int)(StageData.instance.groundData[ax, ay]);
                if (mineral_Data[m])
                    mineral_Data[m].gameObject.SetActive(true);
            }
        }
        else
            mineral_texture.SetPixel(x, y, new Color(0, 0, 0, 0));

        int arrowStartX = exitArrow ? (int)(worldW * value) - 43 : 40;
        int arrowStartY = 15;

        if(SceneController.instance.nowScene.Contains("Stage") && arrowStartX <= x && x < arrowStartX + 7 && arrowStartY <= y && y < arrowStartY + 9)
        {
            int[,] endMark =
                {
                            { 0,0,0,1,0,0,0 },
                            { 0,0,1,1,1,0,0 },
                            { 0,1,0,1,0,1,0 },
                            { 1,0,0,1,0,0,1 },
                            { 0,0,0,1,0,0,0 },
                            { 0,0,0,1,0,0,0 },
                            { 0,0,0,1,0,0,0 },
                            { 0,0,0,1,0,0,0 },
                            { 0,0,0,1,0,0,0 }
                };

            int markX = x - arrowStartX;
            int markY = y - arrowStartY;
            if (endMark[markY, markX] == 1)
            {
                int setPixelX = x + (int)((maptexture.width - worldW * value) / 2) + offset.x;
                int setPixelY = y + (int)((maptexture.height - worldH * value) / 2) + offset.y;

                maptexture.SetPixel(setPixelX, setPixelY, Color.white);
            }
        }
    }

    public void DrawMapGroundData(int x, int y)
    {
        int worldW = StageData.instance.groundData.GetLength(0);
        int worldH = StageData.instance.groundData.GetLength(1);
        float value = maptexture.height / Mathf.Max((float)worldH, (float)worldW) * size;

        Vector2 drawPos = new Vector2(x,y);
        drawPos += playerOffset;
        drawPos *= value;

        for (int ay = (int)drawPos.y - 5; ay <= drawPos.y + 5; ay++)
            for (int ax = (int)drawPos.x - 5; ax <= drawPos.x + 5; ax++)
                DrawMapPixel(ax, ay);

        maptexture.Apply(true);
        mineral_texture.Apply(true);
    }
    #endregion

    Vector2 playerPos;
    Vector2 playerOffset = new Vector2(0, 2);

    #region[플레이어 위치 갱신]
    public void UpdatePlayerPos()
    {
        if (playerPos != (Vector2)Player.instance.transform.position && !CaveManager.inCave)
            DrawPlayerPos();
    }

    public void DrawPlayerPos()
    {
        int worldW = StageData.instance.groundData.GetLength(0);
        int worldH = StageData.instance.groundData.GetLength(1);
        float value = maptexture.height / Mathf.Max((float)worldH, (float)worldW) * size;

        Vector2 deletePos = playerPos;
        deletePos += playerOffset;
        deletePos *= value;

        for (int y = (int)deletePos.y - 4; y <= deletePos.y + 4; y++)
            for (int x = (int)deletePos.x - 4; x <= deletePos.x + 4; x++)
            {
                int setPixelX = x + (int)((maptexture.width - worldW * value) / 2) + offset.x;
                int setPixelY = y + (int)((maptexture.height - worldH * value) / 2) + offset.y;
                maptexture.SetPixel(setPixelX, setPixelY, Empty_Color);
                DrawMapPixel(x, y);
            }

        playerPos = (Vector2)Player.instance.transform.position;

        Vector2 drawPos = playerPos;
        drawPos += playerOffset;
        drawPos *= value;

        for (int y = (int)drawPos.y - 3; y <= drawPos.y + 3; y++)
            for (int x = (int)drawPos.x - 3; x <= drawPos.x + 3; x++)
            {
                int setPixelX = x + (int)((maptexture.width - worldW * value) / 2) + offset.x;
                int setPixelY = y + (int)((maptexture.height - worldH * value) / 2) + offset.y;
                if (Vector2.Distance(drawPos, new Vector2(x, y)) < 3)
                    maptexture.SetPixel(setPixelX, setPixelY, Color.red);
            }

        maptexture.Apply(true);
        mineral_texture.Apply(true);
    }
    #endregion

    List<Vector2> treasurePos = new List<Vector2>();
    List<bool> treasureCheck = new List<bool>();

    #region[보물 위치 갱신]
    public void UpdateTreasurePos()
    {
        if(treasurePos != null)
            for (int i = 0; i < treasurePos.Count; i++)
            {
                if (ObjectManager.instance.treasurePos[i].activeSelf)
                {
                    if (treasurePos[i] != (Vector2)ObjectManager.instance.treasurePos[i].transform.position)
                        DrawTreasurePos(i);

                }
                else if (treasureCheck[i])
                {
                    treasureCheck[i] = false;
                    DrawTreasurePos(i);
                }
            }
    }

    public void DrawTreasurePos(int i)
    {
        int worldW = StageData.instance.groundData.GetLength(0);
        int worldH = StageData.instance.groundData.GetLength(1);
        float value = maptexture.height / Mathf.Max((float)worldH, (float)worldW) * size;

        Vector2 deletePos = treasurePos[i];
        deletePos *= value;

        for (int y = (int)deletePos.y - 3; y <= deletePos.y + 3; y++)
            for (int x = (int)deletePos.x - 3; x <= deletePos.x + 3; x++)
            {
                int setPixelX = x + (int)((maptexture.width - worldW * value) / 2) + offset.x;
                int setPixelY = y + (int)((maptexture.height - worldH * value) / 2) + offset.y;
                treasure_texture.SetPixel(setPixelX, setPixelY, new Color(0,0,0,0));
            }

        treasurePos[i] = (Vector2)ObjectManager.instance.treasurePos[i].transform.position;

        if (ObjectManager.instance.treasurePos[i].activeSelf)
        {
            Vector2 drawPos = treasurePos[i];
            drawPos *= value;

            int[,] treasureMark =
                {
                            { 1,1,0,0,0,1,1 },
                            { 1,1,1,0,1,1,1 },
                            { 0,1,1,1,1,1,0 },
                            { 0,0,1,1,1,0,0 },
                            { 0,1,1,1,1,1,0 },
                            { 1,1,1,0,1,1,1 },
                            { 1,1,0,0,0,1,1 }
                };

            for (int y = (int)drawPos.y - 3; y <= drawPos.y + 3; y++)
                for (int x = (int)drawPos.x - 3; x <= drawPos.x + 3; x++)
                {
                    int markX = x - (int)drawPos.x + 3;
                    int markY = y - (int)drawPos.y + 3;

                    int setPixelX = x + (int)((maptexture.width - worldW * value) / 2) + offset.x;
                    int setPixelY = y + (int)((maptexture.height - worldH * value) / 2) + offset.y;
                    treasure_texture.SetPixel(setPixelX, setPixelY, treasureMark[markX, markY] == 1 ? Color.red : new Color(0, 0, 0, 0));
                }
        }

        treasure_texture.Apply(true);
    }
    #endregion

    Color GroundDataToColor(StageData.GroundLayer groundLayer)
    {
        switch (groundLayer)
        {
            case StageData.GroundLayer.Dirt: return Dirt_Color; //Dirt
            case StageData.GroundLayer.Stone: return Stone_Color; //Stone
            case StageData.GroundLayer.Copper: return Copper_Color; //Copper
            case StageData.GroundLayer.Sand: return Sand_Color; //Sand
            case StageData.GroundLayer.Granite: return Granite_Color; //Granite
            case StageData.GroundLayer.Iron: return Iron_Color; //Iron
            case StageData.GroundLayer.Silver: return Silver_Color; //Silver
            case StageData.GroundLayer.Gold: return Gold_Color; //Gold
            case StageData.GroundLayer.Mithril: return Mithril_Color; //Mithril
            case StageData.GroundLayer.Diamond: return Diamond_Color; //Diamond
            case StageData.GroundLayer.Magnetite: return Magnetite_Color; //Magnetite
            case StageData.GroundLayer.Titanium: return Titanium_Color; //Titanum
            case StageData.GroundLayer.Cobalt: return Cobalt_Color; //Cobalt
            case StageData.GroundLayer.Ice: return Ice_Color; //Ice
            case StageData.GroundLayer.UnBreakable: return UnBreakable_Color; //NonBreak
            case StageData.GroundLayer.Grass: return Grass_Color; //Grass
            case StageData.GroundLayer.HearthStone: return HearthStone_Color; //HeathStone
            default:
                return Empty_Color; //Empty
        }
    }
}
