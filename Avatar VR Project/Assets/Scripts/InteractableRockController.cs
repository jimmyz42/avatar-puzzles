using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableRockController : MonoBehaviour {

    public float timer;

    private float x_cord;
    private float z_cord;
	// Use this for initialization
	void Start ()
    {
        x_cord = transform.position.x;
        z_cord = transform.position.z;
        StartCoroutine(AddingMovableRocks());	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Appear()
    {
        gameObject.SetActive(true);
    }
    IEnumerator AddingMovableRocks()
    {
        yield return new WaitForSeconds(timer);
        this.transform.position = new Vector3(x_cord, 22, z_cord);


    }
}
