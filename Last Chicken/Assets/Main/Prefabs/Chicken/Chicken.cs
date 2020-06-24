using UnityEngine;

public class Chicken : CustomCollider
{
    public static Chicken instance;

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public float baseSpeed;
    float speed;

    public float baseJumpPower;
    float jumpPower;

    public float baseGravity = 4;
    float gravity;

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    [System.NonSerialized] public float cryTime = 3;

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public enum Pattern { 왼쪽으로, 왼쪽점프, 대기, 제자리점프, 오른쪽으로, 오른쪽점프 };
    public Pattern pattenType = Pattern.대기;
    [System.NonSerialized] public float patternTime = 3;
    [System.NonSerialized] public float orderTime = 0;
    [System.NonSerialized] public Vector2 orderPos;

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    enum MoveDic { 경사아래로 = -1, 앞으로 = 0, 경사위로 = 1 };
    bool moveFlag;

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    bool grounded;

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    [System.NonSerialized] public bool jumpFlag;

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    int flipX = 0;

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    Animator animator;

    SpriteRenderer spriteRenderer;

    new Rigidbody2D rigidbody2D;

    BoxCollider2D bodyCollider;

    GameObject chickenLight;

    [System.NonSerialized] public GameObject deleteChickenImg;
    Animator deleteChickenAni;

    [System.NonSerialized] public LineRenderer chickenRope;

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region[Awake]
    private void Awake()
    {
        instance = this;

        transform.position = new Vector3(transform.position.x, transform.position.y, -35.1f);

        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody2D = GetComponent<Rigidbody2D>();

        Transform checkList;
        checkList = transform.Find("CheckList");
        bodyCollider = checkList.Find("Body").GetComponent<BoxCollider2D>();

        chickenLight = transform.Find("Light").gameObject;

        deleteChickenImg = transform.Find("DeleteChicken").gameObject;
        deleteChickenAni = deleteChickenImg.GetComponent<Animator>();

        chickenRope = transform.Find("Rope").GetComponent<LineRenderer>();
    }
    #endregion

    #region[Update]
    void Update()
    {
        if (GameManager.instance.gamePause)
            return;
        ChickenPattern();
        ChickenAni();
        ChickenDelete();
    }
    #endregion

    #region[LateUpdate]
    public void LateUpdate()
    {
        ChickenBuff();
        if (GameManager.instance.gamePause)
            return;
        ChickenAct();
    }
    #endregion

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region[닭 패턴]
    void ChickenPattern()
    {
        if (orderTime > 0)
        {
            orderTime -= Time.deltaTime;
            if (Mathf.Abs(orderPos.x - transform.position.x) < 1)
            {
                if (UnityEngine.Random.Range(0, 100) > 50)
                    pattenType = Pattern.대기;
                else
                   pattenType = Pattern.제자리점프;
            }
            else if (orderPos.x > transform.position.x)
            {
                if (orderPos.y > transform.position.y)
                    pattenType = Pattern.오른쪽점프;
                else
                    pattenType = Pattern.오른쪽으로;
            }
            else if (orderPos.x < transform.position.x)
            {
                if (orderPos.y > transform.position.y)
                    pattenType = Pattern.왼쪽점프;
                else
                    pattenType = Chicken.Pattern.왼쪽으로;
            }
        }
        else
        {
            patternTime -= Time.deltaTime;
            //우는 동안은 패턴이 안변함
            if (patternTime < 0 && CryingCheck.aniTime == 0)
            {
                patternTime = 3;
                jumpFlag = false;

                pattenType = (Pattern)Random.Range(0, 6);
            }
        }
        switch (pattenType)
        {
            case Pattern.왼쪽으로:
                ChickenMove(-1);
                break;
            case Pattern.오른쪽으로:
                ChickenMove(+1);
                break;
            case Pattern.대기:
                ChickenMove(+0);
                break;
            case Pattern.왼쪽점프:
                ChickenMove(-1);
                ChickenJump();
                break;
            case Pattern.오른쪽점프:
                ChickenMove(-1);
                ChickenJump();
                break;
            case Pattern.제자리점프:
                ChickenMove(+0);
                ChickenJump();
                break;
        }

    }
    #endregion

    #region[닭 행동]
    void ChickenAct()
    {
        ChickenFall();
    }
    #endregion

    #region[닭 낙하]
    void ChickenFall()
    {
        //아래에 지형이 있는지 검사
        grounded = IsAtTerrain(bodyCollider, new Vector2(0, -0.2f));

        if (grounded)
        {
            if (rigidbody2D.gravityScale != 0 && rigidbody2D.velocity.y <= 0)
            {
                //닭을 멈추고
                rigidbody2D.velocity = Vector2.zero;

                rigidbody2D.gravityScale = 0;

                animator.SetBool("IsGround?", grounded);
            }
        }
        else
        {
            rigidbody2D.gravityScale = gravity;

            animator.SetBool("IsGround?", grounded);
        }
    }
    #endregion

