using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ClosePreviewScript : MonoBehaviour {

    private UnityAction close;
    // Use this for initialization
    private void Awake()
    {
        close = new UnityAction(ExitPreview);
    }

    private void OnEnable()
    {
        EventManager.StartListening("ClosePreview", close);
    }

    private void OnDisable()
    {
        EventManager.StopListening("ClosePreview", close);
    }

    void ExitPreview()
    {
        Destroy(gameObject);
    }
}
