using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Planet : MonoBehaviour {
    [SerializeField] float myMultiplier_Radius = 0.5f;
    [SerializeField] float myMultiplier_Speed = 2;
    private float myRadius = 0;
    private float mySpeed = 0;
    private float myStartRotationZ = 0;
    private float myRotationZ = 0;

    [Header("Rotate Back")]
    [SerializeField] float myRotateBack_Speed = 1;
    private float myRotateBack_Process = 0;
    private float myRotateBack_StartValue = 0;
    private bool doRotateBack = false;

    [SerializeField] Transform myTransform_Children = null;
    [SerializeField] Transform myTransform_Display = null;
    [SerializeField] Gradient myGradient = null;
    [SerializeField] LineRenderer myLineRenderer = null;
    [SerializeField] CircleCollider2D myCollider = null;

    private CS_Planet myParent = null;
    private List<CS_Planet> myChildrenList = new List<CS_Planet> ();
    private bool isEnd = true;
    private TrailRenderer myTrail = null;

    // lerp radius
    private float myLerpRadiusMultiplier_To = -1;
    private float myLerpRadiusMultiplier_From = -1;
    private bool doLerpRadiusMultiplier = false;
    private float myLerpRadiusMultiplier_Process = 1;
    [SerializeField] float myLerpRadiusMultiplier_Speed = 2;

    public void Init (CS_Planet g_parent, float g_radius, float g_speed, float g_rotationZ, Gradient g_gradient = null) {
        myParent = g_parent;
        if (myParent != null) {
            myParent.SetChild (this);
        }

        myMultiplier_Radius = g_radius;
        myMultiplier_Speed = g_speed;
        myStartRotationZ = g_rotationZ;
        myRotationZ = g_rotationZ;

        // set color
        if (g_gradient == null) {
            HideLine ();
        } else {
            myLineRenderer.colorGradient = g_gradient;
        }
    }

    public void SetMultiplier_Speed (float g_multiplier, bool g_doResetRotation = false) {
        myMultiplier_Speed = g_multiplier;
        if (g_doResetRotation == true) {
            CS_GameManager.Instance.ResetRotation ();
        }
    }

    public float GetMultiplier_Speed () {
        return myMultiplier_Speed;
    }

    public void LerpMultiplier_Radius (float g_multiplier) {
        myLerpRadiusMultiplier_To = g_multiplier;
        myLerpRadiusMultiplier_From = myMultiplier_Radius;
        myLerpRadiusMultiplier_Process = 0;
        doLerpRadiusMultiplier = true;
    }

    private void Update_LerpRadiusMultiplier () {
        if (doLerpRadiusMultiplier == false) {
            return;
        }

        myLerpRadiusMultiplier_Process += Time.fixedDeltaTime * myLerpRadiusMultiplier_Speed;
        if (myLerpRadiusMultiplier_Process >= 1) {
            myLerpRadiusMultiplier_Process = 1;
            doLerpRadiusMultiplier = false;
        }

        myMultiplier_Radius = Mathf.Lerp (myLerpRadiusMultiplier_From, myLerpRadiusMultiplier_To, myLerpRadiusMultiplier_Process);
    }

    public void SetMultiplier_Radius (float g_multiplier) {
        myMultiplier_Radius = g_multiplier;
    }

    public float GetMultiplier_Radius () {
        return myMultiplier_Radius;
    }

    public float GetMultiplier_TargetRadius () {
        if (doLerpRadiusMultiplier) {
            return myLerpRadiusMultiplier_To;
        }
        return myMultiplier_Radius;
    }

    public float GetStartRotationZ () {
        return myStartRotationZ;
    }

    public void RotateBack () {
        doRotateBack = true;
        myRotateBack_StartValue = myRotationZ;
        //if (myRotateBack_StartValue - myRotationZ > 180) {
        //    myRotateBack_StartValue -= 360;
        //}
        myRotateBack_Process = 0;
    }

    private void UpdateRotateBack () {
        myRotateBack_Process += Time.fixedDeltaTime * myRotateBack_Speed;
        if (myRotateBack_Process >= 1) {
            myRotateBack_Process = 1;
            doRotateBack = false;
            // clear trail
            ClearTrail ();
        }

        myRotationZ = Mathf.Lerp (myRotateBack_StartValue, myStartRotationZ, myRotateBack_Process);
    }

    public void SetChild (CS_Planet g_child) {
        myChildrenList.Add (g_child);
        g_child.transform.SetParent (myTransform_Children);
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
        // lerp multiplier
        Update_LerpRadiusMultiplier ();

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
        myCollider.radius = Mathf.Abs (myRadius);
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
        if (doRotateBack == false) {
            myRotationZ += Time.fixedDeltaTime * mySpeed;
            if (myRotationZ > 360) {
                myRotationZ -= 360;
            }
        } else {
            UpdateRotateBack ();
        }
        this.transform.localRotation = Quaternion.Euler (0, 0, myRotationZ);
    }

    public void SetTrail (TrailRenderer g_trail) {
        myTrail = g_trail;
        myTrail.transform.parent = myTransform_Children;
        myTrail.transform.localPosition = Vector3.zero;
    }

    public void ClearTrail () {
        if (myTrail == null) {
            return;
        }
        myTrail.Clear ();
    }

    public void HideLine () {
        myLineRenderer.enabled = false;
        myCollider.enabled = false;
    }

    public float GetChildMaxSpeedMultiplier () {
        float t_multiplier = 0;
        foreach(CS_Planet t_child in myChildrenList) {
            t_multiplier = Mathf.Max (Mathf.Abs (t_child.GetMultiplier_Speed ()), t_multiplier);
        }
        return t_multiplier;
    }

    public bool OnClick () {
        return CS_GameManager.Instance.OnClickPlanet (this);
    }
}
