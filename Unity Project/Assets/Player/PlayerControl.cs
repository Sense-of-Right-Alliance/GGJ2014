using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class PlayerControl : MonoBehaviour
{
	public float moveForce = 500f;			// Amount of force added to move the player left and right.
	public float maxSpeed = 1f;				// The fastest 

  float chaseForce = 501f;
  float maxChaseSpeed = 0.5f;

	public int id = 1; // identity of the player (assigned 1 through 4)

  private Animator anim;          // Reference to the player's animator component.

  private float tackleTimer = 0.0f;
  private bool isTackled = false;
  
  private GameObject arrow;

  // types of controller input (names should those in Input Manager)
  private enum ControllerInput { Horizontal, Vertical, A, B, X, Y, Trigger }
  // association between a controller input type and the name assigned to it in the Input Manager
  private Dictionary<ControllerInput, string> InputName;

  void Awake()
  {
    // Setting up references.
    anim = GetComponent<Animator>();
    arrow = transform.Find("Arrow").gameObject;
  }

	// Use this for initialization
  void Start()
  {
    // associate input names
    InputName = new Dictionary<ControllerInput, string>();
    foreach (var input in Enum.GetValues(typeof(ControllerInput)).Cast<ControllerInput>())
      InputName.Add(input, FormatInputName(input));
  }
  
  // uses our standardized formatting to return the name for a given input type
  string FormatInputName(ControllerInput inputType)
  {
    return "P" + id.ToString() + "_" + inputType.ToString();
  }
	
	// Update is called once per frame
	void Update()
  {
    if (GetComponent<PlayerClass>().playerClass != Class.BaldMan && GetComponent<PlayerClass>().playerClass != Class.Detective && !isTackled)
    {
      if (Input.GetButtonDown(InputName[ControllerInput.Y]))
      {
        GetComponent<PlayerTrend>().TryChangeHat(Hat.Pope);
      }
    
      if (Input.GetButtonDown(InputName[ControllerInput.B]))
      {
        GetComponent<PlayerTrend>().TryChangeHat(Hat.Bowler);
      }
    
      if (Input.GetButtonDown(InputName[ControllerInput.X]))
      {
        GetComponent<PlayerTrend>().TryChangeHat(Hat.Top);
      }
    }
    
    if (Input.GetAxis(InputName[ControllerInput.Trigger]) > 0.5f || Input.GetAxis(InputName[ControllerInput.Trigger]) < -0.5f) 
    {
       // TODO-DG: Increase Arrow opacity by tiny every update loop  
      var color = arrow.GetComponent<SpriteRenderer>().color;
      color.a += 0.01f;
      arrow.GetComponent<SpriteRenderer>().color = color;
    } else {
      var color = arrow.GetComponent<SpriteRenderer>().color;
      color.a -= 0.02f;
      arrow.GetComponent<SpriteRenderer>().color = color;
    }
      //Debug.Log(Input.GetAxis(InputName[ControllerInput.Trigger]));

    if (Input.GetButtonDown(InputName[ControllerInput.A]))
    {
      GetComponent<PlayerClass>().TryInitiateBaldMode();
    }

    if (tackleTimer > 0.0f)
    {
      tackleTimer -= Time.deltaTime;
      if(tackleTimer <= 0.0f) {
        isTackled = false;
        transform.Rotate(0, 0, -90);
      }
    }
  }

	// Fixed update for ensure physics stuff happens only once per frame.
	void FixedUpdate()
  {
    if (!isTackled && GetComponent<PlayerClass>().CanMove)
    {
      float h = Input.GetAxis(InputName[ControllerInput.Horizontal]);

      float v = -Input.GetAxis(InputName[ControllerInput.Vertical]);

      float force = moveForce;
      if(GetComponent<PlayerClass>().baldMode) {
        force = chaseForce;
      }

      rigidbody2D.AddForce(new Vector2(h, v).normalized * force * Time.deltaTime);

      if(rigidbody2D.velocity.magnitude > maxChaseSpeed) {
        Debug.Log("Too Fast!");
        rigidbody2D.velocity = new Vector2(h, v).normalized * maxChaseSpeed; 
      }
	        // TODO-DG: Find screen dimensions here and don't add force if they're going to go out of bounds.
      // TODO-NP: Get me the proper x and y values to plug into the little magic iffffs down there. (I can't seem to access the player transform in here)
      // TODO-ALL: OR, use Unity magic to just do collision with the screen edges and GUI? Then they would bounce.
      /*if ( this.x >= 9.5 && h > 0.0f) {
         h = 0.0f;
      } else if ( this.x <= -9.5 && h < 0.0f) {
        h = 0.0f;
      }
      
      if ( this.y >= 4.0 && v > 0.0f) {
        v = 0.0f;
      } else if ( this.y <= 3.0 && v < 0.0f) {
        v = 0.0f;
      }*/

      float currSpeed = new Vector2(h, v).magnitude;
      //Debug.Log(currSpeed);

      // The Speed animator parameter is set to the absolute value of the horizontal input.
      anim.SetFloat("Speed", Mathf.Abs(new Vector2(h, v).magnitude));
    }
	}

  public void GetTackled() {
    transform.Rotate(0, 0, 90);
    tackleTimer = 5.0f;
    isTackled = true;
  }

  
}
