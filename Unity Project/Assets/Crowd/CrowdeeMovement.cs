using UnityEngine;
using System.Collections;

public class CrowdeeMovement : MonoBehaviour {
	
	enum CrowdeeState { idle, walk	}

	private CrowdeeState state;
	// Waits to change state
	private float timer;
	private float max_wait = 2.0f;
	private float min_wait = 1.0f;

	// Movement
	private float horiz = 0.0f;
	private float vert = 0.0f;
	private float speed = 300;
	public float moveForce = 1f;			// Amount of force added to move the player left and right.
	public float maxSpeed = 0.05f;				// The fastest

	private float x_bound_pos = 9.0f;
    private float x_bound_neg = -9.0f;
	private float y_bound_pos = 2.5f;
    private float y_bound_neg = -3.5f;

  private Animator anim;          // Reference to the player's animator component.

  void Awake()
  {
    // Setting up references.
    anim = GetComponent<Animator>();
  }

	// Use this for initialization
	void Start () {
		timer = Random.Range (min_wait, max_wait);
		float randomizer = Random.value;
		if (randomizer <= 0.2f) {
			  x_bound_pos = 0.5f;
        y_bound_pos = 0.5f;
			  Walk ();
		} else if (randomizer <= 0.4f) {
        x_bound_neg = -0.5f;
        y_bound_pos = 0.0f;
	  		state = CrowdeeState.idle;
		} else if (randomizer <= 0.6f) {
        x_bound_pos = 0.2f;
        y_bound_neg = -1.0f;        
        Walk ();
		} else if (randomizer <= 0.8f) {
        x_bound_neg = -0.2f;
        y_bound_neg = -0.5f;
        state = CrowdeeState.idle;
    } else {
        Walk ();
    }
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
	    // Move the player via their rigidbody.
	    rigidbody2D.AddForce(new Vector2(horiz * moveForce * Time.deltaTime, vert * moveForce * Time.deltaTime));
		
	    if (Mathf.Abs (rigidbody2D.velocity.magnitude) > maxSpeed) {
	      rigidbody2D.velocity = new Vector2 (Mathf.Sign (rigidbody2D.velocity.x) * maxSpeed, Mathf.Sign (rigidbody2D.velocity.y) * maxSpeed);
	    }
		
	    anim.SetFloat("Speed", Mathf.Abs(new Vector2(horiz,vert).magnitude));

	}

	void Walk() {
		// DG: HARDCODED WIDTH/HEIGHTS BECAUSE WHY IS THE SCREEN WIDTH 1000?
		if (this.transform.position.x >= x_bound_pos) {
			horiz = Random.value * -1.0f;
		} else if (this.transform.position.x <= x_bound_neg) {
			horiz = Random.value * 1.0f;
		} else {
			horiz = Random.value * 2.0f - 1.0f;	
		}

		if (this.transform.position.y >= 3.0f * y_bound_pos / 4.0f) {
				vert = Random.value * -1.0f;
		} else if (this.transform.position.y <= y_bound_neg) {
				vert = Random.value * 1.0f;
		} else {
            vert = Random.value * 2.0f - 1.0f;
        }


		state = CrowdeeState.walk;
	}

	void Wait() {
		state = CrowdeeState.idle;
	}
}
