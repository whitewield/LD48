using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class CS_GameManager : MonoBehaviour {
    private static CS_GameManager instance = null;
    public static CS_GameManager Instance { get { return instance; } }

    [SerializeField] GameObject myPlanetPrefab = null;
    [SerializeField] GameObject myTrailPrefab = null;
    [SerializeField] GameObject myTrailAnswerPrefab = null;
    [SerializeField] List<int> myArrangeList = new List<int> ();
    private List<CS_Planet> myPlanetList = new List<CS_Planet> ();
    private List<CS_Planet> myAnswerList = new List<CS_Planet> ();
    [SerializeField] float myAnswerSpeed = 180;
    [SerializeField] float myRootRadius = 5;
    [SerializeField] float myRootSpeed = 36;
    [SerializeField] float myDefaultRadius = 0.5f;
    [SerializeField] float myDefaultSpeed = 1;
    private List<CS_Planet> myPlanetRootList = new List<CS_Planet> ();
    private List<CS_Planet> myPlanetEndList = new List<CS_Planet> ();
    [SerializeField] List<float> myRadiusList = new List<float> ();
    [SerializeField] List<float> mySpeedList = new List<float> ();
    [SerializeField] List<Gradient> myGradientList = new List<Gradient> ();
    private int seed;

    private CS_Planet myCurrentPlanet;
    private float myCurrentValue;

    [Header ("UI")]
    [SerializeField] GraphicRaycaster myGraphicRaycaster = null;
    [SerializeField] GameObject myPage_Planet = null;
    [SerializeField] CS_UI_Area_Speed myArea_Speed = null;
    [SerializeField] CS_UI_Area_Radius myArea_Radius = null;

    [SerializeField] Slider mySlider_Main = null;
    [SerializeField] float myRootSpeedMultiplier = 360;     // used by slider

    [SerializeField] GameObject myPrefab_Option = null;
    [SerializeField] Transform myTransform_Options = null;
    [SerializeField] CS_UI_Option myCurrentOption = null;


    private bool isGameOver = false;

    private void Awake () {
        if (instance != null && instance != this) {
            Destroy (this.gameObject);
        } else {
            instance = this;
        }
    }

    private void Start () {
        // init UI
        // hide planet page
        myPage_Planet.SetActive (false);
        // init slider
        mySlider_Main.SetValueWithoutNotify (Mathf.Sqrt (myRootSpeed / myRootSpeedMultiplier));
        // hide current option
        myCurrentOption.gameObject.SetActive (false);

        // get seed
        seed = (int)System.DateTime.Now.Ticks;
        Random.InitState (seed);

        // temp store parent
        List<CS_Planet> t_answerParents = new List<CS_Planet> ();
        // create answer list
        for (int i = 0; i < myArrangeList.Count; i++) {
            // create planet
            GameObject f_planetObject = Instantiate (myPlanetPrefab, this.transform);
            CS_Planet f_planet = f_planetObject.GetComponent<CS_Planet> ();
            float f_radius = myRootRadius;
            float f_speed = myAnswerSpeed;
            // get parent
            CS_Planet f_parent = null;
            if (myArrangeList[i] >= i) {
                // root
                f_parent = null;
                t_answerParents.Add (f_planet);
            } else {
                f_parent = myAnswerList[myArrangeList[i]];
                // get random radius and speed for list
                f_radius = myRadiusList[Random.Range (0, myRadiusList.Count)];
                f_speed = mySpeedList[Random.Range (0, mySpeedList.Count)];
                // random speed sign
                if (f_speed != 1) {
                    f_speed = (Random.Range (0, 2) * 2 - 1) * f_speed;
                }

                Debug.Log (f_radius + "-" + f_speed);
            }
            // get random rotation Z
            float  f_rotationZ = Random.Range (0, 360f);
            // init planet
            f_planet.Init (f_parent, f_radius, f_speed, f_rotationZ);
            // add to list
            myAnswerList.Add (f_planet);
        }

        // set parent speed
        foreach (CS_Planet f_planet in t_answerParents) {
            f_planet.SetMultiplier_Speed (myAnswerSpeed / f_planet.GetChildMaxSpeedMultiplier ());
        }


        // create planets
        for (int i = 0; i < myArrangeList.Count; i++) {
            // create planet
            GameObject f_planetObject = Instantiate (myPlanetPrefab, this.transform);
            f_planetObject.name = "Planet (" + i + ")";
            CS_Planet f_planet = f_planetObject.GetComponent<CS_Planet> ();
            float f_radius = myRootRadius;
            float f_speed = myRootSpeed;
            // get parent
            CS_Planet f_parent = null;
            if (myArrangeList[i] >= i) {
                // root
                f_parent = null;
                myPlanetRootList.Add (f_planet);
            } else {
                f_parent = myPlanetList[myArrangeList[i]];
                f_radius = myDefaultRadius;
                f_speed = myDefaultSpeed;
            }
            // get gradient
            Gradient f_gradient = myGradientList[Random.Range (0, myGradientList.Count)];
            // init planet
            f_planet.Init (f_parent, f_radius, f_speed, myAnswerList[i].GetStartRotationZ (), f_gradient);
            // add to list
            myPlanetList.Add (f_planet);
        }

        FixedUpdate ();

        // add answer trail
        foreach (CS_Planet f_planet in myAnswerList) {
            if (f_planet.CheckEnd () == true) {
                // add trail
                GameObject f_trailObject = Instantiate (myTrailAnswerPrefab, this.transform);
                TrailRenderer f_trail = f_trailObject.GetComponent<TrailRenderer> ();
                f_planet.SetTrail (f_trail);
            }
        }

        // add trail
        foreach (CS_Planet f_planet in myPlanetList) {
            if (f_planet.CheckEnd() == true) {
                // add trail
                GameObject f_trailObject = Instantiate (myTrailPrefab, this.transform);
                TrailRenderer f_trail = f_trailObject.GetComponent<TrailRenderer> ();
                f_planet.SetTrail (f_trail);
                // add to list
                myPlanetEndList.Add (f_planet);
            }
        }

        // generate options
        for (int i = 0; i < myPlanetList.Count; i++) {
            if (myPlanetRootList.Contains (myPlanetList[i])) {
                continue;
            }

            if (myPlanetEndList.Contains(myPlanetList[i]) == false) {
                CreateOption (myAnswerList[i].GetMultiplier_Speed ());
            }
            CreateOption (myAnswerList[i].GetMultiplier_Radius ());
        }
    }

    private void CreateOption (float g_value) {
        GameObject t_object = Instantiate (myPrefab_Option, myTransform_Options);
        CS_UI_Option t_option = t_object.GetComponent<CS_UI_Option> ();
        t_option.Set (g_value);
    }

    private void FixedUpdate () {
        foreach (CS_Planet f_planet in myAnswerList) {
            f_planet.UpdateRadius ();
            f_planet.UpdateSpeed ();
            f_planet.UpdateRotation ();
        }

        foreach (CS_Planet f_planet in myPlanetList) {
            f_planet.UpdateRadius ();
            f_planet.UpdateSpeed ();
            f_planet.UpdateRotation ();
        }
    }

    private void Update () {
        if (Input.GetKeyDown (KeyCode.R)) {
            SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
        }
    }

    public void ResetRotation () {
        foreach(CS_Planet f_planet in myPlanetList) {
            f_planet.RotateBack ();
        }
    }

    public void ClearTrail () {
        foreach (CS_Planet f_planet in myPlanetEndList) {
            f_planet.ClearTrail ();
        }
    }

    public bool OnClickPlanet (CS_Planet g_planet) {
        if (myCurrentPlanet == g_planet) {
            return false;
        }
        myCurrentPlanet = g_planet;
        CS_CameraManager.Instance.LookAt (g_planet);

        if (myPlanetRootList.Contains (g_planet) || g_planet == null) {
            myPage_Planet.SetActive (false);
            myArea_Speed.Set (null);
            myArea_Radius.Set (null);
        } else {
            myPage_Planet.SetActive (true);
            myArea_Radius.Set (g_planet);
            if (myPlanetEndList.Contains (g_planet)) {
                myArea_Speed.gameObject.SetActive (false);
                myArea_Speed.Set (null);
            } else {
                myArea_Speed.gameObject.SetActive (true);
                myArea_Speed.Set (g_planet);
            }
        }

        return true;
    }

    private bool CheckResult () {
        for (int i = 0; i < myPlanetList.Count; i++) {
            if (myPlanetRootList.Contains (myPlanetList[i])) {
                continue;
            }

            if (myPlanetEndList.Contains (myPlanetList[i]) == false) {
                if (myPlanetList[i].GetMultiplier_Speed () != myAnswerList[i].GetMultiplier_Speed ()) {
                    return false;
                }
            }
            if (myPlanetList[i].GetMultiplier_TargetRadius () != myAnswerList[i].GetMultiplier_Radius ()) {
                return false;
            }
        }

        // done
        isGameOver = true;
        OnClickPlanet (null);
        // show game over effect
        return true;
    }

    // UI

    public void OnOption_Click (float g_value) {
        myCurrentValue = g_value;
        myCurrentOption.gameObject.SetActive (true);
        myCurrentOption.transform.position = Input.mousePosition;
        myCurrentOption.Set (myCurrentValue);
        Debug.Log ("OnOption_Click");
    }

    public void OnOption_Drag () {
        myCurrentOption.transform.position = Input.mousePosition;
        Debug.Log ("OnOption_Drag");
    }

    public void OnOption_Release () {
        myCurrentOption.gameObject.SetActive (false);

        PointerEventData t_pointerEventData = new PointerEventData (EventSystem.current);
        t_pointerEventData.position = Input.mousePosition;

        List<RaycastResult> t_raycastResultList = new List<RaycastResult> ();

        myGraphicRaycaster.Raycast (t_pointerEventData, t_raycastResultList);

        foreach (RaycastResult f_result in t_raycastResultList) {
            if (f_result.gameObject.GetComponent<CS_UI_Area_Radius> () == true) {
                // set radius
                if (myCurrentPlanet != null) {
                    myCurrentPlanet.LerpMultiplier_Radius (myCurrentValue);
                    FixedUpdate ();
                    ClearTrail ();
                }
            } else if (f_result.gameObject.GetComponent<CS_UI_Area_Speed> () == true) {
                // set speed
                if (myCurrentPlanet != null) {
                    myCurrentPlanet.SetMultiplier_Speed (myCurrentValue, true);
                }
            }
        }

        if (isGameOver == false) {
            CheckResult ();
        }
    }

    public void OnSlider_Speed (float g_value) {
        float t_speed = g_value * g_value * myRootSpeedMultiplier;

        foreach (CS_Planet f_root in myPlanetRootList) {
            f_root.SetMultiplier_Speed (t_speed, false);
        }
    }

    public void HideAll () {

    }

    public void ShowAll () {

    }

}
