using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableMirrorController : MonoBehaviour {

	public static GameObject selectedMirror = null;
	public int dir;
	public float spinSpeed;
	public float riseSpeed;

	private float realSpinSpeed;
	private float angleToSpin;
	private static int mirror_dirs = Init_Mirrors.mirror_dirs;

	private Init_Mirrors manager;

    public AudioClip[] windSfxs;
    public AudioSource audioSource;

    void PlayWindSfx()
    {
        int index = Random.Range(0, windSfxs.Length);
        audioSource.clip = windSfxs[index];
        audioSource.Play();
    }

	// Use this for initialization
	void Start () {
		dir = Random.Range (0, mirror_dirs);
		// Unity rotation is CW, not CCW, so negate
		gameObject.transform.Rotate(new Vector3(0, -dir * 360f / mirror_dirs, 0));
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.anyKey) {
//			Debug.Log ("key pressed");
//			Debug.Log (Input.GetKey (KeyCode.UpArrow));
			if (Input.GetKey (KeyCode.UpArrow))
				spinMirror (true);
			if (Input.GetKey (KeyCode.DownArrow))
				spinMirror (false);
			if (Input.GetKey (KeyCode.LeftArrow))
				spinMirror (true);
			if (Input.GetKey (KeyCode.RightArrow))
				spinMirror (false);
		}
		if (gameObject.transform.position.y < 0) {
			float newY = Mathf.Min (0f, gameObject.transform.position.y + riseSpeed * Time.deltaTime);
//			Debug.Log (newY);
			Vector3 target = gameObject.transform.position;
			target.y = newY;
			gameObject.transform.position = target;
		}
//		Debug.Log (angleToSpin);
		if (angleToSpin != 0) {
			float angle;
			if (angleToSpin > 0) {
				angle = Mathf.Min (angleToSpin, realSpinSpeed * Time.deltaTime);
			} else {
				angle = Mathf.Max (angleToSpin, -realSpinSpeed * Time.deltaTime);
			}
			angleToSpin -= angle;
			// Unity rotation is CW, not CCW, so negate
			gameObject.transform.Rotate (new Vector3 (0, -angle, 0));
		}
	}

	void OnMouseDown() {
//		Debug.Log ("mirror mouse clicked");
		if (selectedMirror != gameObject) {
			unselectMirror (); // unselect that mirror
			selectMirror (); // select this mirror
		} else {
			//unselectMirror (); // clicking selected mirror unselects it
		}
	}

	public void setManager(Init_Mirrors manager) {
		this.manager = manager;
	}

	public void selectMirror() {
		selectedMirror = gameObject;
        // TODO Art Team (show mirror selected?)
    }

	public void unselectMirror() {
		if (selectedMirror != null) {
			// TODO Art Team (show mirror unselect?)
		}
		selectedMirror = null;
	}

	public void spinMirror(bool isCCW, int amt = 1) {
		if (selectedMirror != gameObject)
			return; // invalid, can only spin self
        PlayWindSfx();
        dir = isCCW ? (dir+amt) % mirror_dirs : (dir - (amt % mirror_dirs) + mirror_dirs) % mirror_dirs;
		angleToSpin += isCCW ? amt * 360f / mirror_dirs : -amt * 360f / mirror_dirs;
		realSpinSpeed = spinSpeed * Mathf.Sqrt (amt);
		manager.updateLaserPath ();
	}
}
