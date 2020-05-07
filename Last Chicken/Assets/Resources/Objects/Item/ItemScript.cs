﻿using System.Collections;
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
                    else if (ItemManager.CheckGetActiveItem(transform.name))
                    {
                        Player.instance.ActItem(transform.name);
                        transform.name = "";
                        num = 0;
                    }
                    else
                    {
                        //빈슬롯검사
                        int emptySlot = -1;
                        if (ItemManager.CheckPassiveItem(transform.name))
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
                            if (ItemManager.CheckPassiveItem(transform.name))
                                getSlot = GameManager.instance.selectNum;

                            ////////////////////////////////////////////////////////////////////////////////////////
                            //getSlot에 아이템을 넣어줌
                            string tempItem = GameManager.instance.itemSlot[getSlot];
                            int tempItemCount = GameManager.instance.itemNum[getSlot];
                            float tempItemCool = GameManager.instance.itemCool[getSlot];

                            GameManager.instance.itemSlot[getSlot] = transform.name;
                            GameManager.instance.itemNum[getSlot] = num;
                            GameManager.instance.itemCool[getSlot] = cool;

                            transform.name = tempItem;
                            num = tempItemCount;
                            cool = tempItemCool;
                        }
                        //빈슬롯존재
                        else
                        {
                            int getSlot = emptySlot;
                            GameManager.instance.selectNum = emptySlot;
                            ////////////////////////////////////////////////////////////////////////////////////////
                            //0번슬롯에 아이템을 채워줌
                            GameManager.instance.itemSlot[getSlot] = transform.name;
                            GameManager.instance.itemNum[getSlot] = num;
                            GameManager.instance.itemCool[getSlot] = cool;

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