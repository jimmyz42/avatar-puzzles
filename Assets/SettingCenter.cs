using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingCenter : MonoBehaviour {

    public GameObject target;
    public LeafController[] leaves;
    // Use this for initialization
	void Start ()
    {
        leaves = gameObject.GetComponentsInChildren<LeafController>();
        foreach (Transform leaf in transform )
        {
            var s=leaf.gameObject.GetComponent<LeafController>();
            s.SetTarget(target.transform);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
