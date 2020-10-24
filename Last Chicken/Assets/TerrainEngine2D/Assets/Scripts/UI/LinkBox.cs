using UnityEngine;

// Copyright (C) 2020 Matthew K Wilson

#if UNITY_EDITOR
using UnityEditor;
namespace TerrainEngine2D.Editor
{
    [CustomEditor(typeof(LinkBox))]
    public class LinkBoxCustomInspector : UnityEditor.Editor
    {
        private bool showing;

        public override void OnInspectorGUI()
        {
            //Setup GUIStyles
            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
            buttonStyle.normal.textColor = new Color32(18, 5, 166, 255);
            GUIStyle headerStyle = new GUIStyle(EditorStyles.boldLabel);
            headerStyle.alignment = TextAnchor.UpperCenter;
            headerStyle.fixedHeight = EditorGUIUtility.singleLineHeight * 2;
            headerStyle.fontSize = 18;

            EditorGUILayout.LabelField("Terrain Engine 2D", headerStyle);
            EditorGUILayout.Space();

            EditorGUILayout.HelpBox("Welcome and thank you for purchasing Terrain Engine 2D! If this is your first time here I recommend clicking on the 'Getting Started' link " +
    "below. I also highly recommend you join the Discord channel, information and updates on the asset are posted there frequently, it is also the best way to interact " +
    "with the community and get in touch with the developer. Enjoy!", MessageType.Info, true);


            if (GUILayout.Button(new GUIContent("Getting Started", "Click here for help on how to get started with the engine, you will be taken to the Terrain Engine 2D Documentation"), buttonStyle))
                Application.OpenURL("https://activegamedev.com/TerrainEngine2D-Documentation.html");
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(new GUIContent("Website", "Click here to be taken to the Terrain Engine 2D website"), buttonStyle))
                Application.OpenURL("https://activegamedev.com/TerrainEngine2D.html");
            if (GUILayout.Button(new GUIContent("API", "Click here to be taken to the Terrain Engine 2D API"), buttonStyle))
                Application.OpenURL("https://activegamedev.com/API/html/annotated.html");
            if (GUILayout.Button(new GUIContent("Asset Store page", "Click here to be taken to the Terrain Engine 2D Asset Store Page"), buttonStyle))
                Application.OpenURL("https://assetstore.unity.com/packages/tools/terrain/terrain-engine-2d-115381");
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Need additional help?", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Join the Discord and get in touch with the community!");
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(new GUIContent("Join the Discord!", "Click here to join the Discord!"), buttonStyle))
                Application.OpenURL("https://discord.gg/PBphAvq");
            if (GUILayout.Button(new GUIContent("Contact the Dev", "Click here for the email to contact the developer"), buttonStyle))
                Application.OpenURL("mailto:contact@activegamedev.com");
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Extras", EditorStyles.boldLabel);
            if (GUILayout.Button(new GUIContent("Extra Sample Project", "Click here to be taken to the Terrain Engine 2D Example Project"), buttonStyle))
                Application.OpenURL("https://activegamedev.com/TerrainEngine2D-Example.html");
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Enjoying the asset?", EditorStyles.boldLabel);
            //EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Why not help support the development by leaving a review.");
            if (GUILayout.Button(new GUIContent("Leave a Review", "Click here to be taken to the review page of Terrain Engine 2D on the Asset Store"), buttonStyle))
                Application.OpenURL("https://assetstore.unity.com/packages/tools/terrain/terrain-engine-2d-115381#reviews");
            //EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();


        }
    }
}
#endif

namespace TerrainEngine2D
{
    public class LinkBox : MonoBehaviour
    {

    }
}