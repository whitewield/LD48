using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CS_InputManager : MonoBehaviour {
    void Update () {
        if (Input.GetMouseButtonDown (0) && EventSystem.current.IsPointerOverGameObject () == false) {
            RaycastHit2D[] t_hitArray = Physics2D.RaycastAll (Camera.main.ScreenToWorldPoint (Input.mousePosition), Vector2.zero);
            if (t_hitArray != null && t_hitArray.Length > 0) {
                CS_Planet t_planetMin1st = null;
                CS_Planet t_planetMin2nd = null;
                foreach (RaycastHit2D t_hit in t_hitArray) {
                    CS_Planet f_planet = t_hit.collider.GetComponent<CS_Planet> ();
                    f_planet = FillMinSizePlanet (ref t_planetMin1st, f_planet);
                    FillMinSizePlanet (ref t_planetMin2nd, f_planet);
                }
                if (t_planetMin1st != null) {
                    if (t_planetMin1st.OnClick () == false) {
                        if (t_planetMin2nd != null) {
                            t_planetMin2nd.OnClick ();
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="g_spot">a spot to store smaller planet</param>
    /// <param name="g_target">planet to compare with</param>
    /// <returns>the larger planet</returns>
    private CS_Planet FillMinSizePlanet (ref CS_Planet g_spot, CS_Planet g_target) {
        if (g_target == null) {
            return null;
        }

        CS_Planet t_origin = g_spot;
        if (t_origin == null) {
            g_spot = g_target;
            return null;
        }

        if (t_origin.GetRadius() > g_target.GetRadius()) {
            g_spot = g_target;
            return t_origin;
        }

        return g_target;
    }
}
