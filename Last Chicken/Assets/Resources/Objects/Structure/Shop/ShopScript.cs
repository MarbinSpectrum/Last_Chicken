using Custom;
using TerrainEngine2D;
using UnityEngine;
using System.Collections.Generic;

public class ShopScript : AreaScript
{
    public static ShopScript instance;

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    [System.NonSerialized] public bool thisUse = false;
    [System.NonSerialized] public List<int> itmeList = new List<int>();
    [System.NonSerialized] public List<bool> itmeBuyList = new List<bool>();
    [System.NonSerialized] public bool onArea;
    [System.NonSerialized] public bool shopVIP;
    [System.NonSerialized] public int randomDice;
    GameObject uiMouse;

    public List<GameObject> languageData = new List<GameObject>();
    bool upTrigger;

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 

    #region[Awake]
    public override void Awake()
    {
        instance = this;
        bodyCollider = GetComponent<BoxCollider2D>();

        uiMouse = transform.Find("UIMouse").gameObject;
        Init();
    }
    #endregion

    #region[Update]
    public override void Update()
    {
        for (int i = 0; i < languageData.Count; i++)
            if (languageData[i])
                languageData[i].SetActive(languageData[i].transform.name.Contains(GameManager.instance.playData.language.ToString()));
        onArea = IsAtPlayer(bodyCollider);
        UseArea();
        UpdateShopData();
    }
    #endregion

    #region[OnEnable]
    void OnEnable()
    {
        instance = this;
    }
    #endregion

    #region[Init]
    public override void Init()
    {
        SetShopItem();
        used = false;
        thisUse = false;
        act = false;
    }
    #endregion

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region[UseArea]
    //플레이어가 오브젝트를 사용할 수 있는지 검사
    public override void UseArea()
    {
        if (GameManager.instance.gamePause)
            return;
        if (Input.GetAxisRaw("Vertical") == 0)
            upTrigger = false;
        if (!used && IsAtPlayer(bodyCollider))
        {
            if (Input.GetKeyDown(KeyCode.W) || (Input.GetAxisRaw("Vertical") > 0 && !upTrigger))
            {
                upTrigger = true;
                if (Player.instance.canControl && !thisUse)
                {
                    Player.instance.canControl = false;
                    thisUse = true;
                }
                else if (thisUse)
                {
                    Player.instance.canControl = true;
                    thisUse = false;
                }
            }
            uiMouse.SetActive(!thisUse);
        }
        else
            uiMouse.SetActive(false);
    }
    #endregion

    #region[상점정보 업데이트]
    void UpdateShopData()
    {
        shopVIP = GameManager.instance.playData.shopVIP;
        randomDice = GameManager.instance.playData.randomDice;

        for (int i = 0; i < 3; i++)
        {
            UIManager.instance.shopItemCost[i].text = GetItemValue(ItemManager.itemName[itmeList[i]]) + "$";
            if(GameManager.instance.playData.language == PlayData.Language.한국어)
                UIManager.instance.shopItemName[i].text = ItemManager.instance.itemData[itmeList[i]].itemName;
            else if (GameManager.instance.playData.language == PlayData.Language.English)
                UIManager.instance.shopItemName[i].text = ItemManager.instance.itemData[itmeList[i]].itemName_Eng;
            UIManager.instance.shopItemImg[i].sprite = ItemManager.instance.itemData[itmeList[i]].itemImg;
            if (itmeBuyList[i])
            {
                UIManager.instance.shopItemImg[i].color = Color.black;
                for (int k = 0; k < 4; k++)
                    UIManager.instance.shopItemExplan[i, k].text = "";

                if (GameManager.instance.playData.language == PlayData.Language.한국어)
                    UIManager.instance.shopItemExplan[i, 1].text = "[매진]";
                else if (GameManager.instance.playData.language == PlayData.Language.English)
                    UIManager.instance.shopItemExplan[i, 1].text = "[Sold Out]";
            }
            else
            {

                for(int k = 0; k < 4; k++)
                {
                    if (GameManager.instance.playData.language == PlayData.Language.한국어)
                        UIManager.instance.shopItemExplan[i, k].text = ItemManager.instance.itemData[itmeList[i]].shopItemExplain[k];
                    else if (GameManager.instance.playData.language == PlayData.Language.English)
                        UIManager.instance.shopItemExplan[i, k].text = ItemManager.instance.itemData[itmeList[i]].shopItemExplain_Eng[k];
                }
                UIManager.instance.shopItemImg[i].color = Color.white;
            }
        }
    }
    #endregion

