using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReFlooringScript : MonoBehaviour {

    public GameObject target;
    public Vector3 endPos;
    public bool endGame;
    public float speed;
    public GameObject Goal;
    public bool startMove;
    // Use this for initialization
    void Start ()
    {
        //endGame = false;
        target = GameObject.FindGameObjectWithTag("StartFloor");
        endPos = target.transform.position;
        Goal = GameObject.FindGameObjectWithTag("Finish");
        //gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (gameObject.active)
        {
            MovingFloorUp();
        }
        if (transform.position==endPos)
        {
            DoorController d = Goal.GetComponent<DoorController>();
            d.SetEnding(true);
        }
	}

    void MovingFloorUp()
    {
        gameObject.transform.position =Vector3.MoveTowards(gameObject.transform.position, new Vector3(transform.position.x, endPos.y, transform.position.z), speed * Time.deltaTime);
    }

    public void StartMoving(bool canMove)
    {
        startMove = canMove;
    }

}
