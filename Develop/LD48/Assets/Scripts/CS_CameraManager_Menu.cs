using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_CameraManager_Menu : CS_CameraManager {
    protected override void FixedUpdate () {
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
                t_size = myLookAtTarget.transform.lossyScale.x / 2 + 1;
            }
            myCamera.orthographicSize = Mathf.Lerp (myCamera.orthographicSize, t_size, myLookAtProcess);

        } else {
            if (myLookAtTarget == null) {
                this.transform.position = Vector3.zero;
            } else {
                this.transform.position = myLookAtTarget.transform.position;
                myCamera.orthographicSize = myLookAtTarget.transform.lossyScale.x / 2 + 1;
            }
        }
    }
}
