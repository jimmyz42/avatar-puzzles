using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManagerController : MonoBehaviour {

    public GameObject[] worlds;

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
