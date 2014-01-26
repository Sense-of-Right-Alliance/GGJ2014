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
      if (numberOfPeople > 4)
      {
        score += 2*numberOfPeople;
      }
    }
  }

  // For pickPocket!!!
  // called on a collision, passing in the colliding persons hat.
  public void HandleBump(GameObject p, Hat theirHat) {
    if (playerClass == Class.PickPocket)
    {
      //Debug.Log("Is Pickpocket: " + theirHat.ToString() + " " + GetComponent<PlayerTrend>().CurrentHat.ToString());
      if (theirHat != GetComponent<PlayerTrend>().CurrentHat
          && rigidbody2D.velocity.magnitude > 0.05f
          && (score >= 2 || p.tag != "Player"))
      {
        Instantiate(testExplosion, transform.position, transform.rotation);
        score += 2;
        if(p.tag == "Player") { // theif steals from other players
          p.GetComponent<PlayerClass>().score -= 2;
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
           && collider.gameObject.GetComponent<PlayerClass>().score >= 20) {
      collider.gameObject.GetComponent<PlayerControl>().GetTackled();
      score += 20;
      collider.gameObject.GetComponent<PlayerClass>().score -= 20;
    }
  }
}
