using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Init_Pipes : MonoBehaviour {

	public float sceneStartTimer;
	public GameObject pipeTemplate;
	public GameObject holeTemplate;
	public GameObject rockTemplate;
	public float pipeSpeed = 100.0f;
	public int numPipes = 8;
	public int numRocks = 12;
	public float minRockSize = 10.0f;
	public float maxRockSize = 50.0f;

	private float unit_w;
	private float unit_off;
	private float pipe_R = 2f, pipe_h = 3.5f;
	private float rock_buffer = 10f, pipe_buffer = 0.5f;
	private GameObject[] pipes;
	private Dictionary<int, int> pipeToHole = new Dictionary<int, int>();
	private int selectedPipe = -1, selectedHole = -1;
	private int tentativePipes = 0;

	public bool PlayEndGame;

	// Use this for initialization
	void Start () {
		pipes = new GameObject[numPipes];
		unit_w = 400.0f / numPipes;
		unit_off = -(numPipes-1.0f)/2.0f * unit_w;
		StartCoroutine(InitPipeTimer());
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.anyKey) {
			if (Input.GetKey (KeyCode.LeftArrow))
				retractPipe ();
			if (Input.GetKey (KeyCode.RightArrow))
				extendPipe ();
		}
		if (checkWinCondition () || PlayEndGame) {
			onGameWin ();
		}
	}

	IEnumerator InitPipeTimer()
	{
		yield return new WaitForSeconds(sceneStartTimer);
		initPipes ();
	}

	struct line {
		public float x1, y1, x2, y2;
		public line(float x1, float y1, float x2, float y2) {
			this.x1 = x1;
			this.y1 = y1;
			this.x2 = x2;
			this.y2 = y2;
		}
	}

	struct rock {
		public float x, y, r;
		public rock(float x, float y, float r) {
			this.x = x;
			this.y = y;
			this.r = r;
		}
	}

	private void initPipes() {
		List<int> perm = new List<int> ();
		for (int i = 0; i < numPipes; i++) {
			perm.Add (i);
			int j = Random.Range (0, i + 1);
			perm [i] = perm [j];
			perm [j] = i;
		}

		List<line> lines = new List<line> ();
		List<rock> rocks = new List<rock> ();

		for (int i = 0; i < numPipes; i++) {
			pipeToHole [i] = -1;
			lines.Add (new line (-200, unit_w * i + unit_off, 200, unit_w * i + unit_off));
		}
		for (int i = 0; i < numPipes; i++) {
			// create pipe
			Vector3 pos = new Vector3 (unit_w * i + unit_off, 83 + pipe_h * i, 443);
			GameObject pipe = Instantiate (pipeTemplate, pos, Quaternion.identity, gameObject.transform.GetChild(0));
			pipe.transform.localScale = new Vector3 (1.25f, 1.25f, 1.25f);
			pipes [i] = pipe;
			WaterPipeScript control = pipe.GetComponent<WaterPipeScript> ();
			control.setManager (this);
			control.setNum (i);
		}
		for (int i = 0; i < numPipes; i++) {
			// create hole
			Vector3 pos = new Vector3 (unit_w * i + unit_off, 93, -200);
			GameObject hole = Instantiate(holeTemplate, pos, Quaternion.Euler(90, 0, 0), gameObject.transform.GetChild(1));
			hole.transform.localScale = new Vector3 (0.5992696f, 1.1f, 2.69027f);
			GoalHoleScript control = hole.GetComponent<GoalHoleScript> ();
			control.setManager (this);
			control.setNum (i);
		}
		for (int i = 0; i < numRocks; i++) {
			float x, y, r;
			do {
				x = Random.Range(0.0f, 400.0f);
				y = Random.Range(0.0f, 400.0f);
				r = Mathf.Min(x, y, 400.0f-x, 400.0f-y, maxRockSize);
				x -= 400.0f/2;
				y -= 400.0f/2;
				foreach(rock rock1 in rocks) {
					r = Mathf.Min(r, Mathf.Sqrt((rock1.x-x)*(rock1.x-x) + (rock1.y-y)*(rock1.y-y))-rock1.r-rock_buffer);
				}
				foreach(line line1 in lines) {
					r = Mathf.Min(r, distToLine(x, y, line1.x1, line1.y1, line1.x2, line1.y2)-pipe_R-pipe_buffer);
				}
			} while(r < minRockSize);
			rocks.Add(new rock(x, y, r));
			// create rock
			Vector3 pos = new Vector3 (y, 50, -x);
			GameObject rock = Instantiate(rockTemplate, pos, Quaternion.identity, gameObject.transform.GetChild(2));
			rock.transform.localScale = new Vector3 (r, 50.0f, r);
		}
	}

	private float distToLine(float x0, float y0, float x1, float y1, float x2, float y2) {
		return Mathf.Abs(x0 * (y2-y1) - y0 * (x2-x1) + x2 * y1 - y2 * x1)/Mathf.Sqrt((y2-y1)*(y2-y1) + (x2-x1)*(x2-x1));
	}

	// Abilities: call this when bend right
	public void extendPipe() {
		if (selectedPipe == -1 || selectedHole == -1)
			return; // must select both pipe and hole
		if(pipeToHole[selectedPipe] != -1 || pipeToHole.ContainsValue(selectedHole))
			return; // pipe must be unmatched, hole must be unmatched
		tentativePipes++;
		pipeToHole [selectedPipe] = selectedHole;

		float angle = Mathf.Atan2 ((selectedHole - selectedPipe) * unit_w, 400.0f);
		Vector3 rotCenter = new Vector3(unit_w * selectedPipe + unit_off, 83 + pipe_h * selectedPipe, 200);
		pipes [selectedPipe].transform.RotateAround (rotCenter, Vector3.up, -angle * 180.0f/Mathf.PI);
		Vector3 velocity = new Vector3 (Mathf.Sin (angle) * pipeSpeed, 0, -Mathf.Cos (angle) * pipeSpeed); 
		pipes [selectedPipe].GetComponent<WaterPipeScript> ().setVelocity(velocity);
		pipes [selectedPipe].GetComponent<WaterPipeScript> ().setZLimit (pipes [selectedPipe].transform.position.z);

		selectedPipe = -1;
		selectedHole = -1;
	}

	// Abilities: call this when bend left
	public void retractPipe() {
		if (selectedPipe == -1 || selectedHole == -1)
			return; // must select both pipe and hole
		if(pipeToHole[selectedPipe] != selectedHole)
			return; // must be matched to unmatch
		float angle = Mathf.Atan2 ((selectedHole - selectedPipe) * unit_w, 400.0f);
		Vector3 velocity = new Vector3 (-Mathf.Sin (angle) * pipeSpeed, 0, Mathf.Cos (angle) * pipeSpeed); 
		pipes [selectedPipe].GetComponent<WaterPipeScript> ().setVelocity(velocity);

		pipeToHole [selectedPipe] = -1;
		selectedPipe = -1;
		selectedHole = -1;
	}

	public void setSelectedPipe(int num) {
		selectedPipe = num;
	}

	public void setSelectedHole(int num) {
		selectedHole = num;
	}

	public void dissociatePipe(int num) {
		pipeToHole[num] = -1;
	}

	public void finalizePipe() {
		tentativePipes--;
	}

	public bool checkWinCondition() {
		if (tentativePipes > 0 || pipeToHole.Count < numPipes)
			return false; // still animating
		for (int i = 0; i < numPipes; i++) {
			if (pipeToHole [i] == -1)
				return false;
		}
		return true;
	}

	public void onGameWin()
	{
		// TODO Brianna
	}
}
