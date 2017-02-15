using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Controller2D))]
[RequireComponent (typeof (Fighter))]
public class Player : MonoBehaviour {

	public float bottomOfTheWorld = -10.0f;

	public Vector2 startPosition;
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

	public bool spawnNextToEndzone = false;

	public bool attemptingInteraction = false;
	Controller2D controller;
		

	private GameManager gameManager;

	internal void Start() {
		controller = GetComponent<Controller2D> ();
		Reset ();
		gravity = -(2 * jumpHeight) / Mathf.Pow (timeToJumpApex, 2);
		controller.setGravityScale(gravity);
		jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;

		gameManager = FindObjectOfType<GameManager> ();
	}

	public void Reset() {
		if (spawnNextToEndzone)
			startPosition = new Vector2 (105f, 14f); // this is right next to the endzone.
		else
			startPosition = new Vector2 (-4.0f, 1.5f);
		transform.position = startPosition;
		controller.accumulatedVelocity = Vector2.zero;
		controller.alive = true;
		controller.health = 100.0f;
		FindObjectOfType<PlayerCursor> ().currentPower = 20.0f;
		// reset should also bring back the startblock, if we want to keep using it.
	}

	internal void Update() {

		if (controller.collisions.above || controller.collisions.below) {
			velocity.y = 0.0f;
		}

		float inputX = 0.0f;
		float inputY = 0.0f;
		if (Input.GetKey(leftKey)) { 
			controller.setFacingLeft (true);
			inputX = -1.0f; 
		}  
		else if (Input.GetKey(rightKey)) { 
			inputX = 1.0f; 
			controller.setFacingLeft (false);
		}

		if (Input.GetKey(upKey)) { inputY = 1.0f; } 
		else if (Input.GetKey(downKey) ){ inputY = -1.0f; }

		if (Input.GetKeyDown (downKey)) {
			gameObject.GetComponent<Fighter> ().tryAttack ();
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
		controller.Move (velocity, input);

		controller.alive = transform.position.y >= bottomOfTheWorld && controller.health > 0;

		if (!controller.alive) {
			gameManager.gameOver = true;
			gameManager.winner = 2;
			Reset ();
		}
	}
}
