using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

public class CrowdeeTrend : Trend
{
  // time to wait before abandoning current trend
  protected virtual float MinSameTrendMilliseconds { get { return 5000; } } // minimum time to wait before giving up spreading current trend and trying to start a new one
  protected virtual float MaxSameTrendMilliseconds { get { return 30000; } } // maximum time to wait before giving up spreading current trend and trying to start a new one
  protected Timer SpreadSameTrendTimer;
  protected bool TryNewTrend = true;
  // time to wait after cooldown before initiating a hat event
  protected virtual float MinNoActionMilliseconds { get { return 2000; } } // maximum time after hat cools down before trying to use hat
  protected virtual float MaxNoActionMilliseconds { get { return 10000; } } // maximum time after hat cools down before trying to use hat
  protected Timer NoActionTimer;
  protected IEnumerable<Hat> crowdHats = System.Enum.GetValues(typeof(Hat)).Cast<Hat>().Except(new List<Hat>(){ Hat.Workman, Hat.NoHat });
  
  bool NoActionTimerElapsed, HatCooldownTimerElapsed, StartSameTrendTimer;
  
	// Use this for initialization
	protected override void Start()
  {
    base.Start();
    StartNoActionTimer(Random.Range(MinNoActionMilliseconds, MaxNoActionMilliseconds));
  }
  
  // Update is called once per frame
  protected override void Update()
  {
    base.Update();
    
    // once the no action timer has elapsed, it's time to get out your hat
    if (NoActionTimerElapsed)
    {
      Hat stylishHat = CurrentHat;
      if (TryNewTrend)
      {
        stylishHat = crowdHats.OrderBy(u => Random.value).First();
        StartSpreadSameTrendTimer(Random.Range(MinSameTrendMilliseconds, MaxSameTrendMilliseconds));
        TryNewTrend = false;
      }
      ChangeHat(stylishHat, 1.0f, GetComponent<PlayerClass>(), GetComponent<PlayerClass>());
      NoActionTimerElapsed = false;
    }
    if (HatCooldownTimerElapsed)
    {
      StartNoActionTimer(Random.Range(MinNoActionMilliseconds, MaxNoActionMilliseconds));
      HatCooldownTimerElapsed = false;
    }
    if (StartSameTrendTimer)
    {
      StartSpreadSameTrendTimer(Random.Range(MinSameTrendMilliseconds, MaxSameTrendMilliseconds));
    }
  }
  
  // starts the no action timer for a random amount of time up to the defined maximum
  void StartNoActionTimer(float time)
  {
    //random = MinNoActionMilliseconds + CSRNG.NextDouble() * (MaxNoActionMilliseconds - MinNoActionMilliseconds);//CSRNG.NextDouble() * MaxNoActionMilliseconds;
    NoActionTimer = new Timer(time)
    {
      AutoReset = false,
    };
    NoActionTimer.Elapsed += (object sender, ElapsedEventArgs e) => NoActionTimerElapsed = true;
    NoActionTimer.Start();
  }
  
  // starts the spread same trend timer for a random amount of time within the defined limits
  void StartSpreadSameTrendTimer(float time)
  {
    SpreadSameTrendTimer = new Timer(time) //MinSameTrendMilliseconds + CSRNG.NextDouble()*(MaxSameTrendMilliseconds - MinSameTrendMilliseconds
    {
      AutoReset = false,
    };
    // once the spread same trend timer has elapsed, it's time to try something new
    SpreadSameTrendTimer.Elapsed += (object sender, ElapsedEventArgs e) => TryNewTrend = true;
    SpreadSameTrendTimer.Start();
  }
  
  protected override void HatCooldownTimer_Elapsed(object sender, ElapsedEventArgs e)
  {
    base.HatCooldownTimer_Elapsed(sender, e);
    HatCooldownTimerElapsed = true;
  }
  
  public override void ChangeHat(Hat newHat, float transmissionChance, PlayerClass initiator, PlayerClass self)
  {
    if (newHat != CurrentHat)
      StartSameTrendTimer = true;
    
    base.ChangeHat(newHat, transmissionChance, initiator, self);
    NoActionTimer.Stop();
  }
}
