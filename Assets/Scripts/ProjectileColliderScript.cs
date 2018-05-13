using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ProjectileColliderScript : MonoBehaviour {


    [System.Serializable]
    public class SelectionCallback : UnityEvent { }

    public ProjectileColliderScript.SelectionCallback onMirrorCollision;


    // Use this for initialization
    private void OnCollisionEnter(Collision collision)
    {
        string HitTag = collision.collider.tag;
        if( HitTag != "Player" || HitTag =="GameMirror")
        {
            gameObject.SetActive(false);
            onMirrorCollision.Invoke();
        }   
    }
}
