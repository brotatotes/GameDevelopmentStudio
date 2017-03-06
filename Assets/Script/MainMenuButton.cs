using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenuButton : MonoBehaviour, IPointerClickHandler {

	public string sceneName;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		Debug.Log ("OnPointerClick");
		if (eventData.button == PointerEventData.InputButton.Left) {
			Debug.Log ("left button");
			setScene ();
			//	godCursor.GetComponent<PlayerCursor> ().leftObj = spawnObj;
		}
	}
	public void setScene() {
		Debug.Log ("loading new scene");
		SceneManager.LoadScene (sceneName, LoadSceneMode.Single);
	}
}
