﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureObject : CustomCollider 
{
    public enum ObjectType { 부술수있음 , 부술수없음};
    [HideInInspector] public ObjectType objectType;
    public enum SpecialType { 없음, 깔리면데미지, 닿으면데미지, 아이템드랍, 치이면몬스터죽음, 움직임 };
    protected SpecialType specialType;

    public enum DamageSound { 없음, 나무, 돌 , 철 ,생물};
    protected DamageSound damageSound;

    protected GameObject body;
    protected GameObject piece;
    protected BoxCollider2D bodyCollider;
    protected new Rigidbody2D rigidbody2D;

    protected float damageTime; //데미지
    protected int maxHp;
    protected int nowHp;
    protected float time = 0;
    float cool = 0;
    int moveDicX = 0;

    bool updateFlag = false;

    [HideInInspector] public string inItem = "Random";

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region[Awake]
    public virtual void Awake()
    {
        UpdateStats();
        nowHp = maxHp;
        body = transform.Find("Body").gameObject;
        bodyCollider = body.transform.Find("Parts").Find("Body").GetComponent<BoxCollider2D>();
        piece = transform.Find("Piece").gameObject;
        rigidbody2D = GetComponent<Rigidbody2D>();
    }
    #endregion

    #region[Start]
    public virtual void Start()
    {
        UpdateStats();
        nowHp = maxHp;
    }
    #endregion

    #region[Update]
    public virtual void Update()
    {
        if (updateFlag)
            UpdateStats();
        ObjectActive();
    }
    #endregion

    #region[OnEnable]
    public virtual void OnEnable()
    {
        nowHp = maxHp;
        time = 0;
        cool = 1000;
        updateFlag = false;
        body.SetActive(true);
        piece.SetActive(false);
    }
    #endregion

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region[활성화설정]
    public virtual void ObjectActive()
    {
        if (!body.activeSelf)
        {
            time += Time.deltaTime;

            if (time > 2)
                gameObject.SetActive(false);
        }
        else
            SpecialEvent();

        damageTime -= Time.deltaTime;
    }
    #endregion

    #region[능력치 갱신]
    public virtual void UpdateStats()
    {
        if (ObjectManager.FindData(transform.name) == -1 || !ObjectManager.instance)
            return;

        updateFlag = true;
        maxHp = ObjectManager.instance.obejctData[ObjectManager.FindData(transform.name)].Hp;
        objectType = ObjectManager.instance.obejctData[ObjectManager.FindData(transform.name)].ObjectType;
        specialType = ObjectManager.instance.obejctData[ObjectManager.FindData(transform.name)].SpecialType;
        damageSound = ObjectManager.instance.obejctData[ObjectManager.FindData(transform.name)].DamageSound;
    }
    #endregion

    #region[오브젝트 이벤트]
    public virtual void SpecialEvent()
    {
        if (specialType == SpecialType.깔리면데미지 && rigidbody2D.velocity.y < -10)
            DownDamage();
        else if (specialType == SpecialType.닿으면데미지)
            EnterDamage();
        else if (specialType == SpecialType.치이면몬스터죽음)
            HitMonsterDamage();
        else if (specialType == SpecialType.움직임)
            ObjectMove();
    }

    public virtual void DownDamage()
    {
        Vector2 digPos = (Vector2)transform.position + new Vector2(bodyCollider.offset.x * transform.localScale.x, bodyCollider.offset.y * transform.localScale.y - 1f);
        Vector2 newSize = new Vector2(Mathf.Abs(bodyCollider.size.x * transform.localScale.x) * 0.9f, Mathf.Abs(bodyCollider.size.y * transform.localScale.y));
        RaycastHit2D[] targets = Physics2D.BoxCastAll(digPos, newSize, 0, Vector2.zero, 0, 1 << LayerMask.NameToLayer("Body"));
        for (int i = 0; i < targets.Length; i++)
        {
            if (targets[i].transform.tag.Equals("Monster"))
            {
                Monster monster = targets[i].transform.GetComponent<Monster>();
                if (monster)
                    monster.Damage(1);
            }
            else if (targets[i].transform.tag.Equals("Player"))
                Player.instance.PlayerDamage(0.5f);
        }
    }

    public virtual void EnterDamage()
    {
        Vector2 digPos = (Vector2)transform.position + new Vector2(bodyCollider.offset.x * transform.localScale.x, bodyCollider.offset.y * transform.localScale.y);
        Vector2 newSize = new Vector2(Mathf.Abs(bodyCollider.size.x * transform.localScale.x) * 0.9f, Mathf.Abs(bodyCollider.size.y * transform.localScale.y) * 0.9f);
        RaycastHit2D[] targets = Physics2D.BoxCastAll(digPos, newSize, 0, Vector2.zero, 0, 1 << LayerMask.NameToLayer("Body"));
        for (int i = 0; i < targets.Length; i++)
            if (targets[i].transform.tag.Equals("Player"))
                Player.instance.PlayerDamage(0.5f);
    }

    public virtual void HitMonsterDamage()
    {
        Vector2 digPos;
        Vector2 newSize;
        RaycastHit2D[] targets;
        if (Vector2.Distance(rigidbody2D.velocity, Vector2.zero) > 3)
        {
            digPos = (Vector2)transform.position + new Vector2(bodyCollider.offset.x * transform.localScale.x, bodyCollider.offset.y * transform.localScale.y - 1f);
            newSize = new Vector2(Mathf.Abs(bodyCollider.size.x * transform.localScale.x) * 0.9f, Mathf.Abs(bodyCollider.size.y * transform.localScale.y));
            targets = Physics2D.BoxCastAll(digPos, newSize, 0, Vector2.zero, 0, 1 << LayerMask.NameToLayer("Body"));
            for (int i = 0; i < targets.Length; i++)
            {
                if (targets[i].transform.tag.Equals("Monster"))
                {
                    Monster monster = targets[i].transform.GetComponent<Monster>();
                    if (monster)
                        monster.Damage(10);
                }
            }
            if (cool >= 0.25f)
            {
                digPos = (Vector2)transform.position + new Vector2(bodyCollider.offset.x * transform.localScale.x, bodyCollider.offset.y * transform.localScale.y - 1f);
                newSize = new Vector2(Mathf.Abs(bodyCollider.size.x * transform.localScale.x) * 0.9f, Mathf.Abs(bodyCollider.size.y * transform.localScale.y));
                targets = Physics2D.BoxCastAll(digPos, newSize, 0, Vector2.zero, 0, 1 << LayerMask.NameToLayer("Body"));
                for (int i = 0; i < targets.Length; i++)
                {
                    if (targets[i].transform.name == "MineCart")
                        continue;
                    if (targets[i].transform.tag.Equals("Object"))
                    {
                        StructureObject structureObject = targets[i].transform.GetComponent<StructureObject>();
                        if (structureObject)
                        {
                            structureObject.BreakObject(30);
                            if (structureObject.objectType == StructureObject.ObjectType.부술수있음)
                            {
                                bool dicX = structureObject.transform.position.x < transform.position.x ? true : false;
                                EffectManager.instance.Attack(structureObject.transform.position, dicX, Random.Range(0, 3));
                            }
                        }
                    }
                }
                cool = 0;
            }
        }
        cool += Time.deltaTime;
    }

    public virtual void ObjectMove()
    {
        if (cool > 3)
        {
            moveDicX = Random.Range(0, 100) < 50 ? 1 : -1;
            cool = 0;
        }
        bool flag = true;
        for (int i = 1; i <= 3; i++)
            if (StageData.instance.GetBlock(new Vector2Int((int)transform.position.x, (int)transform.position.y) + new Vector2Int((int)Mathf.Sign(moveDicX), -i)) != (StageData.GroundLayer)(-1))
                flag = false;
        if (flag)
            cool = 100;

        rigidbody2D.velocity = new Vector2(moveDicX, rigidbody2D.velocity.y);

        cool += Time.deltaTime;
    }
    #endregion

    #region[오브젝트 피격처리]
    public virtual void BreakObject(int n)
    {
        if (damageTime > 0)
            return;
        if (objectType == ObjectType.부술수있음)
            ObjectBreak(n);
        else if (objectType == ObjectType.부술수없음)
        {
            SoundManager.instance.AttackIron();
            Vector3 knockback = new Vector3(300, 0, 0);
            if (Mathf.Abs(rigidbody2D.velocity.x) > 1.5f)
                knockback *= 4;
            float dicX = transform.position.x < Player.instance.transform.position.x ? -1 : +1;
            rigidbody2D.AddForce(new Vector2(dicX * knockback.x, knockback.y), ForceMode2D.Force);
        }

        if(damageSound == DamageSound.나무)
            SoundManager.instance.WoodObjectAttack();
        else if (damageSound == DamageSound.돌)
            SoundManager.instance.AttackStone();
        else if (damageSound == DamageSound.철)
            SoundManager.instance.AttackIron();
        else if (damageSound == DamageSound.생물)
            SoundManager.instance.MonsterDamage();
    }

    public virtual void ObjectBreak(int n)
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
        }
        else
        {
            StartCoroutine(Vibration(10));
        }
    }

    public IEnumerator Vibration(int n,float power = 0.05f)
    {
        Vector3 basePos = transform.position;
        for(int i = 0; i < n; i++)
        {
            Vector3 knockback = new Vector3(power, power, 0);
            Quaternion v3Rotation = Quaternion.Euler(0f, 0f, Random.Range(0, 360f));
            knockback = v3Rotation * knockback;
            transform.position += knockback;
            yield return new WaitForSeconds(0.01f);
            transform.position = basePos;
        }
    }
    #endregion
}
