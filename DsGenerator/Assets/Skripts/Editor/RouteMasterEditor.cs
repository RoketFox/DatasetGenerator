using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RouteMaster))]
public class RouteMasterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        RouteMaster routeMaster = (RouteMaster)target;

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Add point"))
        {
            routeMaster.CreatePoint();
        }

        if (GUILayout.Button("Delete last"))
        {
            routeMaster.DeleteLastPoint();
        }

            EditorGUILayout.EndHorizontal();
        
        if (GUILayout.Button("Delete all"))
        {
            routeMaster.DeleteAllPoints();
        }
    }
}
