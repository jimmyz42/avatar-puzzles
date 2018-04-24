using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class AirRoomEventSystem : MonoBehaviour {

    //public GameObject Walls;
    public float wallSpeed;
    public float wallDelay;


    private UnityAction makeLeaves;
    


    private void Start()
    {
        makeLeaves = new UnityAction(Start);
    }
    void Awake()
    {

        makeLeaves = new UnityAction(RemoveWalls);
        


    }

    void OnEnable()
    {
        EventManager.StartListening("RemoveWalls", makeLeaves);

    }

    void OnDisable()
    {
        EventManager.StopListening("RemoveWalls", makeLeaves);
    }

    void RemoveWalls()
    {

        //Debug.Log("in Remove walls");
        //WallController w = Walls.GetComponent<WallController>();
        //w.SetParams(true, wallSpeed, wallDelay);
        //w.SetEndPos(-100);
    }
}
