using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

// types of hats in the game
public enum Hat { NoHat, Pope, Bowler, Top, Workman }

public class Trend : MonoBehaviour
{
  private Animator Anim; // Reference to the player's animator component.
  protected Dictionary<Hat,GameObject> HatObjects; // collection of our hat game objects
  public Hat CurrentHat; // hat currently wornly by character
  
  // default hat worn by characters when the game starts
  protected virtual Hat DefaultHat { get { return Hat.NoHat; } }
  
  // hat cooldown
  protected virtual double HatCooldownMillseconds { get { return 3000; } } // minimum time between hat uses
  protected Timer HatCooldownTimer;
  protected bool HatOnCooldown = false; // whether the hat can be used currently
  
  public bool StolenFromRecently = false;
  
  bool TransformWaiting = false;
  Hat TransformHat;
  float TransformTransmission = 0.0f;
  PlayerClass TransformInitiator;
  
  bool HatWaiting = false;
  Hat HatOnCall;
  
  bool AnimTriggerWaiting = false;
  string AnimTriggerString = null;
  
  bool RespondingToTrendEvent = false;
  Hat TrendEventHat;
  float TrendEventTransmissionRate;
  PlayerClass TrendEventInitiator;
  
  bool StartingTrendEvent;
  Hat NewTrendEventHat;
  
  protected virtual void Awake()
  {
    // Setting up references.
    Anim = GetComponent<Animator>();
    HatObjects = new Dictionary<Hat,GameObject>()
    {
      {Hat.Pope, transform.Find("PopeHat").gameObject},
      {Hat.Bowler, transform.Find("BowlerHat").gameObject},
      {Hat.Top, transform.Find("TopHat").gameObject},
      {Hat.Workman, transform.Find("DickHat").gameObject},
    };
  }
  
	// Use this for initialization
	protected virtual void Start()
  {
    SetCurrentHat(DefaultHat);
    if (tag == "Player")
    {
      switch(GetComponent<PlayerClass>().playerClass) {
        case(Class.Detective):
          SetCurrentHat(Hat.Workman);
          break;
        case(Class.PickPocket):
          SetCurrentHat(Hat.Bowler);
          break;
        case(Class.RoughHouser):
          SetCurrentHat(Hat.Pope);
          break;
        case(Class.TrendSetter):
          SetCurrentHat(Hat.Top);
          break;
        case(Class.BaldMan):
          SetCurrentHat(Hat.Pope);
          break;
      }
    }

    HatCooldownTimer = new Timer(HatCooldownMillseconds) // timer counts down after hat has been used
    {
      AutoReset = false,
    };
    HatCooldownTimer.Elapsed += HatCooldownTimer_Elapsed;
	}
	
	// Update is called once per frame
  protected virtual void Update()
  {
    Debug.DrawLine(transform.position, transform.position + Vector3.right * 3);
    // these must be done in main thread because they access UnityEngine's renderer
    if (AnimTriggerWaiting)
    {
      Anim.SetTrigger(AnimTriggerString);
      AnimTriggerWaiting = false;
    }
    
    // only npcs and the trend setter can cause trends
    if (TransformWaiting && ((tag == "Player" && GetComponent<PlayerClass>().playerClass == Class.TrendSetter) || tag != "Player"))
    {
      if(tag == "Player" && GetComponent<PlayerClass>().playerClass == Class.TrendSetter) {
        GetComponent<SFXPlayer>().PlaySFX("Crowd");
      }

      //Debug.Log(TransformTransmission);
      var furtherTransmissionChance = TransformTransmission * 0.5f - 0.05f;
      var nearbyObjects = GetObjectsInRadius(transform.position, 1);
      int trendCount = 0; // for counting how many you successfully influenced (trendsetter)
      foreach (var gameObject in nearbyObjects)
      {
        if (gameObject.tag != "Player" 
            || (gameObject.GetComponent<PlayerClass>().playerClass == Class.BaldMan 
            && !gameObject.GetComponent<PlayerClass>().immuneToTrends)) {
          var trend = gameObject.GetComponent<Trend>();

          if (trend != null && trend.CurrentHat != TransformHat && TransformTransmission > Random.value)
          {
            if (gameObject.tag == "Player"  && gameObject.GetComponent<PlayerClass>().playerClass == Class.BaldMan)
              furtherTransmissionChance = 0;

            trendCount++;
            trend.RespondToTrendEvent(TransformHat, furtherTransmissionChance, Random.Range(100,400), TransformInitiator);
          }
        }
      }

      if (tag == "Player")
      {
        GetComponent<PlayerClass>().HandleTrendSet(trendCount);
      }
      
      TransformWaiting = false;
    }
    
	  if (HatWaiting)
    {
      if (CurrentHat != HatOnCall)
        StolenFromRecently = false;
      
      // hide all hats
      foreach (var hatObject in HatObjects.Values)
        hatObject.renderer.enabled = false;
      
      // display the new hat and update reference to the current hat
      if(HatOnCall != Hat.NoHat) 
        HatObjects[HatOnCall].renderer.enabled = true;
      CurrentHat = HatOnCall;
      
      HatWaiting = false;
    }
    
    if (RespondingToTrendEvent)
    {
      RespondingToTrendEvent = false;
      if (CurrentHat != TrendEventHat) {
        //if(TrendEventInitiator != null && TrendEventInitiator.tag == "Player" && TrendEventInitiator.playerClass == Class.TrendSetter)
          //GetComponent<SFXPlayer>().PlaySFX("Crowd");
        ChangeHat(TrendEventHat, TrendEventTransmissionRate, TrendEventInitiator, GetComponent<PlayerClass>());
      }
    }
    
    if (StartingTrendEvent)
    {
      GetComponent<PlayerClass>().MakeTrendAnimation();
      ChangeHat(NewTrendEventHat, 1.0f, GetComponent<PlayerClass>(), GetComponent<PlayerClass>());
      StartingTrendEvent = false;
    }
  }
  
