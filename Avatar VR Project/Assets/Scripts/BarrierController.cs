using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Falling_Rock")
        {
            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
            rb.velocity = -rb.velocity;
        }
    }
}
