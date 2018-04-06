using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangingScene : MonoBehaviour {

    public GameObject Switch;
    private GameObject Barrier;
    private GameObject[] Floors;
    public int isContinueCount;
    public float barrierTimer;
	// Use this for initialization
	void Start ()
    {
        Barrier = GameObject.Find("Barrier");
	}
	
	// Update is called once per frame
	void Update ()
    {
        Floors = GameObject.FindGameObjectsWithTag("Floor");
        if (Floors.Length <1)
        {
            Switch.active = true;
            isContinueCount += 1;
            StartCoroutine(TurningOffBarrier());
        }
    }

    IEnumerator TurningOffBarrier()
    {
        yield return new WaitForSeconds(barrierTimer);
        Barrier.active = false;
        this.enabled = false;
    }
}
