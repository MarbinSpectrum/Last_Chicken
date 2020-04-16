using Custom;
using UnityEngine;

public class DamageJudgMent : MonoBehaviour
{
    public BoxCollider2D damageJudgMent;

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region[Update]
    void Update()
    {
        //공격시간이 일정하게 지난후 아직 공격이 진핸이안됬을때
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
            {
                if (Exception.IndexOutRange(x, y, StageData.instance.groundData))
                    if (StageData.instance.groundData[x, y] != (StageData.GroundLayer)(-1))
                        if (GroundManager.instance.groundHp[x, y] > 0)
                        {
                            SoundGroup[(int)StageData.instance.groundData[x, y]]++;
                            AttackTerrain(new Vector2Int(x, y), Player.instance.attackPower);
                        }
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
                        SoundManager.instance.AttackMithril(); break;
                    case StageData.GroundLayer.Diamond:
                        SoundManager.instance.AttackDiamond(); break;
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
    public static void AttackTerrain(Vector2Int pos, int damage)
    {
        if (!Exception.IndexOutRange(pos.x, pos.y, GroundManager.instance.groundHp))
            return;

        if (StageData.instance.groundData[pos.x, pos.y] == StageData.GroundLayer.UnBreakable)
            return;

        if (StageData.instance.groundData[pos.x, pos.y] == (StageData.GroundLayer)(-1))
            return;

        if (SceneController.instance.CheckEventMap())
            return;

        #region[광물이펙트]
        if (GroundManager.instance.groundHp[pos.x, pos.y] > 0)
        {
            switch ((StageData.GroundLayer)(StageData.instance.groundData[pos.x, pos.y]))
            {
                case StageData.GroundLayer.Dirt:
                    EffectManager.instance.DigGround(new Vector3(pos.x, pos.y),GroundManager.instance.dirtColor); break;
                case StageData.GroundLayer.Stone:
                    EffectManager.instance.DigGround(new Vector3(pos.x, pos.y), GroundManager.instance.stoneColor); break;
                case StageData.GroundLayer.Copper:
                    EffectManager.instance.DigGround(new Vector3(pos.x, pos.y), GroundManager.instance.copperColor); break;
                case StageData.GroundLayer.Sand:
                    EffectManager.instance.DigGround(new Vector3(pos.x, pos.y), GroundManager.instance.sandColor); break;
                case StageData.GroundLayer.Granite:
                    EffectManager.instance.DigGround(new Vector3(pos.x, pos.y), GroundManager.instance.graniteColor); break;
                case StageData.GroundLayer.Iron:
                    EffectManager.instance.DigGround(new Vector3(pos.x, pos.y), GroundManager.instance.ironColor); break;
                case StageData.GroundLayer.Silver:
                    EffectManager.instance.DigGround(new Vector3(pos.x, pos.y), GroundManager.instance.silverColor); break;
                case StageData.GroundLayer.Gold:
                    EffectManager.instance.DigGround(new Vector3(pos.x, pos.y), GroundManager.instance.goldColor); break;
                case StageData.GroundLayer.Mithril:
                    EffectManager.instance.DigGround(new Vector3(pos.x, pos.y), GroundManager.instance.mithrilColor); break;
                case StageData.GroundLayer.Diamond:
                    EffectManager.instance.DigGround(new Vector3(pos.x, pos.y), GroundManager.instance.diamondColor); break;
                case StageData.GroundLayer.Magnetite:
                    EffectManager.instance.DigGround(new Vector3(pos.x, pos.y), GroundManager.instance.magnetiteColor); break;
                case StageData.GroundLayer.Titanium:
                    EffectManager.instance.DigGround(new Vector3(pos.x, pos.y), GroundManager.instance.titaniumColor); break;
                case StageData.GroundLayer.Cobalt:
                    EffectManager.instance.DigGround(new Vector3(pos.x, pos.y), GroundManager.instance.cobaltColor); break;
                case StageData.GroundLayer.Ice:
                    EffectManager.instance.DigGround(new Vector3(pos.x, pos.y), GroundManager.instance.iceColor); break;
                case StageData.GroundLayer.Grass:
                    EffectManager.instance.DigGround(new Vector3(pos.x, pos.y), GroundManager.instance.grassColor); break;
                case StageData.GroundLayer.HearthStone:
                    EffectManager.instance.DigGround(new Vector3(pos.x, pos.y), GroundManager.instance.hearthStoneColor); break;
            }
        }
        #endregion

        //광물 데미지 처리
        GroundManager.instance.groundHp[pos.x, pos.y] -= damage;

        #region[광물체력에 따른 이미지 변경]
        int maxHp = GroundManager.instance.GetBlockMaxHp(pos.x, pos.y);
        if (maxHp * 0.75f < GroundManager.instance.groundHp[pos.x, pos.y] && GroundManager.instance.groundHp[pos.x, pos.y] < maxHp * 1.00f)
            StageData.instance.SetBlockVariation(pos.x, pos.y, (byte)StageData.Layers.Ground, 0);
        else if (maxHp * 0.50f < GroundManager.instance.groundHp[pos.x, pos.y] && GroundManager.instance.groundHp[pos.x, pos.y] <= maxHp * 0.75f)
            StageData.instance.SetBlockVariation(pos.x, pos.y, (byte)StageData.Layers.Ground, 1);
        else if (maxHp * 0.25f < GroundManager.instance.groundHp[pos.x, pos.y] && GroundManager.instance.groundHp[pos.x, pos.y] <= maxHp * 0.50f)
            StageData.instance.SetBlockVariation(pos.x, pos.y, (byte)StageData.Layers.Ground, 2);
        else if (maxHp * 0.00f < GroundManager.instance.groundHp[pos.x, pos.y] && GroundManager.instance.groundHp[pos.x, pos.y] <= maxHp * 0.25f)
            StageData.instance.SetBlockVariation(pos.x, pos.y, (byte)StageData.Layers.Ground, 3);
        #endregion

        #region[광물 드랍]
        if (GroundManager.instance.groundHp[pos.x, pos.y] <= 0)
        {
            Vector2 Force = new Vector2(Random.Range(-0.5f, 0.5f), 1);
            Force *= Force * 1600;
            switch ((StageData.GroundLayer)(StageData.instance.groundData[pos.x, pos.y]))
            {
                //case StageData.GroundLayer.Dirt:
                //    ItemManager.instance.SpawnMineral(new Vector3(pos.x, pos.y), Force, "Dirt"); break;
                //case StageData.GroundLayer.Stone:
                //    ItemManager.instance.SpawnMineral(new Vector3(pos.x, pos.y), Force, "Stone"); break;
                case StageData.GroundLayer.Copper:
                    ItemManager.instance.SpawnMineral(new Vector3(pos.x, pos.y), Force, "Copper"); break;
                //case StageData.GroundLayer.Sand:
                //    ItemManager.instance.SpawnMineral(new Vector3(pos.x, pos.y), Force, "Sand"); break;
                case StageData.GroundLayer.Granite:
                    ItemManager.instance.SpawnMineral(new Vector3(pos.x, pos.y), Force, "Granite"); break;
                case StageData.GroundLayer.Iron:
                    ItemManager.instance.SpawnMineral(new Vector3(pos.x, pos.y), Force, "Iron"); break;
                case StageData.GroundLayer.Silver:
                    ItemManager.instance.SpawnMineral(new Vector3(pos.x, pos.y), Force, "Silver"); break;
                case StageData.GroundLayer.Gold:
                    ItemManager.instance.SpawnMineral(new Vector3(pos.x, pos.y), Force, "Gold"); break;
                case StageData.GroundLayer.Mithril:
                    ItemManager.instance.SpawnMineral(new Vector3(pos.x, pos.y), Force, "Mithril"); break;
                case StageData.GroundLayer.Diamond:
                    ItemManager.instance.SpawnMineral(new Vector3(pos.x, pos.y), Force, "Diamond"); break;
                case StageData.GroundLayer.Magnetite:
                    ItemManager.instance.SpawnMineral(new Vector3(pos.x, pos.y), Force, "Magnetite"); break;
                case StageData.GroundLayer.Titanium:
                    ItemManager.instance.SpawnMineral(new Vector3(pos.x, pos.y), Force, "Titanium"); break;
                case StageData.GroundLayer.Cobalt:
                    ItemManager.instance.SpawnMineral(new Vector3(pos.x, pos.y), Force, "Cobalt"); break;
            }
        }
        #endregion

        #region[해당위치 광물 제거]
        if (GroundManager.instance.groundHp[pos.x, pos.y] <= 0)
        {
            GroundManager.instance.groundHp[pos.x, pos.y] = 0;
            StageData.instance.RemoveBlock(pos);
            StageData.instance.groundData[pos.x, pos.y] = (StageData.GroundLayer)(-1);
            GroundManager.instance.LinkArea(pos);
        }
        #endregion
    }
    #endregion

    #region[몬스터 공격]
    public void AttackMonster()
    {
        //공격범위에 해당하는 적을을 공격
        Vector2 digPos = (Vector2)Player.instance.transform.position + new Vector2(damageJudgMent.offset.x * Player.instance.transform.localScale.x, damageJudgMent.offset.y * Player.instance.transform.localScale.y);
        Vector2 newSize = new Vector2(Mathf.Abs(damageJudgMent.size.x * Player.instance.transform.localScale.x), Mathf.Abs(damageJudgMent.size.y * Player.instance.transform.localScale.y));
        RaycastHit2D[] monsters = Physics2D.BoxCastAll(digPos, newSize, 0, Vector2.zero, 0, 1 << LayerMask.NameToLayer("Body"));
        for (int i = 0; i < monsters.Length; i++)
        {
            if (monsters[i].transform.tag.Equals("Monster"))
            {
                Monster monster = monsters[i].transform.GetComponent<Monster>();
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
    public void AttackObject()
    {
        //공격범위에 해당하는 적을을 공격
        Vector2 digPos = (Vector2)Player.instance.transform.position + new Vector2(damageJudgMent.offset.x * Player.instance.transform.localScale.x, damageJudgMent.offset.y * Player.instance.transform.localScale.y);
        Vector2 newSize = new Vector2(Mathf.Abs(damageJudgMent.size.x * Player.instance.transform.localScale.x), Mathf.Abs(damageJudgMent.size.y * Player.instance.transform.localScale.y));
        RaycastHit2D[] objects = Physics2D.BoxCastAll(digPos, newSize, 0, Vector2.zero, 0, 1 << LayerMask.NameToLayer("Body"));
        for (int i = 0; i < objects.Length; i++)
        {
            if (objects[i].transform.tag.Equals("Object"))
            {
                StructureObject structureObject = objects[i].transform.GetComponent<StructureObject>();
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