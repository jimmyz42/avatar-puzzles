using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileColliderScript : MonoBehaviour {

    // Use this for initialization
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag != "Player")
        {
            gameObject.SetActive(false);
        }
    }
}
