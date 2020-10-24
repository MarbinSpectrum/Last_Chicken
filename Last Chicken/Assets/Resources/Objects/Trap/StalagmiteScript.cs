using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StalagmiteScript : TrapScript
{
    public enum Type { 낙석석순, 바닥석순 }
    [Header("석순종류")]
    public Type stalagmiteType;

    bool flag = false;
    float fallTime = 0;

    BoxCollider2D bottomCollider;
    BoxCollider2D falllCollider;

    #region[Awake]
    public override void Awake()
    {
        base.Awake();
        bottomCollider = body.transform.Find("Bottom").GetComponent<BoxCollider2D>();
        if (stalagmiteType == Type.낙석석순)
            falllCollider = body.transform.Find("Fall").GetComponent<BoxCollider2D>();
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
        StalagmiteAct();
    }
    #endregion

    #region[OnEnable]
    public override void OnEnable()
    {
        base.OnEnable();
        rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
        flag = false;
        fallTime = 0;
    }
    #endregion

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region[석순종류에 따른 액션]
    public void StalagmiteAct()
    {
        if (flag)
            fallTime += Time.deltaTime;

        if (!flag && !IsAtTerrain(bottomCollider))
        {
            ObjectBreak(nowHp);
            flag = true;
            return;
        }

        if (!flag && stalagmiteType == Type.낙석석순)
        {
            if(IsAtPlayer(falllCollider))
            {
                rigidbody2D.velocity = Vector2.zero;
                rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
                flag = true;
            }
        }
    }
    #endregion

    #region[닿았을때 데미지]
    public override void EnterDamage()
    {
        if(stalagmiteType == Type.낙석석순 && ItemManager.instance.HasItemCheck("Mine_Helmet"))
            return;
        Vector2 digPos = (Vector2)transform.position + new Vector2(bodyCollider.offset.x * transform.localScale.x, bodyCollider.offset.y * transform.localScale.y);
        Vector2 newSize = new Vector2(Mathf.Abs(bodyCollider.size.x * transform.localScale.x) * 0.9f, Mathf.Abs(bodyCollider.size.y * transform.localScale.y) * 0.9f);
        RaycastHit2D[] targets = Physics2D.BoxCastAll(digPos, newSize, 0, Vector2.zero, 0, 1 << LayerMask.NameToLayer("Body"));
        for (int i = 0; i < targets.Length; i++)
            if (targets[i].transform.tag.Equals("Player"))
                Player.instance.PlayerDamage(damage);
    }
    #endregion

    #region[낙하 데미지]
    public override void DownDamage()
    {
        if (rigidbody2D.velocity.y > -0.1f || fallTime > 1)
            return;
        Vector2 digPos = (Vector2)transform.position + new Vector2(bodyCollider.offset.x * transform.localScale.x, bodyCollider.offset.y * transform.localScale.y - 1f);
        Vector2 newSize = new Vector2(Mathf.Abs(bodyCollider.size.x * transform.localScale.x) * 0.9f, Mathf.Abs(bodyCollider.size.y * transform.localScale.y));
        RaycastHit2D[] targets = Physics2D.BoxCastAll(digPos, newSize, 0, Vector2.zero, 0, 1 << LayerMask.NameToLayer("Body"));
        for (int i = 0; i < targets.Length; i++)
            if (targets[i].transform.tag.Equals("Player"))
            {
                ObjectBreak(nowHp);
                if (!ItemManager.instance.HasItemCheck("Mine_Helmet"))
                    Player.instance.PlayerDamage(damage);
            }
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
                if (inItem.Equals("Random"))
                    ItemManager.instance.SpawnItemRandomAtObject(transform.position);
                else
                    ItemManager.instance.SpawnItem(transform.position, inItem);
            }

            body.SetActive(false);
            piece.SetActive(true);
            StartCoroutine(ObjectUnAct(2));
        }
        else
        {
            StartCoroutine(Vibration(10,0.15f));
        }
    }
    #endregion
}
