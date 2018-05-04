using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GoalHoleScript : MonoBehaviour {

	private Init_Pipes manager;
	int num;

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

	void OnMouseDown() {
		selectHole ();
	}

	public void setManager(Init_Pipes manager) {
		this.manager = manager;
	}

	public void setNum(int num) {
		this.num = num;
	}

	// Abilities: call this method
	public void selectHole() {
		// TODO Brianna animation?
		manager.setSelectedHole(num);
	}

	// Abilities: call this method
	public void unselectHole() {
		// TODO Brianna animation?
		manager.setSelectedHole (-1);
	}
}
