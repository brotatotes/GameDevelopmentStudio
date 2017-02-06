using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Controller2D))]
public class Player : MonoBehaviour {

	private bool isAlive = true;
	public float bottomOfTheWorld = -20.0f;

	public Vector2 startPosition;
	public float startPositionX = -8.0f;
	public float startPositionY = 1.5f;
	public float jumpHeight = 4.0f;
	public float timeToJumpApex = .4f;
	public string playerName = "Player 1";
	float accelerationTimeAirborne = .2f;
	float accelerationTimeGrounded = .1f;
	float moveSpeed = 8.0f;

	float gravity;
	float jumpVelocity;
	Vector3 velocity;
	float velocityXSmoothing;

	public string leftKey = "a";
	public string rightKey = "d";
	public string upKey = "space";
	public string downKey = "s";
	public string jumpKey = "w";

	public bool attemptingInteraction = false;
	Controller2D controller;

	void Start() {
		Reset ();
		controller = GetComponent<Controller2D> ();
		gravity = -(2 * jumpHeight) / Mathf.Pow (timeToJumpApex, 2);
		controller.setGravityScale(gravity);
		jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
//		print ("Gravity: " + gravity + "  Jump Velocity: " + jumpVelocity);
	}

	void Reset() {
		startPosition = new Vector2 (-8.0f, 1.5f);
		transform.position = startPosition;
		isAlive = true;
	}

	void Update() {

		if (controller.collisions.above || controller.collisions.below) {
			velocity.y = 0.0f;
		}

		float inputX = 0.0f;
		if (Input.GetKey(leftKey)) {
			inputX = -1.0f;
		} else if (Input.GetKey(rightKey)) {
			inputX = 1.0f;
		}
		float inputY = 0.0f;
		if (Input.GetKey(upKey)) {
			inputY = 1.0f;
		} else if (Input.GetKey(downKey) ){
			inputY = -1.0f;
		}
		if (Input.GetKeyDown (downKey)) {
			attemptingInteraction = true;
		} else {
			attemptingInteraction = false;
		}

				
		if (Input.GetKey (jumpKey) && controller.collisions.below) {
			velocity.y = jumpVelocity;
		}
		float targetVelocityX = inputX * moveSpeed;
		velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below)?accelerationTimeGrounded:accelerationTimeAirborne);
		Vector2 input = new Vector2 (inputX, inputY);
		velocity.y += gravity * Time.deltaTime;
		//controller.Move (velocity * Time.deltaTime, input);
		//Debug.Log(velocity.y);
		controller.Move (velocity, input);

		if (transform.position.y < bottomOfTheWorld) {
			isAlive = false;
		}

		if (!isAlive) {
			Reset ();
		}

//		Debug.Log (transform.position.x + ", " + transform.position.y);

	}
}
