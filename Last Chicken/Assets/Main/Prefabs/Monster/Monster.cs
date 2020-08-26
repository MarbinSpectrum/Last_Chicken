using Custom;
using System.Collections.Generic;
using TerrainEngine2D;
using UnityEngine;

public abstract class Monster : CustomCollider
{
    public struct AStarRoute
    {
        public int w;
        public Vector2Int parents;
    }

    AStarRoute[,] Area;   //탐사 맵
    List<Vector2Int> OpenList = new List<Vector2Int>(); //열린리스트
    HashSet<Vector2Int> CloseList = new HashSet<Vector2Int>(); //닫힌리스트
    bool explore;   //탐사 성공 여부
    float reExploreTime = 0;    //탐색주기

    protected List<Vector2> AreaList = new List<Vector2>(); //이동경로
    protected int nowPoint = -1;  //이동포인터
    protected Vector2Int nowPos;  //몬스터위치
    protected Vector2Int targetPos; //타겟위치
    protected int range = 25;   //탐색범위
    protected bool playerSeeck;
    protected int AstarRange = 20;
    protected bool iceFlag = false;
    protected float iceVelocity;
    protected float slipperyValue = 0.8f;

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public enum MonsterType { Fly, Ground, Dig };
    protected MonsterType monsterType;    //몬스터종류

    protected int maxHp; //체력
    [HideInInspector] public int hp;

    protected float speed;     //속도
    protected bool moveFlag;
    [HideInInspector] public bool grounded;   //지상여부
    protected int moveDic = 0;        //이동방향

    protected float patrolTime = 0;

    protected float attackPower;   //공격력

    [HideInInspector] public bool damage; //데미지 
    protected float stunTime = 0.5f;   //경직시간
    protected float isStunTime;

    protected float gravity = 8;


    protected Vector2 knockback = new Vector2(750, 2000);   //넉백수치
    protected Vector2 jumpPower = Vector2.zero;

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    protected Animator animator;
    protected BoxCollider2D boxCollider2D;
    new protected Rigidbody2D rigidbody2D;

    GameObject light;
    public static bool useMonsterRadar; 

    World world;

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region[Awake]
    public virtual void Awake()
    {
        animator = GetComponent<Animator>();
        Transform checkList = transform.Find("CheckList");
        boxCollider2D = checkList.Find("Body").GetComponent<BoxCollider2D>();
        rigidbody2D = GetComponent<Rigidbody2D>();

        light = transform.Find("Light").gameObject;
        light.transform.localScale = new Vector3(1, 1, 1);
    }
    #endregion

    #region[OnEnable]
    public virtual void OnEnable()
    {
        UpdateStats();
        world = World.Instance;
        hp = maxHp;
        if (world)
            Area = new AStarRoute[world.WorldWidth, world.WorldHeight];
    }
    #endregion

    #region[Update]
    public virtual void Update()
    {
        if (!world)
            return;
        MakeAreaList();
        Damage();

        Vector2Int offset = new Vector2Int(0, 2);
        nowPos = new Vector2Int(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y));
        targetPos = new Vector2Int(Mathf.FloorToInt(Player.instance.transform.position.x) + offset.x, Mathf.FloorToInt(Player.instance.transform.position.y) + offset.y);

        light.SetActive(useMonsterRadar);

