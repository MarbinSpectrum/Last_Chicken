using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecordManager : MonoBehaviour
{
    public static RecordManager instance;

    ///////////////////////////////////////////////////////////////////////////////////

    private CanvasScaler canvasScaler;

    private ScrollRect monsterScrollView;
    private RectTransform monsterContent;
    private GridLayoutGroup monsterGrid;
    private ScrollRect itemScrollView;
    private RectTransform itemContent;
    private GridLayoutGroup itemGrid;

    ///////////////////////////////////////////////////////////////////////////////////

    public RectTransform monsterData;
    public Vector2 monsterDataSpace;
    public Vector2 monsterDataSize;
    public RectTransform itemData;
    public Vector2 itemDataSpace;
    public Vector2 itemDataSize;

    public List<GameObject> languageData = new List<GameObject>();

    #region[Awake]
    private void Awake()
    {
        instance = this;
        canvasScaler = GetComponent<CanvasScaler>();
        GameObject scrollView = transform.Find("몬스터도감").gameObject;
        if(scrollView != null)
        {
            monsterScrollView = scrollView.GetComponent<ScrollRect>();
            monsterContent = monsterScrollView.content;
            monsterGrid = monsterScrollView.content.GetComponent<GridLayoutGroup>();
        }

        scrollView = transform.Find("아이템도감").gameObject;
        if (scrollView != null)
        {
            itemScrollView = scrollView.GetComponent<ScrollRect>();
            itemContent = itemScrollView.content;
            itemGrid = itemScrollView.content.GetComponent<GridLayoutGroup>();
        }

    }
    #endregion

    #region[Start]
    private void Start()
    {
        for (int i = 0; i < MonsterManager.monsterName.Length; i++)
        {
            GameObject temp = Instantiate(monsterData, monsterContent.transform).gameObject;
            temp.GetComponent<MonsterData>().monster_name = MonsterManager.monsterName[i];
        }

        for (int i = 0; i < ItemManager.itemName.Length; i++)
        {
            GameObject temp = Instantiate(itemData, itemContent.transform).gameObject;
            temp.GetComponent<ItemData>().item_name = ItemManager.itemName[i];
        }

        Invoke("ShowMonsterList", 0.1f);
    }
    #endregion

    #region[Update]
    void Update()
    {
        for (int i = 0; i < languageData.Count; i++)
            if (languageData[i])
                languageData[i].SetActive(languageData[i].transform.name.Contains(GameManager.instance.playData.language.ToString()));

        if (monsterData != null)
            monsterData.sizeDelta = monsterDataSize;
        if (monsterGrid != null)
        {
            monsterGrid.cellSize = monsterDataSize;
            monsterGrid.spacing = monsterDataSpace;
        }
        if (monsterContent != null)
            monsterContent.sizeDelta = new Vector2(monsterContent.sizeDelta.x,200 + monsterDataSize.y * Mathf.Ceil(MonsterManager.monsterName.Length / 3f) + monsterDataSpace.y * (MonsterManager.monsterName.Length / 3));


        if (itemData != null)
            itemData.sizeDelta = itemDataSize;
        if (itemGrid != null)
        {
            itemGrid.cellSize = itemDataSize;
            itemGrid.spacing = itemDataSpace;
        }
        if (itemContent != null)
            itemContent.sizeDelta = new Vector2(itemContent.sizeDelta.x, 200 + itemDataSize.y * Mathf.Ceil(ItemManager.itemName.Length / 3f) + itemDataSpace.y * (ItemManager.itemName.Length / 3));
    }
    #endregion

    ///////////////////////////////////////////////////////////////////////////////////

    #region[나가기]
    /// <summary> 타이틀화면으로 이동하는 처리 </summary>
    public void GoTitle()
    {
        if (SceneController.instance.nowSceneMoving)
            return;
        SoundManager.instance.BtnClick();
        SceneController.instance.MoveScene("Title");
    }


    #endregion

    #region[아이템 리스트 확인]
    public void ShowItemList()
    {
        monsterScrollView.gameObject.SetActive(false);
        itemScrollView.gameObject.SetActive(true);
        itemContent.anchoredPosition = new Vector2(0, 0);
    }
    #endregion

    #region[몬스터 리스트 확인]
    public void ShowMonsterList()
    {
        monsterScrollView.gameObject.SetActive(true);
        itemScrollView.gameObject.SetActive(false);
        monsterContent.anchoredPosition = new Vector2(0, 0);
    }
    #endregion

}
