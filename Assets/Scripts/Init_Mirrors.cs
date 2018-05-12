using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Init_Mirrors : MonoBehaviour {

	public float mirrorDelay;
	public float laserDelay;
	public GameObject mirrorTemplate;
	public GameObject laserTemplate;
	public int rows = 10;
	public int cols = 10;
	public int numMirrors = 20;
	public int solutionMirrors = 10;
	public float laserHeight = 3;
	public static int mirror_dirs = 16, laser_dirs = 8;

    public bool PlayEndGame;
    public float winDelay;
    public GameObject Walls;
    public float wallSpeed;
    public float mirrorDestroy;
    public float exitDelay;
    public float smokeDelay; 
    

	private int fillerMirrors;
	private int unit_w = 10, unit_d = 10;
	private Dictionary<Vector2Int, GameObject> mirrors = new Dictionary<Vector2Int, GameObject>();
	private GameObject laser;
	private int laserStartX, laserEndX;
	private static int[] delta_x = {1, 1, 0, -1, -1, -1, 0, 1};
	private static int[] delta_y = {0, 1, 1, 1, 0, -1, -1, -1};

	// Use this for initialization
	void Start () {
		StartCoroutine(InitMirrorTimer());
		StartCoroutine(InitLaserTimer());
	}

	IEnumerator InitMirrorTimer()
	{
		yield return new WaitForSeconds(mirrorDelay);
		initMirrors ();
	}

	IEnumerator InitLaserTimer()
	{
		yield return new WaitForSeconds(laserDelay);
		initLasers ();
	}
	
	// Update is called once per frame
	void Update () {
	}

	private void initMirrors() {
		laserStartX = Random.Range (0, cols);
		laserEndX = Random.Range (0, cols);
		fillerMirrors = numMirrors - solutionMirrors;
		HashSet<Vector2Int> laserSet = new HashSet<Vector2Int> ();
		HashSet<Vector2Int> mirrorSet = new HashSet<Vector2Int> ();
		// init laser set with surrounding 4 squares so they don't get filled
		for (int i = rows / 2 - 1; i <= rows / 2; i++) {
			for (int j = cols / 2 - 1; j <= cols / 2; j++) {
				laserSet.Add (new Vector2Int (j, i));
			}
		}
		while (!randomSolution (ref laserSet, ref mirrorSet))
			;
		randomFiller (laserSet, mirrorSet);

		foreach (Vector2Int point in mirrorSet) {
			Vector3 pos = getTransformPos (point, -5f);
			GameObject mirror = Instantiate(mirrorTemplate, pos, Quaternion.identity, gameObject.transform);
			InteractableMirrorController control = mirror.GetComponent<InteractableMirrorController> ();
			control.setManager (this);
			mirrors.Add (point, mirror);
		}
	}

	private void initLasers() {
		Vector3 laserStartPos = new Vector3 ((laserStartX - (cols - 1) / 2.0f) * unit_w, laserHeight, -50);
		laser = Instantiate(laserTemplate, laserStartPos, Quaternion.identity);
		updateLaserPath ();
	}

	private Vector3 getTransformPos(Vector2Int p, float y = 0f) {
		return new Vector3 ((p.x - (cols - 1) / 2.0f) * unit_w, y, (p.y - (rows - 1) / 2.0f) * unit_d);
	}

	private bool randomSolution(ref HashSet<Vector2Int> origLaserSet, ref HashSet<Vector2Int> origMirrorSet) {
		if (solutionMirrors == 1 || (solutionMirrors == 2 && laserStartX == laserEndX))
			return false;
		HashSet<Vector2Int> laserSet = new HashSet<Vector2Int> (origLaserSet);
		HashSet<Vector2Int> mirrorSet = new HashSet<Vector2Int> (origMirrorSet);
		List<Vector3Int> possiblePos = new List<Vector3Int> (); // x, y, dir (z is dir)
		for (int i = 0; i < rows; i++) {
			if (!laserSet.Contains (new Vector2Int (laserStartX, i))) {
				possiblePos.Add (new Vector3Int (laserStartX, i, 2));
			}
		}
		if (possiblePos.Count == 0)
			return false;
		Vector3Int curPos = possiblePos [Random.Range (0, possiblePos.Count)];
		for (int i = 0; i <= curPos.y; i++) {
			laserSet.Add (new Vector2Int (laserStartX, i));
		}
		mirrorSet.Add (new Vector2Int (laserStartX, curPos.y));

		for (int n = 2; n <= solutionMirrors; n++) {
			possiblePos = new List<Vector3Int> ();
			for(int i = 0; i<laser_dirs; i++) {
				if(i == curPos.z) continue;
				for(int nx = curPos.x + delta_x[i], ny = curPos.y + delta_y[i]; inBounds(nx, ny) && !mirrorSet.Contains(new Vector2Int(nx, ny)); nx += delta_x[i], ny += delta_y[i]) {
					if(!laserSet.Contains(new Vector2Int(nx, ny))) possiblePos.Add(new Vector3Int(nx, ny, i));
				}
			}
			if (n == solutionMirrors - 1) possiblePos = possiblePos.Where (x => x[0] != laserEndX).ToList();
			else if(n == solutionMirrors) possiblePos = possiblePos.Where (x => x[0] == laserEndX).ToList();
			if(possiblePos.Count == 0) return false;
			int px = curPos.x, py = curPos.y;
			curPos = possiblePos [Random.Range (0, possiblePos.Count)];
			for (; px != curPos.x || py != curPos.y; px += delta_x [curPos.z], py += delta_y [curPos.z]) {
				laserSet.Add (new Vector2Int(px, py));
			}
			laserSet.Add (new Vector2Int (curPos.x, curPos.y));
			mirrorSet.Add (new Vector2Int (curPos.x, curPos.y));
		}
		origLaserSet = laserSet;
		origMirrorSet = mirrorSet;
		return true;
	}

	private void randomFiller(HashSet<Vector2Int> laserSet, HashSet<Vector2Int> mirrorSet) {
		for(int i=0; i<fillerMirrors; i++) {
			int tx, ty;
			Vector2Int pt;
			do {
				tx = Random.Range(0, cols);
				ty = Random.Range(0, rows);
				pt = new Vector2Int(tx, ty);
			} while(laserSet.Contains(pt));
			laserSet.Add (pt);
			mirrorSet.Add (pt);
		}		
	}

	private bool inBounds(int x, int y) {
		return x >= 0 && y >= 0 && x < cols && y < rows;
	}

	private bool inBoundsNotMirror(int x, int y) {
		return x >= 0 && y >= 0 && x < cols && y < rows && !mirrors.ContainsKey(new Vector2Int(x, y));
	}

	public void updateLaserPath() {
		LineRenderer line = laser.transform.GetChild (0).gameObject.GetComponent<LineRenderer> ();
		line.enabled = true;
		Vector3Int curPos = new Vector3Int (laserStartX, -1, 2);
		List<Vector3> pts = new List<Vector3> ();
		pts.Add (getTransformPos (new Vector2Int (curPos.x, curPos.y), laserHeight));
		while (true) {
			do {
				curPos.x += delta_x[curPos.z];
				curPos.y += delta_y[curPos.z];
			} while(inBoundsNotMirror(curPos.x, curPos.y));
//			Debug.Log ("curpos: " + curPos.x + "," + curPos.y);
			pts.Add (getTransformPos (new Vector2Int (curPos.x, curPos.y), laserHeight));

			if(mirrors.ContainsKey(new Vector2Int(curPos.x, curPos.y))) {
				int mDir = mirrors [new Vector2Int (curPos.x, curPos.y)].GetComponent<InteractableMirrorController> ().dir;
//				Debug.Log ("mdir= "+mDir);
				if(mDir < 2*curPos.z && 2*curPos.z < mDir + laser_dirs || 0 <= 2*curPos.z && 2*curPos.z < mDir + laser_dirs - mirror_dirs) {
					curPos.z = (mDir - curPos.z + laser_dirs) % laser_dirs;
				} else break;
			} else break;
		}
//		Debug.Log ("pt cnt; "+pts.Count);
//		foreach (Vector3 pt in pts) {
//			Debug.Log ("pt: "+pt.x + ", " + pt.y + ", " + pt.z);
//		}
//		foreach (KeyValuePair<Vector2Int, GameObject> pair in mirrors) {
//			Debug.Log("dir: "+pair.Value.GetComponent<InteractableMirrorController>().dir);
//		}
		line.positionCount = pts.Count;
		line.SetPositions (pts.ToArray ());

		if ((curPos.x == laserEndX && curPos.y == rows && curPos.z == 2) || PlayEndGame) {
			onGameWin ();
		}
	}

	public void onGameWin()
    {
        Debug.Log("Ending game");
        EventManager.TriggerEvent("CompletedWorld");
        StartCoroutine(EndGame());
		// TODO Brianna, make the doors open, or the walls sink, or something
	}

    IEnumerator EndGame()
    {
        Destroy(GameObject.FindGameObjectWithTag("StartLaser"));
        yield return new WaitForSeconds(winDelay);
        //Debug.Log("in end game");
        WallController w = Walls.GetComponent<WallController>();
        w.speed = wallSpeed;
        w.SetEndPos(-100);
        w.SetMove(true);
        yield return new WaitForSeconds(mirrorDestroy);
        
        DestoryMirrors();
        yield return new WaitForSeconds(exitDelay);
        EventManager.TriggerEvent("StartLeaves");
        yield return new WaitForSeconds(smokeDelay);
        EventManager.TriggerEvent("StartSmoke");


    }

    void DestoryMirrors()
    {
        GameObject[] clones = GameObject.FindGameObjectsWithTag("GameMirror");
        foreach (var clone in clones)
        {
            Destroy(clone);
        }
    }
}
