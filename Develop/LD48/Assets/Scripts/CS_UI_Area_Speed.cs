using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CS_UI_Area_Speed : MonoBehaviour {
    [SerializeField] Text myText = null;
    private CS_Planet myPlanet = null;
    public void Set (CS_Planet g_planet) {
        myPlanet = g_planet;
    }

    private void Update () {
        if (myPlanet == null) {
            return;
        }

        myText.text = "×" + myPlanet.GetMultiplier_Speed ().ToString ("0.##");
    }
}
