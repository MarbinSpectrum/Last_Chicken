﻿using Custom;
using UnityEngine;

public class Bat : Monster
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    float isPatrolTime = 3;
    bool batStop;
    MoveDic patrolDic = 0;
    public enum MoveDic { 오른쪽, 왼쪽, 위, 아래, 오른쪽_위, 오른쪽_아래, 왼쪽_위, 왼쪽_아래 };

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

    #region[OnEnable]
    public override void OnEnable()
    {
        base.OnEnable();
    }
    #endregion

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region[능력치 갱신]
    public override void UpdateStats()
    {
        if (monsterIndex == -1 || updateFlag || MonsterManager.instance == null)
            return;
        if (monsterIndex == -2)
            monsterIndex = MonsterManager.FindData(transform.name);
        updateFlag = true;
        monsterType = MonsterType.Fly;
        maxHp = MonsterManager.instance.monsterData[monsterIndex].Hp;
        speed = MonsterManager.instance.monsterData[monsterIndex].Speed;
        attackPower = MonsterManager.instance.monsterData[monsterIndex].AttackPower;
        jumpPower = MonsterManager.instance.monsterData[monsterIndex].JumpPower;
    }
    #endregion

    #region[애니메이션]
    public void Ani()
    {
        animator.SetBool(DAMAGE, damage);
        animator.SetBool(STOP, batStop);
        if (!damage)
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
        if (Vector2.Distance(nowPos, Player.instance.transform.position) < 10 && nowPos.y >= Player.instance.transform.position.y)
            if (Mathf.Sign(Player.instance.transform.localScale.x) == Mathf.Sign(transform.localScale.x))
                if ((Player.instance.transform.localScale.x > 0 && Player.instance.transform.position.x < transform.position.x) ||
                    (Player.instance.transform.localScale.x < 0 && Player.instance.transform.position.x > transform.position.x))
                {
                    MovingFly(0, 0);
                    batStop = true;
                    return;
                }

        batStop = false;

        if (Exception.IndexOutRange(nowPos, GroundManager.instance.linkArea) &&
            GroundManager.instance.FindParent(GroundManager.instance.linkArea[nowPos.x, nowPos.y]) == 1 &&
            Vector2.Distance(nowPos, Player.instance.transform.position) < AstarRange && 
            !Player.instance.bat_Hate_Light)
        {
            if(AreaList != null)
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

                #region[횃불이나 헬멧착용시 박쥐가 플레이어를 피해가게함]
                if (Player.instance.bat_Hate_Light)
                {
                    switch (patrolDic)
                    {
                        case MoveDic.오른쪽:
                            if (transform.position.x < Player.instance.transform.position.x)
                                patrolDic = MoveDic.왼쪽;
                            break;
                        case MoveDic.왼쪽:
                            if (transform.position.x > Player.instance.transform.position.x)
                                patrolDic = MoveDic.오른쪽;
                            break;
                        case MoveDic.위:
                            if (transform.position.y < Player.instance.transform.position.y)
                                patrolDic = MoveDic.아래;
                            break;
                        case MoveDic.아래:
                            if (transform.position.y > Player.instance.transform.position.y)
                                patrolDic = MoveDic.위;
                            break;
                        case MoveDic.오른쪽_위:
                            if (transform.position.x < Player.instance.transform.position.x && transform.position.y < Player.instance.transform.position.y)
                                patrolDic = MoveDic.왼쪽_아래;
                            break;
                        case MoveDic.오른쪽_아래:
                            if (transform.position.x < Player.instance.transform.position.x && transform.position.y > Player.instance.transform.position.y)
                                patrolDic = MoveDic.왼쪽_위;
                            break;
                        case MoveDic.왼쪽_위:
                            if (transform.position.x > Player.instance.transform.position.x && transform.position.y < Player.instance.transform.position.y)
                                patrolDic = MoveDic.오른쪽_아래;
                            break;
                        case MoveDic.왼쪽_아래:
                            if (transform.position.x > Player.instance.transform.position.x && transform.position.y > Player.instance.transform.position.y)
                                patrolDic = MoveDic.오른쪽_위;
                            break;
                    }
                }
                #endregion

                switch (patrolDic)
                {
                    case MoveDic.오른쪽:
                        MovingFly(+speed, 0);
                        break;
                    case MoveDic.왼쪽:
                        MovingFly(-speed, 0);
                        break;
                    case MoveDic.위:
                        MovingFly(0, +speed);
                        break;
                    case MoveDic.아래:
                        MovingFly(0, -speed);
                        break;
                    case MoveDic.오른쪽_위:
                        MovingFly(+speed, +speed);
                        break;
                    case MoveDic.오른쪽_아래:
                        MovingFly(+speed, -speed);
                        break;
                    case MoveDic.왼쪽_위:
                        MovingFly(-speed, +speed);
                        break;
                    case MoveDic.왼쪽_아래:
                        MovingFly(-speed, -speed);
                        break;
                }

            }
        }
    }
    #endregion

}