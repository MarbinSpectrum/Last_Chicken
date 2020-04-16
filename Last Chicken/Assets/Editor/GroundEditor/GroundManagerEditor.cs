using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[CustomEditor(typeof(GroundManager))]
public class GroundManagerEditor : MyEditor
{
    public GroundManager groundManager;

    public int[] mineHp = new int[16];

    public Texture2D dirt;
    public Texture2D stone;
    public Texture2D copper;
    public Texture2D sand;
    public Texture2D granite;
    public Texture2D iron;
    public Texture2D silver;
    public Texture2D gold;
    public Texture2D mithril;
    public Texture2D diamond;
    public Texture2D magnetite;
    public Texture2D titanium;
    public Texture2D cobalt;
    public Texture2D ice;
    public Texture2D grass;
    public Texture2D hearthStone;

    Vector2 mineScroll;

    #region[OnEnable]
    private void OnEnable()
    {
        groundManager = target as GroundManager;
    }
    #endregion

    #region[OnInspectorGUI]
    public override void OnInspectorGUI()
    {
        //변경사항 검사시작
        EditorGUI.BeginChangeCheck();

        GUI.color = Color.white;
        EditorGUILayout.BeginVertical("box");
        mineScroll = EditorGUILayout.BeginScrollView(mineScroll, GUILayout.Width(275), GUILayout.Height(300));

        GroundData("흙 체력", dirt, ref groundManager.dirtHp, ref groundManager.dirtColor, ref groundManager.dirtValue);
        GroundData("돌 체력", stone, ref groundManager.stoneHp, ref groundManager.stoneColor, ref groundManager.stoneValue);
        GroundData("구리 체력", copper, ref groundManager.copperHp, ref groundManager.copperColor, ref groundManager.copperValue);
        GroundData("모래 체력", sand, ref groundManager.sandHp, ref groundManager.sandColor);
        GroundData("화강암 체력", granite, ref groundManager.graniteHp, ref groundManager.graniteColor, ref groundManager.graniteValue);
        GroundData("철 체력", iron, ref groundManager.ironHp, ref groundManager.ironColor, ref groundManager.ironValue);
        GroundData("은 체력", silver, ref groundManager.silverHp, ref groundManager.silverColor, ref groundManager.silverValue);
        GroundData("금 체력", gold, ref groundManager.goldHp, ref groundManager.goldColor, ref groundManager.goldValue);
        GroundData("미스릴 체력", mithril, ref groundManager.mithrilHp, ref groundManager.mithrilColor, ref groundManager.mithrilValue);
        GroundData("다이아 체력", diamond, ref groundManager.diamondHp, ref groundManager.diamondColor, ref groundManager.diamondValue);
        GroundData("자철석 체력", magnetite, ref groundManager.magnetiteHp, ref groundManager.magnetiteColor, ref groundManager.magnetiteValue);
        GroundData("티타늄 체력", titanium, ref groundManager.titaniumHp, ref groundManager.titaniumColor, ref groundManager.titaniumValue);
        GroundData("코발트 체력", cobalt, ref groundManager.cobaltHp, ref groundManager.cobaltColor, ref groundManager.cobaltValue);
        GroundData("얼음 체력", ice, ref groundManager.iceHp, ref groundManager.iceColor);
        GroundData("잔디 체력", grass, ref groundManager.grassHp, ref groundManager.grassColor);
        GroundData("하스스톤 체력", hearthStone, ref groundManager.hearthStoneHp, ref groundManager.hearthStoneColor);

        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();

        //변경사항 검사종료
        if (EditorGUI.EndChangeCheck())
        {
            //변경사항체크
            Undo.RecordObject(groundManager, "ChangeGroundManaer");
            var prefabStage = UnityEditor.Experimental.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage();

            if (prefabStage != null)
                EditorSceneManager.MarkSceneDirty(prefabStage.scene);
            else
            {
                EditorUtility.SetDirty(groundManager);
                PrefabUtility.RecordPrefabInstancePropertyModifications(this);
                EditorSceneManager.MarkSceneDirty(groundManager.gameObject.scene);
            }
        }
    }
    #endregion

