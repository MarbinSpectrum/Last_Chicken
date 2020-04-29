using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScript : CustomCollider
{
    protected new Rigidbody2D rigidbody2D;
    protected BoxCollider2D bodyCollider;
    protected SpriteRenderer spriteRenderer;

    //[HideInInspector]
    public int num;
    public float cool;

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region[Awake]
    public virtual void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        bodyCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = transform.Find("SpriteImg").GetComponent<SpriteRenderer>();
    }
    #endregion

    #region[Start]
    public virtual void Start()
    {

    }
    #endregion

    #region[Update]
    public virtual void Update()
    {
        GetItem();
        UpdateStats();
    }
    #endregion

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    #region[능력치 갱신]
    public virtual void UpdateStats()
    {
        if(ItemManager.FindData(transform.name) == -1)
            spriteRenderer.color = new Color(1, 1, 1, 0);
        else
            spriteRenderer.color = new Color(1, 1, 1, 1);

        if (ItemManager.FindData(transform.name) != -1)
            spriteRenderer.sprite = ItemManager.instance.itemData[ItemManager.FindData(transform.name)].itemImg;

        cool += Time.deltaTime;
    }
    #endregion

    #region[아이템 획득]
    public virtual void GetItem()
    {
        if (!Player.instance)
            return;

        if (GameManager.getItemDelay < 0 && IsAtPlayer(bodyCollider))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (ItemManager.FindData(transform.name) != -1)
                {
                    string getItem = transform.name;
                    if (ItemManager.instance.AddItemCheck(transform.name))
                    {
                        ItemManager.instance.AddItem(transform.name, num);
                        transform.name = "";
                        num = 0;
                    }
                    else
                    {
                        int emptySlot = -1;
                        for (int i = 0; i < 6; i++)
                        {
                            if (!GameManager.instance.slotAct[i])
                                break;
                            if (GameManager.instance.itemSlot[i].Equals(""))
                            {
                                emptySlot = i;
                                break;
                            }
                        }

                        //빈슬롯없음
                        if (emptySlot == -1)
                        {
                            ////////////////////////////////////////////////////////////////////////////////////////
                            //0번 슬롯에 아이템을 넣어줌
                            string tempItem = GameManager.instance.itemSlot[0];
                            int tempItemCount = GameManager.instance.itemNum[0];
                            float tempItemCool = GameManager.instance.itemCool[0];

                            GameManager.instance.itemSlot[0] = transform.name;
                            GameManager.instance.itemNum[0] = num;
                            GameManager.instance.itemCool[0] = cool;

                            transform.name = tempItem;
                            num = tempItemCount;
                            cool = tempItemCool;
                        }
                        //빈슬롯존재
                        else
                        {
                            ////////////////////////////////////////////////////////////////////////////////////////
                            //활성화슬롯갯수를 파악
                            int actSlotNum = 0;
                            for (int i = 0; i < 6; i++)
                                if (GameManager.instance.slotAct[i])
                                    actSlotNum++;
                            ////////////////////////////////////////////////////////////////////////////////////////
                            //빈슬롯이 0번이 될때까지 회전
                            if (actSlotNum >= 1)
                            {
                                for (int i = 0; i < 6; i++)
                                {
                                    if (GameManager.instance.itemSlot[0].Equals(""))
                                        break;
                                    string tempItem = GameManager.instance.itemSlot[0];
                                    int tempItemCount = GameManager.instance.itemNum[0];
                                    float tempItemCool = GameManager.instance.itemCool[0];

                                    for (int j = 0; j < actSlotNum - 1; j++)
                                    {
                                        GameManager.instance.itemSlot[j] = GameManager.instance.itemSlot[j + 1];
                                        GameManager.instance.itemNum[j] = GameManager.instance.itemNum[j + 1];
                                        GameManager.instance.itemCool[j] = GameManager.instance.itemCool[j + 1];
                                    }
                                    GameManager.instance.itemSlot[actSlotNum - 1] = tempItem;
                                    GameManager.instance.itemNum[actSlotNum - 1] = tempItemCount;
                                    GameManager.instance.itemCool[actSlotNum - 1] = tempItemCool;
                                }
                            }
                            ////////////////////////////////////////////////////////////////////////////////////////
                            //0번슬롯에 아이템을 채워줌
                            GameManager.instance.itemSlot[0] = transform.name;
                            GameManager.instance.itemNum[0] = num;
                            GameManager.instance.itemCool[0] = cool;

                            transform.name = "";
                            num = 0;
                        }
                    }

                    EffectManager.instance.NowItem(spriteRenderer.sprite);
                    GameManager.getItemDelay = 0.1f;
                    EffectManager.instance.GetItem(transform.position, false, getItem);
                    SoundManager.instance.ItemGet();
                    UIManager.instance.MoveItem();
                }
            }
        }
    }
    #endregion
}