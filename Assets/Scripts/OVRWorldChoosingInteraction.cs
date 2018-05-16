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

    public void OnHoverEnter(Transform t)
    {
        if (t.gameObject.tag == "EarthWorld" || t.gameObject.tag == "FireWorld" || t.gameObject.tag == "AirWorld" || t.gameObject.tag == "WaterWorld")
        {
            PreviewScene previewScene = t.gameObject.GetComponent<PreviewScene>();
            previewScene.OnMouseEnter();
        }
    }

    public void OnHoverExit(Transform t)
    {
        if (t.gameObject.tag == "EarthWorld" || t.gameObject.tag == "FireWorld" || t.gameObject.tag == "AirWorld" || t.gameObject.tag == "WaterWorld")
        {
            PreviewScene previewScene = t.gameObject.GetComponent<PreviewScene>();
            previewScene.OnMouseExit();
        }
    }

    public void OnSelect( Transform t)
    {
        if (t.gameObject.tag == "EarthWorld"  || t.gameObject.tag == "FireWorld" || t.gameObject.tag == "AirWorld" || t.gameObject.tag == "WaterWorld")
        {
           
            RoomWorldController worldController = t.gameObject.GetComponent<RoomWorldController>();
            worldController.isSelected = true;
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
