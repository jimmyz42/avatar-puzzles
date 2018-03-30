using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CliffContoller : MonoBehaviour {

    public float falling_speed;
    private Rigidbody rb;
    private int wait;
    private Vector3 start_pos;
    public float extra_speed;

	// Use this for initialization
	void Start ()
    {
        gameObject.tag = "Falling_Rock";
        rb = GetComponent<Rigidbody>();
        if (extra_speed==0)
        {
            extra_speed = 10;
        }
        
        if (falling_speed<=0)
        {
            falling_speed = (Random.value * 10)+5;
        }
        wait = Random.Range(0, 50);

    }
	
	// Update is called once per frame
	void Update ()
    {
        if (wait<5)
        {
            Move();
            //rb.useGravity = true;
        }
        else
        {
            wait -= 1;
        }
      
		
	}


    void Move ()
    {
        rb.AddForce(0, -falling_speed*extra_speed, 0);
    }
}
