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
        //get's current rotation
        pipe_rot = transform.rotation;

        // Quick way to activate shatter effect
        if (Input.GetKeyDown(KeyCode.A))
            Shatter();
    }

    //Triggered if hitting a rock pillar
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("collided 2");
        if (other.tag == "RockPillar")
        {
            Shatter();
        }

    }

    //Shatter: Causes the pipe to dissolve and return to its Start position
    public void Shatter()
    {
        waterInpipe.SetActive(false);
        waterfall.SetActive(false);
        dissolver.max = 10;
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
        yield return new WaitForSeconds(4.5f);
        transform.position = oriPos;
        transform.rotation = oriRot;
        dissolver.Restore();
        yield return new WaitForSeconds(.5f);
        Restore();

    }
}
