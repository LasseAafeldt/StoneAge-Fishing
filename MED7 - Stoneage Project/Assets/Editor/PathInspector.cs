using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PathVisualiser))]
public class PathInspector : Editor
{

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
        }
    }

    void OnInspectorUpdate()
    {
        Repaint();
    }
}
