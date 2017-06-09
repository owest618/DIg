using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

public class MapCreaterConfig : EditorWindow {
    static MapCreaterConfig save;
    public static Sprite[] setterras = new Sprite[Enum.GetValues(typeof(Terra)).Length];
    public static Sprite[] setobjects = new Sprite[Enum.GetValues(typeof(MovingObject)).Length];
    int selected = 0;
    public static void ShowTestMainWindow()
    {
        EditorWindow.GetWindow(typeof(MapCreaterConfig));
    }
    
    void ini()
    {
        
    }
    

    void OnGUI()
    {
        selected = GUILayout.Toolbar(selected, new string[] { "terra", "object" });
        var iconRect = new[] { GUILayout.Width(64), GUILayout.Height(64) };
        GUILayout.BeginVertical();
        if (selected == 0)
        {
            Array terralist = Enum.GetValues(typeof(Terra));
            foreach (Terra a in terralist)
            {
                GUILayout.BeginHorizontal();
                setterras[(int)a] = EditorGUILayout.ObjectField(setterras[(int)a], typeof(Sprite), false, iconRect) as Sprite;
                EditorGUILayout.LabelField(a.ToString());
                GUILayout.EndHorizontal();
            }
        }
        if (selected == 1)
        {
            Array terralist = Enum.GetValues(typeof(MovingObject));
            foreach (MovingObject a in terralist)
            {
                GUILayout.BeginHorizontal();
                setobjects[(int)a] = EditorGUILayout.ObjectField(setobjects[(int)a], typeof(Sprite), false, iconRect) as Sprite;
                EditorGUILayout.LabelField(a.ToString());
                GUILayout.EndHorizontal();
            }
        }
        GUILayout.EndVertical();
    }
}
