using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorController : MonoBehaviour {

    public Transform target;
    public ParticleSystem exit;
    public float speed;
    public float doorDelay;
    public float exitDelay;
    public bool ending;
	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (ending)
        {
            StartCoroutine(EndingtheGame());
        }
	}

    public void MovingDoor()
    {
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, target.position, speed * Time.deltaTime);

    }

    public void SetEnding(bool isEnd)
    {
        ending = isEnd;
    }

    IEnumerator EndingtheGame()
    {
        yield return new WaitForSeconds(doorDelay);
        MovingDoor();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag=="Player")
        {
            StartCoroutine(ExitingRoom());
        }
    }

    IEnumerator ExitingRoom()
    {
        exit.Play();
        yield return new WaitForSeconds(exitDelay);
        SceneManager.LoadScene("AstralRoom");
    }

}
