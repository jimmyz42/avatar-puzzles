using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LakeScript : MonoBehaviour {

    public float speed;
    public float delay;
    public bool canMoveDown;
    public bool canMove;
    public GameObject Holes;

    private Vector3 initalPos;
    private Vector3 bottomPos;
    private Vector3 target;
    public float segment_distance;
    private int num_of_goals;

    private UnityAction drop;
    private UnityAction raise;
    private UnityAction fall;
    private UnityAction seg;

	void Awake ()
    {
        initalPos = transform.position;
        bottomPos = new Vector3(initalPos.x, .5f, initalPos.z);
        transform.position = initalPos;
        drop = new UnityAction(IsMovingDown);
        raise = new UnityAction(RaiseWaterLevel);
        fall = new UnityAction(LowerWaterLevel);
        seg = new UnityAction(CalculateDistance);
	}

    void OnEnable()
    {
        EventManager.StartListening("StartLevel", drop);
        EventManager.StartListening("TurnOnALakeWaterfall", raise);
        EventManager.StartListening("TurnOffALakeWaterfall", fall);
        EventManager.StartListening("CalculateSegment", seg);
    }

    void OnDisable()
    {
        EventManager.StopListening("StartLevel", drop);
        EventManager.StopListening("TurnOnALakeWaterfall", raise);
        EventManager.StopListening("TurnOffALakeWaterfall", fall);
        EventManager.StopListening("CalculateSegment", seg);

    }
    private void Start()
    {
        
    }
    void CalculateDistance()
    {
        float d= Vector3.Distance(initalPos, bottomPos);
        num_of_goals =Holes.GetComponentsInChildren<Transform>().Length;
        segment_distance = d / num_of_goals;
    }
    void Update()
    {
        if (canMoveDown)
        {
            MoveDown();
        }
        else if (canMove && (transform.position.y <=initalPos.y && transform.position.y>=bottomPos.y))
        {
            transform.position = Vector3.MoveTowards(transform.position, target, 5 * Time.deltaTime);

        }

    }

    void MoveDown()
    {
        if (transform.position==bottomPos)
        {
            canMoveDown = false;
        }
        transform.position = Vector3.MoveTowards(transform.position, bottomPos, speed * Time.deltaTime);

    }

    void IsMovingDown()
    {
        StartCoroutine(InitPoolTimer());
    }

    IEnumerator InitPoolTimer()
    {
        yield return new WaitForSeconds(delay);
        canMoveDown = true;
    }

    void RaiseWaterLevel()
    {
        canMove = true;
        target = new Vector3(transform.position.x, transform.position.y + segment_distance, transform.position.z);
    }

    void LowerWaterLevel()
    {
        canMove = true;
        target = new Vector3(transform.position.x, transform.position.y - segment_distance, transform.position.z);

    }
}
