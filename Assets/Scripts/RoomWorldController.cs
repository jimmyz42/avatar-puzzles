using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class RoomWorldController : MonoBehaviour
{
    private UnityAction someListener;

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
    
    
    public void SetReturn(Vector3 pos, float d)
    {
        retPos = pos;
        delay = d;
        

    }
    void Start ()
    {
        isSelected = false;
        transitioning = false;
        insideSmoke = GetComponentInChildren<ParticleSystem>();
        gmc = GameObject.Find("GameManager").GetComponent<GameManagerController>();
        if (gmc.world == this.name)
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
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, retPos, speed * Time.deltaTime);
        else
        {
            StartCoroutine(GetGalaxyBadge());

        }

    }
    void TranstionSmoke()
    {
        insideSmoke.startSize = 1000;
    }

    void OnMouseDown()
    {
        if (gameObject.tag == "EarthWorld" || gameObject.tag=="AirWorld" || gameObject.tag=="FireWorld")
        {
            isSelected = true;
        }

        
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
        yield return new WaitForSeconds(timer);
        SceneManager.LoadScene(connectedWorld);

    }

    public IEnumerator GetGalaxyBadge()
    {
        explosion=Instantiate(Resources.Load("WorldExplosion")) as GameObject;
        Instantiate(explosion);
        //explosion.SetActive(true);
        yield return new WaitForSeconds(delay);
        galaxy= Instantiate(Resources.Load("EarthGalaxy")) as GameObject;
        Instantiate(galaxy);
        //galaxy.SetActive(true);
        Destroy(gameObject);

    }
}
