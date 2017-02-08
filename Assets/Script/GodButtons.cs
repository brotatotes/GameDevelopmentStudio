using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GodButtons : MonoBehaviour, IPointerClickHandler {

	public GameObject spawnObj;
	public GameObject godCursor;
	public void OnPointerClick(PointerEventData eventData)
	{
		Debug.Log ("OnPointerClick");
		if (eventData.button == PointerEventData.InputButton.Left) {
			godCursor.GetComponent<PlayerCursor> ().leftObj = spawnObj;
		} else {
			godCursor.GetComponent<PlayerCursor> ().rightObj = spawnObj;
			gameObject.GetComponent<Button> ().Invoke ("OnClick", 0.0f);
		}
	}
}
