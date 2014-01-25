using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PlayerControl : MonoBehaviour {
	
	public float moveForce = 500f;			// Amount of force added to move the player left and right.
	public float maxSpeed = 1f;				// The fastest 

	public int id = 1;

  private Animator anim;          // Reference to the player's animator component.

  private string Input_Horizontal, Input_Vertical, Input_A, Input_B, Input_X, Input_Y, Input_Trigger;



  void Awake()
  {
    // Setting up references.
    anim = GetComponent<Animator>();
  }

	// Use this for initialization
	void Start () {
		GetAxes ();
	}

	void GetAxes() {
    Input_Horizontal = "P" + id.ToString () + "_Horizontal";
    Input_Vertical = "P" + id.ToString () + "_Vertical";
    Input_A = "P" + id.ToString () + "_A";
    Input_B = "P" + id.ToString () + "_B";
    Input_X = "P" + id.ToString () + "_X";
    Input_Y = "P" + id.ToString () + "_Y";
    Input_Trigger = "P" + id.ToString () + "_Trigger";
	}
	
	// Update is called once per frame
	void Update () {
    if (Input.GetButtonDown(Input_A))
    {
      Debug.Log("A" + " " + Input_A);
      GetComponent<PlayerTrend>().ChangeHat(Hat.Pope);
    }
    if (Input.GetButtonDown(Input_B))
    {
      GetComponent<PlayerTrend>().ChangeHat(Hat.Bowler);
    }
    if (Input.GetButtonDown(Input_X))
    {
      GetComponent<PlayerTrend>().ChangeHat(Hat.Top);
    }
  }
  
	// Fixed update for ensure physics stuff happens only once per frame.
	void FixedUpdate () {
    float h = Input.GetAxis(Input_Horizontal);

    float v = -Input.GetAxis(Input_Vertical);

		rigidbody2D.AddForce(new Vector2(h * moveForce * Time.deltaTime,
                                     
                                     v * moveForce * Time.deltaTime));

    float currSpeed = new Vector2(h, v).magnitude;
    //Debug.Log(currSpeed);

    // The Speed animator parameter is set to the absolute value of the horizontal input.
    anim.SetFloat("Speed", Mathf.Abs(new Vector2(h,v).magnitude));

		// Cache the horizontal input.
		/*float h = Input.GetAxis(axes[0]);
		
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
		if(v * rigidbody2D.velocity.y < maxSpeed)
			// ... add a force to the player. 
			rigidbody2D.AddForce(Vector2.right * v * moveForce);
		
		// If the player's horizontal velocity is greater than the maxSpeed...
		if(Mathf.Abs(rigidbody2D.velocity.y) > maxSpeed)
			// ... set the player's velocity to the maxSpeed in the x axis.
			rigidbody2D.velocity = new Vector2(Mathf.Sign(rigidbody2D.velocity.x), rigidbody2D.velocity.y * maxSpeed);*/
		
	}

  
}
