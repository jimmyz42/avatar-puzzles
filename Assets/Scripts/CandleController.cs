using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleController : MonoBehaviour {

    // Use this for initialization
    public GameObject fire;
    public bool TurnFireOn;

    void Start ()
    {
        fire = gameObject.transform.GetChild(0).gameObject;

	}
	
	// Update is called once per frame
	void Update ()
    {
		if (TurnFireOn)
        {
            fire.SetActive(true);
        }
        else
        {
            fire.SetActive(false);
        }
	}

    public void PutFireOn(bool t)
    {
        TurnFireOn = t;
    }
}
