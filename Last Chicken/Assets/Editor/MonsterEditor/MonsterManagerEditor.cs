using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[CustomEditor(typeof(MonsterManager))]
public class MonsterManagerEditor : MyEditor
{
    public MonsterManager monsterManager;

    public Texture2D batImg;
    public Texture2D ratImg;
    public Texture2D snakeImg;

    Vector2 stageScroll;
    Rect lastRect;

    #region[OnEnable]
    private void OnEnable()
    {
        monsterManager = target as MonsterManager;
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
        if (monsterManager.monsterData.Length < MonsterManager.monsterName.Length)
        {
            List<MonsterManager.MonsterStats> temp = new List<MonsterManager.MonsterStats>();
            for (int i = 0; i < monsterManager.monsterData.Length; i++)
            {
                MonsterManager.MonsterStats emp = new MonsterManager.MonsterStats();
                emp.Hp = monsterManager.monsterData[i].Hp;
                emp.Speed = monsterManager.monsterData[i].Speed;
                emp.AttackPower = monsterManager.monsterData[i].AttackPower;
                emp.JumpPower = monsterManager.monsterData[i].JumpPower;
                temp.Add(emp);
            }

            monsterManager.monsterData = new MonsterManager.MonsterStats[MonsterManager.monsterName.Length];
            for (int i = 0; i < MonsterManager.monsterName.Length; i++)
                monsterManager.monsterData[i] = new MonsterManager.MonsterStats();

            for (int i = 0; i < temp.Count; i++)
            {
                monsterManager.monsterData[i].Hp = temp[i].Hp;
                monsterManager.monsterData[i].Speed = temp[i].Speed;
                monsterManager.monsterData[i].AttackPower = temp[i].AttackPower;
                monsterManager.monsterData[i].JumpPower = temp[i].JumpPower;
            }
        }
        else if (monsterManager.monsterData.Length > MonsterManager.monsterName.Length)
        {
            List<MonsterManager.MonsterStats> temp = new List<MonsterManager.MonsterStats>();
            for (int i = 0; i < monsterManager.monsterData.Length; i++)
            {
                MonsterManager.MonsterStats emp = new MonsterManager.MonsterStats();
                emp.Hp = monsterManager.monsterData[i].Hp;
                emp.Speed = monsterManager.monsterData[i].Speed;
                emp.AttackPower = monsterManager.monsterData[i].AttackPower;
                emp.JumpPower = monsterManager.monsterData[i].JumpPower;
                temp.Add(emp);
            }

            monsterManager.monsterData = new MonsterManager.MonsterStats[MonsterManager.monsterName.Length];
            for (int i = 0; i < MonsterManager.monsterName.Length; i++)
                monsterManager.monsterData[i] = new MonsterManager.MonsterStats();

            for (int i = 0; i < BuffManager.buffName.Length; i++)
            {
                monsterManager.monsterData[i].Hp = temp[i].Hp;
                monsterManager.monsterData[i].Speed = temp[i].Speed;
                monsterManager.monsterData[i].AttackPower = temp[i].AttackPower;
                monsterManager.monsterData[i].JumpPower = temp[i].JumpPower;
            }
        }
        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        GUI.color = Color.white;
        EditorGUILayout.BeginVertical("box");
        stageScroll = EditorGUILayout.BeginScrollView(stageScroll, GUILayout.Width(275), GUILayout.Height(300));

        for(int i = 0; i < MonsterManager.monsterName.Length; i++)
        {
            EditorGUILayout.BeginVertical("helpbox", GUILayout.Width(250));
            MonsterData(ref monsterManager.monsterData[MonsterManager.FindData(MonsterManager.monsterName[i])], getTexture(MonsterManager.monsterName[i]), getName(MonsterManager.monsterName[i]));
            if(MonsterManager.monsterName[i].Equals("Snake"))
            {
                EditorGUILayout.LabelField("점프가속도", dataStyle, GUILayout.Width(70));
                monsterManager.monsterData[MonsterManager.FindData(MonsterManager.monsterName[i])].JumpPower = 
                    EditorGUILayout.Vector2Field("", monsterManager.monsterData[MonsterManager.FindData(MonsterManager.monsterName[i])].JumpPower);
            }
            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(monsterManager, "ChangeMonsterManaer");
            var prefabStage = UnityEditor.Experimental.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage();

            if (prefabStage != null)
                EditorSceneManager.MarkSceneDirty(prefabStage.scene);
            else
            {
                EditorUtility.SetDirty(monsterManager);
                PrefabUtility.RecordPrefabInstancePropertyModifications(this);
                EditorSceneManager.MarkSceneDirty(monsterManager.gameObject.scene);
            }
        }
    }
    #endregion

    #region[몬스터 데이터]
    void MonsterData(ref MonsterManager.MonsterStats data, Texture2D monsterImg, string emp)
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

        if (monsterImg != null)
        {
            Vector2 size = new Vector2(2f, 2f);
            EditorGUILayout.LabelField(emp, nameStyle, GUILayout.Width(240 - size.x * monsterImg.width), GUILayout.Height(30));
            GUILayout.Button("", GUI.skin.label, GUILayout.Width(monsterImg.width * size.x), GUILayout.Height(monsterImg.height * size.y));
            Rect lastRect = GUILayoutUtility.GetLastRect();
            GUI.DrawTexture(lastRect, monsterImg);
        }
        else
        {
            EditorGUILayout.LabelField(emp, nameStyle, GUILayout.Width(240), GUILayout.Height(30));
        }
        EditorGUILayout.EndHorizontal();

        data.Hp = IntField("체력", data.Hp, dataStyle);
        data.Speed = FloatField("이동속도", data.Speed, dataStyle);
        float attackEmpty = FloatField("공격력", data.AttackPower, dataStyle);
        attackEmpty = (((int)(attackEmpty * 10)/5)*5)/10f;
        data.AttackPower = attackEmpty;
    }
    #endregion

    #region[인스펙터에 표시할 데이터]
    public static string getName(string s)
    {
        switch (s)
        {
            case "Bat":
                return "박쥐";
            case "Rat":
                return "쥐";
            case "Snake":
                return "뱀";
            case "Mole":
                return "두더지";
        }
        return "";
    }

    public Texture2D getTexture(string s)
    {
        switch(s)
        {
            case "Bat":
                return batImg;
            case "Rat":
                return ratImg;
            case "Snake":
                return snakeImg;
        }
        return null;
    }
    #endregion
}