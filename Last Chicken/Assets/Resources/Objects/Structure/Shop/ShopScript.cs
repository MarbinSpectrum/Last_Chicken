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
    
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 

    #region[Awake]
    public override void Awake()
    {
        bodyCollider = GetComponent<BoxCollider2D>();
        instance = this;
    }
    #endregion

    #region[Update]
    public override void Update()
    {
        onArea = IsAtPlayer(bodyCollider);
        UseArea();
        UpdateShopData();
    }
    #endregion

    #region[OnEnable]
    void OnEnable()
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
        if (!used && IsAtPlayer(bodyCollider))
        {
            if (Input.GetMouseButtonDown(1))
            {
                Player.instance.canControl = thisUse;
                thisUse = !thisUse;
            }
        }
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
            UIManager.instance.shopItemName[i].text = ItemManager.instance.itemData[itmeList[i]].itemName;
            UIManager.instance.shopItemImg[i].sprite = ItemManager.instance.itemData[itmeList[i]].itemImg;
            if (itmeBuyList[i])
            {
                UIManager.instance.shopItemImg[i].color = Color.black;
                for (int k = 0; k < 4; k++)
                    UIManager.instance.shopItemExplan[i, k].text = "";
                UIManager.instance.shopItemExplan[i, 1].text = "[매진]";
            }
            else
            {
                for(int k = 0; k < 4; k++)
                    UIManager.instance.shopItemExplan[i,k].text = ItemManager.instance.itemData[itmeList[i]].shopItemExplain[k];
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
            do { itemIndex = ItemManager.instance.GetRandomItemAtShop(); } while (itmeList.Contains(itemIndex));

            itmeList.Add(itemIndex);
            itmeBuyList.Add(false);
        }
    }
    #endregion

    #region[상점 아이템 구매]
    public void BuyItem(int itemNum)
    {
        if (itmeBuyList[itemNum])
            return;

        int cost = GetItemValue(ItemManager.itemName[itmeList[itemNum]]);

        if (GameManager.instance.playerMoney < cost)
            return;

        GameManager.instance.playerMoney -= cost;

        if(ItemManager.instance.HasItemCheck("SaleCoupon"))
            ItemManager.instance.UseItem("SaleCoupon");

        itmeBuyList[itemNum] = true;

        string itemName = ItemManager.itemName[itmeList[itemNum]];

        int itemCount = 0;
        if (itemName.Equals("BoomItem"))
            itemCount = (int)ItemManager.instance.itemData[ItemManager.FindData(itemName)].value1;
        else if (itemName.Equals("Dynamite"))
            itemCount = (int)ItemManager.instance.itemData[ItemManager.FindData(itemName)].value1;

        if (ItemManager.instance.AddItemCheck(itemName))
            ItemManager.instance.AddItem(itemName, itemCount);
        else
        {
            if (ItemManager.instance.AddItemCheck(itemName))
                ItemManager.instance.AddItem(itemName, itemCount);
            else if (ItemManager.CheckActiveItem(itemName))
            {
                if (GameManager.instance.activeItem.Equals(""))
                {
                    GameManager.instance.activeItem = itemName;
                    transform.name = "";
                }
                else
                {
                    string tempName = GameManager.instance.activeItem;
                    int tmpNum = GameManager.instance.activeItemNum;

                    GameManager.instance.activeItem = itemName;
                    GameManager.instance.activeItemNum = itemCount;

                    ItemManager.instance.SpawnItem(Player.instance.transform.position, tempName, tmpNum);
                }
            }
            else
            {
                //int emptySlot = -1;
                //for (int i = 0; i < 5; i++)
                //{
                //    if (!GameManager.instance.passiveSlotAct[i])
                //        break;
                //    if (GameManager.instance.passiveItem[i].Equals(""))
                //    {
                //        emptySlot = i;
                //        break;
                //    }
                //}

                //if (emptySlot != -1)
                //{
                //    GameManager.instance.passiveItem[emptySlot] = itemName;
                //    GameManager.instance.passiveItemNum[emptySlot] = itemCount;
                //}
                //else
                //{
                //    string tempName = GameManager.instance.passiveItem[GameManager.instance.passivePointer];
                //    int tempNum = GameManager.instance.passiveItemNum[GameManager.instance.passivePointer];

                //    GameManager.instance.passiveItem[GameManager.instance.passivePointer] = itemName;
                //    GameManager.instance.passiveItemNum[GameManager.instance.passivePointer] = itemCount;

                //    ItemManager.instance.SpawnItem(Player.instance.transform.position, itemName, itemCount);
                //}

                string tempName = GameManager.instance.passiveItem[GameManager.instance.passivePointer];
                int tempNum = GameManager.instance.passiveItemNum[GameManager.instance.passivePointer];

                GameManager.instance.passiveItem[GameManager.instance.passivePointer] = itemName;
                GameManager.instance.passiveItemNum[GameManager.instance.passivePointer] = itemCount;

                ItemManager.instance.SpawnItem(Player.instance.transform.position, tempName, tempNum);

                int emptySlot = -1;
                for (int i = 0; i < 5; i++)
                {
                    if (!GameManager.instance.passiveSlotAct[i])
                        break;
                    if (GameManager.instance.passiveItem[i].Equals(""))
                    {
                        emptySlot = i;
                        break;
                    }
                }

                if (emptySlot != -1)
                    GameManager.instance.passivePointer = emptySlot;
            }

            EffectManager.instance.GetItem(transform.position, false, itemName);
            SoundManager.instance.ItemGet();

        }
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
