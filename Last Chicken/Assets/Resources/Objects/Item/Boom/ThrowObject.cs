using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowObject : CustomCollider
{
    new Rigidbody2D rigidbody2D;
    BoxCollider2D damageCollider;

    public int damage;

    float cool = 0;

    #region[Awake]
    void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        damageCollider = transform.Find("DamageCollider").GetComponent<BoxCollider2D>();
    }
    #endregion

    #region[Update]
    void Update()
    {
        if(cool >= 0.25f && Vector2.Distance(rigidbody2D.velocity,Vector2.zero) > 5)
        {
            AttackMonster(damage);
            AttackObject(damage);
            cool = 0;
        }
        cool += Time.deltaTime;
    }
    #endregion

    #region[OnEnable]
    private void OnEnable()
    {
        cool = 1000;
    }
    #endregion

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region[몬스터 공격]
    public void AttackMonster(int damage)
    {
        RaycastHit2D[] monsters =
            Physics2D.BoxCastAll
            (
                (Vector2)transform.position + GetAngleOffset(damageCollider),
                new Vector2(damageCollider.size.x * Mathf.Abs(transform.localScale.x), damageCollider.size.y * Mathf.Abs(transform.localScale.y)),
                damageCollider.transform.eulerAngles.z * (transform.localScale.x < 0 ? -1 : 1),
                Vector2.zero, 1,
                1 << LayerMask.NameToLayer("Body")
            );

        for (int i = 0; i < monsters.Length; i++)
        {
            if (monsters[i].transform.tag.Equals("Monster"))
            {
                Monster monster = monsters[i].transform.GetComponent<Monster>();
                if (monster)
                {
                    monster.Damage(damage);
                    bool dicX = monster.transform.position.x < transform.position.x ? true : false;
                    if (monster.hp > 0)
                        EffectManager.instance.DamageBlood(monster.transform.position, dicX, Random.Range(0, 2));
                    else
                    {
                        if (monster.grounded)
                            EffectManager.instance.DieBlood(monster.transform.position, dicX, 0);
                        else
                            EffectManager.instance.DieBlood(monster.transform.position, dicX, 1);
                    }

                    EffectManager.instance.Attack(monster.transform.position, dicX, Random.Range(0, 3));
                }

            }
        }          
    }
    #endregion

    #region[오브젝트 공격]
    public void AttackObject(int damage)
    {
        RaycastHit2D[] objects =
            Physics2D.BoxCastAll
            (
                (Vector2)transform.position + GetAngleOffset(damageCollider),
                new Vector2(damageCollider.size.x * Mathf.Abs(transform.localScale.x), damageCollider.size.y * Mathf.Abs(transform.localScale.y)),
                damageCollider.transform.eulerAngles.z * (transform.localScale.x < 0 ? -1 : 1),
                Vector2.zero, 1,
                1 << LayerMask.NameToLayer("Body")
            );

        for (int i = 0; i < objects.Length; i++)
        {
            if (objects[i].transform.tag.Equals("Object"))
            {
                StructureObject structureObject = objects[i].transform.GetComponent<StructureObject>();
                if (structureObject)
                {
                    structureObject.BreakObject(damage);
                    if (structureObject.objectType == StructureObject.ObjectType.부술수있음)
                    {
                        bool dicX = structureObject.transform.position.x < transform.position.x ? true : false;
                        EffectManager.instance.Attack(structureObject.transform.position, dicX, Random.Range(0, 3));
                    }
                }
            }
        }
    }
    #endregion
}
