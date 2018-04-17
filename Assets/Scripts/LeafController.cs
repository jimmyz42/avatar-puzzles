using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafController : MonoBehaviour {

    public Transform target;
    public float speed;
    public float fallDelay;
    public float degreesPerSecond = 15.0f;
    public float amplitude = 0.5f;
    public float frequency = 1f;

    public Rigidbody rb;
    public Collider col;

    public bool doingTwirls;
    Vector3 posOffset = new Vector3();
    Vector3 tempPos = new Vector3();
    
    // Use this for initialization
    void Start ()
    {
        //posOffset = transform.position;
        doingTwirls = true;
        frequency = Random.Range(.4f, .7f);
        SetRotation();
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        //StartCoroutine(FallLeaf());
        
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (doingTwirls)
        {
            Twirl();
            Bounce();
        }
        else
        {

            rb.drag = 0;
            col.isTrigger = false;
        }

        
	}

    public void Twirl()
    {
        transform.RotateAround(target.position, Vector3.up, speed * Time.deltaTime);
        transform.Rotate(new Vector3(0, 15, 0) * Time.deltaTime);
    }
    public void Bounce()
    {
        // Float up/down with a Sin()
        tempPos = transform.position;
        tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * (Random.value*amplitude);
        transform.position = tempPos;
    }

    void SetRotation()
    {
        var euler = transform.eulerAngles;
        euler.y = Random.Range(0.0f, 360.0f);
        transform.eulerAngles = euler;
    }

    public void SetTarget(Transform targetFromParent)
    {
        target = targetFromParent;
    }

    public void SetDelay(float d)
    {
        fallDelay = d;
    }

    IEnumerator FallLeaf()
    {
        yield return new WaitForSeconds(fallDelay);
        doingTwirls = false;
    }

    public void TwirlsOff(bool t)
    {
        doingTwirls = !t;
        Debug.Log("leaf");
    }

}