  protected virtual void HatCooldownTimer_Elapsed(object sender, ElapsedEventArgs e)
  {
    HatOnCooldown = false;
  }
  
  // determines whether the character can change to a given hat at this time
  protected virtual bool CanChangeHat(Hat newHat)
  {
    return !HatOnCooldown;
  }

  public void TryChangeHat(Hat newHat)
  {
    if (CanChangeHat(newHat)) // check if the hat can be swapped
    {
      NewTrendEventHat = newHat;
      StartingTrendEvent = true;
    }
  }
  
  public void RespondToTrendEvent(Hat newHat, float transmissionChance, float time, PlayerClass initiator)
  {
    Timer waitTimer = new Timer(time)
    {
      AutoReset = false,
    };
    waitTimer.Elapsed += (object sender, ElapsedEventArgs e) => RespondingToTrendEvent = true;
    waitTimer.Start();
    
    TrendEventHat = newHat;
    TrendEventInitiator = initiator;
    TrendEventTransmissionRate = transmissionChance;
  }
  
  public virtual void ChangeHat(Hat newHat, float transmissionChance, PlayerClass initiator, PlayerClass self)
  {
    if (self != null && self.playerClass == Class.Detective)
    {
      return;
    }
    
    if (initiator != null && initiator.playerClass == Class.TrendSetter && initiator != self)
      initiator.score += 0.5f;
    
    // update the current hat
    SetCurrentHat(newHat);
    if(self != null && self.playerClass == Class.BaldMan)
    {
      self.BaldModeOff(); //self.baldMode = false;
    }
    // display swap animation (must be done in main thread)
    AnimTriggerString = "Hat";
    AnimTriggerWaiting = true;
  
    // trigger local trend
    if (transmissionChance > 0)
      InitiateTrend(newHat, transmissionChance, initiator);
  
    // initiate hat cooldown
    HatOnCooldown = true;
    HatCooldownTimer.Start();
  }
  
  protected virtual void InitiateTrend(Hat trendyHat, float transmissionChance, PlayerClass initiator)
  {
    TransformHat = trendyHat;
    TransformTransmission = transmissionChance;
    TransformWaiting = true;
    TransformInitiator = initiator;
  }
  
  IEnumerable<GameObject> GetObjectsInRadius(Vector3 center, float radius)
  {
    var colliders = Physics2D.OverlapCircleAll(center, radius);//Physics.OverlapSphere(center, radius);
    return colliders.Select(u => u.gameObject);
    //return Physics.OverlapSphere(center, radius).Select(u => u.gameObject);
  }
  
  public void SetCurrentHat(Hat newHat)
  {
    // this must be done in main thread because it accesses UnityEngine's renderer
    HatOnCall = newHat;
    HatWaiting = true;
  }
}
