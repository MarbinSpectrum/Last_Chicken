using Custom;
using UnityEngine;

public class Mole : Monster
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    float isPatrolTime = 3;
    float digGroundTime = 0;

    MoveDic patrolDic = 0;
    public enum MoveDic { 오른쪽, 왼쪽, 위, 아래, 오른쪽_위, 오른쪽_아래, 왼쪽_위, 왼쪽_아래,정지 };

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

    #region[능력치 갱신]
    public override void UpdateStats()
    {
        monsterType = MonsterType.Dig;
        maxHp = MonsterManager.instance.monsterData[MonsterManager.FindData(transform.name)].Hp;
        speed = MonsterManager.instance.monsterData[MonsterManager.FindData(transform.name)].Speed;
        attackPower = MonsterManager.instance.monsterData[MonsterManager.FindData(transform.name)].AttackPower;
  
        if (boxCollider2D)
            boxCollider2D.isTrigger = true;
    }
    #endregion

    #region[애니메이션]
    public void Ani()
    {
        //animator.SetBool("Damage", damage);

        if (damage || moveDic == 0)
            return;

        if (monsterType == MonsterType.Ground)
            transform.localScale = new Vector3(-moveDic * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else if (monsterType == MonsterType.Dig)
        {
            if (rigidbody2D.velocity.x > 0)
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            else if (rigidbody2D.velocity.x < 0)
                transform.localScale = new Vector3(+Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }
    #endregion

    #region[이동처리]
    public void Move()
    {
        #region[상태변환]
        if (monsterType == MonsterType.Dig && StageData.instance.GetBlock(nowPos.x, nowPos.y) == (StageData.GroundLayer)(-1))
        {
            monsterType = MonsterType.Ground;
            boxCollider2D.isTrigger = false;
            jumpPower = Vector2.zero;
            if (nowPos.x < Player.instance.transform.position.x)
            {
                jumpPower.x = +1000;
                moveDic = 1;
            }
            else
            {
                jumpPower.x = -1000;
                moveDic = -1;
            }

            if (nowPos.y < Player.instance.transform.position.y)
                jumpPower.y = +1000;
            else
                jumpPower.y = -1000;

            Jumping(jumpPower);

            digGroundTime = 0;
        }
        else if (monsterType == MonsterType.Ground)
        {
            digGroundTime += Time.deltaTime;
            if (digGroundTime > 5)
            {
                digGroundTime = 0;
                if (grounded)
                {
                    boxCollider2D.isTrigger = true;
                    transform.position += new Vector3(0, -2.5f, 0);
                    monsterType = MonsterType.Dig;
                }
            }
        }
        #endregion

        if (monsterType == MonsterType.Dig)
        {
            if (Vector2.Distance(nowPos, Player.instance.transform.position) < AstarRange && AreaList != null)
            {
                if (Vector2.Distance(nowPos, Player.instance.transform.position) >= 2)
                {
                    if (Exception.IndexOutRange(nowPoint + 1, AreaList))
                    {
                        if (Vector2.Distance(transform.position, AreaList[nowPoint + 1]) < 0.2f)
                            nowPoint = nowPoint + 1;
                        else
                        {
                            if (Mathf.Abs(transform.position.x - AreaList[nowPoint + 1].x) < 0.15f)
                                MovingFly(+0, rigidbody2D.velocity.y);
                            else if (transform.position.x < AreaList[nowPoint + 1].x)
                                MovingFly(+speed, rigidbody2D.velocity.y);
                            else if (transform.position.x > AreaList[nowPoint + 1].x)
                                MovingFly(-speed, rigidbody2D.velocity.y);

                            if (Mathf.Abs(transform.position.y - AreaList[nowPoint + 1].y) < 0.15f)
                                MovingFly(rigidbody2D.velocity.x, +0);
                            else if (transform.position.y < AreaList[nowPoint + 1].y)
                                MovingFly(rigidbody2D.velocity.x, +speed);
                            else if (transform.position.y > AreaList[nowPoint + 1].y)
                                MovingFly(rigidbody2D.velocity.x, -speed);
                        }
                    }
                    else
                        rigidbody2D.velocity = new Vector2(0, 0);
                }
                else
                {
                    if (Mathf.Abs(transform.position.x - Player.instance.transform.position.x) < 0.15f)
                        MovingFly(+0, rigidbody2D.velocity.y);
                    else if (transform.position.x < Player.instance.transform.position.x)
                        MovingFly(+speed, rigidbody2D.velocity.y);
                    else if (transform.position.x > Player.instance.transform.position.x)
                        MovingFly(-speed, rigidbody2D.velocity.y);


                    if (Mathf.Abs(transform.position.y - Player.instance.transform.position.y) < 0.15f)
                        MovingFly(rigidbody2D.velocity.x, +0);
                    else if (transform.position.y < Player.instance.transform.position.y)
                        MovingFly(rigidbody2D.velocity.x, +speed);
                    else if (transform.position.y > Player.instance.transform.position.y)
                        MovingFly(rigidbody2D.velocity.x, -speed);
                }
            }
            else
            {
                if (patrolTime < 0)
                {
                    Random.InitState((int)Time.time * Random.Range(0, 100));
                    patrolTime = isPatrolTime;
                    patrolDic = (MoveDic)Random.Range(0, 8);
                }
                else
                {
                    patrolTime -= Time.deltaTime;
                    switch (patrolDic)
                    {
                        case MoveDic.오른쪽:
                            if (StageData.instance.GetBlock(nowPos.x + 1, nowPos.y) == (StageData.GroundLayer)(-1))
                                patrolDic = MoveDic.정지;
                            else
                                MovingFly(+speed, 0);
                            break;
                        case MoveDic.왼쪽:
                            if (StageData.instance.GetBlock(nowPos.x - 1, nowPos.y) == (StageData.GroundLayer)(-1))
                                patrolDic = MoveDic.오른쪽;
                            else
                                MovingFly(-speed, 0);
                            break;
                        case MoveDic.위:
                            if (StageData.instance.GetBlock(nowPos.x, nowPos.y + 1) == (StageData.GroundLayer)(-1))
                                patrolDic = MoveDic.아래;
                            else
                                MovingFly(0, +speed);
                            break;
                        case MoveDic.아래:
                            if (StageData.instance.GetBlock(nowPos.x, nowPos.y - 1) == (StageData.GroundLayer)(-1))
                                patrolDic = MoveDic.위;
                            else
                                MovingFly(0, -speed);
                            break;
                        case MoveDic.오른쪽_위:
                            if (StageData.instance.GetBlock(nowPos.x + 1, nowPos.y + 1) == (StageData.GroundLayer)(-1))
                                patrolDic = MoveDic.정지;
                            else
                                MovingFly(+speed, +speed);
                            break;
                        case MoveDic.오른쪽_아래:
                            if (StageData.instance.GetBlock(nowPos.x + 1, nowPos.y - 1) == (StageData.GroundLayer)(-1))
                                patrolDic = MoveDic.정지;
                            else
                                MovingFly(+speed, -speed);
                            break;
                        case MoveDic.왼쪽_위:
                            if (StageData.instance.GetBlock(nowPos.x - 1, nowPos.y + 1) == (StageData.GroundLayer)(-1))
                                patrolDic = MoveDic.오른쪽_아래;
                            else
                                MovingFly(-speed, +speed);
                            break;
                        case MoveDic.왼쪽_아래:
                            if (StageData.instance.GetBlock(nowPos.x - 1, nowPos.y - 1) == (StageData.GroundLayer)(-1))
                                patrolDic = MoveDic.오른쪽_위;
                            else
                                MovingFly(-speed, -speed);
                            break;
                        case MoveDic.정지:
                            MovingFly(0, 0);
                            break;
                    }
                }
            }
        }
        else if (monsterType == MonsterType.Ground)
        {
            if (Vector2.Distance(nowPos, targetPos) < range &&
            Exception.IndexOutRange(nowPos, GroundManager.instance.linkArea) &&
            GroundManager.instance.linkArea[nowPos.x, nowPos.y] &&
                Mathf.Abs(nowPos.y - targetPos.y) <= 5)
            {
                if (Mathf.Abs(nowPos.x - targetPos.x) < 0.15f)
                    MovingGround(+0);
                else if (nowPos.x < targetPos.x)
                    MovingGround(+speed);
                else if (nowPos.x > targetPos.x)
                    MovingGround(-speed);
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