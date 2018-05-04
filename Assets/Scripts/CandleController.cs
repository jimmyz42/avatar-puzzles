using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleController : MonoBehaviour {

    // Use this for initialization
    private GameObject fire;
    public bool TurnFireOn;

	private static GameObject selectedCandle = null;
	private Init_Candles manager;
	private int row, col;

    void Start ()
    {
        fire = gameObject.transform.GetChild(0).gameObject;
	}


    public void toggle()
    {
        OnMouseDown();
    }
	void OnMouseDown() {
		// Select and Unselect are not needed for clicks
		// They only exist because the OVR controller uses them
		selectCandle ();
		toggleCandle ();
		unselectCandle ();
	} 
		
	public void selectCandle() {
		selectedCandle = this.gameObject;
	}

	public void unselectCandle() {
		selectedCandle = null;
	}

	public void toggleCandle() {
		manager.toggleCandle (row, col);
	}

	public void setManager(Init_Candles manager) {
		this.manager = manager;
	}

	public void setCoords(int row, int col) {
		this.row = row;
		this.col = col;
	}
	
	// Update is called once per frame
	void Update ()
    {
		fire.SetActive (TurnFireOn);
	}

    public void PutFireOn(bool t)
    {
		//fire.SetActive (t);
		TurnFireOn = t;
    }

	public bool isFireOn() {
		//return fire.activeSelf;
		return TurnFireOn;
	}

	public void toggleFireOn() {
		//fire.SetActive (!fire.activeSelf);
		TurnFireOn = !TurnFireOn;
	}
}
