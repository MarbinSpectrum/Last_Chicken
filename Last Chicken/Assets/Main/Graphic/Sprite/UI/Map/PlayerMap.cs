using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TerrainEngine2D;
using Custom;

public class PlayerMap : MonoBehaviour
{
    public static PlayerMap instance;

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
    public Texture2D maptexture;
    public Image mapImgback;
    public Image mapImg;
    public GameObject map;
    private void Awake()
    {
        instance = this;
    }

    public void CameraSettingReset()
    {
        CameraController.Instance.SetOffset(0);
        Invoke("CameraEdgeCheck", 2.5f);
    }

    public void CameraEdgeCheck()
    {
        CameraController.Instance.edgeCheck = true;
        CameraController.Instance.edgeX = true;
    }

    void Update()
    {
        mapImgback.color = default_Color;

        if(!GameManager.instance.InGame())
        {
            thisUse = false;
            map.SetActive(false);
            return;
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            if (Player.instance.grounded && Player.instance.canControl && !Player.instance.damage && !thisUse)
            {
                if (!CaveManager.inCave)
                {
                    CameraController.Instance.SetOffset(13);
                    CameraController.Instance.edgeX = false;
                    CameraController.Instance.edgeCheck = false;
                    CancelInvoke();
                }
                Player.instance.canControl = false;
                map.SetActive(true);
                thisUse = true;
            }
            else if (thisUse)
            {
                if (!CaveManager.inCave)
                {
                    CameraSettingReset();
                }
                Player.instance.canControl = true;
                map.SetActive(false);
                thisUse = false;
            }

            if (map.activeSelf && mapImg && maptexture && StageData.instance)
            {

                for (int i = 0; i < maptexture.width; i++)
                    for (int j = 0; j < maptexture.height; j++)
                        maptexture.SetPixel(i, j, default_Color);
                if (!CaveManager.inCave)
                {
                    int worldW = StageData.instance.groundData.GetLength(0);
                    int worldH = StageData.instance.groundData.GetLength(1);
                    float value;
                    if (worldH >= worldW)
                    {
                        value = maptexture.height / (float)worldH * size;
                        for (int i = 0; i < worldW * value; i++)
                            for (int j = 0; j < maptexture.height; j++)
                            {
                                int ai = (int)(i / value);
                                int aj = (int)(j / value);
                                if (Exception.IndexOutRange(ai, aj, StageData.instance.groundData))
                                    if ((int)StageData.instance.groundData[ai, aj] != -1)
                                    {
                                        int[,] dic = new int[4, 2] { { 1, 0 }, { -1, 0 }, { 0, 1 }, { 0, -1 } };
                                        bool check = false;
                                        for(int d = 0; d < 4; d++)
                                            if (Exception.IndexOutRange(ai + dic[d, 0], aj + dic[d, 1], StageData.instance.groundData))
                                                if ((int)StageData.instance.groundData[ai + dic[d, 0], aj + dic[d, 1]] == -1)
                                                    check = true;
                                        if(!check)
                                            maptexture.SetPixel(i + (int)((maptexture.width - worldW * value) / 2) + offset.x, j + (int)((maptexture.height - worldH * value) / 2) + offset.y, Empty_Color);
                                        else
                                            maptexture.SetPixel(i + (int)((maptexture.width - worldW * value) / 2) + offset.x, j + (int)((maptexture.height - worldH * value) / 2) + offset.y, Color.white);
                                    }
                                    else
                                    {
                                        maptexture.SetPixel(i + (int)((maptexture.width - worldW * value) / 2) + offset.x, j + (int)((maptexture.height - worldH * value) / 2) + offset.y, Color.gray);
                                    }
                            }
                    }
                    else
                    {
                        value = maptexture.width / (float)worldW * size;
                        for (int i = 0; i < maptexture.width; i++)
                            for (int j = 0; j < worldH * value; j++)
                            {
                                int ai = (int)(i / value);
                                int aj = (int)(j / value);
                                if (Exception.IndexOutRange(ai, aj, StageData.instance.groundData))
                                    maptexture.SetPixel(i + (int)((maptexture.width - worldW * value) / 2) + offset.x, j + (int)((maptexture.height - worldH * value) / 2) + offset.y, GrounddataToColor(StageData.instance.groundData[ai, aj]));
                            }
                    }

                    for (int i = 0; i < maptexture.width; i++)
                        for (int j = 0; j < maptexture.height; j++)
                        {
                            Vector2 playerPos = Player.instance.transform.position;
                            playerPos += Vector2.up;
                            playerPos += Vector2.up;
                            playerPos *= value;
                            if (Vector2.Distance(playerPos, new Vector2(i, j)) < 3)
                            {
                                int ai = (int)(i / value);
                                int aj = (int)(j / value);
                                maptexture.SetPixel(i + (int)((maptexture.width - worldW * value) / 2) + offset.x, j + (int)((maptexture.height - worldH * value) / 2) + offset.y, Color.red);
                            }

                        }
                }
                maptexture.Apply(true);

                Sprite sprite = Sprite.Create(maptexture, new Rect(0, 0, maptexture.width, maptexture.height), new Vector2(0.5f, 0.5f));
                mapImg.sprite = sprite;

            }
        }
    }

    Color GrounddataToColor(StageData.GroundLayer groundLayer)
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
