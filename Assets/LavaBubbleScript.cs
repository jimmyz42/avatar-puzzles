using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaBubbleScript : MonoBehaviour {

    public ParticleSystem bubbles;
    public bool canMove;
	// Use this for initialization
	void Start () {
        bubbles = GetComponent<ParticleSystem>();
        while ((-30f < transform.position.x && transform.position.x < 30f) || (30f > transform.position.z && transform.position.z > -30f))
            transform.position = new Vector3(Random.Range(-100, 100), 3f, Random.Range(-100, 100));
        StartCoroutine(Bubbling());
        //bubbles.Stop();
    }
	
	// Update is called once per frame
	void Update ()
    {
        
       /*if(bubbles.isStopped && canMove)
        {
            MovingPos();
            canMove = false;
        }
        //bubbles.Play();
       // Debug.Log(bubbles.isEmitting);*/
        
    }

    void MovingPos()
    {
        //bubbles.Stop();
        transform.position = new Vector3(Random.Range(-100, 100), transform.position.y, Random.Range(-100, 100));
        while ((-30f < transform.position.x && transform.position.x < 30f) || (30f > transform.position.z && transform.position.z > -30f))
        {
            MovingPos();
        }
    }

    IEnumerator Bubbling()
    {
        while (enabled)
        {
            yield return new WaitForSeconds(Random.Range(5f, 20f));
            bubbles.Play();
            //canMove = false;
            yield return new WaitForSeconds(Random.Range(2f, 4f));
            bubbles.Stop();
            MovingPos();
            yield return new WaitForSeconds(Random.Range(1f,4f));
            //canMove = true;

        }


    }


}
