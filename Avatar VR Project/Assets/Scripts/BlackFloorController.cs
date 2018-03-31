using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackFloorController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerExit(Collider other)
    {

        if (other.tag=="Falling_Rock" || other.tag=="Rotating_Rock")
        {
            other.gameObject.active = false;
            //Destroy(other.gameObject);
        }
    }
}
