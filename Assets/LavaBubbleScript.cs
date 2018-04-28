using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaBubbleScript : MonoBehaviour {

    public ParticleSystem bubbles;
    private ParticleSystem.MainModule ma;

	// Use this for initialization
	void Start () {
        bubbles = GetComponent<ParticleSystem>();
        transform.position = new Vector3(Random.Range(-100, 100), 3f, Random.Range(-100, 100));
        ma = bubbles.main;
        ma.startDelay = Random.Range(5, 20);
        //bubbles.Stop();
    }
	
	// Update is called once per frame
	void Update ()
    {
        
        if(bubbles.isStopped)
        {
            MovingPos();
        }
        //bubbles.Play();
        Debug.Log(bubbles.isEmitting);
        //StartCoroutine(Bubbling());
    }

    void MovingPos()
    {
        //bubbles.Stop();
        transform.position = new Vector3(Random.Range(-100, 100), transform.position.y, Random.Range(-100, 100));
        while ((-25f < transform.position.x && transform.position.x < 25f) || (90f > transform.position.z && transform.position.z > 25f))
        {
            MovingPos();
        }
    }

    IEnumerator Bubbling()
    {
        if (true)
        {
            yield return new WaitForSeconds(Random.Range(5f, 20f));
            bubbles.Play();
            yield return new WaitForSeconds(Random.Range(1f, 3f));
        }

        
        MovingPos();

    }


}
