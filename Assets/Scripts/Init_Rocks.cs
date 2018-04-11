using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Init_Rocks : MonoBehaviour {

	public float sceneStartTimer;
	public GameObject rockTemplate;
	public int rows = 10;
	public int cols = 10;
	public int numRocks = 20;
	private int rock_w = 30, rock_d = 30;
	private Dictionary<Vector2Int, GameObject> rocks = new Dictionary<Vector2Int, GameObject>();
    public GameObject SecondFloor;
    public ParticleSystem endTranstion;
    public bool PlayEndGame;

	// Use this for initialization
	void Start () {
		StartCoroutine(InitRockTimer());
        //SecondFloor = GameObject.Find("ReMadeFloor");
        GameObject effect = GameObject.FindGameObjectWithTag("EndEffect");
        endTranstion = effect.GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
		if (checkWinCondition () || PlayEndGame) {
			onGameWin ();
		}
	}

	IEnumerator InitRockTimer()
	{
		yield return new WaitForSeconds(sceneStartTimer);
		initRocks ();
	}

	private void initRocks() {
		HashSet<Vector2Int> points = new HashSet<Vector2Int> ();
		while(points.Count < numRocks) {
			points.Add (new Vector2Int (Random.Range (0, rows), Random.Range (0, cols)));
		}
		foreach (Vector2Int point in points) {
			Vector3 pos = getTransformPos (point);
			GameObject rock = Instantiate(rockTemplate, pos, Quaternion.identity);
			InteractableRockController control = rock.GetComponent<InteractableRockController> ();
			control.setManager (this);
			control.setPosition (point);
			rocks.Add (point, rock);
		}
	}

	public Vector2Int moveRock(Vector2Int pos, int dx, int dy) {
		Vector2Int cur = pos;
		while(inBoundsNotRock(new Vector2Int(cur.x+dx, cur.y+dy))) {
			cur = new Vector2Int(cur.x+dx, cur.y+dy);
		}
		GameObject rock = rocks [pos];
		rocks.Remove (pos);
		rocks.Add (cur, rock);
		return cur;
	}

	private bool inBoundsNotRock(Vector2Int p) {
		return p.x >= 0 && p.y >= 0 && p.x < cols && p.y < rows && !rocks.ContainsKey(p);
	}

	public Vector3 getTransformPos(Vector2Int p, float y = 22) {
		return new Vector3 ((p.x - (cols - 1) / 2.0f) * rock_w, y, (p.y - (rows - 1) / 2.0f) * rock_d);
	}

	// Check for win condition
	public bool checkWinCondition () {
		int middleCol = cols / 2;
		for (int i = 0; i < rows; i++) {
			if (!rocks.ContainsKey (new Vector2Int (middleCol, i)))
				return false;
		}
		return true;
	}

	// Use this handler to do whatever actions need to be taken when game is won
	public void onGameWin()
    {

        StartCoroutine(EndTheGame());
		// TODO Brianna
	}

    IEnumerator EndTheGame()
    {
        endTranstion.Play();
        yield return new WaitForSeconds(5);
        SecondFloor.SetActive(true);
        gameObject.SetActive(false);
    }
}