        iceFlag = (StageData.instance.GetBlock(nowPos + new Vector2Int(0, -2)) == StageData.GroundLayer.Ice);

    }
    #endregion

    #region[LateUpdate]
    private void LateUpdate()
    {
        GroundedCheck();
    }
    #endregion

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region[지상판정]
    public virtual void GroundedCheck()
    {
        if (monsterType == MonsterType.Fly || monsterType == MonsterType.Dig)
        {
            if (!damage)
                rigidbody2D.gravityScale = 0;
            else
                rigidbody2D.gravityScale = gravity;
        }
        else
        {
            grounded = IsAtTerrain(boxCollider2D, new Vector2(0, -0.15f));

            if (grounded)
            {
                if (rigidbody2D.gravityScale != 0 && rigidbody2D.velocity.y <= 0)
                    rigidbody2D.velocity = Vector2.zero;
                rigidbody2D.gravityScale = 0;
            }
            else
                rigidbody2D.gravityScale = gravity;
        }
    }
    #endregion

    #region[데미지]
    public virtual void Damage(int n)
    {
        damage = true;
        isStunTime = stunTime;
        hp -= n;
        rigidbody2D.velocity = Vector2.zero;
        float dicX = transform.position.x < Player.instance.transform.position.x ? -1 : +1;
        float dicY = grounded ? 1 : Player.instance.attackTop ? 1 : 0.01f;
        rigidbody2D.AddForce(new Vector2(dicX * knockback.x, dicY * knockback.y), ForceMode2D.Force);
        SoundManager.instance.MonsterDamage();
    }
    #endregion

    #region[데미지처리]
    public void Damage()
    {
        //데미지를 받았으면 스턴시간동안 경직
        if (damage)
        {
            isStunTime -= Time.deltaTime;
            //경직시간이 풀리면 플래그를 false 처리
            if (isStunTime <= 0)
                damage = false;
        }

        if (hp <= 0)
        {
            int monsterNum = MonsterManager.FindData(transform.name);
            if (monsterNum != -1)
                GameManager.instance.playData.monsterRecords[monsterNum] = true;

            EffectManager.instance.Vibration(EffectManager.instance.monsterDeadVibration.num, EffectManager.instance.monsterDeadVibration.power);
            SoundManager.instance.MonsterDead();
            transform.position = new Vector3(-1000,-1000,transform.position.z);
            MonsterManager.instance.ReSwpawn(world, 25);
            gameObject.SetActive(false);
        }
    }
    #endregion

    #region[이동처리]
    public virtual void MovingGround(float force)
    {
        moveFlag = false;

        if(force == 0)
        {
            if (Mathf.Abs(iceVelocity) < 0.1f)
                iceVelocity = 0;
            else if (iceVelocity > 0)
                iceVelocity -= Time.deltaTime * slipperyValue;
            else if (iceVelocity < 0)
                iceVelocity += Time.deltaTime * slipperyValue;
            force += iceVelocity;
        }

        if (force != 0)
        {
            #region[경사아래로 이동]
            bool DownmoveCheck = IsAtTerrain(boxCollider2D, new Vector2(force, -Mathf.Abs(force)) * Time.deltaTime);

            if (!DownmoveCheck && grounded && !moveFlag)
            {
                moveFlag = true;
                transform.Translate(new Vector3(force, -Mathf.Abs(force), 0) * Time.deltaTime);
            }
            #endregion

            #region[앞으로 이동]
            bool nextMoveCheck = IsAtTerrain(boxCollider2D, new Vector2(force, 0) * Time.deltaTime);

            if (!nextMoveCheck && !moveFlag)
            {
                moveFlag = true;
                transform.Translate(new Vector3(force, 0, 0) * Time.deltaTime);
            }
            #endregion

            #region[경사위로 이동]
            bool upMoveCheck = IsAtTerrain(boxCollider2D, new Vector2(force, Mathf.Abs(force)) * Time.deltaTime);

            if (!upMoveCheck && grounded && !moveFlag)
            {
                moveFlag = true;
                transform.Translate(new Vector3(force, Mathf.Abs(force), 0) * Time.deltaTime);
            }
            #endregion

            #region[이동방향설정]
            if (moveFlag)
            {
                moveDic = force > 0 ? 1 : force < 0 ? -1 : moveDic;
                if(iceFlag)
                    iceVelocity = force;
            }
            #endregion
        }

        if(!moveFlag)
        {
            bool ObjectCheck = IsAtTerrainObject(boxCollider2D, new Vector2(-1, 0)) || IsAtTerrainObject(boxCollider2D, new Vector2(+1, 0));

            if (ObjectCheck)
                moveFlag = true;
        }
    }

    public void MovingFly(float x, float y)
    {
        rigidbody2D.velocity = new Vector2(x, y);
    }

    public void MovingFly(Vector2 vector2)
    {
        rigidbody2D.velocity = vector2;
    }
    #endregion

    #region[이동검사]
    public bool CanMove(float dic)
    {
        if (StageData.instance.GetBlock(nowPos + new Vector2Int((int)Mathf.Sign(dic), 0)) != (StageData.GroundLayer)(-1) &&
            StageData.instance.GetBlock(nowPos + new Vector2Int((int)Mathf.Sign(dic), 1)) != (StageData.GroundLayer)(-1))
            return false;
        return true;
    }
    #endregion

    #region[떨어질수있는 블록 검사]
    public bool CanFallBlock(float dic, int n)
    {
        for (int i = 1; i <= n; i++)
        {
            if (StageData.instance.GetBlock(nowPos + new Vector2Int((int)Mathf.Sign(dic), -i)) != (StageData.GroundLayer)(-1))
                return true;
        }
        return false;
    }
    #endregion

    #region[공격처리]
    public virtual void Attack()
    {
        if (Player.instance && Player.instance.damage)
            return;

        bool check = IsAtPlayer(boxCollider2D);
        if (check)
        {
            int dic = transform.position.x < Player.instance.transform.position.x ? +1 : -1;
            Player.instance.PlayerDamage(attackPower, dic);
        }
    }
    #endregion

    #region[점프처리]
    public void Jumping(float x, float y)
    {
        Jumping(new Vector2(x, y));
    }

    public void Jumping(Vector2 vector2)
    {
        rigidbody2D.velocity = Vector2.zero;
        rigidbody2D.AddForce(vector2);
    }
    #endregion

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region[능력치 갱신]
    public virtual void UpdateStats()
    {
        monsterType = MonsterType.Ground;
        maxHp = MonsterManager.instance.monsterData[MonsterManager.FindData(transform.name)].Hp;
        speed = MonsterManager.instance.monsterData[MonsterManager.FindData(transform.name)].Speed;
        attackPower = MonsterManager.instance.monsterData[MonsterManager.FindData(transform.name)].AttackPower;
        jumpPower = MonsterManager.instance.monsterData[MonsterManager.FindData(transform.name)].JumpPower;
    }
    #endregion

    #region[탐색경로 생성]
    void MakeAreaList()
    {
        if (monsterType != MonsterType.Fly && monsterType != MonsterType.Dig)
            return;

        if (reExploreTime >= 1f && Vector2.Distance(nowPos, targetPos) <= AstarRange)
        {
            reExploreTime = 0;
            //탐색경로를 만듬
            TryRoute();
        }
        else
        {
            reExploreTime += Time.deltaTime;
        }

        //if (AreaList != null)
        //    for (int i = 0; i < AreaList.Count - 1; i++)
        //        Debug.DrawLine(AreaList[i], AreaList[i + 1], Color.red, 0, false);
    }

    void TryRoute()
    {
        if (monsterType == MonsterType.Dig || (Exception.IndexOutRange(nowPos, GroundManager.instance.linkArea) && GroundManager.instance.linkArea[nowPos.x, nowPos.y]))
        {
            //탐색경로도 갱신
            List<Vector2> emp = new List<Vector2>();

            if(monsterType == MonsterType.Dig)
            {
                Vector2Int temp = targetPos + new Vector2Int(0, -3);
                for (int i = 0; i < StageData.Dic8.GetLength(0); i++)
                {
                    int ax = targetPos.x + StageData.Dic8[i, 0];
                    int ay = targetPos.y + StageData.Dic8[i, 1];
                    if(StageData.instance.GetBlock(ax,ay) != (StageData.GroundLayer)(-1))
                    {
                        temp = new Vector2Int(ax, ay);
                        break;
                    }
                    ax = targetPos.x + 2 * StageData.Dic8[i, 0];
                    ay = targetPos.y + 2 * StageData.Dic8[i, 1];
                    if (StageData.instance.GetBlock(ax, ay) != (StageData.GroundLayer)(-1))
                    {
                        temp = new Vector2Int(ax, ay);
                        break;
                    }
                }
                emp = Astar(nowPos, temp);

            }
            else
                emp = Astar(nowPos, targetPos);
            if (emp != null)
            {
                if (AreaList == null)
                    AreaList = new List<Vector2>();
                nowPoint = -1;
                AreaList.Clear();
                for (int i = 0; i < emp.Count; i++)
                    AreaList.Add(emp[i]);
            }
            else
                AreaList = emp;


        }
    }
    #endregion

    #region[Astar 알고리즘]
    public List<Vector2> Astar(Vector2Int start, Vector2Int end)
    {
        if (!Exception.IndexOutRange(start, Area) || !Exception.IndexOutRange(end, Area))
            return null;

        //초기화
        explore = false;
        if (Area != null)
            Area.Initialize();
        if (OpenList.Count > 0)
            OpenList.Clear();
        if (CloseList.Count > 0)
            CloseList.Clear();

        //열린리스트에 시작지점 추가
        OpenList.Add(start);

        //탐색시작
        if (monsterType == MonsterType.Fly)
            FlyExplore(end);
        else if (monsterType == MonsterType.Dig)
            DigExplore(end);

        //탐색결과로 길을 그려줌
        if (explore)
            return DrawRoute(end.x, end.y, start);
        else
            return null;

    }
    #endregion

    #region[날아다니는 경우 탐색]
    public void FlyExplore(Vector2Int end)
    {
        int MinV = 0;

        while (OpenList.Count > 0)
        {
            int MaxV = 2147483647;

            int ax = 0;
            int ay = 0;

            int[] way = { -1, -1, -1, -1, -1, -1, -1, -1 };
            //위 아래 왼쪽 오른쪽 왼위 오위 오아 왼아
            int[,] offset = { { 0, 1 }, { 0, -1 }, { -1, 0 }, { 1, 0 }, { -1, 1 }, { 1, 1 }, { 1, -1 }, { -1, -1 } };

            for (int i = 0; i < 8; i++)
            {
                ax = OpenList[MinV].x + offset[i, 0];
                ay = OpenList[MinV].y + offset[i, 1];
                int mintax = (int)Vector2.Distance(new Vector2(ax, ay), end);
                if (Exception.IndexOutRange(ax, ay, Area))
                {
                    if (i < 4)
                    {
                        if (NotPassList(ax, ay))
                        {
                            if (CheckOpenList(ax, ay))
                            {
                                if (Area[ax, ay].w > Area[OpenList[MinV].x, OpenList[MinV].y].w + 10)
                                {
                                    Area[ax, ay].w = Area[OpenList[MinV].x, OpenList[MinV].y].w + 10;
                                    Area[ax, ay].parents = OpenList[MinV];
                                }
                            }
                            else
                            {
                                OpenList.Add(new Vector2Int(ax, ay));
                                Area[ax, ay].w = Area[OpenList[MinV].x, OpenList[MinV].y].w + 10;
                                Area[ax, ay].parents = OpenList[MinV];
                            }
                        }
                    }
                    else
                    {
                        if (NotPassList(ax, ay) && NotPassList(OpenList[MinV].x, ay) && NotPassList(ax, OpenList[MinV].y))
                        {
                            if (CheckOpenList(ax, ay))
                            {
                                if (Area[ax, ay].w > Area[OpenList[MinV].x, OpenList[MinV].y].w + 14)
                                {
                                    Area[ax, ay].w = Area[OpenList[MinV].x, OpenList[MinV].y].w + 10;
                                    Area[ax, ay].parents = OpenList[MinV];
                                }
                            }
                            else
                            {
                                OpenList.Add(new Vector2Int(ax, ay));
                                Area[ax, ay].w = Area[OpenList[MinV].x, OpenList[MinV].y].w + 14;
                                Area[ax, ay].parents = OpenList[MinV];
                            }
                        }
                    }
                }

                if (CheckOpenList(end.x, end.y))
                {
                    explore = true;
                    break;
                }
            }

            if (explore)
                break;

            for (int i = 0; i < OpenList.Count; i++)
            {
                if (i == MinV)
                {
                    CloseList.Add(OpenList[MinV]);
                    OpenList.RemoveAt(i);
                }
            }

            if (OpenList.Count > 0)
            {
                for (int i = 0; i < OpenList.Count; i++)
                {
                    int tax = (int)Vector2.Distance(OpenList[i], end);
                    if (MaxV > tax + Area[OpenList[i].x, OpenList[i].y].w)
                    {
                        MaxV = tax + Area[OpenList[i].x, OpenList[i].y].w;
                        MinV = i;
                    }
                }
            }
        }
    }
    #endregion

    #region[땅속의 경우 탐색]
    public void DigExplore(Vector2Int end)
    {
        int MinV = 0;

        while (OpenList.Count > 0)
        {
            int MaxV = 2147483647;

            int ax = 0;
            int ay = 0;

            int[] way = { -1, -1, -1, -1, -1, -1, -1, -1 };
            //위 아래 왼쪽 오른쪽 왼위 오위 오아 왼아
            int[,] offset = { { 0, 1 }, { 0, -1 }, { -1, 0 }, { 1, 0 }, { -1, 1 }, { 1, 1 }, { 1, -1 }, { -1, -1 } };

            for (int i = 0; i < 8; i++)
            {
                ax = OpenList[MinV].x + offset[i, 0];
                ay = OpenList[MinV].y + offset[i, 1];
                int mintax = (int)Vector2.Distance(new Vector2(ax, ay), end);
                if (Exception.IndexOutRange(ax, ay, Area))
                {
                    if (i < 4)
                    {
                        if (NotPassList(ax, ay))
                        {
                            if (CheckOpenList(ax, ay))
                            {
                                if (Area[ax, ay].w > Area[OpenList[MinV].x, OpenList[MinV].y].w + 10)
                                {
                                    Area[ax, ay].w = Area[OpenList[MinV].x, OpenList[MinV].y].w + 10;
                                    Area[ax, ay].parents = OpenList[MinV];
                                }
                            }
                            else
                            {
                                OpenList.Add(new Vector2Int(ax, ay));
                                Area[ax, ay].w = Area[OpenList[MinV].x, OpenList[MinV].y].w + 10;
                                Area[ax, ay].parents = OpenList[MinV];
                            }
                        }
                    }
                    else
                    {
                        if (NotPassList(ax, ay) && NotPassList(OpenList[MinV].x, ay) && NotPassList(ax, OpenList[MinV].y))
                        {
                            if (CheckOpenList(ax, ay))
                            {
                                if (Area[ax, ay].w > Area[OpenList[MinV].x, OpenList[MinV].y].w + 14)
                                {
                                    Area[ax, ay].w = Area[OpenList[MinV].x, OpenList[MinV].y].w + 10;
                                    Area[ax, ay].parents = OpenList[MinV];
                                }
                            }
                            else
                            {
                                OpenList.Add(new Vector2Int(ax, ay));
                                Area[ax, ay].w = Area[OpenList[MinV].x, OpenList[MinV].y].w + 14;
                                Area[ax, ay].parents = OpenList[MinV];
                            }
                        }
                    }
                }

                if (CheckOpenList(end.x, end.y))
                {
                    explore = true;
                    break;
                }
            }

            if (explore)
                break;

            for (int i = 0; i < OpenList.Count; i++)
            {
                if (i == MinV)
                {
                    CloseList.Add(OpenList[MinV]);
                    OpenList.RemoveAt(i);
                }
            }

            if (OpenList.Count > 0)
            {
                for (int i = 0; i < OpenList.Count; i++)
                {
                    int tax = (int)Vector2.Distance(OpenList[i], end);
                    if (MaxV > tax + Area[OpenList[i].x, OpenList[i].y].w)
                    {
                        MaxV = tax + Area[OpenList[i].x, OpenList[i].y].w;
                        MinV = i;
                    }
                }
            }
        }
    }
    #endregion

    #region[이동불가 부분 조건]
    public bool NotPassList(int x, int y)
    {
       
        if (Vector2.Distance(transform.position, new Vector2(x, y)) > AstarRange)
            return false;

        if (CloseList.Contains(new Vector2Int(x, y)))
            return false;

        if (monsterType == MonsterType.Fly)
            if (StageData.instance.GetBlock(x, y) != (StageData.GroundLayer)(-1))
                return false;

        if (monsterType == MonsterType.Dig)
            if (StageData.instance.GetBlock(x, y) == (StageData.GroundLayer)(-1))
                return false;

        bool hit = IsAtTerrain(boxCollider2D, new Vector2(x + 0.5f, y + 0.5f), false);

        if (hit && monsterType == MonsterType.Fly)
            return false;

        //  if (monsterType == MonsterType.Fly)
        //     return false;

        return true;
    }

    bool NotPassList(Vector2Int v)
    {
        return NotPassList(v.x, v.y);
    }
    #endregion

    #region[열림 리스트인지 검사]
    public bool CheckOpenList(int x, int y)
    {
        return OpenList.Contains(new Vector2Int(x, y));
    }

    bool CheckOpenList(Vector2Int v)
    {
        return CheckOpenList(v.x, v.y);
    }
    #endregion

    #region[길을 그려줌]
    public List<Vector2> DrawRoute(int x, int y, Vector2Int startPos)
    {
        List<Vector2> emp = new List<Vector2>();
        int ax = x;
        int ay = y;

        while (ax != startPos.x || ay != startPos.y)
        {
            emp.Add(new Vector2(ax + 0.5f, ay + 0.5f));
            int bx = ax;
            int by = ay;
            ax = Area[bx, by].parents.x;
            ay = Area[bx, by].parents.y;
        }
        emp.Reverse();

        return emp;
    }
    #endregion

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
}