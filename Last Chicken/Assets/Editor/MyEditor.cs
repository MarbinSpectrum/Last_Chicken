using System;
using UnityEditor;
using UnityEngine;

public class MyEditor : Editor
{
    public Font fontSilver;
    public Font fontStarDust;

    #region[IntField]
    public virtual int IntField(string name, int input, GUIStyle guiStyle = null)
    {
        guiStyle = guiStyle != null ? guiStyle : GUI.skin.label;
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(name, guiStyle, GUILayout.Width(150));

        var result = EditorGUILayout.IntField(input, GUILayout.Width(90));
        EditorGUILayout.EndHorizontal();
        return result;
    }

    public virtual int IntField(int input, GUIStyle style = null)
    {
        EditorGUILayout.BeginHorizontal();

        var result = EditorGUILayout.IntField(input, GUILayout.Width(90));
        EditorGUILayout.EndHorizontal();
        return result;
    }

    public virtual int IntField(Texture2D texture2D, int input, GUIStyle guiStyle = null)
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

    public virtual int IntField(string name, Texture2D texture2D, int input, GUIStyle guiStyle = null)
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
    public virtual float FloatField(string name, float input, GUIStyle guiStyle = null)
    {
        guiStyle = guiStyle != null ? guiStyle : GUI.skin.label;
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(name, guiStyle, GUILayout.Width(150));

        var result = EditorGUILayout.FloatField(input, GUILayout.Width(90));
        EditorGUILayout.EndHorizontal();
        return result;
    }

    public virtual float FloatField(float input, GUIStyle guiStyle = null)
    {
        guiStyle = guiStyle != null ? guiStyle : GUI.skin.label;
        EditorGUILayout.BeginHorizontal();

        var result = EditorGUILayout.FloatField(input, GUILayout.Width(90));
        EditorGUILayout.EndHorizontal();
        return result;
    }

    public virtual float FloatField(Texture2D texture2D, float input, GUIStyle guiStyle = null)
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

    public virtual float FloatField(string name, Texture2D texture2D, float input, GUIStyle guiStyle = null)
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

    #region[EnumField]
    public virtual Enum EnumField(string name, Enum input, GUIStyle guiStyle = null)
    {
        guiStyle = guiStyle != null ? guiStyle : GUI.skin.label;
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(name, guiStyle, GUILayout.Width(150));

        var result = EditorGUILayout.EnumPopup(input, GUILayout.Width(90));
        EditorGUILayout.EndHorizontal();
        return result;
    }

    public virtual Enum EnumField(Enum input, GUIStyle guiStyle = null)
    {
        guiStyle = guiStyle != null ? guiStyle : GUI.skin.label;
        EditorGUILayout.BeginHorizontal();

        var result = EditorGUILayout.EnumPopup(input, GUILayout.Width(90));
        EditorGUILayout.EndHorizontal();
        return result;
    }

    public virtual Enum EnumField(Texture2D texture2D, Enum input, GUIStyle guiStyle = null)
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

    public virtual Enum EnumField(string name, Texture2D texture2D, Enum input, GUIStyle guiStyle = null)
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

    #region[ToggleField]
    public virtual bool ToggleField(string name, bool input, GUIStyle guiStyle = null)
    {
        guiStyle = guiStyle != null ? guiStyle : GUI.skin.label;

        EditorGUILayout.BeginVertical();
        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.LabelField(name, guiStyle, GUILayout.Width(150));
        var result = EditorGUILayout.Toggle(input);

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();

        return result;
    }

    public virtual bool ToggleField(bool input, GUIStyle guiStyle = null)
    {
        guiStyle = guiStyle != null ? guiStyle : GUI.skin.label;

        EditorGUILayout.BeginVertical();
        EditorGUILayout.BeginHorizontal();

        var result = EditorGUILayout.Toggle(input);

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();

        return result;
    }

    #endregion
}