using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch_Floor : MonoBehaviour {

    public float timer;
    public float sceneStartTimer;

    public GameObject fallenForm;
	// Use this for initialization
	void Start ()
    {

        StartCoroutine(FallingFloorTimer());
	}
	
	// Update is called once per frame
	void Update ()
    { 	}

    IEnumerator FallingFloorTimer()
    {
        yield return new WaitForSeconds(sceneStartTimer);
        //yield return new WaitForSeconds(Random.value*timer);
        Instantiate(fallenForm, new Vector3 ((transform.position.x)-150, (transform.position.y)+10, transform.position.z), transform.rotation);
        Destroy(this.gameObject);
    }
}
