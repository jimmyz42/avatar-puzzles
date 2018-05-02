using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WaterRoomController : MonoBehaviour {

    public float gameStartDelay;

    
	// Use this for initialization
	void Start ()
    {
        StartCoroutine(StartLevel());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator StartLevel()
    {
        yield return new WaitForSeconds(gameStartDelay);
        EventManager.TriggerEvent("StartLevel");
    }
}
