using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_DataManager : MonoBehaviour {
    private static CS_DataManager instance = null;
    public static CS_DataManager Instance { get { return instance; } }

    public List<int> myArrangeList = new List<int> ();

    private void Awake () {
        if (instance != null && instance != this) {
            Destroy (this.gameObject);
        } else {
            instance = this;
        }
        DontDestroyOnLoad (this.gameObject);
    }
}
