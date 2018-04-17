using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelConroller : MonoBehaviour {

    public GameObject Walls;
    public GameObject Leaves;
    public GameObject LaserStart;
    public float wallSpeed;
    public float wallDelay;
    public float leavesDrops;
    public float leavesFallDelay;
    public float gameStartDelay;
    public SettingCenter[] Twirl;

    private WallController walls;
    private LeavesController leaves;
    private LaserController laser;
    

	// Use this for initialization
	void Start ()
    {
        walls = Walls.GetComponent<WallController>();
        Walls.SetActive(false);
        leaves = Leaves.GetComponent<LeavesController>();
        laser = LaserStart.GetComponent<LaserController>();

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
        Walls.SetActive(true);
        walls.SetParams(true, wallSpeed, gameStartDelay);
        yield return new WaitForSeconds(leavesFallDelay);
        leaves.SetParams(true, leavesDrops);

    }
}
