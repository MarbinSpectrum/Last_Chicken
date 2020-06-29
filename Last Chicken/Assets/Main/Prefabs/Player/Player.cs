using System.Collections.Generic;
using TerrainEngine2D;
using TerrainEngine2D.Lighting;
using UnityEngine;

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

    [System.NonSerialized] public Animator animator;

    SpriteRenderer spriteRenderer;

    [System.NonSerialized] public new Rigidbody2D rigidbody2D;

    BoxCollider2D headCollider;
    BoxCollider2D bodyCollider;
    BoxCollider2D hangCollider;
    GameObject damageCollider;

    GameObject chickenHead;
    GameObject chickenHeadLight;
    SpriteRenderer chickenHeadSpriteRenderer;

    GameObject dizzyStar;

    GameObject shiledBuff;
    SpriteRenderer shiledSprite;

    GameObject effectObject;

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    [System.NonSerialized] public bool stop = false;          //플레이어 정지
    [System.NonSerialized] public bool pray = false;          //플레이어 기도
    [System.NonSerialized] public bool canControl = true;    //플레이어 컨트롤

    [System.NonSerialized] public bool notDamage = false;
    [System.NonSerialized] public float nowHp = 3;
    [System.NonSerialized] public float maxHp = 3;


    [System.NonSerialized] public bool damage = false;
    [System.NonSerialized] public bool invincibility = true;
    [System.NonSerialized] public bool invincibilityFlag = false;

    [System.NonSerialized] public float stunTime = -1;
    int damagaAlphaTime = 0;
    float canControllTime = 0.7f;  //스턴시간
    int noDamageTime = 4;     //무적시간

    enum moveDic { 경사아래로 = -1, 앞으로 = 0, 경사위로 = 1 };
    int flipX = 0;
    float slipperyValue = 0.8f;
    bool runFlag = false;
    float playerMoveDirection; //플레이어 이동 방향
    float runSoundCycle = 0;

    bool pressJump = false;     //점프키 누른여부
    float jumpKeyTime = 0;      //점프키 누른시간
    [System.NonSerialized] public bool jumpHigh;

    float fallTime = 0;         //낙하중인 시간
    [System.NonSerialized] public float groundFallTime = 0;
    bool hasFeatherShoes;
    [System.NonSerialized] public bool notFallDamage = false;
    [System.NonSerialized] public bool grounded;
    float groundedTime = 0;
    [System.NonSerialized] public bool inFluid;
    bool[] effectFluid = new bool[5];
    Color[] lastFluidColor = new Color[5];

    [System.NonSerialized] public float bubbleTime;
    bool slip = true;

    bool highJump = true;

    [System.NonSerialized] public bool bat_Hate_Light;
    [System.NonSerialized] public bool chicken_Easy_Catch;

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    bool playerIce = false;
    [System.NonSerialized] public float playerHotTime = 0;

    bool hang;  //매달림 여부
    bool canHang;
    float hangDic = 0;  //매달린방향

    [System.NonSerialized] public int pickLevel;
    [System.NonSerialized] public bool canAttack;
    [System.NonSerialized] public bool attackTop;   //상단 공격여부
    int combo = 0;
    float comboTime = 0;
    [System.NonSerialized] public bool attackFlag = false;

    [System.NonSerialized] public bool getChicken = true;
    [System.NonSerialized] public LineRenderer getChickenCircle;

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    [System.NonSerialized] public GameObject torchLight;
    [System.NonSerialized] public List<BlockLightSource> torchLightSourceList = new List<BlockLightSource>();
    [System.NonSerialized] public GameObject mineHelmetLight;
    [System.NonSerialized] public List<BlockLightSource> mineHelmetLighSourceList = new List<BlockLightSource>();
    [System.NonSerialized] public BlockLightSource playerBlockLightSource;
    Vector2 frontPos = Vector2.zero;

    [System.NonSerialized] public GameObject playerUmbrella;

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
        shiledSprite = shiledBuff.transform.Find("Img").GetComponent<SpriteRenderer>();

        dizzyStar = transform.Find("DizzyStars").gameObject;

        effectObject = transform.Find("Effect").gameObject;

        playerBlockLightSource = transform.Find("Light").Find("PlayerLight").Find("BlockLight").GetComponent<BlockLightSource>();
        torchLight = transform.Find("Light").Find("TorchLight").gameObject;
        for (int i = 0; i < torchLight.transform.childCount; i++)
            torchLightSourceList.Add(torchLight.transform.GetChild(i).GetChild(0).GetComponent<BlockLightSource>());
        mineHelmetLight = transform.Find("Light").Find("MineHelmet").gameObject;
        for (int i = 0; i < mineHelmetLight.transform.childCount; i++)
            mineHelmetLighSourceList.Add(mineHelmetLight.transform.GetChild(i).GetChild(0).GetComponent<BlockLightSource>());
        playerUmbrella = transform.Find("Umbrella").gameObject;

        getChickenCircle = transform.Find("GetChickenCircle").GetComponent<LineRenderer>();
        List<Vector3> createAngleList = new List<Vector3>();
        for(int i = 0; i < 37; i++)
        {
            //if (i < 2 || i > 34)
            //    continue;
            Vector3 angleTemp = new Vector3(0, 4,-180);
            angleTemp = Quaternion.Euler(0, 0, 10 * i) * angleTemp;
            createAngleList.Add(angleTemp);
        }
        Vector3[] crateAngleArray = createAngleList.ToArray();
        getChickenCircle.positionCount = createAngleList.Count;
        getChickenCircle.SetPositions(crateAngleArray);

        nowHp = GameManager.instance.playData.playerNowHp;
        maxHp = GameManager.instance.playData.playerMaxHp;

        if (BuffManager.instance)
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
        if (Chicken.instance && Chicken.instance.transform.gameObject.activeSelf != !getChicken)
        {
            Chicken.instance.transform.gameObject.SetActive(!getChicken);
            chickenHead.SetActive(getChicken);
        }

    }
    #endregion

    #region[LateUpdate]
    public void LateUpdate()
    {
        PlayerLightColor();
        PlayerStateCheck();
        PlayerBuffCheck();
        PlayerReinforceCheck();
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
        PlayerInFluidAct();
        runFlag = false;
        chickenHead.SetActive(getChicken);
        animator.SetBool("Hang", hang);


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

    #region[플레이어 상태 체크]
    public void PlayerStateCheck()
    {

        if (playerHotTime > 0)
        {
            playerHotTime -= Time.deltaTime;
            if (UIManager.instance.playerIceStateAni.GetCurrentAnimatorStateInfo(0).IsName("Stage"))
                UIManager.instance.playerIceStateAni.SetTrigger("Exit");
        }

        playerIce = UIManager.instance.playerIceStateAni.GetCurrentAnimatorStateInfo(0).IsName("Stage");
    }
    #endregion

    #region[플레이어 낙하 및 수중낙하]
    void PlayerFall()
    {
        //아래에 지형이 있는지 검사
        grounded = IsAtTerrain(bodyCollider, new Vector2(0, -0.3f));

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///물에 낙하

        #region[물속인지 검사]
        if (AdvancedFluidDynamics.Instance && AdvancedFluidDynamics.Instance.GetFluidBlock((int)transform.position.x, (int)transform.position.y + 2) != null)
        {
            bool fluidCheck = AdvancedFluidDynamics.Instance.GetFluidBlock((int)transform.position.x, (int)transform.position.y + 2).Weight > 0.1f;

            if (inFluid != fluidCheck)
                inFluid = fluidCheck;
        }
        #endregion

        #region[독에 빠졌는지 검사]
        if (AdvancedFluidDynamics.Instance && AdvancedFluidDynamics.Instance.GetFluidBlock((int)transform.position.x, (int)transform.position.y) != null)
        {
            bool fluidCheck = AdvancedFluidDynamics.Instance.GetFluidBlock((int)transform.position.x, (int)transform.position.y).Weight > 0.1f;
            byte fluidType = AdvancedFluidDynamics.Instance.GetFluidBlock((int)transform.position.x, (int)transform.position.y).Density;
            if (fluidCheck && AdvancedFluidDynamics.Instance.GetFluidType(fluidType) && AdvancedFluidDynamics.Instance.GetFluidType(fluidType).Name.Equals("Poison"))
            {
                Vector2 emp = knockback;
                knockback = new Vector2(1500, 500);   //넉백수치
                PlayerDamage(1, UnityEngine.Random.Range(0, 100) < 50 ? +1 : -1);
                knockback = emp;
            }
        }
        #endregion

        #region[물에 들어갔을시 이펙트]
        for (int i = 1; i < 5; i++)
        {
            if (AdvancedFluidDynamics.Instance && AdvancedFluidDynamics.Instance.GetFluidBlock((int)transform.position.x, (int)transform.position.y + i) != null)
            {
                bool fluidCheck = AdvancedFluidDynamics.Instance.GetFluidBlock((int)transform.position.x, (int)transform.position.y + i).Weight > 0.1f;

                if (effectFluid[i] != fluidCheck)
                {
                    Color color = AdvancedFluidDynamics.Instance.GetFluidBlock((int)transform.position.x, (int)transform.position.y + i).Color;

                    Vector2 upPlopForce = new Vector2(0, +10 - i * 1 + (rigidbody2D.velocity.y < 0 ? (int)(Mathf.Abs(rigidbody2D.velocity.y / 2)) : 0));
                    int upPlopNum = 30 - i * 5 + (rigidbody2D.velocity.y < 0 ? (int)(Mathf.Abs(rigidbody2D.velocity.y / 2)) : 0);

                    //물에 들어갈때
                    if (fluidCheck)
                    {
                        lastFluidColor[i] = color;
                        if(i == 1)
                        {
                            SoundManager.instance.PlayerSplash();
                            if (rigidbody2D.velocity.y < -5)
                                EffectManager.instance.PlopFluid(transform.position + new Vector3(0, i, 0), new Vector2(0, -20), new Color(0.2f, 0.2f, 0.2f, 0.7f), 50);
                        }
                        else
                        {
                            SoundManager.instance.PlayerSplashSmall();
                        }
                    }
                    if (rigidbody2D.velocity.y < -5)
                        EffectManager.instance.PlopFluid(transform.position + new Vector3(0, i, 0), upPlopForce, new Color(lastFluidColor[i].r, lastFluidColor[i].g, lastFluidColor[i].b, 0.2f), upPlopNum);

                    effectFluid[i] = fluidCheck;
                }
            }
        }
        #endregion

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
                if(!inFluid)
                    SoundManager.instance.PlayerLand();
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

                //낙하시간을 0으로 바꿈
                fallTime = 0;

                //우산을 사용중이면 우산을 취소함
                playerUmbrella.SetActive(false);
            }

            if (!notFallDamage && !hasFeatherShoes && !SceneController.instance.CheckEventMap())
                if (groundFallTime > 0.75f && !inFluid)
                {
                    Vector2 emp = knockback;
                    knockback = new Vector2(1500, 500);   //넉백수치
                    PlayerDamage(1, UnityEngine.Random.Range(0, 100) < 50 ? +1 : -1);
                    knockback = emp;
                    StartCoroutine(GroundManager.instance.BreakIceProcess((int)transform.position.x, (int)transform.position.y - 2));

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
            if (rigidbody2D.velocity.y < 0)
                groundFallTime += Time.deltaTime;

            //플레이어를 떨어지게함
            rigidbody2D.gravityScale = gravity;
            if (inFluid)
                rigidbody2D.gravityScale /= 1.5f;
            //우산때문에 매우 천천히 떨어짐
            if (playerUmbrella.activeSelf)
            {
                groundFallTime = 0;
                rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, Mathf.Max(-2, rigidbody2D.velocity.y));
            }
        }
        if (grounded)
            groundedTime += Time.deltaTime;
        else
            groundedTime = 0;

        animator.SetBool("IsGround?", grounded /*&& rigidbody2D.velocity.y <= 0f*/ && groundedTime > 0.1f);
    }
    #endregion

    #region[플레이어 물속에 있을때]
    public void PlayerInFluidAct()
    {
        if (!inFluid)
            return;

        if(bubbleTime > 1)
        {
            bubbleTime = 0;
            int dic = transform.localScale.x < 0 ? -1 : 1;
            if(dic == -1)
                EffectManager.instance.CreateBubbleEffect(transform.position, new Vector4(-0.75f, -0.25f, 2, 3), 5, 20);
            else
                EffectManager.instance.CreateBubbleEffect(transform.position, new Vector4(+0.25f, +0.75f, 2, 3), 5, 20);
        }
        bubbleTime += Time.deltaTime;

    }
    #endregion

    #region[플레이어 매달림]
    void PlayerHang()
    {
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
            if (hangCheck /*&& hangDic != playerMoveDirection*/)
            {
                if(!inFluid)
                    SoundManager.instance.AttackStone();
                //매달리게 처리
                InitHang(playerMoveDirection, true, false);
            }
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
        {
            Vector2Int pos = new Vector2Int(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y) - 1);
            //밟은 땅의 종류에 따른 소리를 킴
            int slipperyGround = (1 << (int)StageData.GroundLayer.Ice);

            if (slip && (1 << (int)StageData.instance.GetBlock(pos) & slipperyGround) != 0 && !CaveManager.inCave)
            {
                if (Mathf.Abs(playerMoveDirection) < 0.1f)
                    playerMoveDirection = 0;
                else if (playerMoveDirection > 0)
                    playerMoveDirection -= Time.deltaTime * slipperyValue;
                else if (playerMoveDirection < 0)
                    playerMoveDirection += Time.deltaTime * slipperyValue;
            }
            else
            {
                playerMoveDirection = 0;
            }
        }

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
                int dirtSoundMask = 
                    (1 << (int)StageData.GroundLayer.Dirt) | 
                    (1 << (int)StageData.GroundLayer.Grass);
                int stoneSoundMask =
                    (1 << (int)StageData.GroundLayer.Stone) |
                    (1 << (int)StageData.GroundLayer.Copper) |
                    (1 << (int)StageData.GroundLayer.Iron) |
                    (1 << (int)StageData.GroundLayer.Silver) |
                    (1 << (int)StageData.GroundLayer.Gold);

                if(!inFluid)
                {

                    if ((1 << (int)StageData.instance.GetBlock(pos) & dirtSoundMask) != 0)
                        SoundManager.instance.PlayerRunDirt();
                    else if ((1 << (int)StageData.instance.GetBlock(pos) & stoneSoundMask) != 0)
                        SoundManager.instance.PlayerRunStone();
                }
            }
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
    #endregion

    #region[플레이어 점프]
    void PlayerJump()
    {
        //땅에 서있거나 매달려 있으면 점프가 가능함
        if (grounded || hang)
        {
            bool downFlag = false;
            if (hang && Input.GetKey(KeyCode.S))
                downFlag = true;

            //점프키룰 누름
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //점프사운드
                if (!inFluid)
                    SoundManager.instance.PlayerJump();
                //점프력이 약해지지 않도록 순간적인 y가속도값을 0으로 해줌
                rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0);
                //위로 점프하도록 물리적으로 위로 밀어줌
                if(!downFlag)
                {
                    float newjumpPower = jumpPower;
                    newjumpPower *= highJump ? (100 + ItemManager.instance.itemData[ItemManager.FindData("Light_Feather")].value0) / 100f : 1;
                    rigidbody2D.AddForce(new Vector2(0, newjumpPower));
                }

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
            if (inFluid)
                EffectManager.instance.CreateBubbleEffect(transform.position, new Vector4(-2, 2, -1, 1), 20, 20);

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
            if (IsAtChicken(bodyCollider) || 
                (chicken_Easy_Catch && Vector2.Distance(new Vector2(transform.position.x, transform.position.y + 2), Chicken.instance.transform.position) < 4))
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

        getChickenCircle.gameObject.SetActive(chicken_Easy_Catch && !getChicken);
        getChickenCircle.transform.localScale = new Vector3(transform.localScale.x, 1, 1);
    }
    #endregion

    #region[플레이어 아이템 사용]
    public void PlayerUseItem()
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        Monster.useMonsterRadar = ItemManager.instance.CanUsePassiveItem("Monster_Radar");
        TreasureBoxScirpt.useTreasureBoxRadar = ItemManager.instance.CanUsePassiveItem("TreasureBox_Radar");
        TrapScript.useTrapRadar = ItemManager.instance.CanUsePassiveItem("Trap_Radar");

        hasFeatherShoes = ItemManager.instance.CanUsePassiveItem("Feather_Shoes");

        bat_Hate_Light = (ItemManager.instance.CanUsePassiveItem("Torch") || ItemManager.instance.CanUsePassiveItem("Mine_Helmet"));

        float maxAttackSpeed = 100;
        float maxAttackPower = 100;

        if (ItemManager.instance.CanUsePassiveItem("Hammer"))
        {
            maxAttackSpeed -= ItemManager.instance.itemData[ItemManager.FindData("Hammer")].value1;
            maxAttackPower += ItemManager.instance.itemData[ItemManager.FindData("Hammer")].value0;
        }

        attackSpeed *= maxAttackSpeed / 100f;
        attackPower = Mathf.FloorToInt((float)(attackPower) * (maxAttackPower / 100f));

        slip = !ItemManager.instance.CanUsePassiveItem("Crampons");

        highJump = ItemManager.instance.CanUsePassiveItem("Light_Feather");

        chicken_Easy_Catch = ItemManager.instance.CanUsePassiveItem("Smart_Gloves");

        if (ItemManager.instance.CanUsePassiveItem("Thermos"))
            playerHotTime = Mathf.Max(playerHotTime, 2);

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        if (Input.GetMouseButtonDown(1))
        {
            if (CaveManager.inCave)
                return;

            if (ShopScript.instance && ShopScript.instance.onArea)
                return;

            if (AltarScript.instance && AltarScript.instance.onArea)
                return;

            if (FountainScript.instance && FountainScript.instance.onArea)
                return;

            if (MovingShop.instance && MovingShop.instance.onArea)
                return;

            if (Smithy.instance && Smithy.instance.onArea)
                return;

            if (ItemManager.instance.CanUseActiveItem("Dynamite"))
            {
                ItemManager.instance.UseItem("Dynamite");
                ObjectManager.instance.Dynamite(transform.position + new Vector3(0, 1, 0));
            }
            else if (ItemManager.instance.CanUseActiveItem("BoomItem"))
            {
                ItemManager.instance.UseItem("BoomItem");
                Vector2 dic = MouseManager.instance.mousePos - (Vector2)transform.position;
                dic = dic.normalized;
                ObjectManager.instance.Boom(transform.position + new Vector3(0,1,0),dic*3000);
            }
            else if (ItemManager.instance.CanUseActiveItem("Splash_Pick"))
            {
                ItemManager.instance.UseItem("Splash_Pick");
                Vector2 dic = MouseManager.instance.mousePos - (Vector2)transform.position;
                dic = dic.normalized;
                ObjectManager.instance.SplashAxe(transform.position + new Vector3(0, 1, 0), dic * 3000);
            }
            else if (ItemManager.instance.CanUseActiveItem("Bell"))
            {
                ItemManager.instance.UseItem("Bell");
                Chicken.instance.orderPos = transform.position;
                Chicken.instance.orderTime = 4;
                SoundManager.instance.PlayerBell();          
            }
            else if (ItemManager.instance.CanUseActiveItem("Umbrella") && !grounded)
            {
                ItemManager.instance.UseItem("Umbrella");
                playerUmbrella.SetActive(true);
            }
        }
    }

    public void ActItem(string name)
    {
        if (name.Equals("Russian_Roulette"))
        {
            SoundManager.instance.PlayerGlup();
            if (UnityEngine.Random.Range(0, 100) > 50)
            {
                nowHp += ItemManager.instance.itemData[ItemManager.FindData("Russian_Roulette")].value1;
                nowHp = nowHp > maxHp ? maxHp : nowHp;
                EffectManager.instance.HearthEffect();
            }
            else
            {
                SoundManager.instance.PlayerGun();
                PlayerDamage(ItemManager.instance.itemData[ItemManager.FindData("Russian_Roulette")].value0, UnityEngine.Random.Range(0, 100) > 50 ? +1 : -1);
            }
        }
        else if (name.Equals("OldPocket"))
        {
            int minValue = Mathf.FloorToInt(ItemManager.instance.itemData[ItemManager.FindData("OldPocket")].value0);
            int maxValue = Mathf.FloorToInt(ItemManager.instance.itemData[ItemManager.FindData("OldPocket")].value1);
            GameManager.instance.playerMoney += UnityEngine.Random.Range(minValue, maxValue + 1);
            SoundManager.instance.PlayerMoney();
        }
        else if (name.Equals("RainbowPocket"))
        {
            Random.InitState((int)Time.time * Random.Range(0, 100));
            float randomValue = Random.Range(0, 100);

            int data = ItemManager.instance.GetRandomItemAtTreasureBox();
            if (data != -1 && randomValue > 5)
                ItemManager.instance.SpawnItem(transform.position, ItemManager.itemName[data]);
            else
            {
                GameManager.instance.playerMoney += 3000;
                SoundManager.instance.PlayerMoney();
            }
        }
        else if (name.Equals("RandomDice"))
        {
            int minValue = Mathf.FloorToInt(ItemManager.instance.itemData[ItemManager.FindData("RandomDice")].value0);
            int maxValue = Mathf.FloorToInt(ItemManager.instance.itemData[ItemManager.FindData("RandomDice")].value1);
            GameManager.instance.playData.randomDice = UnityEngine.Random.Range(minValue, maxValue + 1);
        }
        else if (name.Equals("ShopVIpSpecial"))
        {
            GameManager.instance.playData.shopVIP = true;
        }
        else if (name.Equals("Coke"))
        {
            SoundManager.instance.PlayerGlup();
            nowHp += ItemManager.instance.itemData[ItemManager.FindData("Coke")].value0;
            nowHp = nowHp > maxHp ? maxHp : nowHp;
            EffectManager.instance.HearthEffect();
        }
        else if (name.Equals("Beer"))
        {
            SoundManager.instance.PlayerGlup();
            nowHp += ItemManager.instance.itemData[ItemManager.FindData("Beer")].value0;
            nowHp = nowHp > maxHp ? maxHp : nowHp;
            EffectManager.instance.HearthEffect();
        }
        else if (name.Equals("Coffee"))
        {
            SoundManager.instance.PlayerGlup();
            nowHp += ItemManager.instance.itemData[ItemManager.FindData("Coffee")].value0;
            nowHp = nowHp > maxHp ? maxHp : nowHp;
            EffectManager.instance.HearthEffect();

            playerHotTime = ItemManager.instance.itemData[ItemManager.FindData("Coffee")].value1;
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
        //무적처리
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

        playerUmbrella.SetActive(false);

        //상점사용 중지
        if (ShopScript.instance)
            ShopScript.instance.thisUse = false;

        //플레이어 체력감소
        if (shield > 0)
            shield--;
        else
        {
            if (!notDamage)
            {
                nowHp -= n;
                EffectManager.instance.HearthEffect();
            }
            EffectManager.instance.DamageEffect();
        }

        if(nowHp <= 0)
            animator.SetBool("Dead", nowHp <= 0);

        //닭잡은 상태 취소
        if (getChicken)
        {
            getChicken = false;
            //닭을 플레이어 반대방향으로 넉백
            Chicken.instance.transform.gameObject.SetActive(true);
            if(!ItemManager.instance.CanUsePassiveItem("Rope"))
                Chicken.instance.ChickenJump(knockback.x * -dic,false);
            else
            {
                Chicken.instance.orderTime = 0;
                Chicken.instance.chickenRope.gameObject.SetActive(true);
                Chicken.instance.ChickenJump(0, false);
                Chicken.instance.patternTime = 3;
                Chicken.instance.pattenType = Chicken.Pattern.대기;
            }

            Chicken.instance.cryTime = -5;

            EffectManager.instance.ChickenFeather(transform.position, UnityEngine.Random.Range(0, 100) < 50);
        }

        //매달린 상태 취소
        InitHang();

        //스턴시간 및 데미지받은 상태로 처리
        stunTime = canControllTime;
        damagaAlphaTime = 0;
        damage = true; //데미지 받은 상태로 설정

        //소리출력
        if(nowHp > 0)
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
                    shiledSprite.color = new Color(1, 1, 1, shield * 0.3f);
                    shiledBuff.transform.localScale = new Vector3(transform.localScale.x, 1, 1);
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
            }
        }
        jumpPower = baseJumpPower;
        gravity = baseGravity;

        if (playerIce)
        {
            speed *= 0.75f;
            attackSpeed *= 0.75f;
        }

    }
    #endregion

    #region[플레이어 강화 처리]
    void PlayerReinforceCheck()
    {
        pickLevel = GameManager.instance.playData.pickLevel;

        switch(pickLevel)
        {
            case 1:
                attackPower += 3;
                break;
            case 2:
                attackPower += 3;
                attackSpeed += 0.25f;
                break;
            case 3:
                attackPower += 6;
                attackSpeed += 0.5f;
                break;
            case 4:
                attackPower += 9;
                attackSpeed += 1f;
                break;
        }
    }
    #endregion

    #region[스테이지따른 플레이어 광원색 조정]
    private void PlayerLightColor()
    {
        Color color = new Color(255 / 255f, 180 / 255f, 55 / 255f);
        if (SceneController.instance.nowScene.Contains("Stage01"))
            color = new Color(255 / 255f, 180 / 255f, 55 / 255f);
        else if (SceneController.instance.nowScene.Contains("Stage02") || SceneController.instance.nowScene.Contains("Igloo"))
            color = new Color(204 / 255f, 204 / 255f, 255 / 255f);
        playerBlockLightSource.LightColor = color;
        for (int i = 0; i < mineHelmetLighSourceList.Count; i++)
            mineHelmetLighSourceList[i].LightColor = color;
        for (int i = 0; i < torchLightSourceList.Count; i++)
            torchLightSourceList[i].LightColor = color;
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

        //얼어서 플레이어가 파란색으로 보임
        if (playerIce)
            spriteRenderer.color = new Color(88 / 255f, 91/255f, 1);
        else if(playerHotTime > 0)
            spriteRenderer.color = new Color(255/255f, 198/255f, 198/255f);
        else
            spriteRenderer.color = new Color(1, 1, 1);

        //얼어서 플레이어가 느려짐
        if (playerIce)
            animator.speed = 0.5f;
        else
            animator.speed = 1;

        //공격중에는 effect레이어 수정
        if (attackFlag)
            effectObject.layer = LayerMask.NameToLayer("PostProcess");
        else
            effectObject.layer = LayerMask.NameToLayer("Default");

        //플레이어 방향설정
        PlayerFlipX(flipX);

        //플레이어의 데미지 시간에 따른 애니메이션
        PlayerDamageTime();

        bool GetRunKey = (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A));
        //애니메이션설정
        if (runFlag && GetRunKey)
            animator.SetBool("Run", true);
        else
            animator.SetBool("Run", false);

        animator.SetBool("Pray", pray);

        animator.SetBool("Hang", hang);

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

    #region[플레이어 데미지 시간에 따른 애니메이션]
    void PlayerDamageTime()
    {
        if (nowHp <= 0)
            return;
        dizzyStar.SetActive(stunTime > 0);
        //게임시작한지 5초동안은 무적처리
        if (GameManager.instance.stageTime >= 3.5f && !invincibilityFlag)
        {
            invincibilityFlag = true;
            invincibility = false;
        }

        stunTime -= Time.deltaTime;
        if (stunTime < -noDamageTime)
            damage = false;

        if (!canControl && stunTime < 0 && damage)
        {
            canControl = true;
            Chicken.instance.chickenRope.gameObject.SetActive(false);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////

        damagaAlphaTime++;
        //깜빡이는 효과 처리
        if (Mathf.Abs(damagaAlphaTime) % 90 < 45 && damage)
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