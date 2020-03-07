using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Pool))]
public class PoolEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Pool pool = (Pool) target;

    }
}
