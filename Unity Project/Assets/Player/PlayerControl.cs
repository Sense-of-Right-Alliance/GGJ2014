using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {
	
	public float moveForce = 150f;			// Amount of force added to move the player left and right.
	public float maxSpeed = 1f;				// The fastest 

	public int id = 1;

	private string[] axes;

	// Use this for initialization
	void Start () {
		GetAxes ();
	}

	void GetAxes() {
		axes = new string[7];
		axes [0] = "P" + id.ToString () + "_Horizontal";
		axes [1] = "P" + id.ToString () + "_Vertical";
		axes [2] = "P" + id.ToString () + "_A";
		axes [3] = "P" + id.ToString () + "_B";
		axes [4] = "P" + id.ToString () + "_X";
		axes [5] = "P" + id.ToString () + "_Y";
		axes [6] = "P" + id.ToString () + "_Trigger";
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// Fixed update for ensure physics stuff happens only once per frame.
	void FixedUpdate () {
		
		// Cache the horizontal input.
		float h = Input.GetAxis(axes[0]);
		
		// The Speed animator parameter is set to the absolute value of the horizontal input.
		//anim.SetFloat("Speed", Mathf.Abs(h));
		
		// If the player is changing direction (h has a different sign to velocity.x) or hasn't reached maxSpeed yet...
		if(h * rigidbody2D.velocity.x < maxSpeed)
			// ... add a force to the player. 
			rigidbody2D.AddForce(Vector2.right * h * moveForce);
		
		// If the player's horizontal velocity is greater than the maxSpeed...
		if(Mathf.Abs(rigidbody2D.velocity.x) > maxSpeed)
			// ... set the player's velocity to the maxSpeed in the x axis.
			rigidbody2D.velocity = new Vector2(Mathf.Sign(rigidbody2D.velocity.x) * maxSpeed, rigidbody2D.velocity.y);

		// Cache the horizontal input.
		float v = Input.GetAxis(axes[1]);
		
		// The Speed animator parameter is set to the absolute value of the horizontal input.
		//anim.SetFloat("Speed", Mathf.Abs(h));
		
		// If the player is changing direction (h has a different sign to velocity.x) or hasn't reached maxSpeed yet...
		if(h * rigidbody2D.velocity.x < maxSpeed)
			// ... add a force to the player. 
			rigidbody2D.AddForce(Vector2.right * h * moveForce);
		
		// If the player's horizontal velocity is greater than the maxSpeed...
		if(Mathf.Abs(rigidbody2D.velocity.x) > maxSpeed)
			// ... set the player's velocity to the maxSpeed in the x axis.
			rigidbody2D.velocity = new Vector2(Mathf.Sign(rigidbody2D.velocity.x) * maxSpeed, rigidbody2D.velocity.y);
		
	}
}
