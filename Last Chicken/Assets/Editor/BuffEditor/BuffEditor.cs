using System;
using UnityEditor;
using System.Collections.Generic;
using UnityEditor.SceneManagement;

using UnityEngine;

[CustomEditor(typeof(BuffManager))]
public class BuffEditor : MyEditor
{
    public BuffManager buffManager;

    Vector2 stageScroll;
    Rect lastRect;

    #region[OnEnable]
    private void OnEnable()
    {
        buffManager = target as BuffManager;
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

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        #region[데이터 변경]
        if (buffManager.buffData.Length < BuffManager.buffName.Length)
        {
            List<BuffManager.BuffStats> temp = new List<BuffManager.BuffStats>();
            for(int i = 0; i < buffManager.buffData.Length; i++)
            {
                BuffManager.BuffStats emp = new BuffManager.BuffStats();
                emp.BuffImg = buffManager.buffData[i].BuffImg;
                emp.buffColor = buffManager.buffData[i].buffColor;
                emp.buffGlow = buffManager.buffData[i].buffGlow;
                emp.buffName = buffManager.buffData[i].buffName;
                emp.buffName_Eng = buffManager.buffData[i].buffName_Eng;
                emp.buffExplain = buffManager.buffData[i].buffExplain;
                emp.buffExplain_Eng = buffManager.buffData[i].buffExplain_Eng;
                emp.value = buffManager.buffData[i].value;
                emp.Overlap = buffManager.buffData[i].Overlap;
                emp.time = buffManager.buffData[i].time;
                emp.limitTime = buffManager.buffData[i].limitTime;
                temp.Add(emp);
            }

            buffManager.buffData = new BuffManager.BuffStats[BuffManager.buffName.Length];
            for (int i = 0; i < BuffManager.buffName.Length; i++)
                buffManager.buffData[i] = new BuffManager.BuffStats();

            for (int i = 0; i < temp.Count; i++)
            {
                buffManager.buffData[i].BuffImg = temp[i].BuffImg;
                buffManager.buffData[i].buffColor = temp[i].buffColor;
                buffManager.buffData[i].buffGlow = temp[i].buffGlow;
                buffManager.buffData[i].buffName = temp[i].buffName;
                buffManager.buffData[i].buffName_Eng = temp[i].buffName_Eng;
                buffManager.buffData[i].buffExplain = temp[i].buffExplain;
                buffManager.buffData[i].buffExplain_Eng = temp[i].buffExplain_Eng;
                buffManager.buffData[i].value = temp[i].value;
                buffManager.buffData[i].Overlap = temp[i].Overlap;
                buffManager.buffData[i].time = temp[i].time;
                buffManager.buffData[i].limitTime = temp[i].limitTime;
            }
        }
        else if (buffManager.buffData.Length > BuffManager.buffName.Length)
        {
            List<BuffManager.BuffStats> temp = new List<BuffManager.BuffStats>();
            for (int i = 0; i < buffManager.buffData.Length; i++)
            {
                BuffManager.BuffStats emp = new BuffManager.BuffStats();
                emp.BuffImg = buffManager.buffData[i].BuffImg;
                emp.buffColor = buffManager.buffData[i].buffColor;
                emp.buffGlow = buffManager.buffData[i].buffGlow;
                emp.buffName = buffManager.buffData[i].buffName;
                emp.buffName_Eng = buffManager.buffData[i].buffName_Eng;
                emp.buffExplain = buffManager.buffData[i].buffExplain;
                emp.buffExplain_Eng = buffManager.buffData[i].buffExplain_Eng;
                emp.value = buffManager.buffData[i].value;
                emp.Overlap = buffManager.buffData[i].Overlap;
                emp.time = buffManager.buffData[i].time;
                emp.limitTime = buffManager.buffData[i].limitTime;
                temp.Add(emp);
            }

            buffManager.buffData = new BuffManager.BuffStats[BuffManager.buffName.Length];
            for (int i = 0; i < BuffManager.buffName.Length; i++)
                buffManager.buffData[i] = new BuffManager.BuffStats();

            for (int i = 0; i < BuffManager.buffName.Length; i++)
            {
                buffManager.buffData[i].BuffImg = temp[i].BuffImg;
                buffManager.buffData[i].buffColor = temp[i].buffColor;
                buffManager.buffData[i].buffGlow = temp[i].buffGlow;
                buffManager.buffData[i].buffName = temp[i].buffName;
                buffManager.buffData[i].buffName_Eng = temp[i].buffName_Eng;
                buffManager.buffData[i].buffExplain = temp[i].buffExplain;
                buffManager.buffData[i].buffExplain_Eng = temp[i].buffExplain_Eng;
                buffManager.buffData[i].value = temp[i].value;
                buffManager.buffData[i].Overlap = temp[i].Overlap;
                buffManager.buffData[i].time = temp[i].time;
                buffManager.buffData[i].limitTime = temp[i].limitTime;
            }
        }
        #endregion

        GUI.color = Color.white;
        EditorGUILayout.BeginVertical("box");
        stageScroll = EditorGUILayout.BeginScrollView(stageScroll, GUILayout.Width(330), GUILayout.Height(600));

        for (int i = 0; i < BuffManager.buffName.Length; i++)
        {
            EditorGUILayout.BeginVertical("helpbox", GUILayout.Width(275));
            BuffData(ref buffManager.buffData[BuffManager.FindData(BuffManager.buffName[i])], getName(BuffManager.buffName[i]));
            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(buffManager, "ChangeBuffManaer");
            var prefabStage = UnityEditor.Experimental.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage();

            if (prefabStage != null)
                EditorSceneManager.MarkSceneDirty(prefabStage.scene);
            else
            {
                EditorUtility.SetDirty(buffManager);
                PrefabUtility.RecordPrefabInstancePropertyModifications(this);
                EditorSceneManager.MarkSceneDirty(buffManager.gameObject.scene);
            }
        }
    }
    #endregion

