using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoScript : MonoBehaviour {

    public float videoEndtime = 115.0f;
    public float endVideo = 64.0f;
    public VideoPlayer v; 
    public GameObject gmc;
    public FadeIn fade;
    public AudioSource roomAudio;

    private GameObject videoBubble;
    // Use this for initialization
    private void Awake()
    {
        fade = gmc.GetComponent<FadeIn>();
        v = gameObject.GetComponent<VideoPlayer>();
        videoBubble = transform.GetChild(0).gameObject;
    }
    void Start()
    {
        
        StartCoroutine(IntroChange());
    }

    IEnumerator IntroChange()
    {

        yield return new WaitForSeconds(videoEndtime);
        float fadetime = fade.BeginFade(1);
        yield return new WaitForSeconds(fadetime);
        yield return new WaitForSeconds(endVideo);
        Destroy(videoBubble);
        fade.FadeOut();
        roomAudio.Play();
        //SceneManager.LoadScene("AstralRoom", LoadSceneMode.Additive);
    }
}
