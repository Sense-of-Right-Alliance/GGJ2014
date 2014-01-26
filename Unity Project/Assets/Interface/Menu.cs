using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

  public GameObject[] players;

	// Use this for initialization
	void Start () {
    players[0].GetComponent<PlayerClass>().Set(Class.TrendSetter);
    players[1].GetComponent<PlayerClass>().Set(Class.PickPocket);
    players[2].GetComponent<PlayerClass>().Set(Class.TrendSetter);
    players[3].GetComponent<PlayerClass>().Set(Class.TrendSetter);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
