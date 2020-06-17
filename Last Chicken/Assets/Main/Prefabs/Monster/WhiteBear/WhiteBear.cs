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
    BoxCollider2D headCollider;

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region[Awake]
    public override void Awake()
    {
        UpdateStats();
        headCollider = transform.Find("CheckList").Find("Head").GetComponent<BoxCollider2D>();
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
        //animator.SetBool("Damage", damage);
        //animator.SetBool("Move", moveFlag);
        if (damage || moveDic == 0)
            return;
        transform.localScale = new Vector3(-moveDic * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

    }
    #endregion

    #region[이동처리]
    public void Move()
    {
        float newSpeed = speed;
        if (Vector2.Distance(nowPos, targetPos) < range &&
            Exception.IndexOutRange(nowPos, GroundManager.instance.linkArea) &&
            GroundManager.instance.linkArea[nowPos.x, nowPos.y] &&
                Mathf.Abs(nowPos.y - targetPos.y) <= 5)
        {
            if (Mathf.Abs(nowPos.x - targetPos.x) < 0.15f)
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

    #region[MovingGround]
    public override void MovingGround(float force)
    {
        moveFlag = false;

        if (force != 0)
        {
            #region[경사아래로 이동]
            bool DownmoveCheck = IsAtTerrain(boxCollider2D, new Vector2(force, -Mathf.Abs(force)) * Time.deltaTime) ||
                IsAtTerrain(headCollider, new Vector2(force, -Mathf.Abs(force)) * Time.deltaTime);

            if (!DownmoveCheck && grounded && !moveFlag)
            {
                moveFlag = true;
                transform.Translate(new Vector3(force, -Mathf.Abs(force), 0) * Time.deltaTime);
            }
            #endregion

            #region[앞으로 이동]
            bool nextMoveCheck = IsAtTerrain(boxCollider2D, new Vector2(force, 0) * Time.deltaTime) ||
                IsAtTerrain(headCollider, new Vector2(force,0) * Time.deltaTime);

            if (!nextMoveCheck && !moveFlag)
            {
                moveFlag = true;
                transform.Translate(new Vector3(force, 0, 0) * Time.deltaTime);
            }
            #endregion

            #region[경사위로 이동]
            bool upMoveCheck = IsAtTerrain(boxCollider2D, new Vector2(force, Mathf.Abs(force)) * Time.deltaTime) ||
                IsAtTerrain(headCollider, new Vector2(force, Mathf.Abs(force)) * Time.deltaTime);

            if (!upMoveCheck && grounded && !moveFlag)
            {
                moveFlag = true;
                transform.Translate(new Vector3(force, Mathf.Abs(force), 0) * Time.deltaTime);
            }
            #endregion

            #region[이동방향설정]
            if (moveFlag)
                moveDic = force > 0 ? 1 : force < 0 ? -1 : moveDic;
            #endregion
        }

        if (!moveFlag)
        {
            bool ObjectCheck = IsAtTerrainObject(boxCollider2D, new Vector2(-1, 0)) || IsAtTerrainObject(boxCollider2D, new Vector2(+1, 0)) ||
                IsAtTerrainObject(headCollider, new Vector2(-1, 0)) || IsAtTerrainObject(headCollider, new Vector2(+1, 0));

            if (ObjectCheck)
                moveFlag = true;
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

        bool check = IsAtPlayer(boxCollider2D);
        if (check)
        {
            int dic = transform.position.x < Player.instance.transform.position.x ? +2 : -2;
            Player.instance.PlayerDamage(attackPower, dic);
        }
    }
    #endregion

}