    #region[버프 데이터]
    void BuffData(ref BuffManager.BuffStats data, string emp)
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
        Vector2 size = new Vector2(2f, 2f);
        EditorGUILayout.LabelField(emp, nameStyle, GUILayout.Width(300 - 30 * size.x), GUILayout.Height(30));
        data.BuffImg = (Sprite)EditorGUILayout.ObjectField(data.BuffImg, typeof(Sprite), false, GUILayout.Width(30 * size.x), GUILayout.Height(30 * size.y));

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("버프 이름", dataStyle, GUILayout.Width(120));
        data.buffName = EditorGUILayout.TextField(data.buffName);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Buff Name", dataStyle, GUILayout.Width(120));
        data.buffName_Eng = EditorGUILayout.TextField(data.buffName_Eng);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.LabelField("버프 설명", dataStyle);
        data.buffExplain = EditorGUILayout.TextArea(data.buffExplain, GUILayout.Width(300), GUILayout.Height(50));
        EditorGUILayout.LabelField("Buff Explain", dataStyle);
        data.buffExplain_Eng = EditorGUILayout.TextArea(data.buffExplain_Eng, GUILayout.Width(300), GUILayout.Height(50));

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("버프 컬러", dataStyle, GUILayout.Width(200));
        data.buffColor = EditorGUILayout.ColorField(data.buffColor);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        data.buffGlow = FloatField("버프 Glow", data.buffGlow, dataStyle);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        data.value = IntField("버프 수치", data.value, dataStyle);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        data.Overlap = ToggleField("버프 중첩", data.Overlap, dataStyle);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        data.limitTime = ToggleField("버프 제한시간", data.limitTime, dataStyle);
        EditorGUILayout.EndHorizontal();

        if (data.limitTime)
        {
            EditorGUILayout.BeginHorizontal();
            data.time = FloatField("버프유지시간(s)", data.time, dataStyle);
            EditorGUILayout.EndHorizontal();
        }
        else
        {
            data.time = 60;
        }
    }
    #endregion

    #region[ToggleField]
    public override bool ToggleField(string name, bool input, GUIStyle guiStyle = null)
    {
        guiStyle = guiStyle != null ? guiStyle : GUI.skin.label;

        EditorGUILayout.BeginVertical();
        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.LabelField(name, guiStyle, GUILayout.Width(200));
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
        EditorGUILayout.LabelField(name, guiStyle, GUILayout.Width(200));

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
        EditorGUILayout.LabelField(name, guiStyle, GUILayout.Width(200));

        var result = EditorGUILayout.IntField(input, GUILayout.Width(100));
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
        EditorGUILayout.LabelField(name, guiStyle, GUILayout.Width(200));

        var result = EditorGUILayout.FloatField(input, GUILayout.Width(100));
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

    #region[인스펙터에 표시할 데이터]
    string getName(string s)
    {
        switch (s)
        {
            case "Power":
                return "공격력 버프";
            case "Shield":
                return "방어막 버프";
            case "Speed":
                return "스피드 버프";
            case "AttackSpeed":
                return "공격속도 버프";
            case "Luminous":
                return "닭 광원 버프";
        }
        return s;
    }
    #endregion
}