using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Controller2D))]
public class FollowPlayer : MonoBehaviour {

	public Player followObj;
	public float bottomOfTheWorld = -10.0f;
	public Controller2D controller;
	float gravity;
	float jumpVelocity;
	Vector3 velocity;
	float velocityXSmoothing;

	public float jumpHeight = 4.0f;
	public float timeToJumpApex = .4f;
	public string playerName = "Player 1";
	float accelerationTimeAirborne = .2f;
	float accelerationTimeGrounded = .1f;
	float moveSpeed = 8.0f;
	public bool targetSet = true;

	// Use this for initialization
	private GameManager gameManager;
	void Start () {
		controller = GetComponent<Controller2D> ();
		gameManager = FindObjectOfType<GameManager> ();
		gravity = -(2 * jumpHeight) / Mathf.Pow (timeToJumpApex, 2);
		controller.setGravityScale(gravity);
		jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
		setTarget(FindObjectOfType<Player> ());
	}

	void setTarget(Player target) {
		targetSet = true;
		followObj = target;
	}
	// Update is called once per frame
	void Update () {
		if (controller.collisions.above || controller.collisions.below) {
			velocity.y = 0.0f;
		}
		float inputX = 0.0f;
		float inputY = 0.0f;
		if (followObj.transform.position.x > transform.position.x) {
			controller.setFacingLeft (false);
			inputX = 1.0f;
		} else {
			controller.setFacingLeft (true);
			inputX = -1.0f;
		}
		float targetVelocityX = inputX * moveSpeed;
		velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below)?accelerationTimeGrounded:accelerationTimeAirborne);
		Vector2 input = new Vector2 (inputX, inputY);
		velocity.y += gravity * Time.deltaTime;

		controller.Move (velocity, input);
		controller.alive = transform.position.y >= bottomOfTheWorld && controller.health > 0;
		if (!controller.alive) {
			Destroy (gameObject);
		}
	}
}
