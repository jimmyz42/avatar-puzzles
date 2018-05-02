using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LakeHolesScript : MonoBehaviour {

    public LakeWaterFallScript[] lakeholes;
    public List<LakeWaterFallScript> lakeholesOn;
    public List<LakeWaterFallScript> lakeholesOff;

    private UnityAction start;
    private UnityAction activate;
    private UnityAction inactivate;

    private void Awake()
    {
        lakeholes = GetComponentsInChildren<LakeWaterFallScript>();
        start = new UnityAction(TurnLakeHolesOff);
        activate = new UnityAction(TurnOnOneLakeHole);
        inactivate = new UnityAction(TurnOffOneLakeHole);
    }

    private void OnEnable()
    {
        EventManager.StartListening("StartLevel", start);
        EventManager.StartListening("TurnOnALakeWaterfall", activate);
        EventManager.StartListening("TurnOffALakeWaterfall", inactivate);
    }

    private void OnDisable()
    {
        EventManager.StopListening("StartLevel", start);
        EventManager.StopListening("TurnOnALakeWaterfall", activate);
        EventManager.StopListening("TurnOffALakeWaterfall", inactivate);
    }


    public void TurnLakeHolesOff()
    {
        //Debug.Log("TurnLakesOff was triggered");
        StartCoroutine(TurnAllHolesOff());
    }

    IEnumerator TurnAllHolesOff()
    {
        foreach(LakeWaterFallScript l in lakeholes)
        {
            yield return new WaitForSeconds(1f);
            l.TurnOffWaterfall();
            lakeholesOff.Add(l);


            
        }
    }

    public void TurnOnOneLakeHole()
    {
        //Debug.Log("Turn a water fall on");
        LakeWaterFallScript w = GetAnOffWaterFall();
        if (w != null)
        {
            w.TurnOnWaterFall();
            lakeholesOn.Add(w);
        }
    }

    public void TurnOffOneLakeHole()
    {
        //Debug.Log("Turn a water fall off");
        LakeWaterFallScript w = GetAnOnWaterFall();
        if (w != null)
        {
            w.TurnOffWaterfall();
            lakeholesOff.Add(w);
        }
    }

    public LakeWaterFallScript GetAnOffWaterFall()
    {
        if (lakeholesOff.Count>0)
        {
            LakeWaterFallScript f = lakeholesOff[0];
            lakeholesOff.Remove(f);
            return f;
        }

        return null;
        
    }

    public LakeWaterFallScript GetAnOnWaterFall()
    {
        if (lakeholesOn.Count > 0)
        {
            LakeWaterFallScript f = lakeholesOn[0];
            lakeholesOn.Remove(f);
            return f;
        }

        return null;
    }
}
