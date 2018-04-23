using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class AirRoomEventSystem : MonoBehaviour {

    public GameObject Walls;
    public float wallSpeed;
    public float wallDelay;


    private UnityAction endWalls;


    private void Start()
    {
        
    }
    void Awake()
    {

        endWalls= new UnityAction(RemoveWalls);


    }

    void OnEnable()
    {
        EventManager.StartListening("RemoveWalls", endWalls);

    }

    void OnDisable()
    {
        EventManager.StopListening("RemoveWalls", endWalls);
    }

    void RemoveWalls()
    {

        WallController w = Walls.GetComponent<WallController>();
        w.SetParams(true, wallSpeed, wallDelay);
        w.SetEndPos(-100);
    }
}
