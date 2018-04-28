using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotating_Rock : MonoBehaviour {

    public Transform target;
    public float timer;
    private Rigidbody rb;
    private bool stopSpin;
	// Use this for initialization
	void Start ()
    {
        stopSpin = false;
        rb = GetComponent<Rigidbody>();
        StartCoroutine(StartFalling());
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!stopSpin)
        {
            BasicMovement();
        }
        
        
	}

    void BasicMovement()
    {
        transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);
        transform.RotateAround(target.transform.position, Vector3.up, 10 * Time.deltaTime);
    }

    void Move()
    {
        rb.useGravity = true;
        //rb.AddForce(0, -((Random.value*3) + 1), 0);
    }
    IEnumerator StartFalling()
    {
        yield return new WaitForSeconds(timer);
        Move();
        yield return new WaitForSeconds(2f);
        stopSpin = true;


    }
}
