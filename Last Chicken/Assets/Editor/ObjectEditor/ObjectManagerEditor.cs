using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Collections.Generic;
using UnityEngine;

[CustomEditor(typeof(ObjectManager))]
public class ObjectManagerEditor : MyEditor
{
    public ObjectManager objectManager;

    public Texture2D ladderImg;
    public Texture2D minBoxImg;
    public Texture2D mineCartImg;
    public Texture2D stoneImg0;
    public Texture2D stoneImg1;
    public Texture2D wagonImg;
    public Texture2D woodBoardImg0;
    public Texture2D woodBoardImg1;
    public Texture2D shovelImg;

    public Texture2D woodBoxImg;

    public Texture2D treasureBoxImg;

    public Texture2D stalagmite0;
    public Texture2D stalagmite1;
    public Texture2D stalagmite2;
    public Texture2D stalagmite3;
    public Texture2D stalagmite4;
    public Texture2D stalagmite5;

    Vector2 stageScroll;
    Rect lastRect;

    #region[OnEnable]
    private void OnEnable()
    {
        objectManager = target as ObjectManager;
    }
    #endregion

    #region[OnInspectorGUI]
    public override void OnInspectorGUI()
    {
        //변경사항 검사시작
        EditorGUI.BeginChangeCheck();

        GUIStyle dataStyle = new GUIStyle("label")
        {
            fontSize = 10,
            font = fontStarDust
        };

        #region[데이터 변경]
        if (objectManager.obejctData.Length < ObjectManager.objectName.Length)
        {
            List<ObjectManager.ObjectStats> temp = new List<ObjectManager.ObjectStats>();
            for (int i = 0; i < objectManager.obejctData.Length; i++)
            {
                ObjectManager.ObjectStats emp = new ObjectManager.ObjectStats();
                emp.Hp = objectManager.obejctData[i].Hp;
                emp.ObjectType = objectManager.obejctData[i].ObjectType;
                emp.SpecialType = objectManager.obejctData[i].SpecialType;
                emp.DamageSound = objectManager.obejctData[i].DamageSound;
                temp.Add(emp);
            }

            objectManager.obejctData = new ObjectManager.ObjectStats[ObjectManager.objectName.Length];
            for (int i = 0; i < ObjectManager.objectName.Length; i++)
                objectManager.obejctData[i] = new ObjectManager.ObjectStats();

            for (int i = 0; i < temp.Count; i++)
            {
                objectManager.obejctData[i].Hp = temp[i].Hp;
                objectManager.obejctData[i].ObjectType = temp[i].ObjectType;
                objectManager.obejctData[i].SpecialType = temp[i].SpecialType;
                objectManager.obejctData[i].DamageSound = temp[i].DamageSound;
            }
        }
        else if (objectManager.obejctData.Length > ObjectManager.objectName.Length)
        {
            List<ObjectManager.ObjectStats> temp = new List<ObjectManager.ObjectStats>();
            for (int i = 0; i < objectManager.obejctData.Length; i++)
            {
                ObjectManager.ObjectStats emp = new ObjectManager.ObjectStats();
                emp.Hp = objectManager.obejctData[i].Hp;
                emp.ObjectType = objectManager.obejctData[i].ObjectType;
                emp.SpecialType = objectManager.obejctData[i].SpecialType;
                emp.DamageSound = objectManager.obejctData[i].DamageSound;
                temp.Add(emp);
            }

            objectManager.obejctData = new ObjectManager.ObjectStats[ObjectManager.objectName.Length];
            for (int i = 0; i < ObjectManager.objectName.Length; i++)
                objectManager.obejctData[i] = new ObjectManager.ObjectStats();

            for (int i = 0; i < ObjectManager.objectName.Length; i++)
            {
                objectManager.obejctData[i].Hp = temp[i].Hp;
                objectManager.obejctData[i].ObjectType = temp[i].ObjectType;
                objectManager.obejctData[i].SpecialType = temp[i].SpecialType;
                objectManager.obejctData[i].DamageSound = temp[i].DamageSound;
            }
        }
        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        GUI.color = Color.white;
        EditorGUILayout.BeginVertical("box");
        stageScroll = EditorGUILayout.BeginScrollView(stageScroll, GUILayout.Width(275), GUILayout.Height(300));

        for (int i = 0; i < ObjectManager.objectName.Length; i++)
        {
            EditorGUILayout.BeginVertical("helpbox", GUILayout.Width(250));
            ObjectData(ref objectManager.obejctData[ObjectManager.FindData(ObjectManager.objectName[i])], getTexture(ObjectManager.objectName[i]), getName(ObjectManager.objectName[i]));
            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(objectManager, "ChangeObjectManaer");
            var prefabStage = UnityEditor.Experimental.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage();

            if (prefabStage != null)
                EditorSceneManager.MarkSceneDirty(prefabStage.scene);
            else
            {
                EditorUtility.SetDirty(objectManager);
                PrefabUtility.RecordPrefabInstancePropertyModifications(this);
                EditorSceneManager.MarkSceneDirty(objectManager.gameObject.scene);
            }
        }
    }
    #endregion

