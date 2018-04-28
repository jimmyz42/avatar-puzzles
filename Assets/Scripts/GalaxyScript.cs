using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalaxyScript : MonoBehaviour {

    // Use this for initialization
    public float delay;
    public GameObject[] ex;

	void Start ()
    {
        delay = 1f;
        StartCoroutine(DeleteExplosion());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator DeleteExplosion()
    {
        yield return new WaitForSeconds(delay);
        GameObject[] ex = GameObject.FindGameObjectsWithTag("Explosion");
        //Debug.Log(ex);
        foreach (GameObject x in ex)
        {
            Destroy(x);
        }
    }
}
