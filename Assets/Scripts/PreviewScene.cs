using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine;

public class PreviewScene : MonoBehaviour {

    public bool PreviewShown;
    public float previewTime;
    public bool canPlay;
    public int sceneNum;
	// Use this for initialization
	void Start ()
    {
        StartCoroutine(Starte());
        PreviewShown = false;
        switch (this.name)
        {
            case "EarthWorld":
                sceneNum = 1;
                break;
            case "AirWorld":
                sceneNum = 2;
                break;
            case "FireWorld":
                sceneNum = 3;
                break;
            case "WaterWorld":
                sceneNum = 4;
                break;
            default:
                sceneNum = 0;
                break;
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 1;
            EventManager.TriggerEvent("ClosePreview");
        }
	}

    public void OnMouseEnter()
    {
       
        if (!PreviewShown && sceneNum > 0 && canPlay)
        {
            Time.timeScale = 0;
            SceneManager.LoadScene(sceneNum, LoadSceneMode.Additive);
            PreviewShown = true;
        }
        
        //Application.LoadLevelAdditive(1);
    }
    public void OnMouseExit()
    {
        Time.timeScale = 1;
        StartCoroutine(DestroyPreview());
    }

    IEnumerator Starte()
    {
        yield return new WaitForSeconds(149f);
        canPlay = true;
    }
    IEnumerator DestroyPreview()
    {
        yield return new WaitForSeconds(previewTime);
        EventManager.TriggerEvent("ClosePreview");
    }
}
