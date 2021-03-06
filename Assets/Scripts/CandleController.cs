﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CandleController : MonoBehaviour {

    // Use this for initialization
    private GameObject fire;
    private ParticleSystem flame;
    private ParticleSystem.MainModule f;
    public bool TurnFireOn;
    public bool TurnFireRed;
	public float riseSpeed;

	private static GameObject selectedCandle = null;
	private Init_Candles manager;
	private int row, col;
    private UnityAction red;
    private UnityAction TurnOff;

    public AudioSource audioSource1;
    public AudioClip[] lightSfxs;
    public AudioClip[] extinguishSfxs;

    public void lightCandleSfx()
    {
        int index = Random.Range(0, lightSfxs.Length);
        audioSource1.clip = lightSfxs[index];
        audioSource1.Play();
    }

    public void extinguishCandleSfx()
    {
        int index = Random.Range(0, extinguishSfxs.Length);
        audioSource1.clip = extinguishSfxs[index];
        audioSource1.Play();
    }

    void Awake()
    {
        red = new UnityAction(BecomeRed);
        TurnOff = new UnityAction(Off);
    }

    void OnEnable()
    {
        EventManager.StartListening("TurnTheFlamesRed", red);
        EventManager.StartListening("TurnOffTheCandles", TurnOff);
    }

    void OnDisable()
    {
        EventManager.StopListening("TurnTheFlamesRed", red);
        EventManager.StopListening("TurnOffTheCandles", TurnOff);
    }
    void Start ()
    {
        fire = gameObject.transform.GetChild(0).gameObject;
        flame = fire.GetComponentInChildren<ParticleSystem>();
        f = flame.main;
        f.startColor = Color.white;
        TurnFireRed = false;

    }

    public void toggle()

    {


        OnMouseDown();

    }

    void OnMouseDown() {
        // Select and Unselect are not needed for clicks
        // They only exist because the OVR controller uses thems




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
	
    void Off()
    {
        TurnFireOn = false;
    }
	// Update is called once per frame
	void Update ()
    {
		if (gameObject.transform.position.y < .07f) {
			float newY = Mathf.Min (.07f, gameObject.transform.position.y + riseSpeed * Time.deltaTime);
			Vector3 target = gameObject.transform.position;
			target.y = newY;
			gameObject.transform.position = target;
		}
		fire.SetActive (TurnFireOn);
	}

    public void PutFireOn(bool t)
    {
		//fire.SetActive (t);
		TurnFireOn = t;
        if (t)
        {
            lightCandleSfx();
        }
        else {
            extinguishCandleSfx();
        }
    }

	public bool isFireOn() {
		//return fire.activeSelf;
		return TurnFireOn;
	}

	public void toggleFireOn() {
        //fire.SetActive (!fire.activeSelf);

        if (isFireOn())
        {
            lightCandleSfx();
        }
        else
        {
            extinguishCandleSfx();
        }


        TurnFireOn = !TurnFireOn;
	}

    void BecomeRed()
    {
        f.startColor = Color.red;
        Light red = fire.GetComponentInChildren<Light>();
        red.color = Color.red;
        f.startSize = .87f;
    }
}
