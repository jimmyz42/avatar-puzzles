using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPipeScript : MonoBehaviour {

    public Vector3 oriPos;
    public Quaternion oriRot;
    public GameObject shatter_template;

    private GameObject waterInpipe;
    private GameObject pipe;
    private GameObject waterfall;
    private Vector3 shatter_offset;
    public DissolveEventTrigger dissolver;
    private Quaternion pipe_rot;

	private Init_Pipes manager;
	int num;
	private float z_limit = float.MaxValue;
	private Vector3 velocity = Vector3.zero; // kinematic rigidbodies don't support velocity :(

    private void Awake()
    {
        //Get all the parts of the pipe
        pipe = transform.GetChild(0).gameObject;
        dissolver = pipe.GetComponentInChildren<DissolveEventTrigger>();
        waterfall = transform.GetChild(1).gameObject;
        waterInpipe = transform.GetChild(2).gameObject;
        shatter_offset = new Vector3(6, 5, 0);
    }

    private void Start()
    {
        //Get the starting posistion so it knows what to return too
        oriPos = transform.position;
        oriRot = transform.rotation;
    }

    private void Update()
    {
		transform.position += velocity * Time.deltaTime;
		if (transform.position.z >= z_limit) { // used for moving back
			z_limit = float.MaxValue;
			velocity = Vector3.zero;
			transform.position = oriPos;
			transform.rotation = oriRot;
		}
		//get's current rotation
        pipe_rot = transform.rotation;

        // Quick way to activate shatter effect
        if (Input.GetKeyDown(KeyCode.A))
            Shatter();
    }

	void OnMouseDown() {
		selectPipe ();
	}

    //Triggered if hitting a rock pillar
    private void OnTriggerEnter(Collider other)
    {
		if (other.tag == "RockPillar") {
			Shatter ();
			manager.dissociatePipe (num);
		}
		if (other.tag == "RockPillar" || other.tag == "GoalHole") {
			velocity = Vector3.zero;
			manager.finalizePipe ();
		}
    }

    //Shatter: Causes the pipe to dissolve and return to its Start position
    public void Shatter()
    {
		
        waterInpipe.SetActive(false);
        waterfall.SetActive(false);
        dissolver.max = 10;
		dissolver.speed = -2.5f * 1.12f;
        Instantiate(shatter_template, transform.position-shatter_offset, pipe_rot);
        StartCoroutine(BacktoStart());
    }

    //reactivates the water in the pipe and the spilling water from the pipe
    public void Restore()
    {
        waterInpipe.SetActive(true);
        waterfall.SetActive(true);
    }

    //Returns the pipe to original position and restores the pipe to orginal look
    IEnumerator BacktoStart()
    {
        yield return new WaitForSeconds(2.0f);
        transform.position = oriPos;
        transform.rotation = oriRot;
        dissolver.Restore();
        yield return new WaitForSeconds(.1f);
        Restore();
    }

	public void setManager(Init_Pipes manager) {
		this.manager = manager;
	}

	public void setNum(int num) {
		this.num = num;
	}

	// Abilities: call this method
	public void selectPipe() {
		// TODO Brianna animation?
		manager.setSelectedPipe(num);
	}

	// Abilities: call this method
	public void unselectPipe() {
		// TODO Brianna animation?
		manager.setSelectedPipe (-1);
	}

	public void setZLimit(float zLimit) {
		this.z_limit = zLimit;
	}

	public void setVelocity(Vector3 velocity) {
		this.velocity = velocity;
	}
}
