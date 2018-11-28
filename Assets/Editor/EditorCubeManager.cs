using System.Collections;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CubeManager))]
public class EditorCubeManager : Editor {

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CubeManager cubeManagerScript = (CubeManager)target;
        //if (GUILayout.Button("Setup Cubes"))
        {
            //cubeManagerScript.SetupCubes();
        }

    }

}