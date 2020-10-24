using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[CustomEditor(typeof(ProlgueManager))]
public class PrologueManagerEditor : MyEditor
{
    public ProlgueManager prolgueManager;

    Vector2 sceneScroll;

    #region[OnEnable]
    private void OnEnable()
    {
        prolgueManager = target as ProlgueManager;
    }
    #endregion

    #region[OnInspectorGUI]
    public override void OnInspectorGUI()
    {
        //변경사항 검사시작
        EditorGUI.BeginChangeCheck();

        GUI.color = Color.white;
        EditorGUILayout.BeginVertical("box");
        sceneScroll = EditorGUILayout.BeginScrollView(sceneScroll, GUILayout.Width(275), GUILayout.Height(300));
        for (int i = 0; i < prolgueManager.prolgueDatas.Count; i++)
            SetScene(i, (i + 1) + "번째 장면", ref prolgueManager.prolgueDatas[i].context, ref prolgueManager.prolgueDatas[i].context_Eng, ref prolgueManager.prolgueDatas[i].sprite);

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("장면 추가", GUILayout.Width(200), GUILayout.Height(25)))
            prolgueManager.prolgueDatas.Add(new ProlgueManager.ProlgueData());
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(prolgueManager, "ChangeGroundManaer");
            var prefabStage = UnityEditor.Experimental.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage();

            if (prefabStage != null)
                EditorSceneManager.MarkSceneDirty(prefabStage.scene);
            else
            {
                EditorUtility.SetDirty(prolgueManager);
                PrefabUtility.RecordPrefabInstancePropertyModifications(this);
                EditorSceneManager.MarkSceneDirty(prolgueManager.gameObject.scene);
            }
        }
    }
    #endregion

    #region[장면 설정]
    void SetScene(int n, string name, ref string context, ref string context_Eng, ref Sprite stageBackGround)
    {
        GUI.color = new Color(217 / 255f, 240 / 255f, 247 / 255f);
        EditorGUILayout.BeginVertical("helpbox", GUILayout.Width(220));
        EditorGUILayout.BeginHorizontal();
        GUIStyle nameStyle = new GUIStyle("label")
        {
            fontSize = 20,
            fontStyle = FontStyle.Bold,
            font = fontStarDust
        };
        nameStyle.normal.textColor = new Color(63 / 255f, 72 / 255f, 204 / 255f);
        EditorGUILayout.LabelField(name, nameStyle, GUILayout.Width(190), GUILayout.Height(30));
        GUIStyle removeStyle = new GUIStyle("label")
        {
            fontSize = 15,
            alignment = TextAnchor.MiddleCenter
        };
        if (GUILayout.Button("Χ", removeStyle, GUILayout.Width(30), GUILayout.Height(30)))
        {
            prolgueManager.prolgueDatas.RemoveAt(n);
            return;
        }
        EditorGUILayout.EndHorizontal();
        GUI.color = Color.white;
        EditorGUILayout.BeginHorizontal("box");
        if (stageBackGround)
        {
            GUILayout.Button("", GUI.skin.label, GUILayout.Width(200), GUILayout.Height(100));
            Rect lastRect = GUILayoutUtility.GetLastRect();
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
        EditorGUILayout.LabelField("장면 텍스트", GUILayout.Width(150));
        GUI.color = Color.white;
        context = EditorGUILayout.TextArea(context, "helpbox", GUILayout.Width(140));
        EditorGUILayout.LabelField("장면 텍스트<영어>", GUILayout.Width(150));
        context_Eng = EditorGUILayout.TextArea(context_Eng, "helpbox", GUILayout.Width(140));

        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("장면 배경", GUILayout.Width(60));
        stageBackGround = (Sprite)EditorGUILayout.ObjectField(stageBackGround, typeof(Sprite), false, GUILayout.Width(60), GUILayout.Height(60));
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();
    }
    #endregion
}