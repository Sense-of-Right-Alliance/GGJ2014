using UnityEngine;
using System.Collections;

public class SFXPlayer : MonoBehaviour {

  public AudioClip[] Crowd;
  public AudioClip CrookCoins;
  public AudioClip BaldManGotcha;
  public AudioClip BaldManYaketySax;
  public AudioClip BaldManWindup;
  public AudioClip DetectiveHandcuff;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

  public void PlaySFX(string sfx) {
    audio.volume = 1.0f;
    switch(sfx) {
      case("Crook"):
        audio.PlayOneShot(CrookCoins);
        break;
      case("Crowd"):
        int i = Random.Range(0, Crowd.Length);
        audio.volume = 0.3f;
        audio.PlayOneShot(Crowd[i]);
        break;
      case("Detective"):
        audio.PlayOneShot(DetectiveHandcuff);
        break;
      case("BaldManGotcha"):
        audio.PlayOneShot(BaldManGotcha);
        break;
      case("BaldManWindup"):
        audio.PlayOneShot(BaldManWindup);
        break;
      case("BaldMode"):
        audio.clip = BaldManYaketySax;
        audio.loop = true;
        audio.Play();
        break;
    }
  }

  public void Stop() {
    audio.Stop();
    audio.loop = false;
  }
}
