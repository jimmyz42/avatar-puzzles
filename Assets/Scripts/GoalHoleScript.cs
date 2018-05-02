using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GoalHoleScript : MonoBehaviour {

    // Use this for initialization
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag=="WaterPipe")
        {
            EventManager.TriggerEvent("TurnOnALakeWaterfall");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "WaterPipe")
        {
            EventManager.TriggerEvent("TurnOffALakeWaterfall");
        }
    }
}
