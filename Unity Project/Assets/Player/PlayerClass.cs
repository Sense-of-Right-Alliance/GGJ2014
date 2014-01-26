using UnityEngine;
using System.Collections;

public enum Class { TrendSetter, PickPocket, RoughHouser, Detective } 

public class PlayerClass : MonoBehaviour {

  public GameObject testExplosion;
  public GameObject stalkingEffect;

  public bool isBeingStalked = false;
  public float stalkValue = 0.0f; // value decreases when being stalked, and recharges when not.

  public Class playerClass;
  public int score = 0;
  
  private Class previousDetectedPlayer = Class.Detective;

	// Use this for initialization
	void Start () {
	
	}

  public void Set(Class type) {
    playerClass = type;
  }
	
	// Update is called once per frame
	void Update () {
    switch (playerClass)
    {
      case(Class.TrendSetter):
        HandleTrendSetter();
        break;
      case(Class.PickPocket):
        HandlePickPocket();
        break;
      case(Class.RoughHouser):
        HandleRoughHouser();
        break;
      case(Class.Detective):
        HandleDetective();
        break;
    }
	
    if (!isBeingStalked)
    {
      stalkValue /= 10.0f;
    }
	}

  // parameter is number of people who changed hats with your influence
  public void HandleTrendSet(int numberOfPeople) {
    if (playerClass == Class.TrendSetter)
    {

    }
  }

  // For pickPocket!!!
  // called on a collision, passing in the colliding persons hat.
  public void HandleBump(GameObject gameObject, Hat bumpedHat) {
    if (playerClass == Class.PickPocket)
    {
      var thiefTrend = GetComponent<PlayerTrend>();
      var bumpedClass = gameObject.GetComponent<PlayerClass>();
      //Debug.Log("Is Pickpocket: " + theirHat.ToString() + " " + GetComponent<PlayerTrend>().CurrentHat.ToString());
      if (bumpedHat != thiefTrend.CurrentHat
          && rigidbody2D.velocity.magnitude > 0.05f
          && (bumpedClass == null || bumpedClass.score >= 8))
      {
        Instantiate(testExplosion, transform.position, transform.rotation);
        score += 2;
        if (bumpedClass != null) // thief steals from other players
        {
          var bumpedTrend = gameObject.GetComponent<PlayerTrend>();
          bumpedTrend.StolenFromRecently = true;
          score += 6;
          gameObject.GetComponent<PlayerClass>().score -= 8;
        }
      }
    }
  }

  // Nothing happens here! HandleTrendSet() is called when a player
  // changes a hat.
  void HandleTrendSetter() {
    // NATTA
  }

  void HandlePickPocket() {

  }

  void HandleRoughHouser() {

  }

  void HandleDetective() {
    var colliders = GetObjectsInRadius(transform.position, 0.25f);
    foreach (var c in colliders)
    {
      if(c.tag == "Player" && c.gameObject.GetComponent<PlayerControl>().id != GetComponent<PlayerControl>().id 
         && !c.gameObject.GetComponent<PlayerClass>().isBeingStalked) {
        c.gameObject.GetComponent<PlayerClass>().isBeingStalked = true;
        GameObject effect = (GameObject)Instantiate(stalkingEffect, c.gameObject.transform.position, c.gameObject.transform.rotation);
        effect.GetComponent<StalkingEffect>().Set(gameObject, c.gameObject);
      }
    }
  }
  
  Collider2D[] GetObjectsInRadius(Vector3 center, float radius)
  {
    var colliders = Physics2D.OverlapCircleAll(center, radius);//Physics.OverlapSphere(center, radius);
    return colliders;
    //return Physics.OverlapSphere(center, radius).Select(u => u.gameObject);
  }
  
  void OnCollisionEnter2D(Collision2D collider) {
    HandleBump(collider.gameObject, collider.gameObject.GetComponent<Trend>().CurrentHat);
    if(playerClass == Class.Detective
           && collider.gameObject.tag == "Player" 
           && collider.gameObject.GetComponent<PlayerClass>().isBeingStalked
           && collider.gameObject.GetComponent<PlayerClass>().score >= 20
           && collider.gameObject.GetComponent<PlayerClass>().playerClass != previousDetectedPlayer) {
        previousDetectedPlayer = collider.gameObject.GetComponent<PlayerClass>().playerClass;
      collider.gameObject.GetComponent<PlayerControl>().GetTackled();
      score += 20;
      collider.gameObject.GetComponent<PlayerClass>().score -= 20;
    }
  }
}
