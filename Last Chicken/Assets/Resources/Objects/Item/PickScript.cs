using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Custom;

public class PickScript : ThrowUpdate
{
    Rigidbody2D rigidbody2D;
    BoxCollider2D boxCollider2D;
    float coolTime = 0;

    #region[Awake]
    public override void Awake()
    {
        base.Awake();
        rigidbody2D = GetComponent<Rigidbody2D>();
        boxCollider2D = transform.Find("AttackTerrain").GetComponent<BoxCollider2D>();

    }
    #endregion

    #region[Update]
    public override void Update()
    {
        base.Update();
        if (rigidbody2D.angularVelocity > 90 && Vector2.Distance(rigidbody2D.velocity, Vector2.zero) > 5)
            AttacTerrain(5);

    }
    #endregion

    #region[지형 공격]
    public void AttacTerrain(int damage)
    {
        if(coolTime > 0)
        {
            coolTime -= Time.deltaTime;
            return;
        }

        int[] SoundGroup = new int[17];

        #region[범위에 해당하는 광물 공격]
        //공격범위에 해당하는 광물을 공격
        Vector2 digPos = (Vector2)transform.position + new Vector2(boxCollider2D.offset.x * transform.localScale.x, boxCollider2D.offset.y * transform.localScale.y);
        Vector2 newSize = new Vector2(Mathf.Abs(boxCollider2D.size.x * transform.localScale.x), Mathf.Abs(boxCollider2D.size.y * transform.localScale.y));

        for (int x = (int)(digPos.x - newSize.x / 2); x <= (int)(digPos.x + newSize.x / 2); x++)
            for (int y = (int)(digPos.y - newSize.y / 2); y <= (int)(digPos.y + newSize.y / 2); y++)
                if (Exception.IndexOutRange(x, y, StageData.instance.groundData))
                    if (StageData.instance.groundData[x, y] != (StageData.GroundLayer)(-1))
                        if (GroundManager.instance.groundHp[x, y] > 0)
                            if ((GroundManager.instance.digMask & (int)Mathf.Pow(2, (int)StageData.instance.groundData[x, y])) != 0)
                            {
                                SoundGroup[(int)StageData.instance.groundData[x, y]]++;
                                GroundManager.instance.AttackTerrain(new Vector2Int(x, y), damage);
                                coolTime = 0.1f;
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

    #region[OnEnable]
    public override void OnEnable()
    {
        base.OnEnable();
    }
    #endregion
}
