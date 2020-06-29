using UnityEngine;
using Custom;

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

        float newSpeed = speed;
        if (Vector2.Distance(nowPos, targetPos) < range &&
            Exception.IndexOutRange(nowPos, GroundManager.instance.linkArea) &&
            GroundManager.instance.linkArea[nowPos.x, nowPos.y] &&
                Mathf.Abs(nowPos.y - targetPos.y) <= 5)
        {
            if (Mathf.Abs(nowPos.x - targetPos.x) < 0.25f)
                MovingGround(+0);
            else if (nowPos.x < targetPos.x)
            {
                if (CanMove(newSpeed))
                    MovingGround(+newSpeed);
                else
                    MovingGround(+0);
            }
            else if (nowPos.x > targetPos.x)
            {
                if (CanMove(-newSpeed))
                    MovingGround(-newSpeed);
                else
                    MovingGround(+0);
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
        SoundManager.instance.MonsterDamage();
    }
    #endregion

    #region[Attack]
    public override void Attack()
    {
        if (Player.instance && Player.instance.damage)
            return;

        bool check = IsAtPlayer(attackCollider);
        bool attackRange = IsAtPlayer(attackRangeCollider);

        if (attackRange && !isAttack)
            animator.SetTrigger("Attack");

        if (check)
        {
            int dic = 1;
            if(isAttack && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.35f)
                dic = transform.position.x < Player.instance.transform.position.x ? +2 : -2;
            else
                dic = transform.position.x < Player.instance.transform.position.x ? +1 : -1;
            Player.instance.PlayerDamage(attackPower, dic);
        }
    }
    #endregion

}