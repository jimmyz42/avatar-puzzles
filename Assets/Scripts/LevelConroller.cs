using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelConroller : MonoBehaviour {

    public GameObject Walls;
    public GameObject Leaves;
    //public GameObject inside;

    public float wallSpeed;
    public float wallDelay;
    public float leavesDrops;
    public float leavesFallDelay;
    public float gameStartDelay;


    private WallController walls;
    private LeavesController leaves;
    private bool cont;
    

	// Use this for initialization
	void Start ()
    {
        walls = Walls.GetComponent<WallController>();
        Walls.SetActive(false);
        leaves = Leaves.GetComponent<LeavesController>();
        cont = true;
        //inside.SetActive(false);
        //leaves.delay = leavesDrops;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (cont)
        {
            StartCoroutine(GoingToGame());
        }
	
	}

    IEnumerator GoingToGame()
    {
        yield return new WaitForSeconds(wallDelay);
        Walls.SetActive(true);
        walls.SetParams(true, wallSpeed, gameStartDelay);
        yield return new WaitForSeconds(leavesFallDelay);
        leaves.SetParams(true, leavesDrops);
        cont = false;
        //inside.SetActive(true);
        

        Destroy(this.gameObject);
        Debug.Log("suppose to be dead");
        gameObject.SetActive(false);

    }
}
