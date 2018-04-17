using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallController : MonoBehaviour {

    public bool canMove;
    public float speed;
    public float endPos;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if (canMove)
        {
            MoveDown();
        }
	}

    void MoveDown()
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(0, endPos, 0), speed * Time.deltaTime);
    }

    public void SetParams(bool move, float s)
    {
        canMove = move;
        speed = s;
    }
}
