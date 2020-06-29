using UnityEngine;
using Custom;

public class Penguin : Monster
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    float isPatrolTime = 3;
    MoveDic patrolDic = 0;
    public enum MoveDic { 오른쪽, 왼쪽, 정지 };

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
        animator.SetBool("InIce", iceFlag);
        if (damage || moveDic == 0)
            return;
        transform.localScale = new Vector3(-moveDic * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

    }
    #endregion

    #region[이동처리]
    public void Move()
    {
        if (!playerSeeck && Vector2.Distance(nowPos, targetPos) < range && Mathf.Abs(nowPos.y - targetPos.y) <= 5)
            playerSeeck = true;
        else if (playerSeeck && Vector2.Distance(nowPos, targetPos) >= range * 1.2f)
            playerSeeck = false;

        float newSpeed = speed * (iceFlag ? 3 : 1);
        if (playerSeeck)
        {
            if (Mathf.Abs(nowPos.x - targetPos.x) < 2)
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
                            patrolDic = MoveDic.왼쪽;
                        break;
                    case MoveDic.왼쪽:
                        if (CanFallBlock(-newSpeed, 4) && CanMove(-newSpeed))
                            MovingGround(-newSpeed);
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
    #endregion

}