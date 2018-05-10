using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAirBallScript : MonoBehaviour
{
    public GameObject mainProjectile;
    public GameObject player;
    public ParticleSystem mainParticleSystem;
    public float AirBallForce;

    private Rigidbody proj;

    private void Awake()
    {
        proj = mainProjectile.GetComponent<Rigidbody>();
        mainProjectile.transform.position = player.transform.position;
        mainProjectile.transform.rotation = player.transform.rotation;
    }
    // Update is called once per frame
    void Update ()
    {
	    if (Input.GetKeyDown(KeyCode.Space))
        {
            mainProjectile.SetActive(true);
            proj.AddForce(player.transform.forward * AirBallForce);
            
        }

        if (mainParticleSystem.IsAlive()==false)
        {
            mainProjectile.SetActive(false);
            mainProjectile.transform.position = player.transform.position;
            mainProjectile.transform.rotation = player.transform.rotation;
        }
	}
}
