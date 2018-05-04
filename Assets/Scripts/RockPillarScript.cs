using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockPillarScript : MonoBehaviour {

	public float riseSpeed = 50f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (gameObject.transform.position.y < 50) {
			float newY = Mathf.Min (50f, gameObject.transform.position.y + riseSpeed * Time.deltaTime);
			Vector3 target = gameObject.transform.position;
			target.y = newY;
			gameObject.transform.position = target;
		}
	}
}
