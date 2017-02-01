using UnityEngine;
using System.Collections;

public class PlayerCursor : MonoBehaviour {

	public Texture2D defaultTexture; 
	public CursorMode curMode = CursorMode.Auto;
	public Vector2 hotSpot = Vector2.zero;

	// Use this for initialization
	void Start () {
		Cursor.SetCursor(defaultTexture, hotSpot, curMode);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnMouseEnter(){
		Cursor.SetCursor(defaultTexture, hotSpot, curMode);	
	}

	void OnMouseExit(){
		Cursor.SetCursor(defaultTexture, hotSpot, curMode);	
	}
}
