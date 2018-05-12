using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class WaterRoomExitScript : MonoBehaviour {

    private UnityAction end;

    private GameObject steam;
    private GameObject spit;
    public GameObject smoke;

    public float smokeDelay;
    public float exitDelay;

    private void Awake()
    {
        steam = transform.GetChild(0).gameObject;
        spit = transform.GetChild(1).gameObject;
        smoke = transform.GetChild(2).gameObject;
        end = new UnityAction(EndingLevel);
    }

    private void Start()
    {
        steam.SetActive(false);
        spit.SetActive(false);
        smoke.SetActive(false);
    }
    private void OnEnable()
    {
        EventManager.StartListening("EndLevel", end);
    }

    private void OnDisable()
    {
        EventManager.StartListening("EndLevel", end);
    }
    
    void EndingLevel()
    {
        StartCoroutine(Ending());


    }

    IEnumerator Ending()
    {
        steam.SetActive(true);
        spit.SetActive(true);
        yield return new WaitForSeconds(smokeDelay);
        smoke.SetActive(true);
        yield return new WaitForSeconds(exitDelay);
        GameObject gmc = GameObject.Find("GameManager");
        if (gmc != null)
        {
            float fadeTime = gmc.GetComponent<FadeIn>().BeginFade(1);
            yield return new WaitForSeconds(fadeTime);
        }
        SceneManager.LoadScene("AstralRoom");
    }
}
