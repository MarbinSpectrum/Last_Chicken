using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Collections.Generic;
using UnityEngine;

[CustomEditor(typeof(ItemManager))]
public class ItemEditor : MyEditor
{
    public ItemManager itemManager;

    Vector2 stageScroll;
    Rect lastRect;

    #region[OnEnable]
    private void OnEnable()
    {
        itemManager = target as ItemManager;
    }
    #endregion

    #region[OnInspectorGUI]
    public override void OnInspectorGUI()
    {
        //변경사항 검사시작
        EditorGUI.BeginChangeCheck();

        GUIStyle nameStyle = new GUIStyle("label")
        {
            fontSize = 20,
            fontStyle = FontStyle.Bold,
            font = fontStarDust
        };
        nameStyle.normal.textColor = new Color(63 / 255f, 72 / 255f, 204 / 255f);
        GUIStyle dataStyle = new GUIStyle("label")
        {
            fontSize = 10,
            font = fontStarDust
        };

        #region[데이터 변경]
        if (itemManager.itemData.Length < ItemManager.itemName.Length)
        {
            List<ItemManager.ItemStats> temp = new List<ItemManager.ItemStats>();
            for (int i = 0; i < itemManager.itemData.Length; i++)
            {
                ItemManager.ItemStats emp = new ItemManager.ItemStats();
                emp.itemImg = itemManager.itemData[i].itemImg;
                emp.itemName = itemManager.itemData[i].itemName;
                emp.itemName_Eng = itemManager.itemData[i].itemName_Eng;
                emp.itemExplain = itemManager.itemData[i].itemExplain;
                emp.itemExplain_Eng = itemManager.itemData[i].itemExplain_Eng;
                emp.cost = itemManager.itemData[i].cost;
                emp.value0 = itemManager.itemData[i].value0;
                emp.value1 = itemManager.itemData[i].value1;
                emp.itemLevel = itemManager.itemData[i].itemLevel;
                emp.spawnObject = itemManager.itemData[i].spawnObject;
                emp.spawnTreasureBox = itemManager.itemData[i].spawnTreasureBox;
                emp.spawnShop = itemManager.itemData[i].spawnShop;
                emp.activeItem = itemManager.itemData[i].activeItem;
                emp.stage01 = itemManager.itemData[i].stage01;
                emp.stage02 = itemManager.itemData[i].stage02;
                for (int k = 0; k < 4; k++)
                    emp.shopItemExplain[k] = itemManager.itemData[i].shopItemExplain[k];
                for (int k = 0; k < 4; k++)
                    emp.shopItemExplain_Eng[k] = itemManager.itemData[i].shopItemExplain_Eng[k];
                temp.Add(emp);
            }

            itemManager.itemData = new ItemManager.ItemStats[ItemManager.itemName.Length];
            for (int i = 0; i < ItemManager.itemName.Length; i++)
                itemManager.itemData[i] = new ItemManager.ItemStats();

            for (int i = 0; i < temp.Count; i++)
            {
                itemManager.itemData[i].itemImg = temp[i].itemImg;
                itemManager.itemData[i].itemName = temp[i].itemName;
                itemManager.itemData[i].itemName_Eng = temp[i].itemName_Eng;
                itemManager.itemData[i].itemExplain = temp[i].itemExplain;
                itemManager.itemData[i].itemExplain_Eng = temp[i].itemExplain_Eng;
                itemManager.itemData[i].cost = temp[i].cost;
                itemManager.itemData[i].value0 = temp[i].value0;
                itemManager.itemData[i].value1 = temp[i].value1;
                itemManager.itemData[i].itemLevel = temp[i].itemLevel;
                itemManager.itemData[i].spawnObject = temp[i].spawnObject;
                itemManager.itemData[i].spawnTreasureBox = temp[i].spawnTreasureBox;
                itemManager.itemData[i].spawnShop = temp[i].spawnShop;
                itemManager.itemData[i].activeItem = temp[i].activeItem;
                itemManager.itemData[i].stage01 = temp[i].stage01;
                itemManager.itemData[i].stage02 = temp[i].stage02;
                for (int k = 0; k < 4; k++)
                    itemManager.itemData[i].shopItemExplain[k]  = temp[i].shopItemExplain[k];
                for (int k = 0; k < 4; k++)
                   itemManager.itemData[i].shopItemExplain_Eng[k] = temp[i].shopItemExplain_Eng[k];
            }
        }
        else if (itemManager.itemData.Length > ItemManager.itemName.Length)
        {
            List<ItemManager.ItemStats> temp = new List<ItemManager.ItemStats>();
            for (int i = 0; i < itemManager.itemData.Length; i++)
            {
                ItemManager.ItemStats emp = new ItemManager.ItemStats();
                emp.itemImg = itemManager.itemData[i].itemImg;
                emp.itemName = itemManager.itemData[i].itemName;
                emp.itemName_Eng = itemManager.itemData[i].itemName_Eng;
                emp.itemExplain = itemManager.itemData[i].itemExplain;
                emp.itemExplain_Eng = itemManager.itemData[i].itemExplain_Eng;
                emp.cost = itemManager.itemData[i].cost;
                emp.value0 = itemManager.itemData[i].value0;
                emp.value1 = itemManager.itemData[i].value1;
                emp.itemLevel = itemManager.itemData[i].itemLevel;
                emp.spawnObject = itemManager.itemData[i].spawnObject;
                emp.spawnTreasureBox = itemManager.itemData[i].spawnTreasureBox;
                emp.spawnShop = itemManager.itemData[i].spawnShop;
                emp.activeItem = itemManager.itemData[i].activeItem;
                emp.stage01 = itemManager.itemData[i].stage01;
                emp.stage02 = itemManager.itemData[i].stage02;
                for (int k = 0; k < 4; k++)
                    emp.shopItemExplain[k] = itemManager.itemData[i].shopItemExplain[k];
                for (int k = 0; k < 4; k++)
                    emp.shopItemExplain_Eng[k] = itemManager.itemData[i].shopItemExplain_Eng[k];
                temp.Add(emp);
            }

            itemManager.itemData = new ItemManager.ItemStats[ItemManager.itemName.Length];
            for (int i = 0; i < ItemManager.itemName.Length; i++)
                itemManager.itemData[i] = new ItemManager.ItemStats();

            for (int i = 0; i < ItemManager.itemName.Length; i++)
            {
                itemManager.itemData[i].itemImg = temp[i].itemImg;
                itemManager.itemData[i].itemName = temp[i].itemName;
                itemManager.itemData[i].itemName_Eng = temp[i].itemName_Eng;
                itemManager.itemData[i].itemExplain = temp[i].itemExplain;
                itemManager.itemData[i].itemExplain_Eng = temp[i].itemExplain_Eng;
                itemManager.itemData[i].cost = temp[i].cost;
                itemManager.itemData[i].value0 = temp[i].value0;
                itemManager.itemData[i].value1 = temp[i].value1;
                itemManager.itemData[i].itemLevel = temp[i].itemLevel;
                itemManager.itemData[i].spawnObject = temp[i].spawnObject;
                itemManager.itemData[i].spawnTreasureBox = temp[i].spawnTreasureBox;
                itemManager.itemData[i].spawnShop = temp[i].spawnShop;
                itemManager.itemData[i].activeItem = temp[i].activeItem;
                itemManager.itemData[i].stage01 = temp[i].stage01;
                itemManager.itemData[i].stage02 = temp[i].stage02;
                for (int k = 0; k < 4; k++)
                    itemManager.itemData[i].shopItemExplain[k] = temp[i].shopItemExplain[k];
                for (int k = 0; k < 4; k++)
                    itemManager.itemData[i].shopItemExplain_Eng[k] = temp[i].shopItemExplain_Eng[k];
            }
        }
        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        GUI.color = Color.white;

        EditorGUILayout.BeginVertical("box", GUILayout.Width(600));
        EditorGUILayout.BeginHorizontal();


        EditorGUILayout.BeginVertical();

        EditorGUILayout.LabelField("등장비율(%)", dataStyle, GUILayout.Width(60));

        //EditorGUILayout.BeginVertical("helpbox");
        //EditorGUILayout.LabelField("합이 100이 되도록 해주세요!!", nameStyle, GUILayout.Width(250));
        //EditorGUILayout.EndHorizontal();
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        EditorGUILayout.BeginVertical("helpbox");
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("일반", dataStyle, GUILayout.Width(50));
        itemManager.normalRate = EditorGUILayout.IntField(itemManager.normalRate, GUILayout.Width(50), GUILayout.Height(15));
        EditorGUILayout.LabelField("희귀", dataStyle, GUILayout.Width(50));
        itemManager.rareRate = EditorGUILayout.IntField(itemManager.rareRate ,GUILayout.Width(50), GUILayout.Height(15));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("특별", dataStyle, GUILayout.Width(50));
        itemManager.specialRate = EditorGUILayout.IntField(itemManager.specialRate ,GUILayout.Width(50), GUILayout.Height(15));
        EditorGUILayout.LabelField("전설", dataStyle, GUILayout.Width(50));
        itemManager.legendRate = EditorGUILayout.IntField(itemManager.legendRate ,GUILayout.Width(50), GUILayout.Height(15));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();

    

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndVertical();

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        EditorGUILayout.BeginVertical("box");
        stageScroll = EditorGUILayout.BeginScrollView(stageScroll, GUILayout.Width(600), GUILayout.Height(600));

        for (int i = 0; i < ItemManager.itemName.Length; i++)
        {
            EditorGUILayout.BeginVertical("helpbox", GUILayout.Width(575));
            ItemData(ItemManager.itemName[i], ref itemManager.itemData[ItemManager.FindData(ItemManager.itemName[i])], getName(ItemManager.itemName[i]));
            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(itemManager, "ChangeItemManaer");
            var prefabStage = UnityEditor.Experimental.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage();

            if (prefabStage != null)
                EditorSceneManager.MarkSceneDirty(prefabStage.scene);
            else
            {
                EditorUtility.SetDirty(itemManager);
                PrefabUtility.RecordPrefabInstancePropertyModifications(this);
                EditorSceneManager.MarkSceneDirty(itemManager.gameObject.scene);
            }
        }
    }
    #endregion

