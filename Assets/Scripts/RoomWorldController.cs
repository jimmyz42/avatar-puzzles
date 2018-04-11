using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomWorldController : MonoBehaviour
{

    public Transform target;
    public float speed;
    public float timer;
    public bool isSelected, transitioning;
    public string connectedWorld;
    public ParticleSystem insideSmoke;

 
 
    // Use this for initialization
    void Start ()
    {
        isSelected = false;
        transitioning = false;
        insideSmoke = GetComponentInChildren<ParticleSystem>();

	}
	
	// Update is called once per frame
	void Update ()
    {
        if (isSelected)
        {
            SelectedWorld();
        }
        BasicMovement();
        if (transitioning)
        {
            TranstionSmoke();
        }
	}

    void BasicMovement()
    {
        transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);
        //transform.RotateAround(target.transform.position, Vector3.up, 10 * Time.deltaTime);
    }

    void SelectedWorld()
    {
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, target.position, speed * Time.deltaTime);

    }

    void TranstionSmoke()
    {
        insideSmoke.startSize = 1000;
    }

    void OnMouseDown()
    {
        isSelected = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag=="Player")
        {
            transitioning = true;
            StartCoroutine(EnteringTheWorld());
            
        }
    }

    IEnumerator EnteringTheWorld()
    {
        yield return new WaitForSeconds(timer);
        SceneManager.LoadScene(connectedWorld);

    }
}
