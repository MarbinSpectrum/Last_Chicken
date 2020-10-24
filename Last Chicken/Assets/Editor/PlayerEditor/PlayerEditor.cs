using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[CustomEditor(typeof(Player))]
public class PlayerEditor : UnityEditor.Editor
{
    public Player player;

    public Texture2D attackSpeed;
    public Texture2D speed;
    public Texture2D jumpPower;
    public Texture2D gravity;

    #region[OnEnable]
    private void OnEnable()
    {
        player = target as Player;
    }
    #endregion

    #region[OnInspectorGUI]
    public override void OnInspectorGUI()
    {
        //변경사항 검사시작
        EditorGUI.BeginChangeCheck();

        EditorGUILayout.BeginHorizontal(GUILayout.Width(160), GUILayout.Height(50));
        player.baseAttackSpeed = FloatField("공격속도", attackSpeed, player.baseAttackSpeed);
        player.baseSpeed = FloatField("이동속도", speed, player.baseSpeed);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal(GUILayout.Width(160), GUILayout.Height(50));
        player.baseJumpPower = FloatField("점프력", jumpPower, player.baseJumpPower);
        player.baseGravity = FloatField("중력", gravity, player.baseGravity);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal(GUILayout.Width(160), GUILayout.Height(50));
        player.baseAttackPower = IntField("공격력", attackSpeed, player.baseAttackPower);
        EditorGUILayout.EndHorizontal();

        //변경사항 검사종료
        if (EditorGUI.EndChangeCheck())
        {
            //변경사항체크
            Undo.RecordObject(player, "ChangePlayer");
            var prefabStage = UnityEditor.Experimental.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage();

            if (prefabStage != null)
                EditorSceneManager.MarkSceneDirty(prefabStage.scene);
            else
            {
                EditorUtility.SetDirty(player);
                PrefabUtility.RecordPrefabInstancePropertyModifications(this);
                EditorSceneManager.MarkSceneDirty(player.gameObject.scene);
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