    #region[아이템 데이터]
    void ItemData(string itemName, ref ItemManager.ItemStats data, string emp)
    {
        GUIStyle nameStyle = new GUIStyle("label")
        {
            fontSize = 20,
            fontStyle = FontStyle.Bold,
            font = fontStarDust
        };
        nameStyle.normal.textColor = new Color(63 / 255f, 72 / 255f, 204 / 255f);
        GUIStyle dataStyle = new GUIStyle("label")
        {
            fontSize = 10,
            font = fontStarDust
        };

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        EditorGUILayout.BeginHorizontal();
        Vector2 size = new Vector2(2f, 2f);
        EditorGUILayout.LabelField(emp, nameStyle, GUILayout.Width(560 - 30 * size.x), GUILayout.Height(30));
        data.itemImg = (Sprite)EditorGUILayout.ObjectField(data.itemImg, typeof(Sprite), false, GUILayout.Width(30 * size.x), GUILayout.Height(30 * size.y));
        EditorGUILayout.EndHorizontal();

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("아이템 이름", dataStyle, GUILayout.Width(470));
        data.itemName = EditorGUILayout.TextField(data.itemName, GUILayout.Width(90));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Item Name", dataStyle, GUILayout.Width(470));
        data.itemName_Eng = EditorGUILayout.TextField(data.itemName_Eng, GUILayout.Width(90));
        EditorGUILayout.EndHorizontal();

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        data.itemLevel = (ItemManager.ItemLevel)EnumField("아이템 등급", data.itemLevel, dataStyle);

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        if (data.spawnShop)
        {
            EditorGUILayout.BeginHorizontal();
            data.cost = IntField("아이템가격", data.cost, dataStyle);
            EditorGUILayout.EndHorizontal();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        EditorGUILayout.LabelField("활성화슬롯위치", dataStyle, GUILayout.Width(470));
        EditorGUILayout.BeginVertical("helpbox");
        if (ItemManager.CheckActiveItem(itemName))
            EditorGUILayout.LabelField("액티브 아이템입니다.", dataStyle, GUILayout.Width(230));
        else
            EditorGUILayout.LabelField("패시브 아이템입니다.", dataStyle, GUILayout.Width(230));
        EditorGUILayout.EndVertical();

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        #region[수치조정이 필요한 오브젝트]
        float temp = 0;
        switch (itemName)
        {
            case "Bell":
                temp = FloatField("쿨타임", data.value0, dataStyle);
                temp = temp < 0 ? 0 : temp;
                data.value0 = temp;
                break;
            case "Hammer":
                temp = FloatField("공격력 증가수치(%)", data.value0, dataStyle);
                temp = temp < 0 ? 0 : temp;
                temp = (int)(temp);
                data.value0 = temp;

                temp = FloatField("공격속도 감소수치(%)", data.value1, dataStyle);
                temp = temp < 0 ? 0 : temp;
                temp = (int)(temp);
                data.value1 = temp;
                break;
            case "Coffee":
                temp = FloatField("회복량", data.value0, dataStyle);
                temp = temp < 0 ? 0 : temp;
                temp = (((int)(temp * 10) / 5) * 5) / 10f;
                data.value0 = temp;

                temp = FloatField("따뜻함 유지시간", data.value1, dataStyle);
                temp = temp < 0 ? 0 : temp;
                data.value1 = temp;
                break;
            case "Smart_Light_Pick":
                temp = FloatField("공격속도 증가수치(%)", data.value0, dataStyle);
                temp = temp < 0 ? 0 : temp;
                temp = (int)(temp);
                data.value0 = temp;
                break;
            case "Smart_Heavy_Pick":
                temp = FloatField("공격력 증가수치(%)", data.value0, dataStyle);
                temp = temp < 0 ? 0 : temp;
                temp = (int)(temp);
                data.value0 = temp;
                break;
            case "Advanced_Pick":
                temp = FloatField("공격속도 증가수치(%)", data.value0, dataStyle);
                temp = temp < 0 ? 0 : temp;
                temp = (int)(temp);
                data.value0 = temp;

                temp = FloatField("공격력    증가수치(%)", data.value1, dataStyle);
                temp = temp < 0 ? 0 : temp;
                temp = (int)(temp);
                data.value1 = temp;
                break;
            case "Light_Feather":
                temp = FloatField("점프력 증가수치(%)", data.value0, dataStyle);
                temp = temp < 0 ? 0 : temp;
                temp = (int)(temp);
                data.value0 = temp;
                break;
            case "Coke":
                temp = FloatField("회복량", data.value0, dataStyle);
                temp = temp < 0 ? 0 : temp;
                temp = (((int)(temp * 10) / 5) * 5) / 10f;
                data.value0 = temp;
                break;
            case "Beer":
                temp = FloatField("증가량", data.value0, dataStyle);
                temp = temp < 0 ? 0 : temp;
                temp = (((int)(temp * 10) / 5) * 5) / 10f;
                data.value0 = temp;
                break;
            case "Russian_Roulette":
                temp = FloatField("피해량", data.value0, dataStyle);
                temp = temp < 0 ? 0 : temp;
                temp = (((int)(temp * 10) / 5) * 5) / 10f;
                data.value0 = temp;

                temp = FloatField("회복수치", data.value1, dataStyle);
                temp = temp < 0 ? 0 : temp;
                temp = (((int)(temp * 10) / 5) * 5) / 10f;
                data.value1 = temp;
                break;
            case "Umbrella":
                temp = FloatField("쿨타임", data.value1, dataStyle);
                temp = temp < 0 ? 0 : temp;
                data.value1 = temp;
                break;
            case "Splash_Pick":
                temp = FloatField("데미지", data.value0, dataStyle);
                temp = temp < 0 ? 0 : temp;
                temp = (int)(temp);
                data.value0 = temp;

                temp = FloatField("쿨타임", data.value1, dataStyle);
                temp = temp < 0 ? 0 : temp;
                data.value1 = temp;
                break;
            case "BoomItem":
                temp = FloatField("데미지", data.value0, dataStyle);
                temp = temp < 0 ? 0 : temp;
                temp = (int)(temp);
                data.value0 = temp;

                temp = FloatField("쿨타임", data.value1, dataStyle);
                temp = temp < 0 ? 0 : temp;
                temp = (int)(temp);
                data.value1 = temp;
                break;
            case "Dynamite":
                temp = FloatField("데미지", data.value0, dataStyle);
                temp = temp < 0 ? 0 : temp;
                temp = (int)(temp);
                data.value0 = temp;

                temp = FloatField("쿨타임", data.value1, dataStyle);
                temp = temp < 0 ? 0 : temp;
                temp = (int)(temp);
                data.value1 = temp;
                break;
            case "SaleCoupon":
                temp = FloatField("할인률(%)", data.value0, dataStyle);
                temp = temp < 0 ? 0 : temp;
                temp = (int)(temp);
                data.value0 = temp;
                break;
            case "ShopVIpNormal":
                temp = FloatField("할인률(%)", data.value0, dataStyle);
                temp = temp < 0 ? 0 : temp;
                temp = (int)(temp);
                data.value0 = temp;
                break;
            case "ShopVIpSpecial":
                temp = FloatField("할인률(%)", data.value0, dataStyle);
                temp = temp < 0 ? 0 : temp;
                temp = (int)(temp);
                data.value0 = temp;
                break;
            case "OldPocket":
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField("랜덤한 수치 범위", dataStyle, GUILayout.Width(150));

                temp = EditorGUILayout.FloatField(data.value0, GUILayout.Width(30));
                temp = temp < 0 ? 0 : temp;
                temp = (int)(temp);
                data.value0 = temp;

                EditorGUILayout.LabelField(" ~ ", dataStyle, GUILayout.Width(20));

                temp = EditorGUILayout.FloatField(data.value1, GUILayout.Width(30));
                temp = temp < 0 ? 0 : temp;
                temp = (int)(temp);
                data.value1 = temp;

                EditorGUILayout.EndHorizontal();
                break;
        }
        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        EditorGUILayout.LabelField("나오는곳", dataStyle, GUILayout.Width(50));
        EditorGUILayout.BeginVertical("helpbox");

        EditorGUILayout.BeginHorizontal();
        data.spawnShop = ToggleField("상점", data.spawnShop, dataStyle);
        data.spawnObject = ToggleField("오브젝트", data.spawnObject, dataStyle);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        data.spawnTreasureBox = ToggleField("보물상자", data.spawnTreasureBox, dataStyle);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();
        EditorGUILayout.BeginVertical("helpbox");

        EditorGUILayout.BeginHorizontal();
        data.stage01 = ToggleField("Stage 1", data.stage01, dataStyle);
        data.stage02 = ToggleField("Stage 2", data.stage02, dataStyle);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        EditorGUILayout.LabelField("아이템 설명", dataStyle);
        data.itemExplain = EditorGUILayout.TextArea(data.itemExplain, GUILayout.Width(560), GUILayout.Height(80));
        EditorGUILayout.LabelField("itemExplain", dataStyle);
        data.itemExplain_Eng = EditorGUILayout.TextArea(data.itemExplain_Eng, GUILayout.Width(560), GUILayout.Height(80));
        if (data.spawnShop)
        {
            EditorGUILayout.LabelField("아이템 설명(상점)", dataStyle);
            for (int i = 0; i < 3; i++)
                data.shopItemExplain[i] = EditorGUILayout.TextField(data.shopItemExplain[i], GUILayout.Width(560), GUILayout.Height(20));
            EditorGUILayout.LabelField("itemExplain(Shop)", dataStyle);
            for (int i = 0; i < 3; i++)
                data.shopItemExplain_Eng[i] = EditorGUILayout.TextField(data.shopItemExplain_Eng[i], GUILayout.Width(560), GUILayout.Height(20));
            string sTemp;
            if (ItemManager.CheckActiveItem(itemName))
                sTemp = "<color=#ff0000ff>Active</color>";
            else
                sTemp = "<color=#00ff00ff>Passive</color>";
            data.shopItemExplain[3] = sTemp;
            data.shopItemExplain_Eng[3] = sTemp;
        }
    }
    #endregion

    #region[인스펙터에 표시할 데이터]
    string getName(string s)
    {
        switch (s)
        {
            case "Bell":
                return "방울";
            case "Feather_Shoes":
                return "깃털신발";
            case "Splash_Pick":
                return "투척용곡괭이";
            case "Umbrella":
                return "우산";
            case "Medkit":
                return "구급킷";
            case "Torch":
                return "횃불";
            case "FourLeafClover":
                return "네잎클로버";
            case "Coke":
                return "콜라";
            case "Beer":
                return "맥주";
            case "Monster_Radar":
                return "몬스터탐지기";
            case "Russian_Roulette":
                return "러시안룰렛";
            case "Mine_Helmet":
                return "광산헬멧";
            case "Light_Feather":
                return "가벼운깃털";
            case "Rope":
                return "밧줄";
            case "Dynamite":
                return "다이너마이트";
            case "Smart_Gloves":
                return "찍찍이장갑";
            case "Coffee":
                return "커피";
            case "Smart_Advanced_Pick":
                return "편리한 고급 곡괭이";
            case "MineBag":
                return "광물가방";
            case "MineBag_EX":
                return "광물가방 EX";
            case "SaleCoupon":
                return "상점 할인쿠폰";
            case "ShopVIpNormal":
                return "상점회원증";
            case "ShopVIpSpecial":
                return "상점VIP회원증";
            case "OldPocket":
                return "오래된 보따리";
            case "RandomDice":
                return "랜덤 다이스";
            case "TreasureBox_Radar":
                return "보물 탐지기";
            case "Trap_Radar":
                return "함정 탐지기";
            case "RainbowPocket":
                return "무지개 보따리";
            case "BoomItem":
                return "폭탄";
        }
        return s;
    }
    #endregion

    #region[ToggleField]
    public override bool ToggleField(string name, bool input, GUIStyle guiStyle = null)
    {
        guiStyle = guiStyle != null ? guiStyle : GUI.skin.label;

        EditorGUILayout.BeginVertical();
        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.LabelField(name, guiStyle, GUILayout.Width(50));
        var result = EditorGUILayout.Toggle(input);

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();

        return result;
    }
    #endregion

    #region[EnumField]
    public override Enum EnumField(string name, Enum input, GUIStyle guiStyle = null)
    {
        guiStyle = guiStyle != null ? guiStyle : GUI.skin.label;
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(name, guiStyle, GUILayout.Width(470));

        var result = EditorGUILayout.EnumPopup(input, GUILayout.Width(90));
        EditorGUILayout.EndHorizontal();
        return result;
    }

    public override Enum EnumField(Enum input, GUIStyle guiStyle = null)
    {
        guiStyle = guiStyle != null ? guiStyle : GUI.skin.label;
        EditorGUILayout.BeginHorizontal();

        var result = EditorGUILayout.EnumPopup(input, GUILayout.Width(90));
        EditorGUILayout.EndHorizontal();
        return result;
    }

    public override Enum EnumField(Texture2D texture2D, Enum input, GUIStyle guiStyle = null)
    {
        guiStyle = guiStyle != null ? guiStyle : GUI.skin.label;
        if (texture2D == null)
            return EnumField(input, guiStyle);

        EditorGUILayout.BeginHorizontal();
        GUI.DrawTexture(new Rect(0, 0, 30, 30), texture2D);
        var result = EditorGUILayout.EnumPopup(input, GUILayout.Width(90));
        EditorGUILayout.EndHorizontal();
        return result;
    }

    public override Enum EnumField(string name, Texture2D texture2D, Enum input, GUIStyle guiStyle = null)
    {
        guiStyle = guiStyle != null ? guiStyle : GUI.skin.label;
        if (texture2D == null)
            return EnumField(name, input, guiStyle);

        EditorGUILayout.BeginVertical();
        EditorGUILayout.BeginHorizontal("helpbox", GUILayout.Width(100));

        GUILayout.Button("", GUI.skin.label, GUILayout.Width(50), GUILayout.Height(100));
        Rect lastRect = GUILayoutUtility.GetLastRect();
        GUI.DrawTexture(lastRect, texture2D);

        EditorGUILayout.BeginVertical();
        EditorGUI.indentLevel++;
        EditorGUILayout.LabelField(name, guiStyle, GUILayout.Width(150));
        var result = EditorGUILayout.EnumPopup(input, "helpbox", GUILayout.Width(90));
        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();

        return result;
    }
    #endregion

    #region[IntField]
    public override int IntField(string name, int input, GUIStyle guiStyle = null)
    {
        guiStyle = guiStyle != null ? guiStyle : GUI.skin.label;
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(name, guiStyle, GUILayout.Width(470));

        var result = EditorGUILayout.IntField(input, GUILayout.Width(90));
        EditorGUILayout.EndHorizontal();
        return result;
    }

    public override int IntField(int input, GUIStyle style = null)
    {
        EditorGUILayout.BeginHorizontal();

        var result = EditorGUILayout.IntField(input, GUILayout.Width(90));
        EditorGUILayout.EndHorizontal();
        return result;
    }

    public override int IntField(Texture2D texture2D, int input, GUIStyle guiStyle = null)
    {
        guiStyle = guiStyle != null ? guiStyle : GUI.skin.label;
        if (texture2D == null)
            return IntField(input, guiStyle);

        EditorGUILayout.BeginHorizontal();
        GUI.DrawTexture(new Rect(0, 0, 30, 30), texture2D);
        var result = EditorGUILayout.IntField(input, GUILayout.Width(90));
        EditorGUILayout.EndHorizontal();
        return result;
    }

    public override int IntField(string name, Texture2D texture2D, int input, GUIStyle guiStyle = null)
    {
        guiStyle = guiStyle != null ? guiStyle : GUI.skin.label;
        if (texture2D == null)
            return IntField(name, input, guiStyle);

        EditorGUILayout.BeginVertical();
        EditorGUILayout.BeginHorizontal("helpbox", GUILayout.Width(80));

        GUILayout.Button("", GUI.skin.label, GUILayout.Width(50), GUILayout.Height(50));
        Rect lastRect = GUILayoutUtility.GetLastRect();
        GUI.DrawTexture(lastRect, texture2D);

        EditorGUILayout.BeginVertical();
        EditorGUI.indentLevel++;
        EditorGUILayout.LabelField(name, guiStyle, GUILayout.Width(150));
        var result = EditorGUILayout.IntField(input, "helpbox", GUILayout.Width(90));
        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();

        return result;
    }
    #endregion

    #region[FloatFiled]
    public override float FloatField(string name, float input, GUIStyle guiStyle = null)
    {
        guiStyle = guiStyle != null ? guiStyle : GUI.skin.label;
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(name, guiStyle, GUILayout.Width(470));

        var result = EditorGUILayout.FloatField(input, GUILayout.Width(90));
        EditorGUILayout.EndHorizontal();
        return result;
    }

    public override float FloatField(float input, GUIStyle guiStyle = null)
    {
        guiStyle = guiStyle != null ? guiStyle : GUI.skin.label;
        EditorGUILayout.BeginHorizontal();

        var result = EditorGUILayout.FloatField(input, GUILayout.Width(90));
        EditorGUILayout.EndHorizontal();
        return result;
    }

    public override float FloatField(Texture2D texture2D, float input, GUIStyle guiStyle = null)
    {
        guiStyle = guiStyle != null ? guiStyle : GUI.skin.label;
        if (texture2D == null)
            return FloatField(input, guiStyle);

        EditorGUILayout.BeginHorizontal();
        GUI.DrawTexture(new Rect(0, 0, 30, 30), texture2D);
        var result = EditorGUILayout.FloatField(input, GUILayout.Width(90));
        EditorGUILayout.EndHorizontal();
        return result;
    }

    public override float FloatField(string name, Texture2D texture2D, float input, GUIStyle guiStyle = null)
    {
        guiStyle = guiStyle != null ? guiStyle : GUI.skin.label;
        if (texture2D == null)
            return FloatField(name, input, guiStyle);

        EditorGUILayout.BeginVertical();
        EditorGUILayout.BeginHorizontal("helpbox", GUILayout.Width(80));

        GUILayout.Button("", GUI.skin.label, GUILayout.Width(50), GUILayout.Height(50));
        Rect lastRect = GUILayoutUtility.GetLastRect();
        GUI.DrawTexture(lastRect, texture2D);

        EditorGUILayout.BeginVertical();
        EditorGUI.indentLevel++;
        EditorGUILayout.LabelField(name, guiStyle, GUILayout.Width(150));
        var result = EditorGUILayout.FloatField(input, "helpbox", GUILayout.Width(90));
        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();

        return result;
    }
    #endregion
}