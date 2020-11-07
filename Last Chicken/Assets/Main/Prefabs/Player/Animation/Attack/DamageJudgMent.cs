using Custom;
using UnityEngine;

public class DamageJudgMent : MonoBehaviour
{
    public const int CHECKCOUNT = 25;

    public BoxCollider2D damageJudgMent;

    string BODY = "Body";
    string MONSTER = "Monster";
    string OBJECT = "Object";

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region[Update]
    void Update()
    {
        //공격시간이 일정하게 지난후 아직 공격이 진행이안됬을때
        if (AttackingCheck.aniTime >= 0.25f && damageJudgMent.enabled)
        {
            AttackTerrain();
            AttackMonster();
            AttackObject();
            //공격끝
            gameObject.SetActive(false);
        }
    }
    #endregion

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region[지형 공격]
    public void AttackTerrain()
    {
        int[] SoundGroup = new int[17];

        #region[범위에 해당하는 광물 공격]
        //공격범위에 해당하는 광물을 공격
        Vector2 digPos = (Vector2)Player.instance.transform.position + new Vector2(damageJudgMent.offset.x * Player.instance.transform.localScale.x, damageJudgMent.offset.y * Player.instance.transform.localScale.y);
        Vector2 newSize = new Vector2(Mathf.Abs(damageJudgMent.size.x * Player.instance.transform.localScale.x), Mathf.Abs(damageJudgMent.size.y * Player.instance.transform.localScale.y));

        for (int x = (int)(digPos.x - newSize.x / 2); x <= (int)(digPos.x + newSize.x / 2); x++)
            for (int y = (int)(digPos.y - newSize.y / 2); y <= (int)(digPos.y + newSize.y / 2); y++)
                if (Exception.IndexOutRange(x, y, StageData.instance.groundData))
                    if (StageData.instance.groundData[x, y] != (StageData.GroundLayer)(-1))
                        if (GroundManager.instance.groundHp[x, y] > 0)
                            if ((GroundManager.instance.digMask & (int)Mathf.Pow(2, (int)StageData.instance.groundData[x, y])) != 0)
                            {
                                SoundGroup[(int)StageData.instance.groundData[x, y]]++;
                                //if (StageData.instance.groundData[x, y] != StageData.GroundLayer.Dirt)
                                //    EffectManager.instance.PickAxeFire(new Vector3(x, y));
                                GroundManager.instance.AttackTerrain(new Vector2Int(x, y), Player.instance.attackPower);
                            }
               
        
        #endregion

        #region[광물에 따른 소리 출력]
        for (int i = 0; i < 17; i++)
        {
            SoundGroup[i] = SoundGroup[i] > 4 ? 4 : SoundGroup[i];

            int dirtSoundType = 0;
            if ((StageData.GroundLayer)(i) == StageData.GroundLayer.Dirt)
                dirtSoundType = Random.Range(0, 5);

            for (int j = 0; j < SoundGroup[i]; j++)
                switch ((StageData.GroundLayer)(i))
                {
                    case StageData.GroundLayer.Dirt:
                        SoundManager.instance.AttackDirt(dirtSoundType); break;
                    case StageData.GroundLayer.Stone:
                        SoundManager.instance.AttackStone(); break;
                    case StageData.GroundLayer.Copper:
                        SoundManager.instance.AttackIron(); break;
                    case StageData.GroundLayer.Sand:
                        SoundManager.instance.AttackDirt(); break;
                    case StageData.GroundLayer.Granite:
                        SoundManager.instance.AttackStone(); break;
                    case StageData.GroundLayer.Iron:
                        SoundManager.instance.AttackIron(); break;
                    case StageData.GroundLayer.Silver:
                        SoundManager.instance.AttackGold(); break;
                    case StageData.GroundLayer.Gold:
                        SoundManager.instance.AttackGold(); break;
                    case StageData.GroundLayer.Mithril:
                        SoundManager.instance.AttackGold(); break;
                    case StageData.GroundLayer.Diamond:
                        SoundManager.instance.AttackGold(); break;
                    case StageData.GroundLayer.Magnetite:
                        SoundManager.instance.AttackDiamond(); break;
                    case StageData.GroundLayer.Titanium:
                        SoundManager.instance.AttackMithril(); break;
                    case StageData.GroundLayer.Cobalt:
                        SoundManager.instance.AttackGold(); break;
                    case StageData.GroundLayer.Ice:
                        SoundManager.instance.AttackIce(); break;
                    case StageData.GroundLayer.Grass:
                        SoundManager.instance.AttackDirt(); break;
                    case StageData.GroundLayer.HearthStone:
                        SoundManager.instance.AttackIron(); break;
                }

        }
        #endregion

    }
    #endregion

    #region[몬스터 공격]
    RaycastHit2D[] AttackMonsterArray = new RaycastHit2D[CHECKCOUNT];
    public void AttackMonster()
    {
        //공격범위에 해당하는 적을을 공격
        Vector2 digPos = (Vector2)Player.instance.transform.position + new Vector2(damageJudgMent.offset.x * Player.instance.transform.localScale.x, damageJudgMent.offset.y * Player.instance.transform.localScale.y);
        Vector2 newSize = new Vector2(Mathf.Abs(damageJudgMent.size.x * Player.instance.transform.localScale.x), Mathf.Abs(damageJudgMent.size.y * Player.instance.transform.localScale.y));
        int count = Physics2D.BoxCastNonAlloc(digPos, newSize, 0, Vector2.zero, AttackMonsterArray, 0, 1 << LayerMask.NameToLayer(BODY));

        for (int i = 0; i < count; i++)
        {
            if (AttackMonsterArray[i].transform.tag.Equals(MONSTER))
            {
                Monster monster = MonsterManager.instance.GetMonster(AttackMonsterArray[i].transform.gameObject);

                if (monster)
                {
                    monster.Damage(Player.instance.attackPower);
                    bool dicX = monster.transform.position.x < Player.instance.transform.position.x ? true : false;
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
    RaycastHit2D[] AttackObjectArray = new RaycastHit2D[CHECKCOUNT];
    public void AttackObject()
    {
        //공격범위에 해당하는 적을을 공격
        Vector2 digPos = (Vector2)Player.instance.transform.position + new Vector2(damageJudgMent.offset.x * Player.instance.transform.localScale.x, damageJudgMent.offset.y * Player.instance.transform.localScale.y);
        Vector2 newSize = new Vector2(Mathf.Abs(damageJudgMent.size.x * Player.instance.transform.localScale.x), Mathf.Abs(damageJudgMent.size.y * Player.instance.transform.localScale.y));
        int count = Physics2D.BoxCastNonAlloc(digPos, newSize, 0, Vector2.zero, AttackObjectArray, 0, 1 << LayerMask.NameToLayer(BODY));
        for (int i = 0; i < count; i++)
        {
            if (AttackObjectArray[i].transform.tag.Equals(OBJECT))
            {
                StructureObject structureObject = AttackObjectArray[i].transform.GetComponent<StructureObject>();
                if (structureObject)
                {
                    structureObject.BreakObject(Player.instance.attackPower);
                    if(structureObject.objectType == StructureObject.ObjectType.부술수있음)
                    {
                        bool dicX = structureObject.transform.position.x < Player.instance.transform.position.x ? true : false;
                        EffectManager.instance.Attack(structureObject.transform.position, dicX, Random.Range(0, 3));
                    }
                }
            }
        }
    }
    #endregion
}