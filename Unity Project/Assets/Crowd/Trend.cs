using UnityEngine;
using System.Collections;

public enum Hat { Pope, Bowler, Top }

public class Trend : MonoBehaviour {

  private Hat currentHat;
  private Animator anim;          // Reference to the player's animator component.
  private GameObject PopeHat;
  private GameObject BowlerHat;
  private GameObject TopHat;


  void Awake()
  {
    // Setting up references.
    anim = GetComponent<Animator>();
    PopeHat = transform.Find("PopeHat").gameObject;
    BowlerHat = transform.Find("BowlerHat").gameObject;
    TopHat = transform.Find("TopHat").gameObject;

    PopeHat.renderer.enabled = false;
    TopHat.renderer.enabled = false;
    BowlerHat.renderer.enabled = true;

    currentHat = Hat.Bowler;
  }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

  public void ChangeHat(Hat h) {
    if (h != currentHat)
    {
      switch(h) {
        case(Hat.Bowler):
          PopeHat.renderer.enabled = false;
          TopHat.renderer.enabled = false;
          BowlerHat.renderer.enabled = true;
          break;
        case(Hat.Top):
          PopeHat.renderer.enabled = false;
          TopHat.renderer.enabled = true;
          BowlerHat.renderer.enabled = false;
          break;
        case(Hat.Pope):
          PopeHat.renderer.enabled = true;
          TopHat.renderer.enabled = false;
          BowlerHat.renderer.enabled = false;
          break;
      }
      currentHat = h;
    }
    anim.SetTrigger("Hat");
  }
}
