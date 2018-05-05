using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Init_Candles : MonoBehaviour {

	public float sceneStartTimer;
	public GameObject candleTemplate;
	public int minRows = 2, maxRows = 5, minCols = 5, maxCols = 10;
	public bool randomInit = false;
	private int rows, cols;
	private GameObject[,] candles;
    public bool canWin;

	public bool PlayEndGame;


	// Use this for initialization
	void Start () {
		StartCoroutine(InitCandleTimer());
        StartCoroutine(CheckForWin());
	}
	
	// Update is called once per frame
	void Update () {
        if (canWin && (checkWinCondition () || PlayEndGame)) {
			onGameWin ();
		}
	}

	IEnumerator InitCandleTimer()
	{
		yield return new WaitForSeconds(sceneStartTimer);
		initCandles ();
	}

	private void initCandles() {
		rows = Random.Range (minRows, maxRows);
		cols = Random.Range (minCols, maxCols);
		candles = new GameObject[rows, cols];

		float dist_min = 8.0f/2, dist_max = 48.0f/2;
		float lowerBound = dist_min / Mathf.Pow(dist_max/dist_min, 1.0f/(maxRows-1.0f));
		float upperBound = dist_max * Mathf.Pow(dist_max/dist_min, 1.0f/(maxRows-1.0f));

		// figure out positions
		for (int i = 0; i < rows; i++) {
			float r = lowerBound * Mathf.Pow (upperBound / lowerBound, (i + 1.0f) / (rows + 1.0f));
			for (int j = 0; j < cols; j++) {
				float angle = Mathf.PI*2 * j/cols;
				Vector3 pos = new Vector3 (r * Mathf.Cos (angle), -2.07f,  r * Mathf.Sin (angle));
				GameObject candle = Instantiate(candleTemplate, pos, Quaternion.identity, gameObject.transform);
				CandleController control = candle.GetComponent<CandleController> ();
				control.setManager (this);
				control.setCoords (i, j);
				candles [i, j] = candle;
			}
		}
		if (randomInit) {
			while(!randomCandleConfig ()); // repeat until good
		} else {
			setCandleConfig (false); // all off
		}
	}

	public void toggleCandle(int row, int col) {
		candles [row, col].GetComponent<CandleController> ().toggleFireOn ();
		if(row+1 < rows) candles[row+1, col].GetComponent<CandleController> ().toggleFireOn ();
		if(row-1 >= 0) candles[row-1, col].GetComponent<CandleController> ().toggleFireOn ();
		if(cols >= 2) candles[row, (col+1)%cols].GetComponent<CandleController> ().toggleFireOn ();
		if(cols >= 3) candles[row, (col+cols-1)%cols].GetComponent<CandleController> ().toggleFireOn ();
	}

	// returns true if satisfies parameters (toggles at least minToggle, candles not all on)
	private bool randomCandleConfig(int minToggle = 5) {
		setCandleConfig (true); // start with solved (all on), then randomize
		int numToggle = 0;
		for (int i = 0; i < rows; i++) {
			for (int j = 0; j < cols; j++) {
				if (Random.Range(0.0f, 1.0f) < 0.5f) {
					numToggle++;
					toggleCandle (i, j);
				}
			}
		}	
		return numToggle >= minToggle && !checkWinCondition ();
	}

	private void setCandleConfig(bool on) {
		for (int i = 0; i < rows; i++) {
			for (int j = 0; j < cols; j++) {
				candles [i, j].GetComponent<CandleController> ().PutFireOn (on);
			}
		}		
	}

	public bool checkWinCondition() {
		for (int i = 0; i < rows; i++) {
			for (int j = 0; j < cols; j++) {
				if (!candles [i, j].GetComponent<CandleController> ().isFireOn ()) {
					return false;
				}
			}
		}
		return true;
	}

    IEnumerator CheckForWin()
    {
        yield return new WaitForSeconds(sceneStartTimer + 2f);
        canWin = true;
    }
	public void onGameWin()
    {
        // TODO Brianna
        EventManager.TriggerEvent("TurnTheFlamesRed");
        EventManager.TriggerEvent("StartTheSmoke");
    }

}
