using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyYourself : MonoBehaviour {

    public float delay;
	// Use this for initialization
	void Start () {
        StartCoroutine(Suicide());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator Suicide()
    {
        yield return new WaitForSeconds(delay * 0.95f);
        Destroy(gameObject);
    }
}
