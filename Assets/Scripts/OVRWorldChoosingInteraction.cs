using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OVRWorldChoosingInteraction : MonoBehaviour {

    public AudioSource audioSource;
    public AudioClip selectedWorld;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnSelect( Transform t)
    {
        if (t.gameObject.tag == "EarthWorld")
        {
            Debug.Log("Selected the earth world");
            RoomWorldController earthWorldController = t.gameObject.GetComponent<RoomWorldController>();
            earthWorldController.isSelected = true;
            //earthWorldController.transitioning = true;
            //StartCoroutine(earthWorldController.EnteringTheWorld());
            playSelectedWorldAudio();
        }
    }

    void playSelectedWorldAudio() {
        audioSource.clip = selectedWorld;
        audioSource.Play();
    }
}
