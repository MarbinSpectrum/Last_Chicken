using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Custom;

public class MouseManager : MonoBehaviour
{
    public static MouseManager instance;

    //커서 정보들
    List<Texture2D> normalCurosr = new List<Texture2D>();
    List<Texture2D> attackCurosr = new List<Texture2D>();
    List<Texture2D> unableCurosr = new List<Texture2D>();
    List<Texture2D> pickCurosr = new List<Texture2D>();
    List<Texture2D> clickCurosr = new List<Texture2D>();

    //화면에 따른 커서 사이즈
    int cursorSize = 0;

    List<RaycastResult> rayResults = new List<RaycastResult>();

    [System.NonSerialized] public Vector2Int mousePos;

    Vector2Int updateMousePos;

    [Header("일반커서")]
    public Texture2D normalCursor;

    [Header("공격커서")]
    public Texture2D attackCursor;

    [Header(" X 커서")]
    public Texture2D unableCursor;

    [Header("곡괭이커서")]
    public Texture2D pickCursor;

    [Header("클릭커서")]
    public Texture2D clickCursor;

    int[] mouseSize = new int[6] {21,26,28,33,33,40};

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region[Awake]
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    #endregion

    #region[Start]
    void Start()
    {
        for (int i = 0; i < UIManager.instance.windowOption.GetLength(0); i++)
        {
            Texture2D normal = ResizeTexture2D.ResizeTextureMouse(normalCursor, mouseSize[i], mouseSize[i]);
            normalCurosr.Add(normal);

            Texture2D attack = ResizeTexture2D.ResizeTextureMouse(attackCursor, mouseSize[i], mouseSize[i]);
            attackCurosr.Add(attack);

            Texture2D unable = ResizeTexture2D.ResizeTextureMouse(unableCursor, mouseSize[i], mouseSize[i]);
            unableCurosr.Add(unable);

            Texture2D pick = ResizeTexture2D.ResizeTextureMouse(pickCursor, mouseSize[i], mouseSize[i]);
            pickCurosr.Add(pick);

            Texture2D click = ResizeTexture2D.ResizeTextureMouse(clickCursor, mouseSize[i], mouseSize[i]);
            clickCurosr.Add(click);
        }
        UpdateMouseCursor();
    }
    #endregion

    #region[Update]
    void Update()
    {
        cursorSize = GetCursorSize();
        if (cursorSize < 0)
            return;

        if (KeyManager.nowController != GameController.KeyBoard)
            Cursor.visible = false;
        else
            Cursor.visible = true;


        //마우스 좌표 갱신
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos = new Vector2Int(Mathf.FloorToInt(mousePosition.x), Mathf.FloorToInt(mousePosition.y));

        //마우스가 UI에 레이캐스트 중인지 검사
        var ped = new PointerEventData(null);
        ped.position = Input.mousePosition;
        rayResults.Clear();
        UIManager.instance.graphicRaycaster.Raycast(ped, rayResults);

        //마우스가 이동하면 마우스 포인터 모양 갱신
        if (updateMousePos != mousePos)
        {
            updateMousePos = mousePos;
            UpdateMouseCursor();
        }
    }
    #endregion

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region[화면 사이즈를 고려해서 알맞는 커서를 찾아줌]
    int GetCursorSize()
    {
        for (int i = 0; i < UIManager.instance.windowOption.GetLength(0); i++)
            if (Screen.width == UIManager.instance.windowOption[i, 0] && Screen.height == UIManager.instance.windowOption[i, 1])
                return i;
        return -1;
    }
    #endregion

    #region[마우스 커서 갱신]
    public void UpdateMouseCursor()
    {
        if (AttackCusorConditon())
            Cursor.SetCursor(attackCurosr[cursorSize], Vector2.zero, CursorMode.Auto);
        else if (NormalCusorConditon())
            Cursor.SetCursor(normalCurosr[cursorSize], Vector2.zero, CursorMode.Auto);
    }
    #endregion

    #region[일반 커서 조건]
    bool NormalCusorConditon()
    {
        return true;
    }
    #endregion

    #region[공격 커서 조건]
    bool AttackCusorConditon()
    {
        if (SceneController.instance.nowScene.Equals("Title") || SceneController.instance.nowScene.Equals("Prologue"))
            return false;
        if (rayResults.Count > 0)
            return false;
        if (AttackingCheck.aniTime > 0)
            return true;
        return false;
    }
    #endregion

}