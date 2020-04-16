﻿using UnityEngine;
using Custom;
using TerrainEngine2D;
using TerrainEngine2D.Lighting;

public class Player : CustomCollider
{
    public static Player instance;

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    int shield = 0;
    [System.NonSerialized] public bool shieldFlag;

    public int baseAttackPower = 5;
    [System.NonSerialized] public int attackPower;

    public float baseAttackSpeed = 1;
    [System.NonSerialized] public float attackSpeed;

    public float baseSpeed;
    [System.NonSerialized] public float speed;

    public float baseJumpPower;
    [System.NonSerialized] public float jumpPower;

    public float baseGravity = 4;
    [System.NonSerialized] public float gravity;

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    protected Vector2 knockback = new Vector2(1500, 1200);   //넉백수치

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    Animator animator;

    SpriteRenderer spriteRenderer;

    new Rigidbody2D rigidbody2D;

    BoxCollider2D headCollider;
    BoxCollider2D bodyCollider;
    BoxCollider2D hangCollider;
    GameObject damageCollider;

    GameObject chickenHead;
    GameObject chickenHeadLight;
    SpriteRenderer chickenHeadSpriteRenderer;

    GameObject shiledBuff;

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    [System.NonSerialized] public bool stop = false;          //플레이어 정지
    [System.NonSerialized] public bool pray = false;          //플레이어 기도
    [System.NonSerialized] public bool canControl = true;    //플레이어 컨트롤

    [System.NonSerialized] public float nowHp = 3;
    [System.NonSerialized] public float maxHp = 3;


    [System.NonSerialized] public bool damage = false;
    [System.NonSerialized] public bool invincibility = true;
    [System.NonSerialized] public bool invincibilityFlag = false;

    int stunTime = -1;          //스턴시간
    int noDamageTime = 60;     //무적시간

    enum moveDic { 경사아래로 = -1, 앞으로 = 0, 경사위로 = 1 };
    int flipX = 0;
    bool runFlag = false;
    float playerMoveDirection; //플레이어 이동 방향
    float runSoundCycle = 0;

    bool pressJump = false;     //점프키 누른여부
    float jumpKeyTime = 0;      //점프키 누른시간
    [System.NonSerialized] public bool jumpHigh;

    float fallTime = 0;         //낙하중인 시간
    float groundFallTime = 0;
    bool hasFeatherShoes;
    bool grounded;
    bool inFluid;

    bool hang;  //매달림 여부
    bool canHang;
    float hangDic = 0;  //매달린방향

    [System.NonSerialized] public bool canAttack;
    [System.NonSerialized] public bool attackTop;   //상단 공격여부
    int combo = 0;
    float comboTime = 0;
    [System.NonSerialized] public bool attackFlag = false;

    [System.NonSerialized] public bool getChicken = true;

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    [System.NonSerialized] public GameObject playerLight;
    [System.NonSerialized] public BlockLightSource playerBlockLightSource;
    Vector2 frontPos = Vector2.zero;

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region[Awake]
    public void Awake()
    {
        instance = this;

        transform.position = new Vector3(transform.position.x, transform.position.y, -35);

        animator = GetComponent<Animator>();

        spriteRenderer = GetComponent<SpriteRenderer>();

        rigidbody2D = GetComponent<Rigidbody2D>();

        Transform checkList;
        checkList = transform.Find("CheckList");
        bodyCollider = checkList.Find("Body").GetComponent<BoxCollider2D>();
        headCollider = checkList.Find("Head").GetComponent<BoxCollider2D>();
        hangCollider = checkList.Find("Hang").GetComponent<BoxCollider2D>();
        damageCollider = checkList.Find("Damage").gameObject;

        chickenHead = transform.Find("ChickenHead").gameObject;
        chickenHeadLight = chickenHead.transform.Find("Light").gameObject;
        chickenHeadSpriteRenderer = chickenHead.GetComponent<SpriteRenderer>();

        shiledBuff = transform.Find("Shield").gameObject;

        playerLight = transform.Find("PlayerLight").Find("BlockLight").gameObject;
        playerBlockLightSource = playerLight.GetComponent<BlockLightSource>();

        nowHp = GameManager.instance.playData.playerNowHp;
        maxHp = GameManager.instance.playData.playerMaxHp;

        if(BuffManager.instance)
        {
            for (int i = 0; i < BuffManager.buffName.Length; i++)
            {
                BuffManager.instance.nowBuffList[BuffManager.buffName[i]].hasBuff = GameManager.instance.playData.playerBuffItemHas[i];
                BuffManager.instance.nowBuffList[BuffManager.buffName[i]].hasNum = GameManager.instance.playData.playerBuffItemNum[i];
                BuffManager.instance.nowBuffList[BuffManager.buffName[i]].time = GameManager.instance.playData.playerBuffItemTime[i];
            }
        }
    }
    #endregion

