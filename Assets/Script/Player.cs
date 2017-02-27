using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

[RequireComponent (typeof (Movement))]
[RequireComponent (typeof (Fighter))]
[RequireComponent (typeof (Attackable))]

public class Player : NetworkBehaviour {

	public Vector2 startPosition;
	public float jumpHeight = 4.0f;
	public float timeToJumpApex = .4f;
	public string playerName = "Player 1";
	float accelerationTimeAirborne = .2f;
	float accelerationTimeGrounded = .1f;
	public float moveSpeed = 8.0f;

	float gravity;
	float jumpVelocity;
	Vector3 velocity;
	float velocityXSmoothing;

	public bool spawnNextToEndzone = false;

	public bool attemptingInteraction = false;
	Movement controller;
	Attackable attackable;
	public bool canDoubleJump = true;

	float timeSinceLeft = 0.0f;
	float timeSinceRight = 0.0f;
	float timeSinceLastDash = 0.0f;
	public float dashThreashold = 0.6f;
	float timeSinceLastAttack = 0.0f;
	public float dashTime = 0.15f;

	private GameManager gameManager;

	Animator anim;

	bool leftDown;
	bool left;
	bool rightDown;
	bool right;
	bool attackDown;
	bool jumpDown;

	[ClientRpc]
	public void RpcControls(bool lD, bool l, bool rD, bool r, bool aD, bool jD) {
		leftDown = lD;
		left = l;
		rightDown = rD;
		right = r;
		attackDown = aD;
		jumpDown = jD;
	}
	[Command]
	public void CmdControls(bool lD, bool l, bool rD, bool r, bool aD, bool jD) {
		leftDown = lD;
		left = l;
		rightDown = rD;
		right = r;
		attackDown = aD;
		jumpDown = jD;
	}
	public void updateControls(bool lD, bool l, bool rD, bool r, bool aD, bool jD) {
		leftDown = lD;
		left = l;
		rightDown = rD;
		right = r;
		attackDown = aD;
		jumpDown = jD;
	}

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
		// FindObjectOfType<PlayerCursor> ().currentPower = 20.0f;
		// reset should also bring back the startblock, if we want to keep using it.
	}

	internal void Update() {
		timeSinceLeft += Time.deltaTime;
		timeSinceRight += Time.deltaTime;
		timeSinceLastDash += Time.deltaTime;
		timeSinceLastAttack += Time.deltaTime;
		anim.SetBool ("grounded", controller.grounded);
		anim.SetBool ("tryingToMove", false);
		anim.SetBool ("isattacking", false);
		if (leftDown ) {
			if (timeSinceLeft < dashThreashold && attackable.energy > 25.0f
				&& timeSinceLastDash > 0.5f) {
				controller.addSelfForce (new Vector2 (-45.0f, 0.0f),dashTime);
				attackable.modifyEnergy( -25.0f);
				timeSinceLastDash = 0.0f;
			}
			timeSinceRight += dashThreashold;
			timeSinceLeft = 0.0f;
		}
		if (rightDown) {
			if (timeSinceRight < dashThreashold && attackable.energy > 25.0f
				&& timeSinceLastDash > 0.5f) {
				controller.addSelfForce(new Vector2(45.0f,0.0f),dashTime);
				attackable.modifyEnergy(-25.0f);
				timeSinceLastDash = 0.0f;
			}
			timeSinceLeft += dashThreashold;
			timeSinceRight = 0.0f;
		}

		if (controller.collisions.above || controller.collisions.below) {
			velocity.y = 0.0f;
		}
		if (controller.grounded) {
			canDoubleJump = true;
		}

		float inputX = 0.0f;
		float inputY = 0.0f;
		if (left) { 
			anim.SetBool ("tryingToMove",true);
			controller.setFacingLeft (true);
			inputX = -1.0f; 
		}  
		else if (right) { 
			anim.SetBool ("tryingToMove",true);
			inputX = 1.0f; 
			controller.setFacingLeft (false);
		}

		//if (Input.GetKey(upKey)) { inputY = 1.0f; } 
		//else if (Input.GetKey(downKey) ){ inputY = -1.0f; }

		if (attackDown) {
			if (gameObject.GetComponent<Fighter> ().tryAttack ()) {
				timeSinceLastAttack = 0.0f;
				anim.SetBool ("isattacking", true);
//				Debug.Log ("goteeem");
			}
			attemptingInteraction = true;
		} else {
			attemptingInteraction = false;
		}
				
		if (jumpDown) {
			if (controller.grounded) {
				velocity.y = jumpVelocity;
			} else if (canDoubleJump && attackable.energy > 30.0f) {
				velocity.y = jumpVelocity;
				canDoubleJump = false;
				attackable.modifyEnergy(-25.0f);
				//attackable.energy = attackable.energy - 30.0f;
			}
		}

		float targetVelocityX = inputX * moveSpeed;
		velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below)?accelerationTimeGrounded:accelerationTimeAirborne);
		Vector2 input = new Vector2 (inputX, inputY);
		velocity.y += gravity * Time.deltaTime;
		//Debug.Log (gravity);
		if (isLocalPlayer)
			controller.Move (velocity, input);
		else
			controller.Move (new Vector2(0.0f,-0.01f), Vector2.zero);
		if (!attackable.alive) {
			gameManager.gameOver = true;
			gameManager.winner = 2;
			Reset ();
		}
			
	}
}
