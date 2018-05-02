using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveEventTrigger : MonoBehaviour {

    public Material disolveMaterial;
    public float speed, max;

    private float currentY, startTime;

    private void Awake()
    {
        disolveMaterial.SetFloat("_DisolveY", currentY);
    }
    private void Update()
    {
        if (currentY < max)
        {
            disolveMaterial.SetFloat("_DisolveY", currentY);
            currentY += Time.deltaTime * speed;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            TriggerEffect();
        }
    }

    public void TriggerEffect()
    {
        startTime = Time.time;
        currentY = 0;
    }

    public void Restore()
    {
        currentY = 0;
        disolveMaterial.SetFloat("_DisolveY", currentY);
        max = 0;
         
       
    }
}
