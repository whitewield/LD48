using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Hang.AiryAudio;

public class CS_MenuManager : MonoBehaviour {
    private static CS_MenuManager instance = null;
    public static CS_MenuManager Instance { get { return instance; } }

    [Serializable]
    public class Level {
        public CS_Planet planet;
        public string sceneName;
        public List<int> arrangeList = new List<int> ();
    }

    [SerializeField] List<CS_Planet> myPlanetList = new List<CS_Planet> ();
    [SerializeField] List<Level> myLevels = new List<Level> ();

    private void Awake () {
        if (instance != null && instance != this) {
            Destroy (this.gameObject);
        } else {
            instance = this;
        }
    }

    private void FixedUpdate () {
        foreach (CS_Planet f_planet in myPlanetList) {
            f_planet.UpdateSpeed ();
            f_planet.UpdateRotation ();
        }
    }

    public bool OnClickPlanet (CS_Planet g_planet) {
        AiryAudioSource t_airyAudioSource = AiryAudioManager.Instance.InitAudioSource ("Button");
        AiryAudioActions.Play (t_airyAudioSource);

        CS_CameraManager.Instance.LookAt (g_planet);
        foreach (Level t_level in myLevels) {
            if (t_level.planet == g_planet) {
                CS_DataManager.Instance.myArrangeList = t_level.arrangeList;
                SceneManager.LoadSceneAsync (t_level.sceneName);

                t_airyAudioSource = AiryAudioManager.Instance.InitAudioSource ("Drop");
                AiryAudioActions.Play (t_airyAudioSource);
                return true;
            }
        }
        return true;
    }


    public void OnButtonQuit () {
        Application.Quit ();
    }
}
