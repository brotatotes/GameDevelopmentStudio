using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenuButton : MonoBehaviour, IPointerClickHandler,IPointerEnterHandler {

	public string sceneName;
	public string description;
	Text descripBox;
	// Use this for initialization
	void Start () {
		descripBox = GameObject.FindGameObjectWithTag ("description").GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		if (EventSystem.current.IsPointerOverGameObject ()) {
			
		}
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

	public void OnPointerEnter(PointerEventData eventData)
	{
		//Debug.Log (description);
		descripBox.text = description;
	}
	public void OnPointerOver(PointerEventData eventData)
	{
		Debug.Log ("Overoverover");
	}
}
