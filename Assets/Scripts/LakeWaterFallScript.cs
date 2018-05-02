using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LakeWaterFallScript : MonoBehaviour {

    private GameObject waterfall;

    private void Awake()
    {
        waterfall = transform.GetChild(0).gameObject;
        
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TurnOffWaterfall()
    {
        waterfall.SetActive(false);
    }

    public void TurnOnWaterFall()
    {
        waterfall.SetActive(true);
    }
}
