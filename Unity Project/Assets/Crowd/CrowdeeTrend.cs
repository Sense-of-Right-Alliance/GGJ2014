using UnityEngine;
using System.Collections;

public class CrowdeeTrend : Trend {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void GetObjectsInRadius(Vector2 center, float radius) {
		Collider[] hitColliders = Physics.OverlapSphere(center, radius);
		
		for (var i = 0; i < hitColliders.Length; i++) {
			//hitColliders[i]
		}
	}
}
