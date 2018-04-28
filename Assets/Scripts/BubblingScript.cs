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
        oriPos = transform.position.y;
	}
	
	// Update is called once per frame
	void Update ()
    {
        StartCoroutine(Bubbling());
        transform.RotateAround(target.transform.position, Vector3.up, 10 * Time.deltaTime);
    }

    void Bobbing()
    {
        Vector3 pos = new Vector3(transform.position.x, endPos, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, pos, speed * Time.deltaTime);
    }

    IEnumerator Bubbling()
    {
        yield return new WaitForSeconds(Random.Range(1, 10));
        Bobbing();
        if (transform.position.y == endPos)
        {
            float t = endPos;
            endPos = oriPos;
            oriPos = t;
        }
        
    }
}
