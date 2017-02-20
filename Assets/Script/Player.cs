using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Movement))]
[RequireComponent (typeof (Fighter))]
[RequireComponent (typeof (Attackable))]
public class Player : MonoBehaviour {

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
	Movement controller;
	Attackable attackable;
	public bool canDoubleJump = true;

	float timeSinceLeft = 0.0f;
	float timeSinceRight = 0.0f;
	float timeSinceLastDash = 0.0f;
	public float dashThreashold = 0.6f;

	public bool grounded;

	private GameManager gameManager;

	Animator anim;

	internal void Start() {
		anim = GetComponent<Animator> ();
		controller = GetComponent<Movement> ();
		attackable = GetComponent<Attackable> ();
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
		attackable.resetHealth ();
		FindObjectOfType<PlayerCursor> ().currentPower = 20.0f;
		// reset should also bring back the startblock, if we want to keep using it.
	}

	internal void Update() {
		timeSinceLeft += Time.deltaTime;
		timeSinceRight += Time.deltaTime;
		timeSinceLastDash += Time.deltaTime;
		anim.SetBool ("grounded", controller.collisions.below);
		anim.SetBool ("tryingToMove", false);
		if (Input.GetKeyDown (leftKey) ) {
			if (timeSinceLeft < dashThreashold && attackable.energy > 25.0f
				&& timeSinceLastDash > 1.0f) {
				controller.addToVelocity (new Vector2 (-45.0f, 0.0f));
				attackable.energy -= 25.0f;
				timeSinceLastDash = 0.0f;
			}
			timeSinceRight += dashThreashold;
			timeSinceLeft = 0.0f;
		}
		if (Input.GetKeyDown (rightKey)) {
			if (timeSinceRight < dashThreashold && attackable.energy > 25.0f
				&& timeSinceLastDash > 1.0f) {
				controller.addToVelocity(new Vector2(45.0f,0.0f));
				attackable.energy -= 25.0f;
				timeSinceLastDash = 0.0f;
			}
			timeSinceLeft += dashThreashold;
			timeSinceRight = 0.0f;
		}

		if (controller.collisions.above || controller.collisions.below) {
			velocity.y = 0.0f;
		}
		if (controller.collisions.below) {
			canDoubleJump = true;
		}

		float inputX = 0.0f;
		float inputY = 0.0f;
		if (Input.GetKey(leftKey)) { 
			anim.SetBool ("tryingToMove",true);
			controller.setFacingLeft (true);
			inputX = -1.0f; 
		}  
		else if (Input.GetKey(rightKey)) { 
			anim.SetBool ("tryingToMove",true);
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
				
		if (Input.GetKeyDown (jumpKey)) {
			if (controller.collisions.below) {
				velocity.y = jumpVelocity;
			} else if (canDoubleJump && attackable.energy > 30.0f) {
				velocity.y = jumpVelocity;
				canDoubleJump = false;
				attackable.energy = attackable.energy - 30.0f;
			}
		}

		float targetVelocityX = inputX * moveSpeed;
		velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below)?accelerationTimeGrounded:accelerationTimeAirborne);
		Vector2 input = new Vector2 (inputX, inputY);
		velocity.y += gravity * Time.deltaTime;
		//Debug.Log (gravity);
		controller.Move (velocity, input);

		if (!attackable.alive) {
			gameManager.gameOver = true;
			gameManager.winner = 2;
			Reset ();
		}
			
	}
}
