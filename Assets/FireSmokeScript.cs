using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class FireSmokeScript : MonoBehaviour {

    public GameObject FlameSmoke;
    public Transform player;
    public float speed;
    public float delay;
    private UnityAction smokes;


    void Awake()
    {
        smokes = new UnityAction(Smoking);
        FlameSmoke = GameObject.Find("FireExitSmoke");
        FlameSmoke.SetActive(false);
    }

    void OnEnable()
    {
        EventManager.StartListening("StartTheSmoke", smokes);
    }

    void OnDisable()
    {
        EventManager.StopListening("StartTheSmoke", smokes);
    }

    void Smoking()
    {
        Debug.Log("Smoking was triggered");
        FlameSmoke.SetActive(true);
        FlameSmoke.GetComponent<ParticleSystem>().Play();
        StartCoroutine(Ending());
        

    }

    void Surround()
    {
        FlameSmoke.transform.position = Vector3.MoveTowards(FlameSmoke.transform.position, player.position, speed * Time.deltaTime);
    }

    void ChangingSize()
    {
        ParticleSystem.MainModule f = FlameSmoke.GetComponent<ParticleSystem>().main;
        f.startSize = 40f;
    }

    void Return()
    {
        SceneManager.LoadScene("AstralRoom");

    }

    IEnumerator Ending()
    {
        yield return new WaitForSeconds(delay);
        Surround();
        ChangingSize();
        yield return new WaitForSeconds(delay+2f);
        Return();


    }
   


}
