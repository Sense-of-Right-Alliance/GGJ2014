using UnityEngine;
using System.Collections;

public class RemoveTimer : MonoBehaviour {

  public float timer = 1.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
    timer -= Time.deltaTime;
    if(timer <= 0.0){
      Destroy(gameObject);
    }
	}
}