    #region[지형데이터]
    void GroundData(string name, Texture2D texture2D, ref int input, ref Color color)
    {
        EditorGUILayout.BeginVertical();
        GUI.color = new Color(217 / 255f, 240 / 255f, 247 / 255f);
        EditorGUILayout.BeginHorizontal("helpbox", GUILayout.Width(220));

        GUILayout.Button("", GUI.skin.label, GUILayout.Width(10), GUILayout.Height(30));
        GUILayout.Button("", GUI.skin.label, GUILayout.Width(30), GUILayout.Height(30));
        Rect lastRect = GUILayoutUtility.GetLastRect();
        GUI.color = Color.white;
        GUI.DrawTexture(lastRect, texture2D);

        EditorGUILayout.BeginVertical();
        EditorGUI.indentLevel++;
        EditorGUILayout.LabelField(name, GUILayout.Width(80));
        input = EditorGUILayout.IntField(input, "helpbox", GUILayout.Width(60));
        EditorGUI.indentLevel--;

        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("이펙트 색상", GUILayout.Width(80));
        color = EditorGUILayout.ColorField(color, GUILayout.Width(80), GUILayout.Height(20));
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();
    }

    void GroundData(string name, Texture2D texture2D, ref int input,ref Color color,ref int mineValue)
    {
        EditorGUILayout.BeginVertical("helpbox", GUILayout.Width(220));
        GUI.color = new Color(217 / 255f, 240 / 255f, 247 / 255f);
        EditorGUILayout.BeginHorizontal();

        GUILayout.Button("", GUI.skin.label, GUILayout.Width(10), GUILayout.Height(30));
        GUILayout.Button("", GUI.skin.label, GUILayout.Width(30), GUILayout.Height(30));
        Rect lastRect = GUILayoutUtility.GetLastRect();
        GUI.color = Color.white;
        GUI.DrawTexture(lastRect, texture2D);

        EditorGUILayout.BeginVertical();
        EditorGUI.indentLevel++;
        EditorGUILayout.LabelField(name, GUILayout.Width(80));
        input = EditorGUILayout.IntField(input, "helpbox", GUILayout.Width(60));
        EditorGUI.indentLevel--;

        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("이펙트 색상", GUILayout.Width(80));
        color = EditorGUILayout.ColorField(color, GUILayout.Width(80), GUILayout.Height(20));
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("", GUILayout.Width(60));
        EditorGUILayout.LabelField("광물가격", GUILayout.Width(65));
        mineValue = EditorGUILayout.IntField(mineValue, "helpbox", GUILayout.Width(60));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();
    }
    #endregion

    #region[IntField]
    public override int IntField(string name, Texture2D texture2D, int input, GUIStyle guiStyle = null)
    {
        guiStyle = guiStyle != null ? guiStyle : GUI.skin.label;
        EditorGUILayout.BeginVertical();
        GUI.color = new Color(217 / 255f, 240 / 255f, 247 / 255f);
        EditorGUILayout.BeginHorizontal("helpbox", GUILayout.Width(220));

        GUILayout.Button("", GUI.skin.label, GUILayout.Width(10), GUILayout.Height(30));
        GUILayout.Button("", GUI.skin.label, GUILayout.Width(30), GUILayout.Height(30));
        Rect lastRect = GUILayoutUtility.GetLastRect();
        GUI.color = Color.white;
        GUI.DrawTexture(lastRect, texture2D);

        EditorGUILayout.BeginVertical();
        EditorGUI.indentLevel++;
        EditorGUILayout.LabelField(name, guiStyle, GUILayout.Width(100));
        var result = EditorGUILayout.IntField(input, "helpbox", GUILayout.Width(60));
        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();

        return result;
    }
    #endregion
}