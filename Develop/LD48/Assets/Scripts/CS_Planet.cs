using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Planet : MonoBehaviour {
    [SerializeField] float myMultiplier_Radius = 0.5f;
    [SerializeField] float myMultiplier_Speed = 2;
    private float myRadius = 0;
    private float mySpeed = 0;
    private float myRotationZ = 0;

    [SerializeField] Transform myTransform_Children = null;
    [SerializeField] Transform myTransform_Display = null;
    [SerializeField] Gradient myGradient = null;
    [SerializeField] SpriteRenderer mySpriteRenderer = null;
    [SerializeField] CircleCollider2D myCollider = null;

    private CS_Planet myParent = null;
    private bool isEnd = true;
    private TrailRenderer myTrail = null;

    public void Init (CS_Planet g_parent, float g_radius, float g_speed) {
        myParent = g_parent;
        if (myParent != null) {
            myParent.SetChild (this.transform);
        }

        myMultiplier_Radius = g_radius;
        myMultiplier_Speed = g_speed;

        myRotationZ = Random.Range (0, 360f);
        // temp: set color
        mySpriteRenderer.color = myGradient.Evaluate (Random.Range (0, 1f));
    }

    public void SetChild (Transform g_child) {
        g_child.SetParent (myTransform_Children);
        isEnd = false;
    }

    public bool CheckRoot () {
        if (myParent == null) {
            return true;
        }
        return false;
    }

    public bool CheckEnd () {
        return isEnd;
    }

    public void UpdateRadius () {
        if (myParent != null) {
            myRadius = myMultiplier_Radius * myParent.GetRadius ();
            // update position
            float t_posX = myParent.GetRadius () - myRadius;
            this.transform.localPosition = Vector3.right * t_posX - Vector3.forward;
        } else {
            myRadius = myMultiplier_Radius;
            // update position
            this.transform.localPosition = -Vector3.forward;
        }
        // set size
        myTransform_Display.localScale = new Vector3 (myRadius * 2, myRadius * 2, 1);
        myCollider.radius = myRadius;
    }

    public float GetRadius () {
        return myRadius;
    }

    public void UpdateSpeed () {
        if (myParent != null) {
            mySpeed = myMultiplier_Speed * myParent.GetSpeed ();
        } else {
            mySpeed = myMultiplier_Speed;
        }
    }

    public float GetSpeed () {
        return mySpeed;
    }

    public void UpdateRotation () {
        myRotationZ += Time.fixedDeltaTime * mySpeed;
        this.transform.localRotation = Quaternion.Euler (0, 0, myRotationZ);
    }

    public void SetTrail (TrailRenderer g_trail) {
        myTrail = g_trail;
        myTrail.transform.parent = myTransform_Children;
        myTrail.transform.localPosition = Vector3.zero;
    }

    public void OnMouseDown () {
        Debug.Log ("OnMouseDown" + this.name);
        CS_CameraManager.Instance.LookAt (this.transform, myRadius + 1);
    }
}
