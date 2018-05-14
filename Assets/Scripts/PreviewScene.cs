using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine;

public class PreviewScene : MonoBehaviour {

    public bool PreviewShown;
    public float previewTime;
    public int sceneNum;
	// Use this for initialization
	void Start ()
    {
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

    private void OnMouseEnter()
    {
        
        if (!PreviewShown && sceneNum > 0)
        {
            Time.timeScale = 0;
            SceneManager.LoadScene(sceneNum, LoadSceneMode.Additive);
            PreviewShown = true;
        }
        
        //Application.LoadLevelAdditive(1);
    }
    private void OnMouseExit()
    {
        Time.timeScale = 1;
        StartCoroutine(DestroyPreview());
    }

    IEnumerator DestroyPreview()
    {
        yield return new WaitForSeconds(previewTime);
        EventManager.TriggerEvent("ClosePreview");
    }
}
