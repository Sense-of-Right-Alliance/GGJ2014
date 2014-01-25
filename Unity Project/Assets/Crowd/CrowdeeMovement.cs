using UnityEngine;
using System.Collections;

public class CrowdeeMovement : MonoBehaviour {
	// 	TODO-DG: Fix Me! Pshh, it's totally great now. We're done here.... 
  //  except they walk off the screen and are wierd. Run it and see!

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

  private Animator anim;          // Reference to the player's animator component.

  void Awake()
  {
    // Setting up references.
    anim = GetComponent<Animator>();
  }

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
    // Move the player via their rigidbody.
    rigidbody2D.AddForce(new Vector2(horiz * moveForce * Time.deltaTime, vert * moveForce * Time.deltaTime));

    if (Mathf.Abs (rigidbody2D.velocity.magnitude) > maxSpeed) {
      rigidbody2D.velocity = new Vector2 (Mathf.Sign (rigidbody2D.velocity.x) * maxSpeed, Mathf.Sign (rigidbody2D.velocity.y) * maxSpeed);
    }

    anim.SetFloat("Speed", Mathf.Abs(new Vector2(horiz,vert).magnitude));

	}

	void Walk() {
		horiz = Random.value * 2.0f - 1.0f;
		vert = Random.value * 2.0f - 1.0f;

		state = CrowdeeState.walk;
	}

	void Wait() {
		state = CrowdeeState.idle;
	}
}
