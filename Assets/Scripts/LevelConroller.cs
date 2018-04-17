using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelConroller : MonoBehaviour {

    public GameObject Walls;
    public GameObject Leaves;
    public float wallSpeed;
    public float wallDelay;
    public float leavesDrops;

    private WallController walls;
    private LeavesController leaves;
    

	// Use this for initialization
	void Start ()
    {
        walls = Walls.GetComponent<WallController>();
        leaves = Leaves.GetComponent<LeavesController>();
        //leaves.delay = leavesDrops;
	}
	
	// Update is called once per frame
	void Update ()
    {
        StartCoroutine(GoingToGame());	
	}

    IEnumerator GoingToGame()
    {
        yield return new WaitForSeconds(wallDelay);
        walls.SetParams(true, wallSpeed);
        yield return new WaitForSeconds(8f);
        leaves.SetParams(true, leavesDrops);

    }
}
