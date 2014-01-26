using UnityEngine;
using System.Collections;
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
  protected double random;
  
	// Use this for initialization
	protected override void Start()
  {
    base.Start();
    StartNoActionTimer();
  }
  
  // Update is called once per frame
  protected override void Update()
  {
    base.Update();
  }
  
  // starts the no action timer for a random amount of time up to the defined maximum
  void StartNoActionTimer()
  {
    random = MinNoActionMilliseconds + CSRNG.NextDouble() * (MaxNoActionMilliseconds - MinNoActionMilliseconds);//CSRNG.NextDouble() * MaxNoActionMilliseconds;
    NoActionTimer = new Timer(random)
    {
      AutoReset = false,
    };
    NoActionTimer.Elapsed += NoActionTimer_Elapsed;
    NoActionTimer.Start();
   
  }
  
  // starts the spread same trend timer for a random amount of time within the defined limits
  void StartSpreadSameTrendTimer()
  {
    SpreadSameTrendTimer = new Timer(MinSameTrendMilliseconds + CSRNG.NextDouble()*(MaxSameTrendMilliseconds - MinSameTrendMilliseconds))
    {
      AutoReset = false,
    };
    SpreadSameTrendTimer.Elapsed += SpreadSameTrendTimer_Elapsed;
    SpreadSameTrendTimer.Start();
  }
  
  // once the no action timer has elapsed, it's time to get out your hat
  void NoActionTimer_Elapsed (object sender, ElapsedEventArgs e)
  {

    Hat stylishHat = CurrentHat;
    if (TryNewTrend)
    {
      stylishHat = (Hat)(int)(CSRNG.NextDouble()*System.Enum.GetNames(typeof(Hat)).Length);
      StartSpreadSameTrendTimer();
      TryNewTrend = false;
    }
    ChangeHat(stylishHat, 0.05f);
  }

  // once the spread same trend timer has elapsed, it's time to try something new
  void SpreadSameTrendTimer_Elapsed (object sender, ElapsedEventArgs e)
  {
    TryNewTrend = true;
  }
  
  protected override void HatCooldownTimer_Elapsed(object sender, ElapsedEventArgs e)
  {
    base.HatCooldownTimer_Elapsed(sender, e);
    StartNoActionTimer();
  }
  
  public override void ChangeHat(Hat newHat, float transmissionChance)
  {
    if (newHat != CurrentHat)
      StartSpreadSameTrendTimer();
    
    base.ChangeHat(newHat, transmissionChance);
    NoActionTimer.Stop();
  }
}
