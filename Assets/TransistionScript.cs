using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransistionScript : MonoBehaviour {

    public float videoEndtime = 115.0f;
    public FadeIn fade;
	// Use this for initialization
	void Start ()
    {
        fade = gameObject.GetComponent<FadeIn>();
        StartCoroutine(IntroChange());
	}
	
    IEnumerator IntroChange()
    {
        
        yield return new WaitForSeconds(videoEndtime);
        float fadetime = fade.BeginFade(1);
        yield return new WaitForSeconds(fadetime);
        SceneManager.LoadScene("AstralRoom", LoadSceneMode.Additive);
    }
}
