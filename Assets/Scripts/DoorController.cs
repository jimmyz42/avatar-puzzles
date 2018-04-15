using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorController : MonoBehaviour {

    public Transform target;
    public float speed;
    public float delay;
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
        yield return new WaitForSeconds(delay);
        MovingDoor();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag=="Player")
        {
            SceneManager.LoadScene("AstralRoom");
        }
    }

}
