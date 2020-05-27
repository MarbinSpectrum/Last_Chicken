using UnityEngine;
using Custom;
using System.Collections.Generic;

public class MovingShop : Monster
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public static MovingShop instance;

    float isPatrolTime = 3;
    MoveDic patrolDic = 0;
    new enum Dic { 경사아래로 = -1, 앞으로 = 0, 경사위로 = 1 };
    public enum MoveDic { 오른쪽, 왼쪽, 정지 };
    public bool enermy = false;
    public GameObject uiMouse;
    protected BoxCollider2D headCollider2D;

    public bool onArea;
    public bool thisUse;
    [System.NonSerialized] public List<int> itmeList = new List<int>();
    [System.NonSerialized] public List<bool> itmeBuyList = new List<bool>();
    [System.NonSerialized] public bool shopVIP;
    [System.NonSerialized] public int randomDice;

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region[Awake]
    public override void Awake()
    {
        UpdateStats();
        base.Awake();
        instance = this;
        Transform checkList = transform.Find("CheckList");
        headCollider2D = checkList.Find("Head").GetComponent<BoxCollider2D>();
        uiMouse = transform.Find("UIMouse").gameObject;
    }
    #endregion

    #region[Update]
    public override void Update()
    {
        base.Update();
        Ani();
        UpdateShopData();
        if (!damage)
        {
            Attack();
            Move();
            if (!enermy)
                UseArea();
        }
        enermy = !(hp == maxHp);
    }
    #endregion

    #region[OnEnable]
    public override void OnEnable()
    {
        base.OnEnable();
        SetShopItem();
        thisUse = false;
    }
    #endregion

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region[애니메이션]
    public void Ani()
    {
        animator.SetBool("Damage", damage);
        animator.SetBool("Move", moveFlag);
        if (damage || moveDic == 0)
            return;
        transform.localScale = new Vector3(-moveDic * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

    }
    #endregion

    #region[상점정보 업데이트]
    void UpdateShopData()
    {
        shopVIP = GameManager.instance.playData.shopVIP;
        randomDice = GameManager.instance.playData.randomDice;

        for (int i = 0; i < 3; i++)
        {
            UIManager.instance.movingShopItemCost[i].text = GetItemValue(ItemManager.itemName[itmeList[i]]) + "$";
            UIManager.instance.movingShopItemName[i].text = ItemManager.instance.itemData[itmeList[i]].itemName;
            UIManager.instance.movingShopItemImg[i].sprite = ItemManager.instance.itemData[itmeList[i]].itemImg;
            if (itmeBuyList[i])
            {
                UIManager.instance.movingShopItemImg[i].color = Color.black;
                for (int k = 0; k < 4; k++)
                    UIManager.instance.movingShopItemExplan[i, k].text = "";
                UIManager.instance.movingShopItemExplan[i, 1].text = "[매진]";
            }
            else
            {
                for (int k = 0; k < 4; k++)
                    UIManager.instance.movingShopItemExplan[i, k].text = ItemManager.instance.itemData[itmeList[i]].shopItemExplain[k];
                UIManager.instance.movingShopItemImg[i].color = Color.white;
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

        if (ItemManager.instance.HasItemCheck("SaleCoupon"))
            ItemManager.instance.UseItem("SaleCoupon");

        itmeBuyList[itemNum] = true;

        string itemName = ItemManager.itemName[itmeList[itemNum]];

        int itemCount = 0;

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
            if (emptySlot == -1)
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

    #region[가격조회]
    int GetItemValue(string s)
    {
        int itemIndex = ItemManager.FindData(s);
        if (itemIndex == -1)
            return 0;
        int value = ItemManager.instance.itemData[ItemManager.FindData(s)].cost;
        int minValue = 100;
        minValue = Mathf.Min(minValue, (ItemManager.instance.HasItemCheck("ShopVIpNormal") ? (100 - Mathf.FloorToInt(ItemManager.instance.itemData[ItemManager.FindData("ShopVIpNormal")].value0)) : 100));
        minValue = Mathf.Min(minValue, (shopVIP ? (100 - Mathf.FloorToInt(ItemManager.instance.itemData[ItemManager.FindData("ShopVIpSpecial")].value0)) : 100));
        minValue = Mathf.Min(minValue, (ItemManager.instance.HasItemCheck("SaleCoupon") ? (100 - Mathf.FloorToInt(ItemManager.instance.itemData[ItemManager.FindData("SaleCoupon")].value0)) : 100));
        minValue = Mathf.FloorToInt((float)minValue * ((float)randomDice / 100f));
        return Mathf.FloorToInt(value * (minValue / 100f));
    }
    #endregion

    #region[UseArea]
    //플레이어가 오브젝트를 사용할 수 있는지 검사
    public void UseArea()
    {
        onArea = IsAtPlayer(boxCollider2D);
        if (GameManager.instance.gamePause)
            return;
        if (IsAtPlayer(boxCollider2D))
        {
            if (Input.GetMouseButtonDown(1))
            {
                Player.instance.canControl = thisUse;
                thisUse = !thisUse;
            }
            uiMouse.SetActive(!thisUse);
        }
        else
            uiMouse.SetActive(false);
    }
    #endregion

    #region[공격처리]
    public override void Attack()
    {
        if (!enermy)
            return;

        if (Player.instance && Player.instance.damage)
            return;

        bool check = IsAtPlayer(boxCollider2D);
        if (check)
        {
            int dic = transform.position.x < Player.instance.transform.position.x ? +1 : -1;
            Player.instance.PlayerDamage(attackPower, dic);
        }
    }
    #endregion

    #region[이동처리]
    public void Move()
    {
        float newSpeed = speed * (enermy ? 3 : 1);
        if (onArea && !enermy)
            newSpeed = 0;
        if (Vector2.Distance(nowPos, targetPos) < range &&
            Exception.IndexOutRange(nowPos, GroundManager.instance.linkArea) &&
            GroundManager.instance.linkArea[nowPos.x, nowPos.y] &&
                Mathf.Abs(nowPos.y - targetPos.y) <= 5 && enermy)
        {
            if (Mathf.Abs(nowPos.x - targetPos.x) < 0.15f)
                MovingGround(+0);
            else if (nowPos.x < targetPos.x)
                MovingGround(+newSpeed);
            else if (nowPos.x > targetPos.x)
                MovingGround(-newSpeed);
        }
        else
        {
            if (patrolTime < 0)
            {
                patrolTime = isPatrolTime;
                Random.InitState((int)Time.time * Random.Range(0, 100));
                int r = Random.Range(0, 100);
                if (r < 20)
                    patrolDic = MoveDic.정지;
                else if (r < 60)
                    patrolDic = MoveDic.오른쪽;
                else if (r < 100)
                    patrolDic = MoveDic.왼쪽;
            }
            else
            {
                patrolTime -= Time.deltaTime;
                switch (patrolDic)
                {
                    case MoveDic.오른쪽:
                        if (CanFallBlock(newSpeed, 4) && CanMove(newSpeed))
                            MovingGround(+newSpeed);
                        else
                            patrolDic = MoveDic.왼쪽;
                        break;
                    case MoveDic.왼쪽:
                        if (CanFallBlock(-newSpeed, 4) && CanMove(-newSpeed))
                            MovingGround(-newSpeed);
                        else
                            patrolDic = MoveDic.오른쪽;
                        break;
                    case MoveDic.정지:
                            MovingGround(+0);
                        break;
                }

            }
        }
    }
    #endregion

    #region[이동 오버라이드]
    public override void MovingGround(float force)
    {
        if (force == 0)
            return;

        moveFlag = false;
        //이동 거리
        float moveDistance = force;
        //순간적인 이동 거리
        Vector2 moveValue;
        Dic[] checkArray = new Dic[3] { Dic.경사아래로, Dic.앞으로, Dic.경사위로 };
        foreach (Dic move in checkArray)
        {
            //checkArr을 상용해서 이동할 값을 설정
            moveValue = new Vector2(moveDistance, (int)move * Mathf.Abs(moveDistance)) * Time.deltaTime;
            //이동한 위치에 지형이 있는지 검사
            bool nextPosCheck = IsAtTerrain(boxCollider2D, moveValue) || IsAtTerrain(headCollider2D, moveValue);

            //앞으로 이동중이 아니면 땅위인지 검사해야됨
            if (move != Dic.앞으로 ? grounded : true)
                //다음위치에 지형이 없으면
                if (!nextPosCheck)
                {
                    //앞으로 이동상태로 설정
                    moveFlag = true;
                    //앞으로 이동
                    transform.Translate(moveValue);
                    break;
                }
        }

        if (!moveFlag)
        {
            moveValue = new Vector2(moveDistance, 0) * Time.deltaTime;
            bool frontMoveCheck = IsAtTerrainObject(boxCollider2D, moveValue) || IsAtTerrainObject(headCollider2D, moveValue);
            if (frontMoveCheck)
                moveFlag = true;
        }
    }
    #endregion


}