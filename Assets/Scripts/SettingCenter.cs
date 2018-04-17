using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingCenter : MonoBehaviour {

    public GameObject target;
    public LeafController[] leaves;
    public float falling;
    // Use this for initialization
	void Start ()
    {
        leaves = gameObject.GetComponentsInChildren<LeafController>();
        foreach (LeafController leaf in leaves )
        {
            //var s=leaf.gameObject.GetComponent<LeafController>();
            leaf.SetTarget(target.transform);
            //leaf.SetDelay(falling);
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        StartCoroutine(Fall());
	}

    IEnumerator Fall()
    {
        yield return new WaitForSeconds(falling);
        foreach (LeafController leaf in leaves)
        {
            //var s=leaf.gameObject.GetComponent<LeafController>();
            leaf.doingTwirls = false;
            //leaf.SetDelay(falling);
        }
    }


}
