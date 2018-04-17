using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafController : MonoBehaviour {

    public Transform target;
    public float speed;
    public float degreesPerSecond = 15.0f;
    public float amplitude = 0.5f;
    public float frequency = 1f;
    Vector3 posOffset = new Vector3();
    Vector3 tempPos = new Vector3();
    
    // Use this for initialization
    void Start ()
    {
        //posOffset = transform.position;
        
	}
	
	// Update is called once per frame
	void Update ()
    {
        Twirl();
        Bounce();
	}

    public void Twirl()
    {
        transform.RotateAround(target.position, Vector3.up, speed * Time.deltaTime);
        transform.Rotate(new Vector3(0, 15, 0) * Time.deltaTime);
    }
    public void Bounce()
    {
        // Float up/down with a Sin()
        tempPos = transform.position;
        tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * (Random.value*amplitude);
        transform.position = tempPos;
    }
}
