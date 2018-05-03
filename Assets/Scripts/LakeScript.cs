using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LakeScript : MonoBehaviour {

    public float speed;
    public bool canMoveDown;
    public bool canMove;

    private Vector3 initalPos;
    private Vector3 bottomPos;
    private Vector3 target;
    private float segment_distance;
    private int num_of_goals;

    private UnityAction drop;
    private UnityAction raise;
    private UnityAction fall;

	void Awake ()
    {
        initalPos = new Vector3(-476, 89.4f, -1);
        bottomPos = new Vector3(initalPos.x, .5f, initalPos.z);
        transform.position = initalPos;
        drop = new UnityAction(IsMovingDown);
        raise = new UnityAction(RaiseWaterLevel);
        fall = new UnityAction(LowerWaterLevel);
	}

    void OnEnable()
    {
        EventManager.StartListening("StartLevel", drop);
        EventManager.StartListening("TurnOnALakeWaterfall", raise);
        EventManager.StartListening("TurnOffALakeWaterfall", fall);
    }

    void OnDisable()
    {
        EventManager.StopListening("StartLevel", drop);
        EventManager.StopListening("TurnOnALakeWaterfall", raise);
        EventManager.StopListening("TurnOffALakeWaterfall", fall);
    }
    private void Start()
    {
        num_of_goals = GameObject.FindGameObjectsWithTag("GoalHole").Length;
        CalculateDistance();
    }
    void CalculateDistance()
    {
        float d= Vector3.Distance(initalPos, bottomPos);
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
