﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour {

    // Use this for initialization
    public Transform MirrorCam;
    public Transform PlayerCam;
	
	// Update is called once per frame
	void Update ()
    {
        CalculateRotation();	
	}

    public void CalculateRotation()
    {
        Vector3 dir = (PlayerCam.position - transform.position).normalized;
        Quaternion rot = Quaternion.LookRotation(dir);

        rot.eulerAngles = transform.eulerAngles - rot.eulerAngles;

        MirrorCam.localRotation = rot;
    }
    private void OnBecameInvisible()
    {
    }

}
