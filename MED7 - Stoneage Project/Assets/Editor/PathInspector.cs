using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PathVisualiser))]
public class PathInspector : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        PathVisualiser path = (PathVisualiser) target;

        /*if(GUILayout.Button("Draw Path"))
        {
            path.ln = path.GetComponent<LineRenderer>();
            if (path.hasData)
                path.drawpath();
        }*/
        if (GUILayout.Button("Retrieve Data"))
        {
            path.retrieveData();
            //EditorGUILayout.Slider("Array Index",path.arrayIndex, 0, path.arrayRange);
            //want to spawn a slider after having gotten data
        }
        if (GUILayout.Button("Reset Data"))
        {
            path.resetData();
        }
        //EditorGUILayout.Slider(path.arrayIndex, 0, path.arrayRange);
    }

    void OnInspectorUpdate()
    {
        Repaint();
    }
}