    #region[닭 이동]
    void ChickenMove(int dic)
    {
        int MoveDirection = 0;
        //이동 방향에 따라 방향 값을 설정
        if (dic > 0)
            MoveDirection = +1;
        else if (dic < 0)
            MoveDirection = -1;
        else
            MoveDirection = 0;

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //이동방향으로 이미지 방향 설정
        flipX = MoveDirection != 0 ? MoveDirection : flipX;

        //뛰는 중인지 체크하는 변수
        bool runFlag = false;

        //이동 거리
        float moveDistance = MoveDirection * speed;
        //순간적인 이동 거리
        Vector2 moveValue;

        //이동 했을 경우
        if (moveDistance != 0)
        {
            MoveDic[] checkArray = new MoveDic[3] { MoveDic.경사아래로, MoveDic.앞으로, MoveDic.경사위로 };
            foreach (MoveDic move in checkArray)
            {
                //checkArr을 상용해서 이동할 값을 설정
                moveValue = new Vector2(moveDistance, (int)move * Mathf.Abs(moveDistance)) * Time.deltaTime;
                //이동한 위치에 지형이 있는지 검사
                bool nextPosCheck = IsAtTerrain(bodyCollider, moveValue);

                //앞으로 이동중이 아니면 땅위인지 검사해야됨
                if (move != MoveDic.앞으로 ? grounded : true)
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
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //애니메이션설정
        if (runFlag)
            animator.SetBool("Move", true);
        else
            animator.SetBool("Move", false);

    }
    #endregion

    #region[닭 점프]
    public void ChickenJump(bool groundJump = true)
    {
        //한번 점프했었거나 땅이 아니면 return
        if (jumpFlag || (groundJump ? !grounded : false))
            return;

        //점프력이 약해지지 않도록 순간적인 y가속도값을 0으로 해줌
        rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0);
        //위로 점프하도록 물리적으로 위로 밀어줌
        rigidbody2D.AddForce(new Vector2(0, jumpPower));
        //점프한걸로 처리
        jumpFlag = true;
    }

    public void ChickenJump(float force,bool groundJump = true)
    {
        //한번 점프했었거나 땅이 아니면 return
        if (jumpFlag || (groundJump ? !grounded : false))
            return;

        //점프력이 약해지지 않도록 순간적인 가속도값을 0으로 해줌
        rigidbody2D.velocity = Vector2.zero;
        //위로 점프하도록 물리적으로 위로 밀어줌
        rigidbody2D.AddForce(new Vector2(force, jumpPower));
        //점프한걸로 처리
        jumpFlag = true;

        int MoveDirection = 0;
        //이동 방향에 따라 방향 값을 설정
        if (force > 0)
            MoveDirection = +1;
        else if (force < 0)
            MoveDirection = -1;
        else
            MoveDirection = 0;

        flipX = MoveDirection != 0 ? MoveDirection : flipX;
    }
    #endregion

    #region[닭 소멸]
    public void ChickenDelete()
    {
        if (!deleteChickenImg.activeSelf)
            return;

        if(deleteChickenAni.GetCurrentAnimatorStateInfo(0).IsName("DeleteChicken") && deleteChickenAni.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f)
        {
            for(int i = 0; i < 8; i++)
            {
                Vector2 force = new Vector2(Random.Range(-1.5f, 1.5f), 3);
                EffectManager.instance.LightFeather(transform.position, force * 100, Random.Range(0, 3));
            }
            gameObject.SetActive(false);
        }
    }
    #endregion

    #region[닭 버프처리]
    void ChickenBuff()
    {
        speed = baseSpeed;

        jumpPower = baseJumpPower;

        gravity = baseGravity;

        chickenLight.SetActive(BuffManager.instance.nowBuffList["Luminous"].hasBuff);

        chickenRope.SetPosition(0, new Vector3(transform.position.x, transform.position.y, -35));
        chickenRope.SetPosition(1, new Vector3(Player.instance.transform.position.x, Player.instance.transform.position.y + 2, -35));
    }
    #endregion

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region[닭 애니메이션]
    void ChickenAni()
    {
        ChickenFlipX(flipX);
        ChickenCry();
    }
    #endregion

    #region[닭 울기]
    void ChickenCry()
    {
        cryTime -= Time.deltaTime;
        if(cryTime < 0)
        {
            animator.SetBool("Coco", false);

            cryTime = Random.Range(3, 11);

            //우는 시간이 6초 이상이고 땅에 서있으면 꼬기오하고 울음
            if(cryTime >= 10 && grounded)
            {
                animator.SetBool("Coco", true);
                pattenType = Pattern.대기;
            }
            cryTime = 180;
            animator.SetTrigger("Cry");
        }

        if(animator.GetBool("Coco") && !grounded)
        {
            SoundManager.instance.StopSE_Sound();
        }
    }
    #endregion

    #region[닭 보는 방향]
    void ChickenFlipX(int dic)
    {
        int Direction = 1; //닭 시선 방향
        if (dic > 0)
            Direction = +1;
        else if (dic < 0)
            Direction = -1;
        else
            Direction = transform.localScale.x > 0 ? +1 : -1;

        transform.localScale = new Vector3(Direction * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }
    #endregion
}