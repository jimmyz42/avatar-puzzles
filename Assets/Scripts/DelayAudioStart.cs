using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayAudioStart : MonoBehaviour {

    public AudioSource audioSource;
    public float delaySeconds;

	// Use this for initialization
	void Start () {
        StartCoroutine(DelayStart());
    }

    public IEnumerator DelayStart()
    {
        yield return new WaitForSeconds(delaySeconds);
        StartAudio();
    }

    public void StartAudio()
    {
        audioSource.Play();
    }

	
	// Update is called once per frame
	void Update () {
		
	}
}
