using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodBoxScript : StructureObject
{
    public static int objectNum = 0;

    public Sprite noneBox_Spr;
    public Sprite mineBox_Spr;
    public Sprite itemBox_Spr;

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
        base.Update();
    }
    #endregion

    #region[OnEnable]
    public override void OnEnable()
    {
        base.OnEnable();
        objectNum++;
    }
    #endregion

    #region[InitWoodBox]
    public void InitWoodBox(string item)
    {
        if (item.Equals("Random"))
        {
            Random.InitState(Random.Range(0, 100 * objectNum));
            int itemNum = ItemManager.instance.GetRandomItemAtWoodBox();
            if (itemNum == -1)
            {
                int r = Random.Range(0, 100);
                if (r > 50)
                    inItem = "Mine";
                else
                    inItem = "None";
            }
            else
                inItem = ItemManager.itemName[itemNum];
        }
        else
            inItem = item;

        if(inItem.Equals("Mine"))
            spriteRenderer.sprite = mineBox_Spr;
        else if (inItem.Equals("None"))
            spriteRenderer.sprite = noneBox_Spr;
        else
            spriteRenderer.sprite = itemBox_Spr;

        if (inItem.Equals("Mine"))
            spriteRenderer.GetComponent<SpriteOutline>().outlineSize = 0;
        else
            spriteRenderer.GetComponent<SpriteOutline>().outlineSize = 1;
    }
    #endregion

    #region[ObjectBreak]
    public override void ObjectBreak(int n)
    {
        damageTime = 0.1f;
        nowHp -= n;
        rigidbody2D.velocity = Vector2.zero;
        rigidbody2D.AddForce(new Vector2(0, 25), ForceMode2D.Impulse);
        if (nowHp <= 0)
        {
            if (specialType == SpecialType.아이템드랍)
            {
                if (inItem.Equals("Mine"))
                    ItemManager.instance.SpawnMine(transform.position);
                else if (!inItem.Equals("None"))
                    ItemManager.instance.SpawnItem(transform.position, inItem);
            }

            body.SetActive(false);
            piece.SetActive(true);
            StartCoroutine(ObjectUnAct(2));
        }
        else
        {
            StartCoroutine(Vibration(10));
        }
    }
    #endregion
}
