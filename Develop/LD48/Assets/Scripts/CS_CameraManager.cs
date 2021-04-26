using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_CameraManager : MonoBehaviour {
    private static CS_CameraManager instance = null;
    public static CS_CameraManager Instance { get { return instance; } }

    [SerializeField] Camera myCamera = null;
    private CS_Planet myLookAtTarget = null;
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

    public void LookAt (CS_Planet g_planet) {
        doLookAt = true;
        myLookAtProcess = 0;
        myLookAtTarget = g_planet;
    }

    private void FixedUpdate () {
        if (doLookAt == true) {
            myLookAtProcess += Time.fixedDeltaTime * myLookAtSpeed;
            // stop
            if (myLookAtProcess >= 1) {
                myLookAtProcess = 1;
                doLookAt = false;
            }

            // get position
            Vector3 t_targetPosition = Vector3.zero;
            if (myLookAtTarget != null) {
                t_targetPosition = myLookAtTarget.transform.position;
            }
            t_targetPosition.z = 0;
            // move
            this.transform.position = Vector3.Lerp (this.transform.position, t_targetPosition, myLookAtProcess);
            // scale
            float t_size = 6;
            if (myLookAtTarget != null) {
                t_size = Mathf.Abs (myLookAtTarget.GetRadius ()) + 1;
            }
            myCamera.orthographicSize = Mathf.Lerp (myCamera.orthographicSize, t_size, myLookAtProcess);

        } else {
            if (myLookAtTarget == null) {
                this.transform.position = Vector3.zero;
            } else {
                this.transform.position = myLookAtTarget.transform.position;
                myCamera.orthographicSize = Mathf.Abs (myLookAtTarget.GetRadius ()) + 1;
            }
        }
    }
}
