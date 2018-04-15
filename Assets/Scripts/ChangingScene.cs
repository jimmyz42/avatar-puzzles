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
        Debug.Log(GameObject.FindGameObjectsWithTag("Second_Floor"));
	}
	
	// Update is called once per frame
	void Update ()
    {
        Floors = GameObject.FindGameObjectsWithTag("Second_Floor");
        Debug.Log("here");
        Debug.Log(Floors);
        if (Floors.Length <1)
        {
            Switch.SetActive(true);
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
