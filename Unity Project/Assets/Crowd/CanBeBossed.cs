using UnityEngine;
using System.Collections;
using System.Linq;

public class CanBeBossed : MonoBehaviour {

  public GameObject theBoss;

  private float bossedTimer = 10.0f;
  private bool isBossed = false; 

  private bool init = true;

	// Use this for initialization
	void Start () {

    /*foreach (GameObject player in players) {
      Debug.Log("class = " + player.GetComponent<PlayerClass>().playerClass);
      if(player.GetComponent<PlayerClass>().playerClass == Class.RoughHouser) {
        theBoss = player;

      }
    }*/
	}
	
	// Update is called once per frame
	void Update () {
	  if (isBossed)
    {
      bossedTimer -= Time.deltaTime;
      if(bossedTimer <= 0.0f) {
        isBossed = false;
        //Debug.Log("UnBossed");
      }
    }

    if (init)
    {
      var players = GameObject.FindGameObjectsWithTag("Player").Where(u => u.name != "Player(Clone)");
      //Debug.Log("found " + players.Count());
      foreach(var player in players)//for (int i = 0; i < players.Count; i++)
      {
        //Debug.Log("player '" + player.name + "' " + player.transform.position);// + " " + players[i].GetComponent<PlayerControl>().id);
        //Debug.Log("id = " + players[i].GetComponent<PlayerControl>().id + " class = " + players[i].GetComponent<PlayerClass>().playerClass);
        if(player.GetComponent<PlayerClass>() != null && player.GetComponent<PlayerClass>().playerClass == Class.RoughHouser) {
          theBoss = player;
          
        }
      }
      init = false;
    }
	}

  void OnCollisionEnter2D(Collision2D collider) {
    //Debug.Log(collider.gameObject.tag);
    if (collider.gameObject.tag == "Player" && collider.gameObject.GetComponent<PlayerClass>().playerClass == Class.RoughHouser)
    {
      isBossed = true;
      //Debug.Log("Boss Bumped");
    }
    else if(collider.gameObject.GetComponent<CanBeBossed>().isBossed && !isBossed)
    {
      theBoss.GetComponent<PlayerClass>().score += 2;
      isBossed = true;
    }
  }

  void OnCollisionExit2D(Collision2D collider) {
    if ((collider.gameObject.tag == "Player" && collider.gameObject.GetComponent<PlayerClass>().playerClass == Class.RoughHouser)
      && collider.gameObject.GetComponent<CanBeBossed>().isBossed)
    {
      isBossed = false;
    }
  }
}