    #region[오브젝트 데이터]
    void ObjectData(ref ObjectManager.ObjectStats data, Texture2D objectImg, string emp)
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

        EditorGUILayout.BeginHorizontal();

        if (objectImg != null)
        {
            Vector2 size = new Vector2(2f, 2f);
            EditorGUILayout.LabelField(emp, nameStyle, GUILayout.Width(240 - size.x * objectImg.width), GUILayout.Height(30));
            GUILayout.Button("", GUI.skin.label, GUILayout.Width(objectImg.width * size.x), GUILayout.Height(objectImg.height * size.y));
            Rect lastRect = GUILayoutUtility.GetLastRect();
            GUI.DrawTexture(lastRect, objectImg);
        }
        else
        {
            EditorGUILayout.LabelField(emp, nameStyle, GUILayout.Width(240), GUILayout.Height(30));
        }
        EditorGUILayout.EndHorizontal();

        data.Hp = IntField("체력", data.Hp, dataStyle);
        data.ObjectType = (StructureObject.ObjectType)EnumField("오브젝트 타입",data.ObjectType, dataStyle);
        data.SpecialType = (StructureObject.SpecialType)EnumField("오브젝트 효과", data.SpecialType, dataStyle);
        data.DamageSound = (StructureObject.DamageSound)EnumField("오브젝트 소리", data.DamageSound, dataStyle);
    }
    #endregion

    #region[인스펙터에 표시할 데이터]
    string getName(string s)
    {
        switch (s)
        {
            case "Ladder":
                return "사다리";
            case "MineBox":
                return "광물박스";
            case "MineCart":
                return "광산카트";
            case "Stone0":
                return "돌무더기1";
            case "Stone1":
                return "돌무더기2";
            case "Wagon":
                return "수레";
            case "WoodBoard0":
                return "나무판자1";
            case "WoodBoard1":
                return "나무판자2";
            case "Shovel":
                return "삽";
            case "WoodBox":
                return "나무상자";
            case "TreasureBox":
                return "보물상자";
            case "Stalagmite0":
                return "석순1";
            case "Stalagmite1":
                return "석순2";
            case "Stalagmite2":
                return "석순3";
            case "Stalagmite3":
                return "석순4";
            case "Stalagmite4":
                return "석순5";
            case "Stalagmite5":
                return "석순6";
            case "Sign":
                return "표지판";
        }
        return s;
    }

    Texture2D getTexture(string s)
    {
        switch (s)
        {
            case "Ladder":
                return ladderImg;
            case "MineBox":
                return minBoxImg;
            case "MineCart":
                return mineCartImg;
            case "Stone0":
                return stoneImg0;
            case "Stone1":
                return stoneImg1;
            case "Wagon":
                return wagonImg;
            case "WoodBoard0":
                return woodBoardImg0;
            case "WoodBoard1":
                return woodBoardImg1;
            case "Shovel":
                return shovelImg;
            case "WoodBox":
                return woodBoxImg;
            case "TreasureBox":
                return treasureBoxImg;
            case "Stalagmite0":
                return stalagmite0;
            case "Stalagmite1":
                return stalagmite1;
            case "Stalagmite2":
                return stalagmite2;
            case "Stalagmite3":
                return stalagmite3;
            case "Stalagmite4":
                return stalagmite4;
            case "Stalagmite5":
                return stalagmite5;
        }
        return null;
    }
    #endregion
}