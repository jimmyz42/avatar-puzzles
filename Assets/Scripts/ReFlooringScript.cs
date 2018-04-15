using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReFlooringScript : MonoBehaviour {

    public Transform endPos;
    public bool endGame;
    public float speed;
	// Use this for initialization
	void Start ()
    {
        //endGame = false;
        gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (gameObject.active)
        {
            //gameObject.SetActive(true);
            MovingFloorUp();
        }
	}

    void MovingFloorUp()
    {
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, endPos.position, speed * Time.deltaTime);
    }
}
