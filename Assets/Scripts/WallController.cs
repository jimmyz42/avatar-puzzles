using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallController : MonoBehaviour {

    public bool canMove;
    public float speed;
    public float endPos;
    public float delay;
    public GameObject LaserStart;
    public GameObject RealLaser;
    public LaserController laser;
    //public Vector3 startPos;
    // Use this for initialization
    void Start ()
    {
        laser = RealLaser.GetComponent<LaserController>();
        LaserStart.SetActive(false);
    }
	
	// Update is called once per frame
	void Update ()
    {
	    if (canMove)
        {
            MoveDown();
        }

        if (endPos==transform.position.y)
        {
            StartCoroutine(StartGame());
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

    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(delay);
        LaserStart.SetActive(true);
        laser.TurnLaserOn(true);
    }
}
