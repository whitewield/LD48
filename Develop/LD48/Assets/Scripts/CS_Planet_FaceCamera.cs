using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Planet_FaceCamera : MonoBehaviour {
    private void Update () {
        this.transform.up = Vector3.up;
    }
}
