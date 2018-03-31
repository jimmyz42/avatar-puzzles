using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangingScene : MonoBehaviour {

    public GameObject Switch;
    private GameObject[] Floors;
	// Use this for initialization
	void Start ()
    {
        //Floors = GameObject.FindGameObjectsWithTag("Floor");
	}
	
	// Update is called once per frame
	void Update ()
    {
        Floors = GameObject.FindGameObjectsWithTag("Floor");
        if (Floors.Length <1)
        {
            Switch.active = true;
        }
    }
}
