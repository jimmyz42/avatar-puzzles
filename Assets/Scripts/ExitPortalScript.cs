using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitPortalScript : MonoBehaviour {

    public bool leaving;
    public Transform player;
    public float speed;

    private void Awake()
    {
     
    }
    // Use this for initialization
    private void OnMouseDown()
    {
        leaving = true;
    }

    void Update()
    {
        if (leaving)
        {
            transform.position=Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
        if (transform.position == player.position)
        {
            SceneManager.LoadScene("AstralRoom");
        }
    }
}
