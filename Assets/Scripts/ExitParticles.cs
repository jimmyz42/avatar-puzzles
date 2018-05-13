using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ExitParticles : MonoBehaviour {

    public ParticleSystem[] leaves;
    public GameObject exit;
    public GameObject floorLeaves;
    public float finalExitDelay;

    private UnityAction start;
    private UnityAction smoke;
	
    // Use this for initialization
	void Start ()
    {
        leaves = GetComponentsInChildren<ParticleSystem>();
        exit.SetActive(false);
	}

    void Awake()
    {

        start = new UnityAction(StartParticles);
        smoke = new UnityAction(StartSmoke);

    }

    void OnEnable()
    {
        EventManager.StartListening("StartLeaves", start);
        EventManager.StartListening("StartSmoke", smoke);

    }

    void OnDisable()
    {
        EventManager.StopListening("StartLeaves", start);
        EventManager.StopListening("StartSmoke", smoke);
    }


    public void StartParticles()
    {
        floorLeaves.SetActive(false);
        foreach (ParticleSystem l in leaves)
        {
            l.Play();
        }
    }

    public void StartSmoke()
    {
       
        StartCoroutine(Exiting());
    }

    IEnumerator Exiting()
    {
        exit.SetActive(true);
        yield return new WaitForSeconds(finalExitDelay);
        GameObject gmc = GameObject.Find("GameManager");
        if (gmc != null)
        {
            ScreenFader fadeTime = gmc.GetComponent<ScreenFader>();
            fadeTime.TurnFadeOff(false);
            yield return new WaitForSeconds(fadeTime.fadeTime);
        }    
        SceneManager.LoadScene("AstralRoom");
    }
}
