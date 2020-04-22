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
                    string temp = transform.name;
                    int numTemp = num;

                    if (ItemManager.instance.AddItemCheck(temp))
                    {
                        ItemManager.instance.AddItem(temp, num);
                        transform.name = "";
                        num = 0;
                    }
                    else if (ItemManager.CheckActiveItem(temp))
                    {
                        if (GameManager.instance.activeItem.Equals(""))
                        {
                            transform.name = "";
                            num = 0;

                            GameManager.instance.activeItem = temp;
                            GameManager.instance.activeItemNum = numTemp;
                        }
                        else
                        {
                            transform.name = GameManager.instance.activeItem;
                            num = GameManager.instance.activeItemNum;

                            GameManager.instance.activeItem = temp;
                            GameManager.instance.activeItemNum = numTemp;
                        }
                    }
                    else
                    {
                        transform.name = GameManager.instance.passiveItem[GameManager.instance.passivePointer];
                        num = GameManager.instance.passiveItemNum[GameManager.instance.passivePointer];

                        GameManager.instance.passiveItem[GameManager.instance.passivePointer] = temp;
                        GameManager.instance.passiveItemNum[GameManager.instance.passivePointer] = numTemp;

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

                    UIManager.instance.nowItemImage.sprite = spriteRenderer.sprite;
                    GameManager.getItemDelay = 0.1f;
                    EffectManager.instance.GetItem(transform.position, false, temp);
                    SoundManager.instance.ItemGet();
                }
            }
        }
    }
    #endregion
}