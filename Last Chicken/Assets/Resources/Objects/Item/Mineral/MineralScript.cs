using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineralScript : ItemScript
{
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region[Awake]
    public override void Awake()
    {
        base.Awake();
    }
    #endregion

    #region[Start]
    public override void Start()
    {
        base.Start();
    }
    #endregion

    #region[Update]
    public override void Update()
    {
        UpdateStats();
        GetItem();
        MoveMineral();
    }
    #endregion

    #region[광물이동]
    public void MoveMineral()
    {
        if(ItemManager.instance.HasItemCheck("MineBag"))
        {
            Vector2 dic = Player.instance.transform.position - transform.position;
            dic = dic.normalized;
            dic *= 10;
            transform.position += (Vector3)(dic) * Time.deltaTime;
        }
    }

    #endregion

    #region[이미지수정]
    public override void UpdateStats()
    {
        switch (transform.name)
        {
            case "Dirt":
                spriteRenderer.sprite = GroundManager.instance.dirtMineral;
                break;
            case "Stone":
                spriteRenderer.sprite = GroundManager.instance.stoneMineral;
                break;
            case "Copper":
                spriteRenderer.sprite = GroundManager.instance.copperMineral;
                break;
            case "Granite":
                spriteRenderer.sprite = GroundManager.instance.graniteMineral;
                break;
            case "Iron":
                spriteRenderer.sprite = GroundManager.instance.ironMineral;
                break;
            case "Silver":
                spriteRenderer.sprite = GroundManager.instance.silverMineral;
                break;
            case "Gold":
                spriteRenderer.sprite = GroundManager.instance.goldMineral;
                break;
            case "Mithril":
                spriteRenderer.sprite = GroundManager.instance.mithrilMineral;
                break;
            case "Diamond":
                spriteRenderer.sprite = GroundManager.instance.diamondMineral;
                break;
            case "Magnetite":
                spriteRenderer.sprite = GroundManager.instance.magnetiteMineral;
                break;
            case "Titanium":
                spriteRenderer.sprite = GroundManager.instance.titaniumMineral;
                break;
            case "Cobalt":
                spriteRenderer.sprite = GroundManager.instance.cobaltMineral;
                break;
            default:
                spriteRenderer.sprite = null;
                break;
        }
    }
    #endregion

    #region[아이템 획득]
    public override void GetItem()
    {
        if (!Player.instance)
            return;

        if (IsAtPlayer(bodyCollider))
        {
            SoundManager.instance.ItemGet();
            EffectManager.instance.Glitter(transform.position, EffectManager.instance.getMineColor);

            switch(transform.name)
            {
                case "Dirt":
                    GameManager.instance.playerMoney += GroundManager.instance.dirtValue;
                    break;
                case "Stone":
                    GameManager.instance.playerMoney += GroundManager.instance.stoneValue;
                    break;
                case "Copper":
                    GameManager.instance.playerMoney += GroundManager.instance.copperValue;
                    break;
                case "Granite":
                    GameManager.instance.playerMoney += GroundManager.instance.graniteValue;
                    break;
                case "Iron":
                    GameManager.instance.playerMoney += GroundManager.instance.ironValue;
                    break;
                case "Silver":
                    GameManager.instance.playerMoney += GroundManager.instance.silverValue;
                    break;
                case "Gold":
                    GameManager.instance.playerMoney += GroundManager.instance.goldValue;
                    break;
                case "Mithril":
                    GameManager.instance.playerMoney += GroundManager.instance.mithrilValue;
                    break;
                case "Diamond":
                    GameManager.instance.playerMoney += GroundManager.instance.diamondValue;
                    break;
                case "Magnetite":
                    GameManager.instance.playerMoney += GroundManager.instance.magnetiteValue;
                    break;
                case "Titanium":
                    GameManager.instance.playerMoney += GroundManager.instance.titaniumValue;
                    break;
                case "Cobalt":
                    GameManager.instance.playerMoney += GroundManager.instance.cobaltValue;
                    break;
            }
            transform.position = new Vector3(-1000, -1000, transform.position.z);
            gameObject.SetActive(false);      
        }
    }
    #endregion
}
