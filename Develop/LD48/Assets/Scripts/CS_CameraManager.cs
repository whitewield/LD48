using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_CameraManager : MonoBehaviour {
    private static CS_CameraManager instance = null;
    public static CS_CameraManager Instance { get { return instance; } }

    [SerializeField] Camera myCamera = null;
    private Transform myLookAtTargetTransform = null;
    private float myLookAtTargetSize = 6;
    [SerializeField] float myLookAtSpeed = 10;
    private float myLookAtProcess = 0;
    private bool doLookAt = false;

    private void Awake () {
        if (instance != null && instance != this) {
            Destroy (this.gameObject);
        } else {
            instance = this;
        }
    }

    public void LookAt (Transform g_target, float g_size) {
        if (g_target == myLookAtTargetTransform) {
            return;
        }

        doLookAt = true;
        myLookAtProcess = 0;
        myLookAtTargetTransform = g_target;
        myLookAtTargetSize = g_size;
        if (myLookAtTargetSize == 0) {
            myLookAtTargetSize = 6;
        }
    }

    private void FixedUpdate () {
        if (doLookAt == true) {
            myLookAtProcess += Time.fixedDeltaTime * myLookAtSpeed;
            // stop
            if (myLookAtProcess >= 1) {
                myLookAtProcess = 1;
                doLookAt = false;
                Debug.Log ("stop");
            }

            // get position
            Vector3 t_targetPosition = myLookAtTargetTransform.position;
            t_targetPosition.z = 0;
            // move
            this.transform.position = Vector3.Lerp (this.transform.position, t_targetPosition, myLookAtProcess);
            // scale
            myCamera.orthographicSize = Mathf.Lerp (myCamera.orthographicSize, myLookAtTargetSize, myLookAtProcess);

        } else {
            if (myLookAtTargetTransform == null) {
                this.transform.position = Vector3.zero;
            } else {
                this.transform.position = myLookAtTargetTransform.position;
            }
        }
    }
}
