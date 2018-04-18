using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour {

    public LineRenderer line;
    public Transform startPos;
    public bool laserOn;
	void Start ()
    {
        line = gameObject.GetComponent<LineRenderer>();
        line.enabled = false;
	}
	
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
                /* if (hit.collider.name=="Mirror")
                 {

                 }*/
                //Debug.Log(hit.transform.name);
                //Debug.DrawLine(ray.origin, hit.point,Color.cyan);
                line.SetPosition(1, hit.point);
            }
            else
                line.SetPosition(1, ray.GetPoint(10000));
            

            yield return null;
        }

    }

    public void TurnLaserOn(bool yes)
    {
        laserOn = yes;
    }
}
