using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(StageManager))]
public class StageManagerEditor : MyEditor
{
    public StageManager stageManager;

    Vector2 stageScroll;

    Rect lastRect;

    #region[OnEnable]
    private void OnEnable()
    {
        stageManager = target as StageManager;
    }
    #endregion

    #region[OnInspectorGUI]
    public override void OnInspectorGUI()
    {
        //변경사항 검사시작
        EditorGUI.BeginChangeCheck();

        GUI.color = Color.white;
        EditorGUILayout.BeginVertical("box");
        stageScroll = EditorGUILayout.BeginScrollView(stageScroll, GUILayout.Width(275), GUILayout.Height(300));
        SetStage("튜 토 리 얼", ref stageManager.tutorial_Name, ref stageManager.tutorial_Name_Eng, ref stageManager.tutorial_BackGround);
        SetStage("스테이지 1-1", ref stageManager.stage0101_Name, ref stageManager.stage0101_Name_Eng, ref stageManager.stage0101_BackGround, ref stageManager.stage0101_Monsters, ref stageManager.stage0101_ObjectValue, ref stageManager.stage0101_WoodBoxValue, ref stageManager.stage0101_TrapValue);
        SetStage("스테이지 1-2", ref stageManager.stage0102_Name, ref stageManager.stage0102_Name_Eng, ref stageManager.stage0102_BackGround, ref stageManager.stage0102_Monsters, ref stageManager.stage0102_ObjectValue, ref stageManager.stage0102_WoodBoxValue, ref stageManager.stage0102_TrapValue);
        SetStage("스테이지 1-3", ref stageManager.stage0103_Name, ref stageManager.stage0103_Name_Eng, ref stageManager.stage0103_BackGround, ref stageManager.stage0103_Monsters, ref stageManager.stage0103_ObjectValue, ref stageManager.stage0103_WoodBoxValue, ref stageManager.stage0103_TrapValue);
        SetStage("스테이지 2-1", ref stageManager.stage0201_Name, ref stageManager.stage0201_Name_Eng, ref stageManager.stage0201_BackGround, ref stageManager.stage0201_Monsters, ref stageManager.stage0201_ObjectValue, ref stageManager.stage0201_WoodBoxValue, ref stageManager.stage0201_TrapValue);
        SetStage("스테이지 2-2", ref stageManager.stage0202_Name, ref stageManager.stage0202_Name_Eng, ref stageManager.stage0202_BackGround, ref stageManager.stage0202_Monsters, ref stageManager.stage0202_ObjectValue, ref stageManager.stage0202_WoodBoxValue, ref stageManager.stage0202_TrapValue);
        SetStage("스테이지 2-3", ref stageManager.stage0203_Name, ref stageManager.stage0203_Name_Eng, ref stageManager.stage0203_BackGround, ref stageManager.stage0203_Monsters, ref stageManager.stage0203_ObjectValue, ref stageManager.stage0203_WoodBoxValue, ref stageManager.stage0203_TrapValue);
        SetStage("이글루", ref stageManager.Igloo_Name, ref stageManager.Igloo_Name_Eng, ref stageManager.Igloo_BackGround);
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(stageManager, "ChangeGroundManaer");
            var prefabStage = UnityEditor.Experimental.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage();

            if (prefabStage != null)
                EditorSceneManager.MarkSceneDirty(prefabStage.scene);
            else
            {
                EditorUtility.SetDirty(stageManager);
                PrefabUtility.RecordPrefabInstancePropertyModifications(this);
                EditorSceneManager.MarkSceneDirty(stageManager.gameObject.scene);
            }
        }
    }
    #endregion

    #region[스테이지 설정]
    void SetStage(string name, ref string stageName, ref string stageName_Eng, ref Sprite stageBackGround, ref MonsterManager.SpawnMonster list, ref int objectValue, ref int woodBoxValue, ref int trapValue)
    {
        if(list.monsters.Length < MonsterManager.monsterName.Length)
        {
            List<bool> temp = new List<bool>();
            for (int i = 0; i < list.monsters.Length; i++)
                temp.Add(list.monsters[i]);
            list.monsters = new bool[MonsterManager.monsterName.Length];
            for (int i = 0; i < temp.Count; i++)
                list.monsters[i] = temp[i];

            List<int> tempInt = new List<int>();
            for (int i = 0; i < list.monsterValue.Length; i++)
                tempInt.Add(list.monsterValue[i]);
            list.monsterValue = new int[MonsterManager.monsterName.Length];
            for (int i = 0; i < tempInt.Count; i++)
                list.monsterValue[i] = tempInt[i];
        }
        else if (list.monsters.Length > MonsterManager.monsterName.Length)
        {
            List<bool> temp = new List<bool>();
            for (int i = 0; i < list.monsters.Length; i++)
                temp.Add(list.monsters[i]);
            list.monsters = new bool[MonsterManager.monsterName.Length];
            for (int i = 0; i < MonsterManager.monsterName.Length; i++)
                list.monsters[i] = temp[i];

            List<int> tempInt = new List<int>();
            for (int i = 0; i < list.monsterValue.Length; i++)
                tempInt.Add(list.monsterValue[i]);
            list.monsterValue = new int[MonsterManager.monsterName.Length];
            for (int i = 0; i < MonsterManager.monsterName.Length; i++)
                list.monsterValue[i] = tempInt[i];
        }

        GUI.color = new Color(217 / 255f, 240 / 255f, 247 / 255f);
        EditorGUILayout.BeginVertical("helpbox", GUILayout.Width(220));
        GUIStyle nameStyle = new GUIStyle("label")
        {
            fontSize = 20,
            fontStyle = FontStyle.Bold,
            font = fontStarDust
        };
        nameStyle.normal.textColor = new Color(63 / 255f, 72 / 255f, 204 / 255f);
        EditorGUILayout.LabelField(name, nameStyle, GUILayout.Width(220), GUILayout.Height(30));

        GUI.color = Color.white;
        EditorGUILayout.BeginHorizontal("box");
        if (stageBackGround)
        {
            GUILayout.Button("", GUI.skin.label, GUILayout.Width(200), GUILayout.Height(100));
            lastRect = GUILayoutUtility.GetLastRect();
            GUI.DrawTexture(lastRect, stageBackGround.texture);
        }
        else
        {
            GUIStyle backViewStyle = new GUIStyle("label")
            {
                fontSize = 10,
                font = fontStarDust,
                alignment = TextAnchor.MiddleCenter
            };
            nameStyle.normal.textColor = new Color(63 / 255f, 72 / 255f, 204 / 255f);
            GUILayout.Button("배경 미리보기", backViewStyle, GUILayout.Width(200), GUILayout.Height(100));
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("스테이지 이름", GUILayout.Width(100));
        GUI.color = Color.white;
        stageName = EditorGUILayout.TextArea(stageName, "helpbox", GUILayout.Width(100));
        EditorGUILayout.LabelField("Stage Name", GUILayout.Width(100));
        GUI.color = Color.white;
        stageName_Eng = EditorGUILayout.TextArea(stageName_Eng, "helpbox", GUILayout.Width(100));
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("스테이지 배경", GUILayout.Width(100));
        stageBackGround = (Sprite)EditorGUILayout.ObjectField(stageBackGround, typeof(Sprite), false, GUILayout.Width(60), GUILayout.Height(60));
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.LabelField("등장 몬스터", GUI.skin.label, GUILayout.Width(220), GUILayout.Height(15));
        EditorGUILayout.BeginVertical("box", GUILayout.Width(220));
        for (int i = 0; i < list.monsters.Length; i += 2)
        {
            EditorGUILayout.BeginHorizontal();
            for (int j = i; (j < i + 2 && j < list.monsters.Length); j++)
            {
                EditorGUILayout.BeginVertical("helpbox");

                EditorGUILayout.BeginHorizontal("box", GUILayout.Width(90));
                EditorGUILayout.LabelField(MonsterManagerEditor.getName(MonsterManager.monsterName[j]), GUI.skin.label, GUILayout.Width(75), GUILayout.Height(15));
                list.monsters[j] = EditorGUILayout.Toggle(list.monsters[j], GUILayout.Width(15));
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                int dataValue;
                EditorGUILayout.LabelField("스폰률(%)", GUILayout.Width(60));
                dataValue = EditorGUILayout.IntField(list.monsterValue[j], GUILayout.Width(30));
                dataValue = dataValue < 0 ? 0 : dataValue > 100 ? 100 : dataValue;
                list.monsterValue[j] = dataValue;
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();

        int tempValue;

        EditorGUILayout.BeginVertical("helpbox", GUILayout.Width(220));
        EditorGUILayout.LabelField("몬스터 생성 설정", GUI.skin.box, GUILayout.Width(220), GUILayout.Height(25));

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("몬스터 생성 수치", GUILayout.Width(125));
        tempValue = IntField(list.monsterNum);
        tempValue = tempValue < 0 ? 0 : tempValue > 100 ? 100 : tempValue;
        list.monsterNum = tempValue;
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.LabelField("몬스터 생성 간격", GUILayout.Width(125));
        tempValue = IntField(list.monsterDistance);
        tempValue = tempValue < 0 ? 0 : tempValue > 100 ? 100 : tempValue;
        list.monsterDistance = tempValue;
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("오브젝트 드랍률(%)", GUILayout.Width(125));
        tempValue = IntField(objectValue);
        tempValue = tempValue < 0 ? 0 : tempValue > 100 ? 100 : tempValue;
        objectValue = tempValue;
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("나무상자 드랍률(%)", GUILayout.Width(125));
        tempValue = IntField(woodBoxValue);
        tempValue = tempValue < 0 ? 0 : tempValue > 100 ? 100 : tempValue;
        woodBoxValue = tempValue;
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("함정 드랍률(%)", GUILayout.Width(125));
        tempValue = IntField(trapValue);
        tempValue = tempValue < 0 ? 0 : tempValue > 100 ? 100 : tempValue;
        trapValue = tempValue;
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();
    }

    void SetStage(string name, ref string stageName, ref string stageName_Eng, ref Sprite stageBackGround)
    {
        GUI.color = new Color(217 / 255f, 240 / 255f, 247 / 255f);
        EditorGUILayout.BeginVertical("helpbox", GUILayout.Width(220));
        GUIStyle nameStyle = new GUIStyle("label")
        {
            fontSize = 20,
            fontStyle = FontStyle.Bold,
            font = fontStarDust
        };
        nameStyle.normal.textColor = new Color(63 / 255f, 72 / 255f, 204 / 255f);
        EditorGUILayout.LabelField(name, nameStyle, GUILayout.Width(220), GUILayout.Height(30));

        GUI.color = Color.white;
        EditorGUILayout.BeginHorizontal("box");
        if (stageBackGround)
        {
            GUILayout.Button("", GUI.skin.label, GUILayout.Width(200), GUILayout.Height(100));
            lastRect = GUILayoutUtility.GetLastRect();
            GUI.DrawTexture(lastRect, stageBackGround.texture);
        }
        else
        {
            GUIStyle backViewStyle = new GUIStyle("label")
            {
                fontSize = 10,
                font = fontStarDust,
                alignment = TextAnchor.MiddleCenter
            };
            nameStyle.normal.textColor = new Color(63 / 255f, 72 / 255f, 204 / 255f);
            GUILayout.Button("배경 미리보기", backViewStyle, GUILayout.Width(200), GUILayout.Height(100));
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("스테이지 이름", GUILayout.Width(100));
        GUI.color = Color.white;
        stageName = EditorGUILayout.TextArea(stageName, "helpbox", GUILayout.Width(100));
        EditorGUILayout.LabelField("Stage Name", GUILayout.Width(100));
        GUI.color = Color.white;
        stageName_Eng = EditorGUILayout.TextArea(stageName_Eng, "helpbox", GUILayout.Width(100));
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("스테이지 배경", GUILayout.Width(100));
        stageBackGround = (Sprite)EditorGUILayout.ObjectField(stageBackGround, typeof(Sprite), false, GUILayout.Width(60), GUILayout.Height(60));
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();
    }
    #endregion
}