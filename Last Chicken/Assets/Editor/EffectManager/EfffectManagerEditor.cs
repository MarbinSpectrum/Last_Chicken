using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[CustomEditor(typeof(EffectManager))]
public class EfffectManagerEditor : MyEditor
{
    public EffectManager efffectManager;

    Vector2 mineScroll;

    #region[OnEnable]
    private void OnEnable()
    {
        efffectManager = target as EffectManager;
    }
    #endregion

    #region[OnInspectorGUI]
    public override void OnInspectorGUI()
    {
        //변경사항 검사시작
        EditorGUI.BeginChangeCheck();

        GUI.color = Color.white;

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        mineScroll = EditorGUILayout.BeginScrollView(mineScroll, GUILayout.Width(260), GUILayout.Height(300));

        #region[진동]
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.LabelField("진동 이펙트", GUILayout.Width(128));
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        EditorGUILayout.BeginVertical("helpbox", GUILayout.Width(220));

        #region[폭발진동]
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("폭발진동", GUILayout.Width(128));
        EditorGUILayout.BeginVertical("helpbox");
        efffectManager.boomExplosionVibration.power = FloatField("진동크기",efffectManager.boomExplosionVibration.power);
        efffectManager.boomExplosionVibration.num = IntField("진동수",efffectManager.boomExplosionVibration.num);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndVertical();
        #endregion

        #region[몬스터진동]
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("몬스터진동", GUILayout.Width(128));
        EditorGUILayout.BeginVertical("helpbox");
        efffectManager.monsterDeadVibration.power = FloatField("진동크기", efffectManager.monsterDeadVibration.power);
        efffectManager.monsterDeadVibration.num = IntField("진동수", efffectManager.monsterDeadVibration.num);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndVertical();
        #endregion

        EditorGUILayout.EndVertical();
        EditorGUILayout.EndVertical();
        #endregion

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        EditorGUILayout.EndScrollView();

        //변경사항 검사종료
        if (EditorGUI.EndChangeCheck())
        {
            //변경사항체크
            Undo.RecordObject(efffectManager, "ChangeEffectManaer");
            var prefabStage = UnityEditor.Experimental.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage();

            if (prefabStage != null)
                EditorSceneManager.MarkSceneDirty(prefabStage.scene);
            else
            {
                EditorUtility.SetDirty(efffectManager);
                PrefabUtility.RecordPrefabInstancePropertyModifications(this);
                EditorSceneManager.MarkSceneDirty(efffectManager.gameObject.scene);
            }
        }
    }
    #endregion

    public override float FloatField(string name, float input, GUIStyle guiStyle = null)
    {
        guiStyle = guiStyle != null ? guiStyle : GUI.skin.label;
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(name, guiStyle, GUILayout.Width(90));

        var result = EditorGUILayout.FloatField(input, GUILayout.Width(90));
        EditorGUILayout.EndHorizontal();
        return result;
    }

    public override int IntField(string name, int input, GUIStyle guiStyle = null)
    {
        guiStyle = guiStyle != null ? guiStyle : GUI.skin.label;
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(name, guiStyle, GUILayout.Width(90));

        var result = EditorGUILayout.IntField(input, GUILayout.Width(90));
        EditorGUILayout.EndHorizontal();
        return result;
    }
}