using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSmokeScript : MonoBehaviour {

    public ParticleSystem smoke;
    private ParticleSystem.MainModule s;
    public GameObject player;
    public float speed;
    public bool Moving;
    // Use this for initialization
    void Start()
    {
        smoke = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Moving)
        {
            Surround();
            s.startSize = 80f;
        }
    }

    void Surround()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        
    }
}
