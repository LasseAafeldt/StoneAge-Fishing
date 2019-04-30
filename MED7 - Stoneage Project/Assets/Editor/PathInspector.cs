using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PathVisualiser))]
public class PathInspector : Editor
{
    //private int cameraHieght = 0;
    private bool useWholeDirectory;
    private bool hasData = true;

    public override void OnInspectorGUI()
    {
        PathVisualiser path = (PathVisualiser) target;
        #region If no data is availible
        if (!hasData)
        {
            EditorGUILayout.LabelField("There is no data", EditorStyles.boldLabel);
            path.useWholeDirectory = EditorGUILayout.Toggle("Use Whole Directory ",path.useWholeDirectory);
            if (!path.useWholeDirectory)
            {
                EditorGUILayout.LabelField("File selector", EditorStyles.boldLabel);
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("down"))
                {
                    path.fileToSelect--;
                }
                path.fileToSelect = EditorGUILayout.IntSlider("File number To be handled ", path.fileToSelect, 0, path.numberOfFilesInDirectory - 1);
                if (GUILayout.Button("up"))
                {
                    path.fileToSelect++;
                }
                EditorGUILayout.EndHorizontal();
            }
            if (GUILayout.Button("Retrieve Data"))
            {
                path.retrieveData();
                hasData = true;
            }
            return;
        }
        #endregion

        EditorGUILayout.LabelField("Toggle single file or all files", EditorStyles.boldLabel);
        path.useWholeDirectory = EditorGUILayout.Toggle("Use Whole Directory ", path.useWholeDirectory);

        if (!path.useWholeDirectory)
        {
            EditorGUILayout.LabelField("File selector", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("down"))
            {
                path.fileToSelect--;
            }
            path.fileToSelect = EditorGUILayout.IntSlider("File number To be handled ",path.fileToSelect, 0, path.numberOfFilesInDirectory-1);
            if (GUILayout.Button("up"))
            {
                path.fileToSelect++;
            }
            EditorGUILayout.EndHorizontal();
            /*if(GUILayout.Button("Indicate guidance"))
            {
                path.placeGuidanceMarkers(path.fileToSelect);
            }*/
        }

        base.OnInspectorGUI();

        if (Application.isPlaying)
        {
            if (path.hasData && !path.useWholeDirectory)
            {
                EditorGUILayout.LabelField("Select cameraview to look at", EditorStyles.boldLabel);
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("down"))
                {
                    path.positionToLookAt--;
                }
                path.positionToLookAt = EditorGUILayout.IntSlider("Camera index in file ", path.positionToLookAt, 0, path.getMaxEntriesInFile(path.fileToSelect) - 1);
                if (GUILayout.Button("Up"))
                {
                    path.positionToLookAt++;
                }
                EditorGUILayout.EndHorizontal();
            }
            //cameraHieght = EditorGUILayout.IntSlider("Scene cam hight ", cameraHieght, 0, 500);
            if (GUILayout.Button("Retrieve Data"))
            {
                path.retrieveData();
            }
            if (path.hasData)
            {
                if (GUILayout.Button("Reset Data"))
                {
                    path.resetData();
                    Repaint();
                    hasData = false;
                }
            }
            if(GUILayout.Button("Set sceneview camera"))
            {
                MoveSceneViewCamera();
            }
        }
    }

    void OnInspectorUpdate()
    {
        Repaint();
    }

    static public void MoveSceneViewCamera()
    {
        //secen cam pos piv: (0.0, -140.0, 300.0)
        //float x = 204.0f; float y = 92.0f; float width = 822.0f; float height = 367.0f;
        //Rect position = new Rect(x,y,width,height);
        Vector3 position = new Vector3(0f, -140f, 300);
        Vector3 rotation = new Vector3(90, 0, 0);
        SceneView.lastActiveSceneView.pivot = position;
        //SceneView.lastActiveSceneView.position = position;
        SceneView.lastActiveSceneView.rotation = Quaternion.Euler(rotation);
        SceneView.lastActiveSceneView.Repaint();
        //Debug.Log("secen cam pos piv: " + SceneView.lastActiveSceneView.pivot);
        //Debug.Log("secen cam pos: " + SceneView.lastActiveSceneView.position);
    }
}
