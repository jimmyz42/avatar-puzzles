using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManagerController : MonoBehaviour {

    private UnityAction earth;
    private UnityAction fire;
    private UnityAction water;
    private UnityAction air;
    private UnityAction finish;

    public bool completed;

    public GameObject galaxy;
    public GameObject explosion;
    public Vector3 savedWorldPlaced;
    public Vector3 returnWorldPos;
    public GameObject selectedWorld;
    public string world;
    public string astralScene;
    public bool isReturning;
    public RoomWorldController rmc;
    public float delayForGalaxy;

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        Scene sn = SceneManager.GetActiveScene();
        astralScene = sn.name;

        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }


        earth = new UnityAction(EarthSave);
        fire = new UnityAction(FireSave);
        water = new UnityAction(WaterSave);
        air = new UnityAction(AirSave);
        finish = new UnityAction(Completed);

    }

    void OnEnable()
    {
        EventManager.StartListening("EarthWorld", earth);
        EventManager.StartListening("FireWorld", fire);
        EventManager.StartListening("WaterWorld", water);
        EventManager.StartListening("AirWorld", air);
        EventManager.StartListening("CompletedWorld", finish);
    }

    void OnDisable()
    {
        EventManager.StopListening("EarthWorld", earth);
        EventManager.StopListening("FireWorld", fire);
        EventManager.StopListening("WaterWorld", water);
        EventManager.StopListening("AirWorld", air);
        EventManager.StopListening("CompletedWorld", finish);
    }

    void EarthSave()
    {
        world = "EarthWorld";
        Saving();
    }

    void FireSave()
    {
        world = "FireWorld";
        Saving();
    }

    void WaterSave()
    {
        world = "WaterWorld";
        Saving();
    }

    void AirSave()
    { 
        world = "AirWorld";
        Saving();
    }

    void Completed()
    {
        completed = true;
    }

    public void ResetCompleted()
    {
        completed = false;
    }

    public bool WasCompleted()
    {
        return completed;
    }

    public void Returned()
    {
        isReturning = false;
    }
    void Saving()
    {
        selectedWorld = GameObject.Find(world);
        savedWorldPlaced = selectedWorld.transform.position;
    }

    void OnLevelWasLoaded()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        if (sceneName==astralScene)
        {
            if ( world != null && GameObject.Find(world) != null )
            {
                isReturning = true;
                ReloadWorld();

            }
        }
    }

    void ReloadWorld()
    {
        selectedWorld = GameObject.Find(world);
        returnWorldPos = selectedWorld.transform.position;
        selectedWorld.transform.position = savedWorldPlaced;
        rmc = selectedWorld.GetComponent<RoomWorldController>();
        rmc.SetReturn(returnWorldPos, delayForGalaxy);
        rmc.transitioning = true;
    }
}
