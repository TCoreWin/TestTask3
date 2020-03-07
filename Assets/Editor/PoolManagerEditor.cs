using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PoolManager))]
public class PoolManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PoolManager plsM = (PoolManager) target;
        if (GUILayout.Button("AddPoolAndPopulate"))
        {
        }
    }
}
