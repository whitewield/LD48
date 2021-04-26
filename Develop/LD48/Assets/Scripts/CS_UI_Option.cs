using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CS_UI_Option : MonoBehaviour, IPointerDownHandler, IDragHandler, IBeginDragHandler, IEndDragHandler {
    [SerializeField] Text myText = null;
    private float value = 0.5f;
    public void Set (float g_value) {
        value = g_value;
        myText.text = "×" + g_value.ToString ();
    }

    public void OnBeginDrag (PointerEventData eventData) {

    }

    public void OnDrag (PointerEventData eventData) {
        CS_GameManager.Instance.OnOption_Drag ();
    }

    public void OnEndDrag (PointerEventData eventData) {
        CS_GameManager.Instance.OnOption_Release ();
    }

    public void OnPointerDown (PointerEventData eventData) {
        CS_GameManager.Instance.OnOption_Click (value);
    }
}