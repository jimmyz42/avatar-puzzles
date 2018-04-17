using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeavesController : MonoBehaviour {

    public float delay;
    public bool disappear;
    public float endPos;
    public float speed;
	// Use this for initialization
	void Start ()
    {
       
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(disappear)
        {
            MoveDown();
        }
		
	}


    void MoveDown()
    {
        //transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, endPos, transform.position.z), speed * Time.deltaTime);
    }

    public void SetParams(bool d, float s)
    {
        speed = s;
        disappear = d;
    }


}
