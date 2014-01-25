using UnityEngine;
using System.Collections;

public class CrowdeeMovement : MonoBehaviour {
	// 	TODO-DG: Fix me!

	enum CrowdeeState { idle, walk	}

	private CrowdeeState state;
	// Waits to change state
	private float timer;
	private float max_wait = 4.0f;
	private float min_wait = 1.0f;

	// Movement
	private float horiz = 0.0f;
	private float vert = 0.0f;
	private float speed = 300;
	public float moveForce = 1f;			// Amount of force added to move the player left and right.
	public float maxSpeed = 0.05f;				// The fastest

	// Use this for initialization
	void Start () {
		state = CrowdeeState.idle;
		timer = Random.Range (min_wait, max_wait);
	}
	
	// Update is called once per frame
	void Update () {
		timer -= Time.deltaTime;

		if (timer <= 0.0) 
		{
			ChangeAction();
		}

		if (state == CrowdeeState.walk) {
			HandleMovement();
		}
	}

	void ChangeAction () {
		if (state == CrowdeeState.idle) 
		{
			Walk();
		} 
		else if (state == CrowdeeState.walk) 
		{
			Wait();
		}
		timer = Random.Range (min_wait, max_wait);
	}

	void HandleMovement() {

		// The Speed animator parameter is set to the absolute value of the horizontal input.
		//anim.SetFloat("Speed", Mathf.Abs(h));
		
		// If the player is changing direction (h has a different sign to velocity.x) or hasn't reached maxSpeed yet...
		/*if(horiz * rigidbody2D.velocity.x < maxSpeed)
			// ... add a force to the player. 
			rigidbody2D.AddForce(Vector2.right * horiz * moveForce);
		
		// If the player's horizontal velocity is greater than the maxSpeed...
		if (Mathf.Abs (rigidbody2D.velocity.x) > maxSpeed) {

			//Debug.Log ("Max Speed reached: " + maxSpeed);
			// ... set the playe	r's velocity to the maxSpeed in the x axis.
			rigidbody2D.velocity = new Vector2 (Mathf.Sign (rigidbody2D.velocity.x) * maxSpeed, rigidbody2D.velocity.y);
		}*/

		// If the player is changing direction (h has a different sign to velocity.x) or hasn't reached maxSpeed yet...
		if(vert * rigidbody2D.velocity.y < maxSpeed)

			// ... add a force to the player. 
			rigidbody2D.AddForce(new Vector2(0.0f,vert * moveForce));
		
		// If the player's horizontal velocity is greater than the maxSpeed...
		if (Mathf.Abs (rigidbody2D.velocity.y) > maxSpeed) {
			
			//Debug.Log ("Max Speed reached: " + maxSpeed);
			// ... set the playe	r's velocity to the maxSpeed in the x axis.
			rigidbody2D.velocity = new Vector2 (Mathf.Sign (rigidbody2D.velocity.x), rigidbody2D.velocity.y * maxSpeed);
		}


	}

	void Walk() {
		horiz = Random.value * 2.0f - 1.0f;
		vert = Random.value * 2.0f - 1.0f;

		state = CrowdeeState.walk;

		Debug.Log ("Walk = " + horiz + " " + vert);
	}

	void Wait() {
		Debug.Log ("Wait velocity = " + rigidbody2D.velocity);
		state = CrowdeeState.idle;
	}
}
