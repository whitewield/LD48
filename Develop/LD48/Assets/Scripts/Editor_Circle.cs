using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
public class Editor_Circle : MonoBehaviour {
    [SerializeField] LineRenderer myLineRenderer;
    [SerializeField] int myLinePointCount = 128;

    [ContextMenu("Generate")]
    private void Generate () {
        myLineRenderer.positionCount = myLinePointCount + 2;

        List<Vector3> t_pointList = new List<Vector3> ();
        float t_angle = 2f * Mathf.PI / (float)myLinePointCount;
        for (int i = 0; i < myLinePointCount + 2; i++) {
            float t_x = Mathf.Sin (t_angle * i) * 0.5f;
            float t_y = Mathf.Cos (t_angle * i) * 0.5f;
            t_pointList.Add (new Vector3 (t_x, t_y, 0));
        }
        myLineRenderer.SetPositions (t_pointList.ToArray ());
    }
}

#endif