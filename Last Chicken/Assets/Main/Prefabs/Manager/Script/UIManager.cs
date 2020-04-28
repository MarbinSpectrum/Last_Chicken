﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using SpriteGlow;
using TMPro;
using Custom;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    CanvasScaler canvasScaler;

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    [NonSerialized] public GameObject gameOver;

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    [NonSerialized] public GameObject showTimer;
    [NonSerialized] public Text countDown;

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    [NonSerialized] public GameObject showChickenPos;  //닭 위치 표시
    [NonSerialized] public RectTransform showChickenRectTransform;
    [NonSerialized] public GameObject showChickenPosNormal;
    [NonSerialized] public RectTransform showChickenPosNormalArrow;
    [NonSerialized] public GameObject showChickenPosWarning;
    [NonSerialized] public RectTransform showChickenPosWarningArrow;

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    [NonSerialized] public GameObject showStageName;  //스테이지 이름 출력
    [NonSerialized] public Animator showStageNameAnimator;
    [NonSerialized] public Text showStageNameText;

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    [NonSerialized] public GameObject uiBack;       //UI뒤 이미지
    [NonSerialized] public GameObject pauseMenu;    //일시정지 메뉴

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    [NonSerialized] public GameObject playerHp;    //플레이어 체력

    #region[HearthType]
    public class HearthType
    {
        public GameObject full;
        public GameObject half;
        public GameObject empty;
        public GameObject halfempty;
        public GameObject emptyhalf;
        public Animator hearthAni;
        public HearthType(GameObject g1, GameObject g2, GameObject g3, GameObject g4, GameObject g5,Animator animator)
        {
            full = g1;
            half = g2;
            empty = g3;
            halfempty = g4;
            emptyhalf = g5;
            hearthAni = animator;
        }

        public void FullHearthActive()
        {
            full.SetActive(true);
            half.SetActive(false);
            empty.SetActive(false);
            halfempty.SetActive(false);
            emptyhalf.SetActive(false);
        }

        public void HalfHearthActive()
        {
            full.SetActive(false);
            half.SetActive(true);
            empty.SetActive(false);
            halfempty.SetActive(false);
            emptyhalf.SetActive(false);
        }

        public void EmptyHearthActive()
        {
            full.SetActive(false);
            half.SetActive(false);
            empty.SetActive(true);
            halfempty.SetActive(false);
            emptyhalf.SetActive(false);
        }

        public void HalfEmptyHearthActive()
        {
            full.SetActive(false);
            half.SetActive(false);
            empty.SetActive(false);
            halfempty.SetActive(true);
            emptyhalf.SetActive(false);
        }

        public void EmptyHalfHearthActive()
        {
            full.SetActive(false);
            half.SetActive(false);
            empty.SetActive(false);
            halfempty.SetActive(false);
            emptyhalf.SetActive(true);
        }

        public void UnActive()
        {
            full.SetActive(false);
            half.SetActive(false);
            empty.SetActive(false);
            halfempty.SetActive(false);
            emptyhalf.SetActive(false);
        }
    }
    #endregion

    [NonSerialized] public List<HearthType> hearthData = new List<HearthType>();    //플레이어 체력이미지

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    [NonSerialized] public GameObject playerMoney;      //현재금화
    [NonSerialized] public GameObject[] goldNumObject = new GameObject[6];      //현재금화
    [NonSerialized] public Image[,] goldNum = new Image[6, 10];
    [NonSerialized] public Animator goldAni;
    [NonSerialized] public int uiMoney;

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    [NonSerialized] public GameObject playerItem;    //현재아이템

    [NonSerialized] public GameObject nowItem;    //현재아이템
    [NonSerialized] public Animator nowItemAnimator;
    [NonSerialized] public Image nowItemImage;

    [NonSerialized] public GameObject[] itemObject = new GameObject[6];
    [NonSerialized] public Image[] itemImg = new Image[6];
    [NonSerialized] public Text[] itemNumText = new Text[6];
    [NonSerialized] public Image[] itemCoolImg = new Image[6];

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    [NonSerialized] public GameObject playerState;    //플레이어상태
    [NonSerialized] public Animator playerStateAni;    //현재아이템

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    [NonSerialized] public GameObject playerMap;    //플레이어 지도 UI

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    [NonSerialized] public GameObject explainObject;    //설명텍스트
    [NonSerialized] public RectTransform explainRect;   
    [NonSerialized] public Text explainText;            

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    [NonSerialized] public GameObject altarUI;                      //제단 UI
    [NonSerialized] public Text buffName;                           //설명 텍스트
    [NonSerialized] public Text buffText;                           //설명 텍스트
    [NonSerialized] public EventTrigger[] caseEvent;                //메뉴 이벤트
    [NonSerialized] public GameObject[,] caseObject;
    [NonSerialized] public Image[] caseImage;                         //설명 이미지
    [NonSerialized] public SpriteRenderer[,] caseSpriteRenderer;
    [NonSerialized] public SpriteGlowEffect[,] caseSpriteglow;
    Color itemDarkColor = new Color(0.2f, 0.2f, 0.2f);

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    [NonSerialized] public GameObject shopUI;               //상점 UI
    [NonSerialized] public Text[] shopItemCost;             //상점아이템가격
    [NonSerialized] public Text[] shopItemName;             //상점아이템이름
    [NonSerialized] public Text[,] shopItemExplan;           //상점아이템설명
    [NonSerialized] public Image[] shopItemImg;             //상점아이템이미지
    [NonSerialized] public GameObject[] shopItemSoldOut;         //매진 이미지
    [NonSerialized] public Button[] shopItemBuy;            //아이템 구매 버튼

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    [NonSerialized] public GameObject smithyUI;               //대장간 UI
    [NonSerialized] public Text smithyText;
    [NonSerialized] public GameObject smithyYes;               
    [NonSerialized] public GameObject smithyNo;
    [NonSerialized] public GameObject smithyOk;

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    [NonSerialized] public GameObject settingMenu;  //설정 메뉴
    Dropdown windowDropdown;
    Toggle windowToggle;

    //지원 창 정보들
    public int[,] windowOption = new int[,]
    {
            { 1024,0768 },
            { 1280,0720 },
            { 1280,1024 },
            { 1600,0900 },
            { 1680,1050 },
            { 1920,1080 }
    };

    Slider seSlider;
    Slider bgmSlider;

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    [NonSerialized] public bool goTitle = false;

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    //UI 레이캐스트
    [NonSerialized] public GraphicRaycaster graphicRaycaster;

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region[Awake]
    private void Awake()
    {
        if (instance == null)
            instance = this;

        Transform canvas = transform.Find("Canvas");
        canvasScaler = canvas.GetComponent<CanvasScaler>();

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        gameOver = canvas.Find("GameOver").gameObject;

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        showTimer = canvas.Find("Timer").gameObject;
        countDown = showTimer.transform.Find("Text").GetComponent<Text>();

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        showChickenPos = canvas.Find("ChickenPos").gameObject;
        showChickenRectTransform = showChickenPos.GetComponent<RectTransform>();
        showChickenPosNormal = showChickenPos.transform.Find("Normal").gameObject;
        showChickenPosNormalArrow = showChickenPosNormal.transform.Find("Arrow").GetComponent<RectTransform>();
        showChickenPosWarning = showChickenPos.transform.Find("Warning").gameObject;
        showChickenPosWarningArrow = showChickenPosWarning.transform.Find("Arrow").GetComponent<RectTransform>();

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        showStageName = canvas.Find("ShowStageName").gameObject;
        showStageNameAnimator = showStageName.GetComponent<Animator>();
        showStageNameText = showStageName.GetComponent<Text>();

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        uiBack = canvas.Find("Back").gameObject;
        pauseMenu = canvas.Find("PauseMenu").gameObject;

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        playerItem = canvas.Find("PlayerItem").gameObject;

        nowItem = playerItem.transform.Find("NowItem").gameObject;
        nowItemAnimator = nowItem.GetComponent<Animator>();
        nowItemImage = nowItem.GetComponent<Image>();

        itemObject[0] = playerItem.transform.Find("MainObject").gameObject;
        itemImg[0] = itemObject[0].transform.Find("ItemImg").GetComponent<Image>();
        itemNumText[0] = itemObject[0].transform.Find("Count").GetComponent<Text>();
        itemCoolImg[0] = itemObject[0].transform.Find("Cool").GetComponent<Image>();

        for (int i = 1; i < 6; i++)
        {
            itemObject[i] = playerItem.transform.Find("SubGroup").GetChild(i - 1).gameObject;
            itemImg[i] = itemObject[i].transform.Find("ItemImg").GetComponent<Image>();
            itemNumText[i] = itemObject[i].transform.Find("Count").GetComponent<Text>();
            itemCoolImg[i] = itemObject[i].transform.Find("Cool").GetComponent<Image>();
        }

        #region[마우스가 들어갔을때 이벤트]

        EventTrigger.Entry mainitemPointerEnter = new EventTrigger.Entry();
        mainitemPointerEnter.eventID = EventTriggerType.PointerEnter;
        mainitemPointerEnter.callback.AddListener((data) =>
        {
            if (GameManager.instance.itemSlot[0] != null && ItemManager.FindData(GameManager.instance.itemSlot[0]) != -1)
            {
                explainObject.SetActive(true);
                ExplainPlayerItem(GameManager.instance.itemSlot[0]);

            }
        });
        playerItem.transform.Find("MainObject").GetComponent<EventTrigger>().triggers.Add(mainitemPointerEnter);

        for (int i = 1; i < 6; i++)
        {
            int n = i;
            EventTrigger.Entry itemPointerEnter = new EventTrigger.Entry();
            itemPointerEnter.eventID = EventTriggerType.PointerEnter;
            itemPointerEnter.callback.AddListener((data) =>
            {
                if (GameManager.instance.itemSlot[n] != null && ItemManager.FindData(GameManager.instance.itemSlot[n]) != -1)
                {
                    explainObject.SetActive(true);
                    ExplainPlayerItem(GameManager.instance.itemSlot[n]);

                }
            });
            playerItem.transform.Find("SubGroup").GetChild(i - 1).GetComponent<EventTrigger>().triggers.Add(itemPointerEnter);
        }

        #endregion

        #region[마우스가 나갔을때 이벤트]

        EventTrigger.Entry mainItemPointerExit = new EventTrigger.Entry();
        mainItemPointerExit.eventID = EventTriggerType.PointerExit;
        mainItemPointerExit.callback.AddListener((data) => { explainObject.SetActive(false); });
        playerItem.transform.Find("MainObject").GetComponent<EventTrigger>().triggers.Add(mainItemPointerExit);

        for (int i = 1; i < 6; i++)
        {
            EventTrigger.Entry itemPointerExit = new EventTrigger.Entry();
            itemPointerExit.eventID = EventTriggerType.PointerExit;
            itemPointerExit.callback.AddListener((data) => { explainObject.SetActive(false); });
            playerItem.transform.Find("SubGroup").GetChild(i - 1).GetComponent<EventTrigger>().triggers.Add(itemPointerExit);
        }

        #endregion

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        explainObject = canvas.Find("ExplainText").gameObject;
        explainRect = explainObject.GetComponent<RectTransform>();
        explainText = explainObject.transform.Find("Text").GetComponent<Text>();

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        playerHp = canvas.Find("PlayerHp").gameObject;
        for (int i = 0; i < playerHp.transform.childCount; i++)
            if (playerHp.transform.GetChild(i).transform.name.Equals("Hearth"))
            {
                GameObject fullHearth = playerHp.transform.GetChild(i).Find("HearthFull").gameObject;
                GameObject halfHearth = playerHp.transform.GetChild(i).Find("HearthHalf").gameObject;
                GameObject emptyHearth = playerHp.transform.GetChild(i).Find("HearthEmpty").gameObject;
                GameObject halfemptyHearth = playerHp.transform.GetChild(i).Find("HearthHalfEmpty").gameObject;
                GameObject emptyhalfHearth = playerHp.transform.GetChild(i).Find("HearthEmptyHalf").gameObject;
                Animator hearthAni = playerHp.transform.GetChild(i).GetComponent<Animator>();
                hearthData.Add(new HearthType(fullHearth, halfHearth, emptyHearth, halfemptyHearth, emptyhalfHearth, hearthAni));
            }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        playerMoney = canvas.Find("PlayerMoney").gameObject;
        Transform money = playerMoney.transform.Find("Money");
        goldAni = money.GetComponent<Animator>();
        for (int i = 0; i < 6; i++)
        {
            Transform moneyTemp = money.GetChild(5 - i);
            goldNumObject[i] = moneyTemp.gameObject;
            for (int j = 0; j < 10; j++)
                goldNum[i, j] = moneyTemp.GetChild(j).GetComponent<Image>();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        playerState = canvas.Find("PlayerState").gameObject;
        playerStateAni = playerState.transform.Find("State").GetComponent<Animator>();

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        altarUI = canvas.Find("AltarUI").gameObject;
        Transform select = altarUI.transform.Find("Select");
        buffName = altarUI.transform.Find("BuffName").GetComponent<Text>();
        buffText = altarUI.transform.Find("BuffText").GetComponent<Text>();
        caseEvent = new EventTrigger[3];
        caseImage = new Image[3];
        caseObject = new GameObject[3, 2];
        caseSpriteRenderer = new SpriteRenderer[3,2];
        caseSpriteglow = new SpriteGlowEffect[3,2];
        for (int i = 0; i< 3; i++)
        {
            int number = i;

            Transform caseTransform = select.Find("Case" + i);

            caseEvent[i] = caseTransform.GetComponent<EventTrigger>();
            caseImage[i] = caseTransform.Find("Img").GetComponent<Image>();
            for (int j = 0; j < caseTransform.Find("Effect").childCount; j++)
            {
                caseObject[i, j] = caseTransform.Find("Effect").GetChild(j).gameObject;
                caseSpriteRenderer[i, j] = caseTransform.Find("Effect").GetChild(j).GetComponent<SpriteRenderer>();
                caseSpriteglow[i, j] = caseTransform.Find("Effect").GetChild(j).GetComponent<SpriteGlowEffect>();
            }

            //마우스가 들어갔을때 이벤트
            EventTrigger.Entry pointerEnter = new EventTrigger.Entry();
            pointerEnter.eventID = EventTriggerType.PointerEnter;
            pointerEnter.callback.AddListener((data) => { buffName.text = BuffManager.instance.buffData[AltarScript.instance.buffList[number]].buffName; });
            pointerEnter.callback.AddListener((data) => { buffText.text = BuffManager.instance.buffData[AltarScript.instance.buffList[number]].buffExplain; });
            pointerEnter.callback.AddListener((data) => { caseImage[number].color = Color.white; });
            caseEvent[i].triggers.Add(pointerEnter);

            //마우스가 나갔을때 이벤트
            EventTrigger.Entry pointerExit = new EventTrigger.Entry();
            pointerExit.eventID = EventTriggerType.PointerExit;
            pointerExit.callback.AddListener((data) => { caseImage[number].color = itemDarkColor; });
            //pointerExit.callback.AddListener((data) => { buffName.text = ""; });
            //pointerExit.callback.AddListener((data) => { buffText.text = "닭의 신이 축복을 내립니다.\n축복을 선택해주세요."; });
            caseEvent[i].triggers.Add(pointerExit);

            //마우스 클릭했을때 이벤트
            EventTrigger.Entry pointerClick = new EventTrigger.Entry();
            pointerClick.eventID = EventTriggerType.PointerClick;
            pointerClick.callback.AddListener((data) => { BuffManager.instance.AddBuff(AltarScript.instance.buffList[number]); });
            pointerClick.callback.AddListener((data) => { Player.instance.invincibility = true; AltarScript.instance.thisUse = false; AltarScript.instance.used = true; });
            caseEvent[i].triggers.Add(pointerClick);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        shopUI = canvas.Find("ShopUI").gameObject;
        shopItemCost = new Text[3];
        shopItemName = new Text[3];
        shopItemExplan = new Text[3,4];
        shopItemImg = new Image[3];
        shopItemSoldOut = new GameObject[3];
        shopItemBuy = new Button[3];
        for (int i = 0; i < 3; i++)
        {
            int number = i;

            Transform temp = shopUI.transform.Find("ShopMenu" + i);
            shopItemCost[i] = temp.Find("Cost").GetComponent<Text>();
            shopItemName[i] = temp.Find("Name").GetComponent<Text>();
            for(int k = 0; k < 4; k++)
                shopItemExplan[i,k] = temp.Find("Explain" + k).GetComponent<Text>();
            shopItemImg[i] = temp.Find("Img").GetComponent<Image>();
            shopItemSoldOut[i] = temp.Find("SoldOut").gameObject;
            shopItemBuy[i] = temp.Find("Buy").GetComponent<Button>();
            shopItemBuy[i].onClick.AddListener(() => { ShopScript.instance.BuyItem(number); });
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        smithyUI = canvas.Find("SmithyUI").gameObject;
        smithyYes = smithyUI.transform.Find("Yes").gameObject;
        smithyNo = smithyUI.transform.Find("No").gameObject;
        smithyOk = smithyUI.transform.Find("Ok").gameObject;
        smithyYes.GetComponent<Button>().onClick.AddListener(() =>
        {
            if (ItemManager.instance.HasItemCheck("Hammer"))
            {
                ItemManager.instance.CostItem("Hammer");
                Smithy.instance.used = true;
                Player.instance.pickLevel++;
                Player.instance.canControl = true;
                Smithy.instance.thisUse = false;
            }
            else
            {
                if (GameManager.instance.playerMoney >= Smithy.reinforceCost[Player.instance.pickLevel])
                {
                    GameManager.instance.playerMoney -= Smithy.reinforceCost[Player.instance.pickLevel];
                    Smithy.instance.used = true;
                    Player.instance.pickLevel++;
                    Player.instance.canControl = true;
                    Smithy.instance.thisUse = false;
                }
            }
        });
        smithyNo.GetComponent<Button>().onClick.AddListener(() =>
        {
            Smithy.instance.thisUse = false;
            Player.instance.canControl = true;
        });
        smithyOk.GetComponent<Button>().onClick.AddListener(() =>
        {
            Smithy.instance.thisUse = false;
            Player.instance.canControl = true;
        });
        smithyText = smithyUI.transform.Find("Text").GetComponent<Text>();

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        settingMenu = canvas.Find("SettingMenu").gameObject;
        seSlider = settingMenu.transform.Find("SE").Find("Slider").GetComponent<Slider>();
        bgmSlider = settingMenu.transform.Find("BGM").Find("Slider").GetComponent<Slider>();
        windowDropdown = settingMenu.transform.Find("Window").Find("Dropdown").GetComponent<Dropdown>();
        windowToggle = settingMenu.transform.Find("Toggle").GetComponent<Toggle>();

        graphicRaycaster = canvas.GetComponent<GraphicRaycaster>();

    }
    #endregion

    #region[Start]
    void Start()
    {
        seSlider.value = SoundManager.instance.SE.volume;
        bgmSlider.value = SoundManager.instance.BGM.volume;

        #region[윈도우 창 설정]
        List<Dropdown.OptionData> optionDatas = new List<Dropdown.OptionData>();
        for (int i = 0; i < windowOption.GetLength(0); i++)
            optionDatas.Add(new Dropdown.OptionData(windowOption[i, 0] + " X " + windowOption[i, 1]));
        windowDropdown.AddOptions(optionDatas);
        int select = 0;
        for (int i = 0; i < windowOption.GetLength(0); i++)
            if (windowOption[i, 0] == GameManager.instance.playData.ScreenWidth && windowOption[i, 1] == GameManager.instance.playData.ScreenHeight)
            {
                select = i;
                break;
            }
        windowDropdown.value = select;
        windowDropdown.onValueChanged.AddListener((data) =>
        {
            //canvasScaler.referenceResolution = new Vector2(windowOption[windowDropdown.value, 0], windowOption[windowDropdown.value, 1]);
            GameManager.instance.playData.ScreenWidth = windowOption[windowDropdown.value, 0];
            GameManager.instance.playData.ScreenHeight = windowOption[windowDropdown.value, 1];
            Screen.SetResolution(GameManager.instance.playData.ScreenWidth, GameManager.instance.playData.ScreenHeight, GameManager.instance.playData.fullScreen);
            SoundManager.instance.SelectMenu();
        });

        windowToggle.isOn = !GameManager.instance.playData.fullScreen;
        windowToggle.onValueChanged.AddListener((data) =>
        {
            GameManager.instance.playData.fullScreen = !windowToggle.isOn;
            Screen.SetResolution(GameManager.instance.playData.ScreenWidth, GameManager.instance.playData.ScreenHeight, GameManager.instance.playData.fullScreen);
            SoundManager.instance.SelectMenu();
        });
        #endregion

        SetUIMoney();
    }
    #endregion

    #region[Update]
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !goTitle)
        {
            if (GameManager.instance.InGame())
            {
                if (GameManager.instance.playData.firstGame)
                {
                    if (settingMenu.activeSelf)
                        ActSettingMenu(false);
                    else if (!settingMenu.activeSelf)
                        ActSettingMenu(true);
                }
                else
                {
                    if (settingMenu.activeSelf)
                        ActSettingMenu(false);
                    else if (pauseMenu.activeSelf)
                        ActPauseMenu(false);
                    else if (!pauseMenu.activeSelf)
                        ActPauseMenu(true);
                }
            }
        }

        if (!GameManager.instance.InGame())
        {
            showStageName.SetActive(false);
            showChickenPos.SetActive(false);
            showTimer.SetActive(false);
            playerHp.SetActive(false);
            altarUI.SetActive(false);
            shopUI.SetActive(false);
            smithyUI.SetActive(false);
            explainObject.SetActive(false);
            playerMoney.SetActive(false);
            playerState.SetActive(false);
        }
        else
        {
            #region[제단 UI 조절]
            if (AltarScript.instance)
                altarUI.SetActive(AltarScript.instance.thisUse);
            if (!altarUI.activeSelf)
            {
                buffText.text = "닭의 신이 축복을 내립니다.\n축복을 선택해주세요.";
                for (int i = 0; i < 3; i++)
                    for (int k = 0; k < 2; k++)
                    {
                        caseSpriteRenderer[i, k].color = itemDarkColor;
                        caseSpriteRenderer[i, k].color = itemDarkColor;
                        caseImage[i].color = itemDarkColor;
                    }
            }
            else
            {
                #region[화면크기에 따른 제단 UI 크기조절]
                Vector3 size = new Vector3(1,1,1);
                if (Screen.width == windowOption[0, 0] && Screen.height == windowOption[0, 1])
                    size = new Vector3(2.5f, 2.5f, 1);
                else if (Screen.width == windowOption[1, 0] && Screen.height == windowOption[1, 1])
                    size = new Vector3(2.1f, 2.1f, 1);
                else if (Screen.width == windowOption[2, 0] && Screen.height == windowOption[2, 1])
                    size = new Vector3(1.5f, 1.5f, 1);
                else if (Screen.width == windowOption[3, 0] && Screen.height == windowOption[3, 1])
                    size = new Vector3(1.4f, 1.4f, 1);
                else if (Screen.width == windowOption[4, 0] && Screen.height == windowOption[4, 1])
                    size = new Vector3(1.15f, 1.15f, 1);
                else if (Screen.width == windowOption[5, 0] && Screen.height == windowOption[5, 1])
                    size = new Vector3(1f, 1f, 1);

                for (int i = 0; i < 3; i++)
                    for (int k = 0; k < 2; k++)
                    {
                        caseObject[i, k].transform.localScale = size;
                        caseObject[i, k].transform.localScale = size;
   
                    }

                size = new Vector3(1, 1, 1);
                if (Screen.width == windowOption[0, 0] && Screen.height == windowOption[0, 1])
                    size = new Vector3(1.75f, 1.75f, 1);
                else if (Screen.width == windowOption[1, 0] && Screen.height == windowOption[1, 1])
                    size = new Vector3(1.35f, 1.35f, 1);
                else if (Screen.width == windowOption[2, 0] && Screen.height == windowOption[2, 1])
                    size = new Vector3(1.5f, 1.5f, 1);
                else if (Screen.width == windowOption[3, 0] && Screen.height == windowOption[3, 1])
                    size = new Vector3(1.15f, 1.15f, 1);
                else if (Screen.width == windowOption[4, 0] && Screen.height == windowOption[4, 1])
                    size = new Vector3(1.1f, 1.1f, 1);
                else if (Screen.width == windowOption[5, 0] && Screen.height == windowOption[5, 1])
                    size = new Vector3(0.95f, 0.95f, 1);

                for (int i = 0; i < 3; i++)
                    caseImage[i].transform.localScale = size;
                #endregion
            }
            #endregion

            #region[상점 UI 조절]
            if (ShopScript.instance)
            {
                shopUI.SetActive(ShopScript.instance.thisUse && ShopScript.instance.gameObject.activeSelf);
                for (int i = 0; i < ShopScript.instance.itmeBuyList.Count; i++)
                    shopItemSoldOut[i].SetActive(ShopScript.instance.itmeBuyList[i]);
            }
            #endregion

            #region[모루 UI 조절]
            if (Smithy.instance)
            {
                smithyUI.SetActive(Smithy.instance.thisUse);
                if (Player.instance.pickLevel >= 4)
                {
                    smithyNo.SetActive(false);
                    smithyYes.SetActive(false);
                    smithyOk.SetActive(true);
                    smithyText.text = "더 이상 곡괭이를 강화할 수 없어 보인다.";
                }
                else if (ItemManager.instance.HasItemCheck("Hammer"))
                {
                    smithyNo.SetActive(true);
                    smithyYes.SetActive(true);
                    smithyOk.SetActive(false);
                    smithyText.text = "망치를 사용하면 곡괭이를 한단계 강화할 수 있을것같다.\n강화할까?";
                }
                else
                {
                    smithyNo.SetActive(true);
                    smithyYes.SetActive(true);
                    smithyOk.SetActive(false);
                    if (Player.instance.pickLevel >= 4)
                        return;
                    smithyText.text = Smithy.reinforceCost[Player.instance.pickLevel] + "$을 지불하면 곡괭이를 강화할 수 있을것 같다.\n강화할까?";
                }

            }
            #endregion

            if (Player.instance)
            {
                nowItem.transform.position = Camera.main.WorldToScreenPoint(Player.instance.transform.position + new Vector3(0, 5f, 0));
                ShowPlayerHp();
                PlayerItem();
                PlayerMoney();
            }

            if (Chicken.instance)
            {
                SetChickenPos();
                ShowCountDown();
            }

            playerState.SetActive(true);
        }
    }
    #endregion

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region[닭 위치 표시기]
    void SetChickenPos()
    {  
        if (Player.instance && Player.instance.getChicken || SceneController.instance.nowScene.Equals("Tutorial"))
        {
            showChickenPos.SetActive(false);
            return;
        }

        Vector3 targetScreenPos = Camera.main.WorldToViewportPoint(Chicken.instance.transform.position);
        if (targetScreenPos.x > 1 || targetScreenPos.x < 0 || targetScreenPos.y > 1 || targetScreenPos.y < 0)
        {
            showChickenPos.SetActive(true);

            float screenWitdh = canvasScaler.referenceResolution.x;
            float screenHeight = (float)Screen.height / (float)Screen.width *  canvasScaler.referenceResolution.x;

            Vector2 newPos;

            if (targetScreenPos.x > 1)
                newPos.x = screenWitdh;
            else if (targetScreenPos.x < 0)
                newPos.x = 0;
            else
                newPos.x = targetScreenPos.x * screenWitdh;

            if (newPos.x < 50)
                newPos.x = 50;
            else if (newPos.x > screenWitdh - 150)
                newPos.x = screenWitdh - 150;

            if (targetScreenPos.y > 1)
                newPos.y = screenHeight;
            else if (targetScreenPos.y < 0)
                newPos.y = 0;
            else
                newPos.y = targetScreenPos.y * screenHeight;

            if (newPos.y < 50)
                newPos.y = 50;
            else if (newPos.y > screenHeight - 150)
                newPos.y = screenHeight - 150;

            showChickenRectTransform.anchoredPosition = newPos;

            showChickenPosNormal.SetActive(GameManager.instance.countDown > 5);
            showChickenPosWarning.SetActive(GameManager.instance.countDown <= 5);
            float angle = Vector3.SignedAngle(transform.up, Chicken.instance.transform.position - Player.instance.transform.position, -transform.forward);
            showChickenPosNormalArrow.rotation = Quaternion.Euler(0, 0, -angle - 90);
            showChickenPosWarningArrow.rotation = Quaternion.Euler(0, 0, -angle - 90);
        }
        else
            showChickenPos.SetActive(false);
    }
    #endregion

    #region[플레이어 체력표시]
    void ShowPlayerHp()
    {
        if (SceneController.instance.nowScene.Equals("Tutorial"))
        {
            playerHp.SetActive(false);
            return;
        }

        playerHp.SetActive(true);

        for (int i = 0; i < hearthData.Count; i++)
            hearthData[i].UnActive();

        int hpMax = (Player.instance.maxHp != (int)Player.instance.maxHp) ? (int)Player.instance.maxHp + 1 : (int)Player.instance.maxHp;

        for (int i = 0; i < hpMax - 1; i++)
            hearthData[i].empty.SetActive(true);

        if (Custom.Exception.IndexOutRange(hpMax - 1, hearthData))
        {
            if (hpMax != Player.instance.maxHp)
                hearthData[hpMax - 1].emptyhalf.SetActive(true);
            else
                hearthData[hpMax - 1].empty.SetActive(true);
        }

        int hpNow = (Player.instance.nowHp != (int)Player.instance.nowHp) ? (int)Player.instance.nowHp + 1 : (int)Player.instance.nowHp;

        for (int i = 0; i < hpNow - 1; i++)
            hearthData[i].full.SetActive(true);

        if(Custom.Exception.IndexOutRange(hpNow - 1, hearthData))
        {
            if (hpNow != Player.instance.nowHp)
            {
                if (hpMax != Player.instance.maxHp)
                    hearthData[hpNow - 1].halfempty.SetActive(true);
                else
                    hearthData[hpNow - 1].half.SetActive(true);
            }
            else
                hearthData[hpNow - 1].full.SetActive(true);
        }

        // hearthData[Mathf.FloorToInt(Player.instance.maxHp)].empty.SetActive(true);
        //for (int i = 0; i < hearthData.Count; i++)
        //{
        //    if (i >= Player.instance.maxHp)
        //        break;

        //    if (i == Player.instance.maxHp - 1 && Player.instance.maxHp != (int)(Player.instance.maxHp))
        //        hearthData[i].halfempty.SetActive(true);
        //    else
        //        hearthData[i].empty.SetActive(true);
        //}

        //for (int i = 0; i < hearthData.Count; i++)
        //{
        //    if (i >= Player.instance.nowHp)
        //        break;


        //    if (i == Player.instance.nowHp - 1 && Player.instance.nowHp != (int)(Player.instance.nowHp))
        //        hearthData[i].halfempty.SetActive(true);
        //    else
        //        hearthData[i].full.SetActive(true);
        //}
    }
    #endregion

    #region[플레이어 금화처리]
    public void PlayerMoney()
    {
        if (SceneController.instance.nowScene.Equals("Tutorial"))
        {
            playerMoney.SetActive(false);
            return;
        }
        if (!playerMoney.activeSelf)
            uiMoney = GameManager.instance.playerMoney;

        playerMoney.SetActive(true);

        int value = Mathf.Abs(uiMoney - GameManager.instance.playerMoney) / 10;
        value = value < 10 ? 10 : value;
        if (uiMoney < GameManager.instance.playerMoney)
        {
            if (uiMoney + value <= GameManager.instance.playerMoney)
                uiMoney += value;
            else
                uiMoney = GameManager.instance.playerMoney;
        }
        else
        {
            if (uiMoney - value >= GameManager.instance.playerMoney)
                uiMoney -= value;
            else
                uiMoney = GameManager.instance.playerMoney;
        }

        if(uiMoney != GameManager.instance.playerMoney)
            goldAni.SetTrigger("Act");

        SetUIMoney();

        goldNumObject[5].SetActive(100000 <= uiMoney);
    }

    public void SetUIMoney()
    {
        int uiMoneyTemp = uiMoney;
        for (int number = 0; number < 6; number++)
        {
            for (int i = 0; i < 10; i++)
                goldNum[number, i].enabled = false;

            goldNum[number, uiMoneyTemp % 10].enabled = true;
            uiMoneyTemp /= 10;
        }
    }
    #endregion

    #region[플레이어 아이템]
    void PlayerItem()
    {
        ////////////////////////////////////////////////////////////////////////////////////////
        //활성화슬롯갯수를 파악
        int actSlotNum = 0;
        for (int i = 0; i < 6; i++)
            if (GameManager.instance.slotAct[i])
                actSlotNum++; 

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //아이템교체

        if (Input.GetKeyDown(KeyCode.Q) || Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            ////////////////////////////////////////////////////////////////////////////////////////
            //슬롯 회전
            if (actSlotNum >= 1)
            {
                string tempItem = GameManager.instance.itemSlot[0];
                int tempItemCount = GameManager.instance.itemNum[0];
                float tempItemCool = GameManager.instance.itemCool[0];

                for (int j = 0; j < actSlotNum - 1; j++)
                {
                    GameManager.instance.itemSlot[j] = GameManager.instance.itemSlot[j + 1];
                    GameManager.instance.itemNum[j] = GameManager.instance.itemNum[j + 1];
                    GameManager.instance.itemCool[j] = GameManager.instance.itemCool[j + 1];
                }
                GameManager.instance.itemSlot[actSlotNum - 1] = tempItem;
                GameManager.instance.itemNum[actSlotNum - 1] = tempItemCount;
                GameManager.instance.itemCool[actSlotNum - 1] = tempItemCool;
            }

            MoveItem();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            ////////////////////////////////////////////////////////////////////////////////////////
            //슬롯 회전
            if (actSlotNum >= 1)
            {
                string tempItem = GameManager.instance.itemSlot[actSlotNum - 1];
                int tempItemCount = GameManager.instance.itemNum[actSlotNum - 1];
                float tempItemCool = GameManager.instance.itemCool[actSlotNum - 1];

                for (int j = actSlotNum - 1; j >= 1; j--)
                {
                    GameManager.instance.itemSlot[j] = GameManager.instance.itemSlot[j - 1];
                    GameManager.instance.itemNum[j] = GameManager.instance.itemNum[j - 1];
                    GameManager.instance.itemCool[j] = GameManager.instance.itemCool[j - 1];
                }
                GameManager.instance.itemSlot[0] = tempItem;
                GameManager.instance.itemNum[0] = tempItemCount;
                GameManager.instance.itemCool[0] = tempItemCool;
            }

            MoveItem();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //아이템버리기

        if (Input.GetKeyDown(KeyCode.F) && Player.instance.canControl)
        {
            if (!GameManager.instance.itemSlot[0].Equals(""))
            {
                string tempName = GameManager.instance.itemSlot[0];
                int tempNum = GameManager.instance.itemNum[0];

                ItemManager.instance.SpawnItem(Player.instance.transform.position, tempName, tempNum);

                GameManager.instance.itemSlot[0] = "";
                GameManager.instance.itemNum[0] = 0;

                ////////////////////////////////////////////////////////////////////////////////////////
                //0번슬롯에 아이템이 존재할때까지회전
                MoveItem();
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //아이템창 아이템표시

        if (ItemManager.instance)
        {
            for (int i = 0; i < 6; i++)
            {
                try { itemImg[i].sprite = ItemManager.instance.itemData[ItemManager.FindData(GameManager.instance.itemSlot[i])].itemImg; itemImg[i].enabled = true; }
                catch { itemImg[i].enabled = false; }
            }
        }

        for (int i = 0; i < 6; i++)
        {
            itemNumText[i].text = (GameManager.instance.itemNum[i] > 0) ? GameManager.instance.itemNum[i].ToString() : "";
            itemObject[i].SetActive(GameManager.instance.slotAct[i]);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //설명텍스트

        explainObject.transform.position = Input.mousePosition;

        if (Input.mousePosition.x > Screen.width / 2)
            explainRect.pivot = new Vector2(1, 1);
        else
            explainRect.pivot = new Vector2(0, 1);

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///아이템쿨타임

        for (int i = 0; i < 6; i++)
        {
            if (ItemManager.CheckCoolTimeItem(GameManager.instance.itemSlot[i]))
            {
                itemCoolImg[i].enabled = true;
                float cool = 0;
                switch (GameManager.instance.itemSlot[i])
                {
                    case "Bell":
                        cool = ItemManager.instance.itemData[ItemManager.FindData("Bell")].value0;
                        break;
                }
                cool = (GameManager.instance.itemCool[i] > cool ? cool : GameManager.instance.itemCool[i]) / cool;

                itemCoolImg[i].fillAmount = 1 - cool;
            }
            else
                itemCoolImg[i].enabled = false;
        }
    }

    public void MoveItem()
    {
        //활성화슬롯갯수를 파악
        int actSlotNum = 0;
        for (int i = 0; i < 6; i++)
            if (GameManager.instance.slotAct[i])
                actSlotNum++;

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///아이템앞으로

        List<string> hasItemList = new List<string>();
        List<int> hasItemNumList = new List<int>();
        List<float> hasItemCoolList = new List<float>();

        for (int i = 0; i < actSlotNum; i++)
            if (!GameManager.instance.itemSlot[i].Equals(""))
            {
                hasItemList.Add(GameManager.instance.itemSlot[i]);
                hasItemNumList.Add(GameManager.instance.itemNum[i]);
                hasItemCoolList.Add(GameManager.instance.itemCool[i]);
            }


        for (int i = 0; i < actSlotNum; i++)
        {
            GameManager.instance.itemSlot[i] = "";
            GameManager.instance.itemNum[i] = 0;
            GameManager.instance.itemCool[i] = 0;
        }

        for (int i = 0; i < hasItemList.Count; i++)
        {
            GameManager.instance.itemSlot[i] = hasItemList[i];
            GameManager.instance.itemNum[i] = hasItemNumList[i];
            GameManager.instance.itemCool[i] = hasItemCoolList[i];
        }
    }
    #endregion

    #region[플레이어 아이템 설명표시]
    public void ExplainPlayerItem(string name)
    {
        string temp = "<" + ItemManager.instance.itemData[ItemManager.FindData(name)].itemName + ">\n" + ItemManager.instance.itemData[ItemManager.FindData(name)].itemExplain;
        //if (name.Equals("RandomDice"))
        //{
        //    int value = GameManager.instance.playData.randomDice;
        //    temp += "\n( 아이템가격 : " + value + "% )";
        //}
        explainText.text = temp;

    }

    #endregion

    #region[시간제한표시]
    void ShowCountDown()
    {
        showTimer.SetActive(false);

        if (Player.instance && Player.instance.getChicken)
            return;
        if (SceneController.instance.nowScene.Equals("Tutorial"))
            return;
        if (Player.instance && Player.instance.nowHp <= 0)
            return;
        if ((int)GameManager.instance.countDown >= 0)
        {
            showTimer.SetActive(true);
            countDown.text = (int)GameManager.instance.countDown + "";
        }
    }
    #endregion

    #region[일시정지 메뉴 활성화/비활성화]
    public void ActPauseMenu(bool b)
    {
        SoundManager.instance.SelectMenu();
        GameManager.instance.gamePause = b;
        pauseMenu.SetActive(b);
        uiBack.SetActive(b);
    }
    #endregion

    #region[타이틀로]
    public void GoToTile()
    {
        SoundManager.instance.SelectMenu();
        goTitle = true;
        ActPauseMenu(false);
        SceneController.instance.MoveScene("Title");
    }
    #endregion

    #region[설정 메뉴 활성화/비활성화]
    public void ActSettingMenu(bool b)
    {
        SoundManager.instance.SelectMenu();
        settingMenu.SetActive(b);
        uiBack.SetActive(b);
        if(!pauseMenu.activeSelf)
            GameManager.instance.gamePause = b;
    }
    #endregion

    #region[효과음 & 배경음 슬라이더 크기에 맞게 조절]
    public void SetSE()
    {
        if (SoundManager.instance && seSlider)
        {
            SoundManager.instance.SE.volume = seSlider.value;
            SoundManager.instance.StopSE.volume = seSlider.value;
            GameManager.instance.playData.SE_Volume = seSlider.value;
        }
    }

    public void SetBGM()
    {
        if (SoundManager.instance && bgmSlider)
        {
            SoundManager.instance.BGM.volume = bgmSlider.value;
            SoundManager.instance.SubBGM.volume = bgmSlider.value;
            GameManager.instance.playData.BGM_Volume = bgmSlider.value;
        }
    }
    #endregion
}