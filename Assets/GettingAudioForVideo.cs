using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class GettingAudioForVideo : MonoBehaviour {

    public VideoPlayer vd;
    public AudioSource aud;
    // Use this for initialization
    private void Awake()
    {
        vd = gameObject.GetComponent<VideoPlayer>();
        aud = GameObject.Find("OSTAudio").GetComponent<AudioSource>();

        Debug.Log(vd.audioOutputMode);
        Debug.Log(vd.GetTargetAudioSource(0));
        //vd.SetTargetAudioSource()
        vd.SetTargetAudioSource(0, aud);
        Debug.Log(vd.GetTargetAudioSource(0));
        
    }


}
