using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubblingScript : MonoBehaviour {

    public float speed;
    public float oriPos;
    public float endPos;
    public GameObject target;
	// Use this for initialization
	void Start ()
    {
        transform.position = new Vector3(Random.Range(-100, 100), -5.4f, Random.Range(-100, 100));
        oriPos = transform.position.y;
        speed = Random.Range(2, 6);
        
	}
	
	// Update is called once per frame
	void Update ()
    {
        StartCoroutine(Bubbling());
        
    }

    void Bobbing()
    {
        Vector3 pos = new Vector3(transform.position.x, endPos, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, pos, speed * Time.deltaTime);
    }

    void Circling()
    {
        transform.RotateAround(target.transform.position, Vector3.up, 10 * Time.deltaTime);
    }

    void MovingPos()
    {
        transform.position = new Vector3(Random.Range(-100, 100), transform.position.y, Random.Range(-100, 100));
        while ((-25f<transform.position.x && transform.position.x<25f) || (90f > transform.position.z && transform.position.z > 25f))
        {
            MovingPos();
        }
    }

    IEnumerator Bubbling()
    {
        //yield return new WaitForSeconds(Random.Range(10, 30));
        
        yield return new WaitForSeconds(Random.Range(5, 40));
        Bobbing();
        if (transform.position.y == endPos)
        {
            float t = endPos;
            endPos = oriPos;
            oriPos = t;
            if (endPos==-5.4f && Random.value<.5f)
            {
                yield return new WaitForSeconds(Random.Range(5, 50));
                MovingPos();
            }
            
        }
        
        
    }
}
