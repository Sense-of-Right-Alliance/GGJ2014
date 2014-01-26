using UnityEngine;
using System.Collections;

public class StalkingEffect : MonoBehaviour {

  private GameObject detective;
  private GameObject target;

  private float stalkTimer = 2.0f; // time to wait before accumulating points
  private float pointTimer = 0.2f; // gain points when 0
  private float minStalkDistance = 0.7f;

	// Use this for initialization
	void Start () {
    particleSystem.enableEmission = false;
	}
	
  public void Set(GameObject det, GameObject stalkee) {
    detective = det;
    target = stalkee;
  }

	// Update is called once per frame
	void Update () {
    if (stalkTimer > 0.0f)
    {
      stalkTimer -= Time.deltaTime;
    }

    transform.position = target.transform.position;
    float distance = Vector3.Distance(target.transform.position, detective.transform.position);//Mathf.Abs((target.transform.position - transform.position).magnitude);
    Debug.Log(distance + " > " + minStalkDistance);
    if (distance > minStalkDistance)
    {
      target.GetComponent<PlayerClass>().isBeingStalked = false;
      Destroy(gameObject);

    } else if(stalkTimer <= 0.0f) {
      pointTimer -= Time.deltaTime;
      if(pointTimer <= 0.0f) {
        detective.GetComponent<PlayerClass>().score += 1;

        pointTimer = 0.2f + target.GetComponent<PlayerClass>().stalkValue;
        target.GetComponent<PlayerClass>().stalkValue += pointTimer/15.0f;
      }

      particleSystem.enableEmission = true;
    }
	}
}