    #region[Start]
    public void Start()
    {
        //치킨을 주운 상태에 따라서 치킨을 비활성/활성화시킴
        if (Chicken.instance.transform.gameObject.activeSelf != !getChicken)
        {
            Chicken.instance.transform.gameObject.SetActive(!getChicken);
            chickenHead.SetActive(getChicken);
        }

    }
    #endregion

    #region[LateUpdate]
    public void LateUpdate()
    {
        PlayerBuffCheck();
        PlayerAct();
        PlayerAni();
    }
    #endregion

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region[플레이어 행동]
    void PlayerAct()
    {
        if (GameManager.instance.gamePause)
            return;

        //자동으로 작동되는 행동
        PlayerFall();
        runFlag = false;

        //조작가능상태이거나 멈춰있으면
        if (!canControl || stop)
            return;

        //플레이어 조작에 따른 행동
        PlayerUseItem();
        PlayerAttack();
        PlayerGetChicken();
        PlayerRun();
        PlayerJump();
        PlayerHang();
    }
    #endregion

    #region[플레이어 낙하]
    void PlayerFall()
    {
        //아래에 지형이 있는지 검사
        grounded = IsAtTerrain(bodyCollider, new Vector2(0, -0.3f));

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///물에 낙하

        if (AdvancedFluidDynamics.Instance && AdvancedFluidDynamics.Instance.GetFluidBlock((int)transform.position.x, (int)transform.position.y) != null)
        {
            bool fluidCheck = AdvancedFluidDynamics.Instance.GetFluidBlock((int)transform.position.x, (int)transform.position.y).Weight > 0.1f;
            if (inFluid != fluidCheck)
            {
                Color color = AdvancedFluidDynamics.Instance.GetFluidBlock((int)transform.position.x, (int)transform.position.y).Color;
                EffectManager.instance.PlopFluid(transform.position, new Color(color.r, color.g, color.b, 0.2f));

                rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, rigidbody2D.velocity.y / 1.5f);
                inFluid = fluidCheck;
            }

            byte fluidType = AdvancedFluidDynamics.Instance.GetFluidBlock((int)transform.position.x, (int)transform.position.y).Density;
            if (AdvancedFluidDynamics.Instance.GetFluidType(fluidType) && AdvancedFluidDynamics.Instance.GetFluidType(fluidType).Name.Equals("Poison"))
            {
                Vector2 emp = knockback;
                knockback = new Vector2(1500, 500);   //넉백수치
                PlayerDamage(1, Random.Range(0, 100) < 50 ? +1 : -1);
                knockback = emp;
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //점프 이펙트 생성

        //현재 땅이 아닌 상태이지만 현재 땅인경우
        if (!animator.GetBool("IsGround?") && grounded)
        {
            //일정낙하시간이 끝난후 떨어지고 있는경우
            if (rigidbody2D.velocity.y < 0 && fallTime > 0.2f)
            {
                //높은 점프 상태에 따라 점프 이펙트를 생성
                if (animator.GetBool("JumpHigh"))
                    EffectManager.instance.HighLand(new Vector2(transform.position.x, transform.position.y + 0.2f));
                else
                    EffectManager.instance.Land(new Vector2(transform.position.x, transform.position.y + 0.2f));

                if (!hasFeatherShoes)
                    if (groundFallTime > 1 && !inFluid)
                    {
                        Vector2 emp = knockback;
                        knockback = new Vector2(1500, 500);   //넉백수치
                        PlayerDamage(1, Random.Range(0, 100) < 50 ? +1 : -1);
                        knockback = emp;
                    }

                groundFallTime = 0;

            }

            //높은 점프 상태를 초기화
            animator.SetBool("JumpHigh", false);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //위치에 따른 중력값 조절

        //플레이어가 땅에 있을경우
        if (grounded)
        {
            //중력값이 0이 아니고 떨어지고 있는 상태면
            if (rigidbody2D.gravityScale != 0 && rigidbody2D.velocity.y <= 0)
            {
                //플레이어를 멈추고
                rigidbody2D.velocity = Vector2.zero;

                //매달린 
                InitHang();

                groundFallTime = fallTime;

                //낙하시간을 0으로 바꿈
                fallTime = 0;

            }

            if (!hasFeatherShoes && !SceneController.instance.CheckEventMap())
                if (groundFallTime > 1f && !inFluid)
                {
                    Vector2 emp = knockback;
                    knockback = new Vector2(1500, 500);   //넉백수치
                    PlayerDamage(1, Random.Range(0, 100) < 50 ? +1 : -1);
                    knockback = emp;
                }

            if (groundFallTime != 0)
                groundFallTime = 0;

            rigidbody2D.gravityScale = 0;
        }
        //매달린 상태면
        else if (hang)
        {
            //플레이어를 해당 위치에 정지
            rigidbody2D.velocity = Vector2.zero;
            rigidbody2D.gravityScale = 0;
            fallTime = 0;
        }
        //나머지의 경우
        else
        {
            //낙하시간을 더해주고
            fallTime += Time.deltaTime;

            //플레이어를 떨어지게함
            rigidbody2D.gravityScale = gravity;
            if (inFluid)
                rigidbody2D.gravityScale /= 1.5f;
        }
        animator.SetBool("IsGround?", grounded);
    }
    #endregion

    #region[플레이어 매달림]
    void PlayerHang()
    {
        animator.SetBool("Hang", hang);

        //땅에 있으면 검사할 필요가 없기 때문에 retun
        if (grounded)
            return;
        //공격중에 있으면 검사할 필요가 없기 때문에 retun
        if (attackFlag)
            return;

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //매달리기가 가능한 상태이고 떨어지고 있는 시간이 일정 시간이 지나면
        if (/*canHang && */fallTime > 0.2f)
        {
            //누른 방향에 매달릴수있는 지형이 있는지 검사후
            bool hangCheck = IsAtTerrain(hangCollider, new Vector2(playerMoveDirection * 0.3f, 0));

            //매달릴 수 있는 지형이 존재하고 한번 매달린 방향이 아닐경우
            if (hangCheck && hangDic != playerMoveDirection)
                //매달리게 처리
                InitHang(playerMoveDirection, true, false);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////

        //매달려있지만 매달리고 있는 지형이 없을 경우 매달리지 않은 상태로 설정
        if (hang)
        {
            bool hangCheck = IsAtTerrain(hangCollider, new Vector2(hangDic * 0.3f, 0));
            if (!hangCheck)
                InitHang(hangDic);
            groundFallTime = 0;
        }
    }

    #region[매달린 상태 설정]
    void InitHang()
    {
        hangDic = 0;
        hang = false;
        canHang = false;
    }

    void InitHang(float n)
    {
        hangDic = n;
        hang = false;
        canHang = false;
    }

    void InitHang(bool h, bool c)
    {
        hangDic = 0;
        hang = h;
        canHang = c;
    }

    void InitHang(float n, bool h, bool c)
    {
        hangDic = n;
        hang = h;
        canHang = c;
    }
    #endregion

    #endregion

    #region[플레이어 이동]
    void PlayerRun()
    {
        //누른 키에 따라 이동할려는 방향 값을 설정
        if (Input.GetKey(KeyCode.D))
            playerMoveDirection = +1;
        else if (Input.GetKey(KeyCode.A))
            playerMoveDirection = -1;
        else
            playerMoveDirection = 0;

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //매달리는 중이면 이동 하지 않도록 막기 위해 return
        if (hang)
            return;

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //이동방향으로 이미지 방향 설정 
        //이동방향이 없으면 기존 방향으로 설정
        flipX = playerMoveDirection != 0 ? (int)playerMoveDirection : flipX;

        //이동 거리
        float moveDistance = playerMoveDirection * speed;
        //순간적인 이동 거리
        Vector2 moveValue;

        //이동 했을 경우
        if (moveDistance != 0)
        {
            moveDic[] checkArray = new moveDic[3] { moveDic.경사아래로, moveDic.앞으로, moveDic.경사위로 };
            foreach (moveDic move in checkArray)
            {
                //checkArr을 상용해서 이동할 값을 설정
                moveValue = new Vector2(moveDistance, (int)move * Mathf.Abs(moveDistance)) * Time.deltaTime;
                //이동한 위치에 지형이 있는지 검사
                bool nextPosCheck = IsAtTerrain(bodyCollider, moveValue) || IsAtTerrain(headCollider, moveValue);

                //앞으로 이동중이 아니면 땅위인지 검사해야됨
                if (move != moveDic.앞으로 ? grounded : true)
                    //다음위치에 지형이 없으면
                    if (!nextPosCheck)
                    {
                        //앞으로 이동상태로 설정
                        runFlag = true;
                        //앞으로 이동
                        transform.Translate(moveValue);
                        break;
                    }
            }

            if(!runFlag)
            {
                moveValue = new Vector2(moveDistance, 0) * Time.deltaTime;

                bool frontMoveCheck = IsAtTerrainObject(bodyCollider, moveValue);

                if (frontMoveCheck)
                    runFlag = true;
            }
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //걷는중이면
        if (runFlag)
        {
            //일정한 간격으로 걸음소리가 들리도록 시간을잼
            runSoundCycle += Time.deltaTime;
            //일정시간이 지나면
            if(runSoundCycle > 0.3f)
            {
                runSoundCycle = 0;
                Vector2Int pos = new Vector2Int(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y) - 1);
                //밟은 땅의 종류에 따른 소리를 킴
                if (StageData.instance.GetBlock(pos) == StageData.GroundLayer.Dirt)
                    SoundManager.instance.PlayerRunDirt();
                else if (StageData.instance.GetBlock(pos) == StageData.GroundLayer.Stone)
                    SoundManager.instance.PlayerRunStone();
            }
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
    #endregion

    #region[플레이어 점프 및 착지]
    void PlayerJump()
    {
        //땅에 서있거나 매달려 있으면 점프가 가능함
        if (grounded || hang)
        {
            //점프키룰 누름
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //점프력이 약해지지 않도록 순간적인 y가속도값을 0으로 해줌
                rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0);
                //위로 점프하도록 물리적으로 위로 밀어줌
                rigidbody2D.AddForce(new Vector2(0, jumpPower));

                //점프를 누른상태로 처리
                pressJump = true;

                //매달린 상태 설정
                InitHang(hangDic, false, true);
            }
        }

        /////////////////////////////////////////////////////////////////////////////////////////////

        //점프키가 눌렸으면
        if (pressJump)
        {
            float jumpTimeLimit = 0.5f; //점프키를 누르는 최대시간
            if (Input.GetKey(KeyCode.Space))  //점프키를 누르고 있는 시간을 체크
                jumpKeyTime += Time.deltaTime;

            if (jumpKeyTime >= jumpTimeLimit || Input.GetKeyUp(KeyCode.Space)) //점프키 최대 시간을 넘기거나 점프키를 땟을 경우
            {
                //올라가는 중이면 더이상 위로 올라가지 않도록
                if (rigidbody2D.velocity.y > 0)
                    //y가속도를 0으로 만들어줌
                    rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0);

                //점프키 최대시간이 넘긴 점프였으면 
                //높은 점프 애니메이션으로 설정
                animator.SetBool("JumpHigh", (jumpKeyTime >= jumpTimeLimit));

                //다음 점프를 위하여 변수값 초기화
                jumpKeyTime = 0;
                pressJump = false;
            }
        }
    }
    #endregion

    #region[플레이어 공격]
    void PlayerAttack()
    {
        if (!canAttack)
            return;

        //매달린 상태면 공격을 못하게함
        if (hang)
            return;

        /////////////////////////////////////////////////////////////////////////////////////////

        //마우스위치를 바꾸면 콤보 초기화및 공격 위치 변경
        if (attackTop != (MouseManager.instance.mousePos.y > transform.position.y))
        {
            attackTop = !attackTop;
            combo = 0;
        }

        //if(AttackingCheck.aniTime == 0)
         //   attackFlag = false;

        //콤보 제한시간 처리
        comboTime -= Time.deltaTime;
        if (comboTime <= 0)
            combo = 0;

        /////////////////////////////////////////////////////////////////////////////////////////

        //공격처리
        if (Input.GetMouseButton(0) && !attackFlag)
        {
            //공격중이라고 플레그 설정
            attackFlag = true;

            //공격 애니메이션 설정
            animator.SetTrigger("Attack");
            //상단 하단 공격 여부 설정
            animator.SetBool("AttackTop", attackTop);
            //상단 하단은 콤보에 따라 애니메이션이 다르므로 따로 설정
            if (attackTop)
                animator.SetInteger("Combo", combo % 3 + 1);
            else
                animator.SetInteger("Combo", combo % 2 + 1);
            //공격속도를 설정
            animator.SetFloat("AttackSpeed", attackSpeed);

            //공격을 했으니 콤보를 올려줌
            combo += 1;
            //1초안에 공격을 안하면 콤보가 끊김
            comboTime = 1;

            //데미지 판정 작동
            damageCollider.SetActive(true);

            //마우스의 위치에 따라 이미지 방향 바꿈
            if (MouseManager.instance.mousePos.x > transform.position.x)
                flipX = 1;
            else if (MouseManager.instance.mousePos.x < transform.position.x)
                flipX = -1;

        }
    }
    #endregion

    #region[플레이어 치킨관련]
    void PlayerGetChicken()
    {
        //E키로 치킨이 범위안에 있으면 주움
        if (Input.GetKeyDown(KeyCode.E) && !getChicken)
            if (IsAtChicken(bodyCollider))
            {
                Chicken.instance.transform.gameObject.SetActive(false);
                Chicken.instance.jumpFlag = false;
                getChicken = true;
                //닭머리
                chickenHead.SetActive(getChicken);
            }

        //치킨을 주운 상태면 플레이어 좌표로 치킨을 이동
        if (getChicken)
            Chicken.instance.transform.position = new Vector3(transform.position.x, transform.position.y + 2, Chicken.instance.transform.position.z);
    }
    #endregion

    #region[플레이어 아이템 사용]
    public void PlayerUseItem()
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        Monster.useMonsterRadar = ItemManager.instance.HasItemCheck("Monster_Radar");
        TreasureBoxScirpt.useTreasureBoxRadar = ItemManager.instance.HasItemCheck("TreasureBox_Radar");
        TrapScript.useTrapRadar = ItemManager.instance.HasItemCheck("Trap_Radar");

        hasFeatherShoes = ItemManager.instance.HasItemCheck("Feather_Shoes");

        bool lightFlag = (ItemManager.instance.HasItemCheck("Torch") || ItemManager.instance.HasItemCheck("Mine_Helmet"));
        if (playerLight.activeSelf != lightFlag)
            playerLight.SetActive(lightFlag);

        float maxAttackSpeed = 100;
        float maxAttackPower = 100;

        bool reinforce = ItemManager.instance.HasItemCheck("Smart_Light_Pick") || ItemManager.instance.HasItemCheck("Smart_Heavy_Pick") || ItemManager.instance.HasItemCheck("Smart_Advanced_Pick");

        if (ItemManager.instance.HasItemCheck("Light_Pick"))
            maxAttackSpeed = Mathf.Max(maxAttackSpeed,ItemManager.instance.itemData[ItemManager.FindData("Light_Pick")].value0);
        if (ItemManager.instance.HasItemCheck("Heavy_Pick"))
            maxAttackPower = Mathf.Max(maxAttackPower, ItemManager.instance.itemData[ItemManager.FindData("Heavy_Pick")].value0);
        if (ItemManager.instance.HasItemCheck("Advanced_Pick"))
        {
            maxAttackSpeed = Mathf.Max(maxAttackSpeed, ItemManager.instance.itemData[ItemManager.FindData("Advanced_Pick")].value0);
            maxAttackPower = Mathf.Max(maxAttackPower, ItemManager.instance.itemData[ItemManager.FindData("Advanced_Pick")].value1);
        }
        if (ItemManager.instance.HasItemCheck("Smart_Light_Pick"))
            maxAttackSpeed = Mathf.Max(maxAttackSpeed, ItemManager.instance.itemData[ItemManager.FindData("Smart_Light_Pick")].value0);
        if (ItemManager.instance.HasItemCheck("Smart_Heavy_Pick"))
            maxAttackPower = Mathf.Max(maxAttackPower, ItemManager.instance.itemData[ItemManager.FindData("Smart_Heavy_Pick")].value0);
        if (ItemManager.instance.HasItemCheck("Smart_Advanced_Pick"))
        {
            maxAttackSpeed = Mathf.Max(maxAttackSpeed, ItemManager.instance.itemData[ItemManager.FindData("Smart_Advanced_Pick")].value0);
            maxAttackPower = Mathf.Max(maxAttackPower, ItemManager.instance.itemData[ItemManager.FindData("Smart_Advanced_Pick")].value1);
        }

        attackSpeed *= maxAttackSpeed / 100f;
        attackPower = Mathf.FloorToInt((float)(attackPower) * (maxAttackPower / 100f));

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        if (Input.GetMouseButtonDown(1))
        {
            if (ShopScript.instance && ShopScript.instance.onArea)
                return;

            if (AltarScript.instance && AltarScript.instance.onArea)
                return;

            if (FountainScript.instance && FountainScript.instance.onArea)
                return;

            if (ItemManager.instance.HasItemCheck("Coke"))
            {
                ItemManager.instance.UseItem("Coke");
                SoundManager.instance.PlayerGlup();
                nowHp += ItemManager.instance.itemData[ItemManager.FindData("Coke")].value0;
                nowHp = nowHp > maxHp ? maxHp : nowHp;
                EffectManager.instance.HearthEffect();
            }
            else if (ItemManager.instance.HasItemCheck("Beer"))
            {
                ItemManager.instance.UseItem("Beer");
                SoundManager.instance.PlayerGlup();
                maxHp += ItemManager.instance.itemData[ItemManager.FindData("Beer")].value0;
                maxHp = maxHp >= 10 ? 10 : maxHp;
            }
            else if (ItemManager.instance.HasItemCheck("Dynamite"))
            {
                ItemManager.instance.UseItem("Dynamite");
                ObjectManager.instance.Dynamite(transform.position + new Vector3(0, 1, 0));
            }
            else if (ItemManager.instance.HasItemCheck("BoomItem"))
            {
                ItemManager.instance.UseItem("BoomItem");
                Vector2 dic = MouseManager.instance.mousePos - (Vector2)transform.position;
                dic = dic.normalized;
                ObjectManager.instance.Boom(transform.position + new Vector3(0,1,0),dic*3000);
            }
            else if (ItemManager.instance.HasItemCheck("ShopVIpSpecial"))
            {
                ItemManager.instance.UseItem("ShopVIpSpecial");
                GameManager.instance.playData.shopVIP = true;
            }
            else if (ItemManager.instance.HasItemCheck("Bell"))
            {
                SoundManager.instance.PlayerBell();
                if (Mathf.Abs(transform.position.x - Chicken.instance.transform.position.x) < 1)
                {
                    if (Random.Range(0, 100) > 50)
                        Chicken.instance.pattenType = Chicken.Pattern.대기;
                    else
                        Chicken.instance.pattenType = Chicken.Pattern.제자리점프;
                }
                else if (transform.position.x > Chicken.instance.transform.position.x)
                {
                    if (transform.position.y > Chicken.instance.transform.position.y)
                        Chicken.instance.pattenType = Chicken.Pattern.오른쪽점프;
                    else
                        Chicken.instance.pattenType = Chicken.Pattern.오른쪽으로;
                }
                else if (transform.position.x < Chicken.instance.transform.position.x)
                {
                    if (transform.position.y > Chicken.instance.transform.position.y)
                        Chicken.instance.pattenType = Chicken.Pattern.왼쪽점프;
                    else
                        Chicken.instance.pattenType = Chicken.Pattern.왼쪽으로;
                }
            }
            else if (ItemManager.instance.HasItemCheck("Russian_Roulette") && !damage)
            {
                SoundManager.instance.PlayerGlup();
                ItemManager.instance.UseItem("Russian_Roulette");
                if (Random.Range(0, 100) > 50)
                {
                    nowHp += ItemManager.instance.itemData[ItemManager.FindData("Russian_Roulette")].value1;
                    nowHp = nowHp > maxHp ? maxHp : nowHp;
                    EffectManager.instance.HearthEffect();
                }
                else
                {
                    SoundManager.instance.PlayerGun();
                    PlayerDamage(ItemManager.instance.itemData[ItemManager.FindData("Russian_Roulette")].value0, Random.Range(0, 100) > 50 ? +1 : -1);
                }
            }
            else if (ItemManager.instance.HasItemCheck("OldPocket"))
            {
                ItemManager.instance.UseItem("OldPocket");
                int minValue = Mathf.FloorToInt(ItemManager.instance.itemData[ItemManager.FindData("OldPocket")].value0);
                int maxValue = Mathf.FloorToInt(ItemManager.instance.itemData[ItemManager.FindData("OldPocket")].value1);
                GameManager.instance.playerMoney += Random.Range(minValue, maxValue + 1);
            }
            else if (ItemManager.instance.HasItemCheck("RainbowPocket"))
            {
                ItemManager.instance.UseItem("RainbowPocket");
                int data = ItemManager.instance.GetRandomItemAtShop();
                if (data != -1)
                    ItemManager.instance.SpawnItem(transform.position, ItemManager.itemName[data]);
            }
            else if (ItemManager.instance.HasItemCheck("RandomDice"))
            {
                ItemManager.instance.UseItem("RandomDice");
                int minValue = Mathf.FloorToInt(ItemManager.instance.itemData[ItemManager.FindData("RandomDice")].value0);
                int maxValue = Mathf.FloorToInt(ItemManager.instance.itemData[ItemManager.FindData("RandomDice")].value1);
                GameManager.instance.playData.randomDice = Random.Range(minValue, maxValue + 1);
            }
        }
    }
    #endregion

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region[플레이어 데미지 처리]
    public void PlayerDamage(float n, int dic = 1) // n : 데미지 값  // dic : 날아가는 방향 (1 : 오른쪽 , -1 : 왼쪽)
    {
        //데미지를 받은 중에는 데미지를 다시 못 받게 처리
        if (invincibility)
            return;

        //데미지를 받은 중에는 데미지를 다시 못 받게 처리
        if (damage)
            return;

        ////////////////////////////////////////////////////////////////////////////////////////

        //공격상태 취소
        attackFlag = false;

        //기도 중지
        pray = false;

        //컨트롤 잠금
        canControl = false;

        //플레이어 체력감소
        if(shield > 0)
            shield--;
        else
        {
            if(ItemManager.instance.HasItemCheck("Medkit") && nowHp - n <= 0)
                ItemManager.instance.UseItem("Medkit");
            else
            {
                nowHp -= n;
                EffectManager.instance.HearthEffect();
                EffectManager.instance.DamageEffect();
            }
        }

        //닭잡은 상태 취소
        if (getChicken)
        {
            getChicken = false;
            //닭을 플레이어 반대방향으로 넉백
            Chicken.instance.transform.gameObject.SetActive(true);
            Chicken.instance.ChickenJump(knockback.x * -dic,false);
            Chicken.instance.cryTime = -5;
            //닭머리
            chickenHead.SetActive(getChicken);

            EffectManager.instance.ChickenFeather(transform.position, Random.Range(0, 100) < 50);
        }

        //매달린 상태 취소
        InitHang();

        //스턴시간 및 데미지받은 상태로 처리
        stunTime = 60; //대략 0.5초 멈춤
        damage = true; //데미지 받은 상태로 설정

        //소리출력
        SoundManager.instance.PlayerDamage();

        //중력값조절하고 넉백
        rigidbody2D.gravityScale = gravity;
        rigidbody2D.velocity = Vector2.zero;
        rigidbody2D.AddForce(new Vector2(knockback.x * dic, knockback.y));

        //이미지 방향을조절
        flipX = -dic;

        //피격애니메이션으로
        animator.SetTrigger("Damage");
    }
    #endregion

    #region[플레이어 버프 처리]
    void PlayerBuffCheck()
    {
        for(int i = 0; i < BuffManager.buffName.Length; i++)
        {
            switch(BuffManager.buffName[i])
            {
                case "Shield":
                    if(!shieldFlag)
                    {
                        shield += BuffManager.instance.nowBuffList[BuffManager.buffName[i]].hasNum * BuffManager.instance.buffData[i].value;
                        shieldFlag = true;
                    }
                    shiledBuff.SetActive(shield > 0);
                    break;
                case "Power":
                    attackPower = baseAttackPower + BuffManager.instance.nowBuffList[BuffManager.buffName[i]].hasNum * BuffManager.instance.buffData[i].value;
                    break;
                case "AttackSpeed":
                    attackSpeed = baseAttackSpeed + 0.1f * BuffManager.instance.nowBuffList[BuffManager.buffName[i]].hasNum * BuffManager.instance.buffData[i].value;
                    break;
                case "Speed":
                    speed = baseSpeed + BuffManager.instance.nowBuffList[BuffManager.buffName[i]].hasNum * BuffManager.instance.buffData[i].value;
                    break;
                case "Luminous":
                    chickenHeadLight.SetActive(BuffManager.instance.nowBuffList[BuffManager.buffName[i]].hasBuff);
                    break;
                case "Space":
                    switch (BuffManager.instance.nowBuffList[BuffManager.buffName[i]].hasNum * BuffManager.instance.buffData[i].value)
                    {
                        case 0:
                            GameManager.instance.passiveSlotAct[2] = false;
                            GameManager.instance.passiveSlotAct[3] = false;
                            GameManager.instance.passiveSlotAct[4] = false;
                            break;
                        case 1:
                            GameManager.instance.passiveSlotAct[2] = true;
                            GameManager.instance.passiveSlotAct[3] = false;
                            GameManager.instance.passiveSlotAct[4] = false;
                            break;
                        case 2:
                            GameManager.instance.passiveSlotAct[2] = true;
                            GameManager.instance.passiveSlotAct[3] = true;
                            GameManager.instance.passiveSlotAct[4] = false;
                            break;
                        default:
                        case 3:
                            GameManager.instance.passiveSlotAct[2] = true;
                            GameManager.instance.passiveSlotAct[3] = true;
                            GameManager.instance.passiveSlotAct[4] = true;
                            break;
                    }
                    break;
            }
        }
        jumpPower = baseJumpPower;
        gravity = baseGravity;

    }
    #endregion

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region[플레이어 애니메이션]
    void PlayerAni()
    {
        if (GameManager.instance.gamePause)
            return;
        PlayerFlipX(flipX);
        PlayerDamageTime();

        //애니메이션설정
        if (runFlag)
            animator.SetBool("Run", true);
        else
            animator.SetBool("Run", false);

        animator.SetBool("Pray", pray   );
    }
    #endregion

    #region[플레이어 보는 방향]
    void PlayerFlipX()
    {
        int Direction; //플레이어 시선 방향
        if (playerMoveDirection > 0)
            Direction = +1;
        else if (playerMoveDirection < 0)
            Direction = -1;
        else
            Direction = transform.localScale.x > 0 ? +1 : -1;

        transform.localScale = new Vector3(Direction * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }

    void PlayerFlipX(int dic)
    {
        int Direction; //플레이어 시선 방향
        if (dic > 0)
            Direction = +1;
        else if (dic < 0)
            Direction = -1;
        else
            Direction = transform.localScale.x > 0 ? +1 : -1;

        transform.localScale = new Vector3(Direction * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }
    #endregion

    #region[플레이어 데미지 시간]
    void PlayerDamageTime()
    {
        //게임시작한지 5초동안은 무적처리
        if (GameManager.instance.stageTime >= 3.5f && !invincibilityFlag)
        {
            invincibilityFlag = true;
            invincibility = false;
        }

        stunTime -= 1;
        if (stunTime < -noDamageTime)
            damage = false;

        if (stunTime == 0)
            canControl = true;

        ///////////////////////////////////////////////////////////////////////////////////////////

        //깜빡이는 효과 처리
        if (Mathf.Abs(stunTime) % 45 < 20 && damage)
            spriteRenderer.enabled = false;
        else
            spriteRenderer.enabled = true;
        chickenHeadSpriteRenderer.enabled = spriteRenderer.enabled;
    }
    #endregion

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
}