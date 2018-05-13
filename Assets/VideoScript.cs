using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoScript : MonoBehaviour {

    public float videoEndtime = 115.0f;
    public float endVideo = 64.0f;
    public VideoPlayer v; 
    public GameObject gmc;
    public ScreenFader fade;
    public AudioSource roomAudio;
    public bool CanPlay;

    private GameObject videoBubble;
    // Use this for initialization
    private void Awake()
    {
        fade = gmc.GetComponent<ScreenFader>();
        v = gameObject.GetComponent<VideoPlayer>();
        videoBubble = transform.GetChild(0).gameObject;
    }
    void Start()
    {
        
        StartCoroutine(IntroChange());
    }

    public void DontPlay()
    {
        CanPlay = false;
    }
    IEnumerator IntroChange()
    {

        yield return new WaitForSeconds(videoEndtime);
        float fadetime = fade.fadeTime;
        fade.TurnFadeOff(false);
        yield return new WaitForSeconds(fadetime);
        yield return new WaitForSeconds(endVideo);
        Destroy(videoBubble);
        fade.TurnFadeOff(true);
        roomAudio.Play();
        //SceneManager.LoadScene("AstralRoom", LoadSceneMode.Additive);
    }
}
