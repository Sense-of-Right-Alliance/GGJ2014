using UnityEngine;
using System.Collections;

public enum Class { TrendSetter, PickPocket, RoughHouser, Detective, BaldMan } 

public class PlayerClass : MonoBehaviour {

  public GameObject testExplosion;
  public GameObject stalkingEffect;

  public bool isBeingStalked = false;
  public float stalkValue = 0.0f; // value decreases when being stalked, and recharges when not.



  public bool baldMode = false;
  private float baldModeCooldownTimer = 0.0f;
  private float baldModeCooldown = 10.0f;
  private float baldModeDurationTimer = 0.0f;
  private float baldModeDuration = 3.0f;
  private float baldModeWarmupTimer = 0.0f;
  private float baldModeWarmup = 3.0f;
  public bool immuneToTrends = false;

  private GameObject lastBaldMan;

  public Class playerClass;
  public float score = 0;
  
  private PlayerClass previousDetectedPlayer = null;

  public bool CanMove { get { return playerClass != Class.BaldMan || baldModeWarmupTimer <= 0.0f; } }

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

    if (baldModeDurationTimer > 0.0f)
    {
      baldModeDurationTimer -= Time.deltaTime;
      if(baldModeDurationTimer <= 0.0f)
      {
        immuneToTrends = false;
        baldModeCooldownTimer = baldModeCooldown;
      }
    }

    if (baldModeCooldownTimer > 0.0f)
    {
      baldModeCooldownTimer -= Time.deltaTime;
    }

    if (baldModeWarmupTimer > 0.0f)
    {
      baldModeWarmupTimer -= Time.deltaTime;
      if(baldModeWarmupTimer <= 0.0f) {
        baldMode = true;
        baldModeDurationTimer = baldModeDuration;
        GetComponent<SFXPlayer>().PlaySFX("BaldMode");

      }
    }
	}

  // parameter is number of people who changed hats with your influence
  public void HandleTrendSet(int numberOfPeople) {
    if (playerClass == Class.TrendSetter)
    {

    }
  }

  public void TryInitiateBaldMode() {
    if (playerClass == Class.BaldMan && !baldMode && baldModeCooldownTimer <= 0.0f)
    {
      baldModeWarmupTimer = baldModeWarmup;
      GetComponent<Trend>().SetCurrentHat(Hat.NoHat);
      immuneToTrends = true;
      isBeingStalked = false;
      GetComponent<SFXPlayer>().PlaySFX("BaldManWindup");
      var i = Camera.main.GetComponent<Interface>();
      if(i != null) 
        i.baldModeOn = true;
    }
  }

  public void BaldModeOff() {
    if (baldMode = true)
    {
      baldMode = false;
      baldModeCooldownTimer = baldModeCooldown;
      baldModeDurationTimer = 0.0f;
      GetComponent<SFXPlayer>().Stop();
      var i = Camera.main.GetComponent<Interface>();
      if (i != null) 
        i.baldModeOn = false;
    }
  }

  public void ChangeClass(Class c, Hat h) {
    playerClass = c;
    if (c == Class.BaldMan)
    {
      GetComponent<Trend>().SetCurrentHat(Hat.NoHat);
    }
    else
    {
      GetComponent<Trend>().SetCurrentHat(h);
    }

  }

  public void MakeTrendAnimation() {
    Instantiate(testExplosion, transform.position, transform.rotation);
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
          && (bumpedClass == null || bumpedClass.score >= 8)
          && !gameObject.GetComponent<Trend>().StolenFromRecently)
      {
        Instantiate(testExplosion, transform.position, transform.rotation);
        score += 2;
        GetComponent<SFXPlayer>().PlaySFX("Crook");
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
    var colliders = GetObjectsInRadius(transform.position, 1.0f);
    foreach (var c in colliders)
    {
      if(c.tag == "Player" && c.gameObject.GetComponent<PlayerControl>().id != GetComponent<PlayerControl>().id 
         && !c.gameObject.GetComponent<PlayerClass>().isBeingStalked
         && (c.gameObject.GetComponent<PlayerClass>().playerClass != Class.BaldMan || !c.gameObject.GetComponent<PlayerClass>().baldMode)) {
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
    if (collider.gameObject.tag == "Wall")
    {
      return;
    }
    HandleBump(collider.gameObject, collider.gameObject.GetComponent<Trend>().CurrentHat);
    if (playerClass == Class.Detective
      && collider.gameObject.tag == "Player" 
      && collider.gameObject.GetComponent<PlayerClass>().isBeingStalked
      && collider.gameObject.GetComponent<PlayerClass>().score >= 20
        && (collider.gameObject.GetComponent<PlayerClass>().playerClass != Class.BaldMan || !collider.gameObject.GetComponent<PlayerClass>().baldMode)
        && collider.gameObject.GetComponent<PlayerClass>() != previousDetectedPlayer)
    {
		  previousDetectedPlayer = collider.gameObject.GetComponent<PlayerClass>();
      GetComponent<SFXPlayer>().PlaySFX("Detective");
      collider.gameObject.GetComponent<PlayerControl>().GetTackled();
      score += 20 + (int)((float)collider.gameObject.GetComponent<PlayerClass>().score * 0.10f);
      collider.gameObject.GetComponent<PlayerClass>().score -= 20 + (int)((float)collider.gameObject.GetComponent<PlayerClass>().score * 0.10f);;
      collider.gameObject.GetComponent<PlayerClass>().isBeingStalked = false;
    }
    else if(playerClass == Class.BaldMan
            && collider.gameObject.tag == "Player" 
            && baldMode && collider.gameObject != lastBaldMan)
    {

      ChangeClass(collider.gameObject.GetComponent<PlayerClass>().playerClass, collider.gameObject.GetComponent<Trend>().CurrentHat);
      collider.gameObject.GetComponent<PlayerClass>().ChangeClass(Class.BaldMan, Hat.NoHat);
      GetComponent<SFXPlayer>().Stop();
      collider.gameObject.GetComponent<PlayerClass>().TryInitiateBaldMode();
      BaldModeOff();
      GetComponent<SFXPlayer>().PlaySFX("BaldManGotcha");
      collider.gameObject.GetComponent<PlayerClass>().lastBaldMan = gameObject;

      score += 20 + (int)((float)collider.gameObject.GetComponent<PlayerClass>().score * 0.10f);
    }
  }
}
