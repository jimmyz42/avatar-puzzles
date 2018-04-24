using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallController : MonoBehaviour {

    public bool canMove;
    public float speed;
    public float endPos;
    public float delay;
    public GameObject LM;
//    public GameObject LaserStart;
//    public GameObject RealLaser;
//    public LaserController laser;
    //public Vector3 startPos;

    void Start ()
    {
//        laser = RealLaser.GetComponent<LaserController>();
//        LaserStart.SetActive(false);
    }
	
	// Update is called once per frame
	void Update ()
    {

        FullMovement();

	}
    
    void FullMovement()
    {
        if (endPos==transform.position.y)
        {
            //Do nothing
            SetMove(false);
            canMove = false;
            Destroy(LM);
        }
        else if (canMove)
        {
            MoveDown();
        }
    }
    void MoveDown()
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(0, endPos, 0), speed * Time.deltaTime);
    }

    public void SetParams(bool move, float s,float d)
    {
        canMove = move;
        speed = s;
        delay = d;
    }

    public void SetEndPos(float s)
    {
        endPos = s;
    }

    public void SetMove(bool w)
    {
        canMove = w;
    }
    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(delay);
        SetMove(false);
//        LaserStart.SetActive(true);
//        laser.TurnLaserOn(true);
    }
}
