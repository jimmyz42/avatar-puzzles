using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class GameManagerController : MonoBehaviour {

    private UnityAction earth;
    private UnityAction fire;
    private UnityAction water;
    private UnityAction air;
    private UnityAction finish;
    private UnityAction eg;
    private UnityAction fg;
    private UnityAction ag;
    private UnityAction wg;
    public List<string> doneWorlds;


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
    public GameObject video;

    private bool canPlayVid;

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        Scene sn = SceneManager.GetActiveScene();
        astralScene = sn.name;

        canPlayVid = true;
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }


        earth = new UnityAction(EarthSave);
        fire = new UnityAction(FireSave);
        water = new UnityAction(WaterSave);
        air = new UnityAction(AirSave);
        finish = new UnityAction(Completed);
        eg = new UnityAction(ReplaceEarth);
        fg = new UnityAction(ReplaceFire);
        wg = new UnityAction(ReplaceWater);
        ag = new UnityAction(ReplaceAir);
        //SceneManager.LoadScene(5, LoadSceneMode.Additive);

    }

    void OnEnable()
    {
        EventManager.StartListening("EarthWorld", earth);
        EventManager.StartListening("FireWorld", fire);
        EventManager.StartListening("WaterWorld", water);
        EventManager.StartListening("AirWorld", air);
        EventManager.StartListening("CompletedWorld", finish);
        EventManager.StartListening("EarthWorldDestroyed", eg);
        EventManager.StartListening("FireWorldDestroyed", fg);
        EventManager.StartListening("WaterWorldDestroyed", wg);
        EventManager.StartListening("AirWorldDestroyed", ag);
    }

    void OnDisable()
    {
        EventManager.StopListening("EarthWorld", earth);
        EventManager.StopListening("FireWorld", fire);
        EventManager.StopListening("WaterWorld", water);
        EventManager.StopListening("AirWorld", air);
        EventManager.StopListening("CompletedWorld", finish);
        EventManager.StopListening("EarthWorldDestroyed", eg);
        EventManager.StopListening("FireWorldDestroyed", fg);
        EventManager.StopListening("WaterWorldDestroyed", wg);
        EventManager.StopListening("AirWorldDestroyed", ag);
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

    void ReplaceEarth()
    {
        doneWorlds.Add("EarthWorld");
    }
    void ReplaceFire()
    {
        doneWorlds.Add("FireWorld");
    }
    void ReplaceWater()
    {
        doneWorlds.Add("WaterWorld");
    }
    void ReplaceAir()
    {
        doneWorlds.Add("AirWorld");
    }

    void OnLevelWasLoaded()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        video = GameObject.Find("360Video");
        //ScreenFader f = gameObject.GetComponent<ScreenFader>();
        //f.TurnFadeOff(true);
        if (sceneName==astralScene)
        {
            RespawnBadges();
            if ( world != null && GameObject.Find(world) != null)
            {
                //canPlayVid = false;
                Destroy(video);
                isReturning = true;
                ReloadWorld();

            }
        }
    }

    void RespawnBadges()
    {
        foreach(string w in doneWorlds)
        {
            EventManager.TriggerEvent(w + "Galaxy");
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
