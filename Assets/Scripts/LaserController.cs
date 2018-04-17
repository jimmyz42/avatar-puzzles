using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour {

    public LineRenderer line;
    public Transform startPos;
    //public Vector3 ori;
    //public Vector3 moved;
    public bool laserOn;
	// Use this for initialization
	void Start ()
    {
        line = gameObject.GetComponent<LineRenderer>();
        //startPos = gameObject.GetComponentInParent<Transform>();
        line.enabled = false;
        //ori = transform.position;
        //transform.position = startPos.position;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (laserOn)
        {
            StartCoroutine(MakeLaser());
        }
        
	}

    IEnumerator MakeLaser()
    {
        while (laserOn)
        {
            line.enabled = true;
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;
            //moved = ray.origin;
            line.SetPosition(0, ray.origin);
            line.useWorldSpace = true;

           if (Physics.Raycast(ray, out hit, 1000))
            {
               /* if (hit.collider != null)
                {
                Debug.Log("hit something");
                Debug.Log(hit.transform.name);
                Vector3 newPoint = hit.point - transform.forward;
                line.SetPosition(1, newPoint);

                }*/
                //Debug.Log(hit.transform.name);
                //Debug.DrawLine(ray.origin, hit.point,Color.cyan);
            }
            //else
            //{*/
            line.SetPosition(1, ray.GetPoint(10000));
            //}
            

            yield return null;
        }

    }

    public void TurnLaserOn(bool yes)
    {
        laserOn = yes;
    }
}
