using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using Custom;

[CustomEditor(typeof(Chicken))]
public class ChickenEditor : UnityEditor.Editor
{
    public Chicken chicken;

    public Texture2D speed;
    public Texture2D jumpPower;
    public Texture2D gravity;

    #region[OnEnable]
    private void OnEnable()
    {
        chicken = target as Chicken;
    }
    #endregion

    #region[OnInspectorGUI]
    public override void OnInspectorGUI()
    {
        //변경사항 검사시작
        EditorGUI.BeginChangeCheck();

        EditorGUILayout.BeginHorizontal(GUILayout.Width(160), GUILayout.Height(50));
        chicken.baseSpeed = FloatField("이동속도", speed, chicken.baseSpeed);
        chicken.baseJumpPower = FloatField("점프력", jumpPower, chicken.baseJumpPower);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal(GUILayout.Width(160), GUILayout.Height(50));
        chicken.baseGravity = FloatField("중력", gravity, chicken.baseGravity);
        EditorGUILayout.EndHorizontal();
        //변경사항 검사종료
        if (EditorGUI.EndChangeCheck())
        {
            //변경사항체크
            Undo.RecordObject(chicken, "ChangePlayer");
            var prefabStage = UnityEditor.Experimental.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage();

            if (prefabStage != null)
                EditorSceneManager.MarkSceneDirty(prefabStage.scene);
            else
            {
                EditorUtility.SetDirty(chicken);
                PrefabUtility.RecordPrefabInstancePropertyModifications(this);
                EditorSceneManager.MarkSceneDirty(chicken.gameObject.scene);
            }
        }
    }
    #endregion

    #region[IntField]
    int IntField(string name, int input)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(name, GUILayout.Width(100));

        var result = EditorGUILayout.IntField(input, GUILayout.Width(60));
        EditorGUILayout.EndHorizontal();
        return result;
    }

    int IntField(int input)
    {
        EditorGUILayout.BeginHorizontal();

        var result = EditorGUILayout.IntField(input, GUILayout.Width(60));
        EditorGUILayout.EndHorizontal();
        return result;
    }

    int IntField(Texture2D texture2D, int input)
    {
        if (texture2D == null)
            return IntField(input);

        EditorGUILayout.BeginHorizontal();
        GUI.DrawTexture(new Rect(0, 0, 30, 30), texture2D);
        var result = EditorGUILayout.IntField(input, GUILayout.Width(60));
        EditorGUILayout.EndHorizontal();
        return result;
    }

    int IntField(string name, Texture2D texture2D, int input)
    {
        if (texture2D == null)
            return IntField(name, input);

        EditorGUILayout.BeginVertical();
        EditorGUILayout.BeginHorizontal("helpbox", GUILayout.Width(80));

        GUILayout.Button("", GUI.skin.label, GUILayout.Width(50), GUILayout.Height(50));
        Rect lastRect = GUILayoutUtility.GetLastRect();
        GUI.DrawTexture(lastRect, texture2D);

        EditorGUILayout.BeginVertical();
        EditorGUI.indentLevel++;
        EditorGUILayout.LabelField(name, GUILayout.Width(70));
        var result = EditorGUILayout.IntField(input, "helpbox", GUILayout.Width(60));
        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();

        return result;
    }
    #endregion

    #region[FloatFiled]
    float FloatField(string name, float input)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(name, GUILayout.Width(100));

        var result = EditorGUILayout.FloatField(input, GUILayout.Width(60));
        EditorGUILayout.EndHorizontal();
        return result;
    }

    float FloatField(float input)
    {
        EditorGUILayout.BeginHorizontal();

        var result = EditorGUILayout.FloatField(input, GUILayout.Width(60));
        EditorGUILayout.EndHorizontal();
        return result;
    }

    float FloatField(Texture2D texture2D, float input)
    {
        if (texture2D == null)
            return FloatField(input);

        EditorGUILayout.BeginHorizontal();
        GUI.DrawTexture(new Rect(0, 0, 30, 30), texture2D);
        var result = EditorGUILayout.FloatField(input, GUILayout.Width(60));
        EditorGUILayout.EndHorizontal();
        return result;
    }

    float FloatField(string name, Texture2D texture2D, float input)
    {
        if (texture2D == null)
            return FloatField(name, input);

        EditorGUILayout.BeginVertical();
        EditorGUILayout.BeginHorizontal("helpbox", GUILayout.Width(80));

        GUILayout.Button("", GUI.skin.label, GUILayout.Width(50), GUILayout.Height(50));
        Rect lastRect = GUILayoutUtility.GetLastRect();
        GUI.DrawTexture(lastRect, texture2D);

        EditorGUILayout.BeginVertical();
        EditorGUI.indentLevel++;
        EditorGUILayout.LabelField(name, GUILayout.Width(70));
        var result = EditorGUILayout.FloatField(input, "helpbox", GUILayout.Width(60));
        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();

        return result;
    }
    #endregion

}