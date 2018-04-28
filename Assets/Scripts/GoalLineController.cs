using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalLineController : MonoBehaviour {

    public bool canMove;
    private Vector3 endpos;
    public float delay;
    public float speed;
	// Use this for initialization
	void Start ()
    {
        canMove = false;
        endpos = new Vector3(0, transform.position.y, 0);


    }
	
	// Update is called once per frame
	void Update ()
    {
        //if (canMove)
        //{
            StartCoroutine(MovingGoalLine());
        //}
        
		
	}
    
    void AppearingOnTheBoard()
    {
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, endpos, speed* Time.deltaTime);
    }

    IEnumerator MovingGoalLine()
    {
        yield return new WaitForSeconds(delay);
        AppearingOnTheBoard();
    }
}
