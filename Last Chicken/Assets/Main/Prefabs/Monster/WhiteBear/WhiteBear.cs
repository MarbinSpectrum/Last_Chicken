using UnityEngine;
using Custom;
using TerrainEngine2D;
public class WhiteBear : Monster
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    float isPatrolTime = 3;
    MoveDic patrolDic = 0;
    public enum MoveDic { 오른쪽, 왼쪽, 정지 };
    BoxCollider2D attackCollider;
    BoxCollider2D attackRangeCollider;
    bool isAttack = false;
    bool digGround = false;
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region[Awake]
    public override void Awake()
    {
        UpdateStats();
        attackCollider = transform.Find("CheckList").Find("Attack").GetComponent<BoxCollider2D>();
        attackRangeCollider = transform.Find("CheckList").Find("AttackRange").GetComponent<BoxCollider2D>();
        base.Awake();
    }
    #endregion

    #region[Update]
    public override void Update()
    {
        base.Update();
        Ani();
        if (!damage)
        {
            Attack();
            Move();
        }
    }
    #endregion

    #region[OnEnable]
    public override void OnEnable()
    {
        base.OnEnable();
    }
    #endregion

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region[애니메이션]
    public void Ani()
    {
        animator.SetBool("Damage", damage);
        animator.SetBool("Move", moveFlag);
        isAttack = animator.GetCurrentAnimatorStateInfo(0).IsName("Attack");

        if (damage || moveDic == 0)
            return;
        transform.localScale = new Vector3(-moveDic * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

    }
    #endregion

    #region[이동처리]
    public void Move()
    {
        if (isAttack)
            return;

        if (!playerSeeck && Vector2.Distance(nowPos, targetPos) < range && Mathf.Abs(nowPos.y - targetPos.y) <= 5)
            playerSeeck = true;
        else if (playerSeeck && Vector2.Distance(nowPos, targetPos) >= range * 1.2f)
            playerSeeck = false;

        float newSpeed = speed;
        if (playerSeeck)
        {
            if (Mathf.Abs(nowPos.x - targetPos.x) < 3)
                MovingGround(+0);
            else if (nowPos.x < targetPos.x)
                MovingGround(+newSpeed);
            else if (nowPos.x > targetPos.x)
                MovingGround(-newSpeed);
        }
        else
        {
            if (patrolTime < 0)
            {
                patrolTime = isPatrolTime;
                Random.InitState((int)Time.time * Random.Range(0, 100));
                int r = Random.Range(0, 100);
                if (r < 20)
                    patrolDic = MoveDic.정지;
                else if (r < 60)
                    patrolDic = MoveDic.오른쪽;
                else if (r < 100)
                    patrolDic = MoveDic.왼쪽;
            }
            else
            {
                patrolTime -= Time.deltaTime;
                switch (patrolDic)
                {
                    case MoveDic.오른쪽:
                        if (CanFallBlock(newSpeed, 4) && CanMove(newSpeed))
                            MovingGround(+newSpeed);
                        else
                            patrolDic = MoveDic.정지;
                        break;
                    case MoveDic.왼쪽:
                        if (CanFallBlock(-newSpeed, 4) && CanMove(-newSpeed))
                            MovingGround(-newSpeed);
                        else
                            patrolDic = MoveDic.정지;
                        break;
                    case MoveDic.정지:
                        MovingGround(+0);
                        break;
                }

            }
        }
    }
    #endregion

    #region[Damage]
    public override void Damage(int n)
    {
        damage = true;
        isStunTime = stunTime;
        hp -= n;
        rigidbody2D.velocity = Vector2.zero;
        float dicX = transform.position.x < Player.instance.transform.position.x ? -1 : +1;
        float dicY = 0.01f;
        rigidbody2D.AddForce(new Vector2(dicX * knockback.x, dicY * knockback.y), ForceMode2D.Force);
        if (Vector2.Distance((Vector2)transform.position, (Vector2)CameraController.Instance.transform.position) < 45)
        {
            EffectManager.instance.Vibration(EffectManager.instance.monsterDeadVibration.num, EffectManager.instance.monsterDeadVibration.power);
            SoundManager.instance.MonsterDamage();
        }
    }
    #endregion

    #region[Attack]
    public override void Attack()
    {
        if (Player.instance && Player.instance.damage)
            return;

        bool attackRange = IsAtPlayer(attackRangeCollider);
        bool check = IsAtPlayer(attackCollider);

        if(attackRange)
        {       
            if (attackRange && !isAttack)
                animator.SetTrigger("Attack");

            if (check)
            {
                int dic = 1;
                if (isAttack && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.35f)
                    dic = transform.position.x < Player.instance.transform.position.x ? +2 : -2;
                else
                    dic = transform.position.x < Player.instance.transform.position.x ? +1 : -1;
                Player.instance.PlayerDamage(attackPower, dic);
            }
        }
        else if(playerSeeck)
        {
            int dic = (int)-transform.localScale.x;
            bool breakBlock = !CanMove(dic);
            
            if (breakBlock && !isAttack)
                animator.SetTrigger("Attack");

            int[] SoundGroup = new int[17];

            #region[범위에 해당하는 광물 공격]
            if (!digGround && animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.5f)
            {
                int width = 5;
                int height = 5;
                digGround = true;
                for (int r = 0; r < height; r++)
                    for (int c = 0; c < width; c++)
                    {
                        int ar = r + (int)transform.position.y;
                        int ac = dic * c + (int)transform.position.x;
                        if (Exception.IndexOutRange(ac, ar, StageData.instance.groundData))
                            if (StageData.instance.groundData[ac, ar] != (StageData.GroundLayer)(-1))
                                if (GroundManager.instance.groundHp[ac, ar] > 0)
                                    if ((GroundManager.instance.digMask & (int)Mathf.Pow(2, (int)StageData.instance.groundData[ac, ar])) != 0)
                                    {
                                        SoundGroup[(int)StageData.instance.groundData[ac, ar]]++;
                                        GroundManager.instance.AttackTerrain(new Vector2Int(ac, ar), 10);
                                    }
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

            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                digGround = false;
        }
    }
    #endregion

}