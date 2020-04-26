using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Custom;
public class BoomScript : MonoBehaviour
{

    public int range = 4;
    public string boomname;

    public int damage;
    Animator animator;

    #region[Awake]
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    #endregion

    #region[Update]
    void Update()
    {
        if(ItemManager.FindData(boomname) != -1)
            damage = Mathf.FloorToInt(ItemManager.instance.itemData[ItemManager.FindData(boomname)].value0);
        Vector2Int nowPos = new Vector2Int(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y));
        if (animator)
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            {
                EffectManager.instance.Vibration(EffectManager.instance.boomExplosionVibration.num, EffectManager.instance.boomExplosionVibration.power);

                for(int y = nowPos.y - range; y < nowPos.y + range; y++)
                {
                    for (int x = nowPos.x - range; x < nowPos.x + range; x++)
                    {
                        if (Exception.IndexOutRange(x, y, StageData.instance.groundData))
                            if (StageData.instance.groundData[x, y] != (StageData.GroundLayer)(-1))
                                if (GroundManager.instance.groundHp[x, y] > 0 && Vector2.Distance(nowPos, new Vector2(x, y)) < range)
                                    DamageJudgMent.AttackTerrain(new Vector2Int(x, y), damage);


                    }
                }

                RaycastHit2D[] monsters = Physics2D.CircleCastAll(nowPos, range, Vector2.zero, 0, 1 << LayerMask.NameToLayer("Body"));
                for (int i = 0; i < monsters.Length; i++)
                {
                    if (monsters[i].transform.tag.Equals("Monster"))
                    {
                        Monster monster = monsters[i].transform.GetComponent<Monster>();
                        if (monster)
                            monster.Damage(damage);
                    }
                }

                RaycastHit2D[] objects = Physics2D.CircleCastAll(nowPos, range, Vector2.zero, 0, 1 << LayerMask.NameToLayer("Body"));
                for (int i = 0; i < objects.Length; i++)
                {
                    if (objects[i].transform.tag.Equals("Object"))
                    {
                        StructureObject structureObject = objects[i].transform.GetComponent<StructureObject>();
                        if (structureObject)
                            structureObject.BreakObject(damage);
                    }
                }


                if (Vector2.Distance(Player.instance.transform.position, nowPos) < range)
                    Player.instance.PlayerDamage(1);


                SoundManager.instance.Explosion();
                EffectManager.instance.Explosion(transform.position);
                gameObject.SetActive(false);
            }
    }
    #endregion

    #region[OnEnable]
    private void OnEnable()
    {
        SoundManager.instance.Ignite();
    }  
    #endregion
}
