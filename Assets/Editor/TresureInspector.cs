using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Tresure))]
public class TresureInspector : Editor
{

    Texture2D ontex;
    Texture2D offtex;
    GUIStyle style ;
    int bufw;
    int bufh;
    private void OnEnable()
    {
        int texsiz = 16;
        ontex = new Texture2D(texsiz, texsiz);
        offtex = new Texture2D(texsiz, texsiz);
        for (int i = 0; i < texsiz; i++)
        {
            for (int j = 0; j < texsiz; j++)
            {
                offtex.SetPixel(i, j, new Color(1f, 1f, 1f, 1f));
                ontex.SetPixel(i, j, new Color(0f, 0f, 0f, 1f));
            }
        }
        ontex.Apply();
        offtex.Apply();
        style = new GUIStyle();
        style.stretchWidth = false;
        bufw = serializedObject.FindProperty("w").intValue;
        bufh = serializedObject.FindProperty("h").intValue;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();
        SerializedProperty w = serializedObject.FindProperty("w");
        SerializedProperty h = serializedObject.FindProperty("h");
        SerializedProperty shape = serializedObject.FindProperty("shape");

        GUI.SetNextControlName("w");
        bufw = EditorGUILayout.IntField("w", bufw);

        GUI.SetNextControlName("h");
        bufh = EditorGUILayout.IntField("h", bufh);
        if (GUI.GetNameOfFocusedControl()!="w"&& GUI.GetNameOfFocusedControl() != "h") {
            w.intValue = bufw;
            h.intValue = bufh;
            if (shape.arraySize < w.intValue * h.intValue)
            {
                while (shape.arraySize != w.intValue * h.intValue)
                {
                    shape.InsertArrayElementAtIndex(0);
                    shape.GetArrayElementAtIndex(0).intValue = 0;
                }
            }
            else if (shape.arraySize > w.intValue * h.intValue)
            {
                while (shape.arraySize != w.intValue * h.intValue)
                    shape.DeleteArrayElementAtIndex(0);
            }
         }
        EditorGUILayout.BeginVertical();
        for (int i = 0; i < h.intValue; i++)
        {
            EditorGUILayout.BeginHorizontal();
            for (int j = 0; j < w.intValue; j++)
            {
                if (shape.GetArrayElementAtIndex(i * w.intValue + j).intValue == 0)
                {
                    if (GUILayout.Button(offtex, style))
                        shape.GetArrayElementAtIndex(i * w.intValue + j).intValue = 1;
                }
                else
                {
                    if (GUILayout.Button(ontex, style))
                        shape.GetArrayElementAtIndex(i * w.intValue + j).intValue = 0;
                }
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
        serializedObject.ApplyModifiedProperties();
    }
    
}