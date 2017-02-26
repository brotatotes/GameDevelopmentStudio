using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GodButtons : MonoBehaviour, IPointerClickHandler {

	public GameObject spawnObj;
	public GameObject godCursor;
	public int buttonID;
	public void OnPointerClick(PointerEventData eventData)
	{
		Debug.Log ("OnPointerClick");
		if (eventData.button == PointerEventData.InputButton.Left) {
			setSpawnObj ("left");
		//	godCursor.GetComponent<PlayerCursor> ().leftObj = spawnObj;
		} else {
			setSpawnObj ("right");
			//godCursor.GetComponent<PlayerCursor> ().rightObj = spawnObj;
//			gameObject.GetComponent<Button> ().Invoke ("OnClick", 0.0f);
		}
	}
	public void setSpawnObj(string pointer) {
		PlayerCursor[] cursors = FindObjectsOfType<PlayerCursor> ();
		if (pointer == "left") {
			//godCursor.GetComponent<PlayerCursor> ().leftObj = spawnObj;
			foreach (PlayerCursor pc in cursors) {
				pc.setIndex ("left", buttonID -1);
			}
		} else {
			//godCursor.GetComponent<PlayerCursor> ().rightObj = spawnObj;
			//			gameObject.GetComponent<Button> ().Invoke ("OnClick", 0.0f);
			foreach (PlayerCursor pc in cursors) {
				pc.setIndex ("right", buttonID -1);
			}
		}
	}
}
