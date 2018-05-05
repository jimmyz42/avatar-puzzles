using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingToPlayer : MonoBehaviour {

    public Transform player;
    public float speed;
	// Use this for initialization
	void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (transform.position != player.position)
        transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

	}
}
