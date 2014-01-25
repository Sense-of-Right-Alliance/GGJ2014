using UnityEngine;
using System.Collections;

public class InputTest : MonoBehaviour {

	public float moveForce = 150f;			// Amount of force added to move the player left and right.
	public float maxSpeed = 1f;				// The fastest 

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate () {
		
		// Cache the horizontal input.
		float h = Input.GetAxis("Horizontal");
		
		// The Speed animator parameter is set to the absolute value of the horizontal input.
		//anim.SetFloat("Speed", Mathf.Abs(h));
		
		// If the player is changing direction (h has a different sign to velocity.x) or hasn't reached maxSpeed yet...
		if(h * rigidbody.velocity.x < maxSpeed)
			// ... add a force to the player. 
			rigidbody.AddForce(Vector2.right * h * moveForce);
		
		// If the player's horizontal velocity is greater than the maxSpeed...
		if(Mathf.Abs(rigidbody.velocity.x) > maxSpeed)
			// ... set the player's velocity to the maxSpeed in the x axis.
			rigidbody.velocity = new Vector2(Mathf.Sign(rigidbody.velocity.x) * maxSpeed, rigidbody.velocity.y);
		
		
	}
}
