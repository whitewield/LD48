using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CS_GameManager : MonoBehaviour {
    [SerializeField] GameObject myPlanetPrefab = null;
    [SerializeField] GameObject myTrailPrefab = null;
    [SerializeField] List<int> myArrangeList = new List<int> ();
    private List<CS_Planet> myPlanetList = new List<CS_Planet> ();
    [SerializeField] float myRootRadius = 5;
    [SerializeField] float myRootSpeed = 36;
    private List<CS_Planet> myPlanetRootList = new List<CS_Planet> ();
    private List<CS_Planet> myPlanetEndList = new List<CS_Planet> ();
    [SerializeField] List<float> myRadiusList = new List<float> ();
    [SerializeField] List<float> mySpeedList = new List<float> ();
    private int seed;

    [Header ("UI")]
    [SerializeField] float myRootSpeedMultiplier = 360;

    private void Start () {
        // get seed
        seed = (int)System.DateTime.Now.Ticks;
        Random.InitState (seed);

        // create planets
        for (int i = 0; i < myArrangeList.Count; i++) {
            // create planet
            GameObject f_planetObject = Instantiate (myPlanetPrefab, this.transform);
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
                // get random radius and speed for list
                f_radius = myRadiusList[Random.Range (0, myRadiusList.Count)];
                f_speed = mySpeedList[Random.Range (0, mySpeedList.Count)];
                // random speed sign
                if (f_speed != 1) {
                    f_speed = (Random.Range (0, 2) * 2 - 1) * f_speed;
                }
            }
            // init planet
            f_planet.Init (f_parent, f_radius, f_speed);
            // add to list
            myPlanetList.Add (f_planet);
        }

        FixedUpdate ();

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
    }

    private void FixedUpdate () {
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

    public void OnSlider_Speed (float g_value) {
        float t_speed = g_value * g_value * myRootSpeedMultiplier;

		foreach (CS_Planet f_root in myPlanetRootList) {
			f_root.SetSpeed (t_speed);
		}
	}
}