    #region[상점 아이템 설정]
    public void SetShopItem()
    {
        itmeList.Clear();
        itmeBuyList.Clear();
        for (int i = 0; i < 3; i++)
        {
            int itemIndex = 0;

            for(int j =0; j < 1000; j++)
            { 
                itemIndex = ItemManager.instance.GetRandomItemAtShop();
                if (!itmeList.Contains(itemIndex))
                    break;
            } 

            itmeList.Add(itemIndex);
            itmeBuyList.Add(false);
        }
    }
    #endregion

    #region[상점 아이템 구매]
    public void BuyItem(int itemNum)
    {
        if (itmeBuyList[itemNum])
        {
            SoundManager.instance.CantRun();
            return;
        }

        int cost = GetItemValue(ItemManager.itemName[itmeList[itemNum]]);

        if (GameManager.instance.playerMoney < cost)
        {
            SoundManager.instance.CantRun();
            return;
        }

        GameManager.instance.playerMoney -= cost;

        if(ItemManager.instance.HasItemCheck("SaleCoupon"))
            ItemManager.instance.CostItem("SaleCoupon");

        itmeBuyList[itemNum] = true;

        string itemName = ItemManager.itemName[itmeList[itemNum]];

        int itemCount = 0;
        //if (itemName.Equals("BoomItem"))
        //    itemCount = (int)ItemManager.instance.itemData[ItemManager.FindData(itemName)].value1;
        //else if (itemName.Equals("Dynamite"))
        //    itemCount = (int)ItemManager.instance.itemData[ItemManager.FindData(itemName)].value1;

        if (ItemManager.instance.AddItemCheck(itemName))
            ItemManager.instance.AddItem(itemName, itemCount);
        else if (ItemManager.CheckGetActiveItem(itemName))
            Player.instance.ActItem(itemName);
        else
        {
            //빈슬롯검사
            int emptySlot = -1;
            if (ItemManager.CheckPassiveItem(itemName))
            {
                for (int i = 1; i < 6; i++)
                {
                    if (!GameManager.instance.slotAct[i])
                        break;
                    if (GameManager.instance.itemSlot[i].Equals(""))
                    {
                        emptySlot = i;
                        break;
                    }
                }
            }
            else
            {
                if (GameManager.instance.itemSlot[0].Equals(""))
                    emptySlot = 0;
            }

            //빈슬롯없음
            if(emptySlot == -1)
            {
                int getSlot = 0;
                if (ItemManager.CheckPassiveItem(itemName))
                    getSlot = GameManager.instance.selectNum;

                ////////////////////////////////////////////////////////////////////////////////////////
                //getSlot에 아이템을 넣어줌
                string tempItem = GameManager.instance.itemSlot[getSlot];
                int tempItemCount = GameManager.instance.itemNum[getSlot];
                float tempItemCool = GameManager.instance.itemCool[getSlot];

                ItemManager.instance.SpawnItem(Player.instance.transform.position, tempItem, tempItemCool, tempItemCount);

                GameManager.instance.itemSlot[getSlot] = itemName;
                GameManager.instance.itemNum[getSlot] = itemCount;
                GameManager.instance.itemCool[getSlot] = 10000;

            }
            //빈슬롯존재
            else
            {
                int getSlot = emptySlot;
                GameManager.instance.selectNum = emptySlot;
                ////////////////////////////////////////////////////////////////////////////////////////
                //emptySlot에 아이템을 채워줌
                GameManager.instance.itemSlot[getSlot] = itemName;
                GameManager.instance.itemNum[getSlot] = itemCount;
                GameManager.instance.itemCool[getSlot] = 10000;
            }
            UIManager.instance.MoveItem();
        }

        SoundManager.instance.ItemGet();
        EffectManager.instance.GetItem(Player.instance.transform.position, false, itemName);
    }
    #endregion

    #region[아이템가격]
    int GetItemValue(string s)
    {
        int itemIndex = ItemManager.FindData(s);
        if (itemIndex == -1)
            return 0;
        int value = ItemManager.instance.itemData[ItemManager.FindData(s)].cost;
        int minValue = 100;
        minValue = Mathf.Min(minValue,(ItemManager.instance.HasItemCheck("ShopVIpNormal") ? (100 - Mathf.FloorToInt(ItemManager.instance.itemData[ItemManager.FindData("ShopVIpNormal")].value0)) : 100));
        minValue = Mathf.Min(minValue, (shopVIP ? (100 - Mathf.FloorToInt(ItemManager.instance.itemData[ItemManager.FindData("ShopVIpSpecial")].value0)) : 100));
        minValue = Mathf.Min(minValue,(ItemManager.instance.HasItemCheck("SaleCoupon") ? (100 - Mathf.FloorToInt(ItemManager.instance.itemData[ItemManager.FindData("SaleCoupon")].value0)) : 100));
        minValue = Mathf.FloorToInt((float)minValue * ((float)randomDice / 100f));
        return Mathf.FloorToInt(value * (minValue / 100f));
    }
    #endregion
}
