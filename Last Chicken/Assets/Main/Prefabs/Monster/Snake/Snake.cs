using UnityEngine;
using Custom;

public class Snake : Monster
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    float isPatrolTime = 3;
    MoveDic patrolDic = 0;
    public enum MoveDic { 오른쪽, 왼쪽, 정지 };

    bool attack;
    bool attackFlag;
    float attackTime = 0;

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region[Awake]
    public override void Awake()
    {
        UpdateStats();
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
        else
        {
            attackTime = 0;
            attack = false;
        }
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
        if (damage || moveDic == 0)
            return;
        transform.localScale = new Vector3(-moveDic * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

    }
    #endregion

    #region[이동처리]
    public void Move()
    {
        animator.SetBool("Attack", attack);

        attackTime += Time.deltaTime;

        if (!playerSeeck && Vector2.Distance(nowPos, targetPos) < range && Mathf.Abs(nowPos.y - targetPos.y) <= 5)
            playerSeeck = true;
        else if (playerSeeck && Vector2.Distance(nowPos, targetPos) >= range * 1.2f)
            playerSeeck = false;

        //공격을 이용한 이동
        if (attack)
        {
            if (!attackFlag)
            {
                attackFlag = true;
                if (nowPos.x < targetPos.x)
                {
                    moveDic = +1;
                    Jumping(new Vector2(+jumpPower.x, jumpPower.y));
                }
                else if (nowPos.x > targetPos.x)
                {
                    moveDic = -1;
                    Jumping(new Vector2(-jumpPower.x, jumpPower.y));
                }
            }

            if (grounded && attackTime > 0.1f || attackTime > 2f)
            {
                attack = false;
                attackTime = 0;
            }
        }
        //일반 이동
        else
        {
            if (playerSeeck)
            {


                if (attackTime > 2)
                {
                    attackTime = 0;
                    attackFlag = false;
                    attack = true;
                }
                else
                {
                    if (Mathf.Abs(nowPos.x - targetPos.x) < 0.15f)
                        MovingGround(+0);
                    else if (nowPos.x < targetPos.x)
                        MovingGround(+speed);
                    else if (nowPos.x > targetPos.x)
                        MovingGround(-speed);
                }
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
                            if (CanFallBlock(speed, 4) && CanMove(speed))
                                MovingGround(+speed);
                            else
                                patrolDic = MoveDic.왼쪽;
                            break;
                        case MoveDic.왼쪽:
                            if (CanFallBlock(-speed, 4) && CanMove(-speed))
                                MovingGround(-speed);
                            else
                                patrolDic = MoveDic.오른쪽;
                            break;
                        case MoveDic.정지:
                            MovingGround(+0);
                            break;
                    }
                }
            }
        }
    }
    #endregion

}