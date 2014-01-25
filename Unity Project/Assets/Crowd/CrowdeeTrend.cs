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
		var hitColliders = Physics.OverlapSphere(center, radius);

		foreach (var collider in hitColliders)
    {
    
    }
	}
}
