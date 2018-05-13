using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class RoomWorldController : MonoBehaviour
{
    private UnityAction someListener;
    private UnityAction gal;

    public GameObject galaxy;
    public GameObject explosion;
    public GameManagerController gmc;
    public Transform target;
    public Vector3 retPos;
    public float speed;
    public float timer;
    public bool isSelected, transitioning;
    public string connectedWorld;
    public ParticleSystem insideSmoke;
    public float delay;

    private void Awake()
    { 
        galaxy.SetActive(false);
        isSelected = false;
        transitioning = false;
        insideSmoke = GetComponentInChildren<ParticleSystem>();
        gmc = GameObject.Find("GameManager").GetComponent<GameManagerController>();
        gal = new UnityAction(GetBadge);   
    }

    private void OnEnable()
    {
        EventManager.StartListening(name + "Galaxy", gal);
    }

    private void OnDisable()
    {
        EventManager.StopListening(name + "Galaxy", gal);
    }

    void GetBadge()
    {
        galaxy.SetActive(true);
        Destroy(gameObject);
    }
    public void SetReturn(Vector3 pos, float d)
    {
        retPos = pos;
        delay = d;
        

    }
    
    void Start ()
    {
        
        if (gmc.world == this.name && gmc.WasCompleted())
        {
         /*   explosion = GameObject.FindGameObjectWithTag("Explosion");
            galaxy = GameObject.FindGameObjectWithTag("EarthGalaxy");
            explosion.transform.position = this.transform.position;
            explosion.SetActive(false);
            galaxy.SetActive(false);*/
        }





    }
	
	// Update is called once per frame
	void Update ()
    {
        BasicMovement();
        if (!gmc.isReturning)
        {
            if (isSelected)
            {
                SelectedWorld();
            }
        
            if (transitioning)
            {
                TranstionSmoke();
            }
        }
        if (gmc.isReturning && gmc.world==this.name)
        {
            ReturnToPos();
        }
   
	}

    void BasicMovement()
    {
        transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);
        //transform.RotateAround(target.transform.position, Vector3.up, 10 * Time.deltaTime);
    }

    void SelectedWorld()
    {
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, target.position, speed * Time.deltaTime);

    }

    void ReturnToPos()
    {
        if (transform.position != retPos)
        {
            this.transform.position = Vector3.MoveTowards(gameObject.transform.position, retPos, speed * Time.deltaTime);
        }
        else if(gmc.WasCompleted())
        {
            StartCoroutine(GetGalaxyBadge());
            gmc.ResetCompleted();
        }
        else
        {
            gmc.Returned();
            transitioning = false;
        }

    }
    void TranstionSmoke()
    {
        ParticleSystem.MainModule i = insideSmoke.main;
        i.startSize = 1000;
    }

    void OnMouseDown()
    {
            isSelected = true;   
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(gmc.isReturning);
        if (!gmc.isReturning && other.tag=="Player")
        {
            transitioning = true;
            EventManager.TriggerEvent(this.name);
            StartCoroutine(EnteringTheWorld());
            
        }
    }

    public IEnumerator EnteringTheWorld()
    {
        ScreenFader fadeTime = gmc.GetComponent<ScreenFader>();
        fadeTime.TurnFadeOff(false);
        //yield return new WaitForSeconds(timer);
        yield return new WaitForSeconds(fadeTime.fadeTime);
        SceneManager.LoadScene(connectedWorld);

    }


    public IEnumerator GetGalaxyBadge()
    {
        explosion=Instantiate(Resources.Load("WorldExplosion")) as GameObject;
        Instantiate(explosion, transform.position, transform.rotation);
        yield return new WaitForSeconds(delay);
        galaxy.SetActive(true);
        EventManager.TriggerEvent(name + "Destroyed");
        Destroy(gameObject);

    }
}
