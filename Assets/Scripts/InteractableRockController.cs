using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableRockController : MonoBehaviour {
	// NOTE to abilities team: Please call the following methods:
	// selectRock(), unselectRock(), slideRock(dir)

	// Assume that game grid is [-150, 150] x [-150, 150]
	// rocks are 30 x 30, so 10 x 10 grid

    public float timer;
	public float speed;
	public enum Direction {RIGHT, UP, LEFT, DOWN};
	public static GameObject selectedRock = null;
	private Init_Rocks manager;
	private Vector2Int position;
	private Vector3 targetPos;

	private static int[] delta_x = { 1, 0, -1, 0 };
	private static int[] delta_z = { 0, 1, 0, -1 };

    private bool isMoving = false;
    public AudioSource audioSource;
    public AudioClip slidingSFX;
    public AudioClip collisionSFX;

	// Use this for initialization
	void Start ()
    {
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.anyKey) {
			if (Input.GetKey (KeyCode.UpArrow))
				slideRock (Direction.UP);
			if (Input.GetKey (KeyCode.DownArrow))
				slideRock (Direction.DOWN);
			if (Input.GetKey (KeyCode.LeftArrow))
				slideRock (Direction.LEFT);
			if (Input.GetKey (KeyCode.RightArrow))
				slideRock (Direction.RIGHT);
		}
		// Move the rock
		Vector3 target = targetPos;
		target.y = gameObject.transform.position.y; // don't change y, only x and z
		gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, target, speed * Time.deltaTime);

        // detect collision and play collision sound
        if (isMoving) {
            if (Vector3.Distance(target, gameObject.transform.position) < 2) {
                playCollisionSfx();
                isMoving = false;
            }
        }

	}

    public void playCollisionSfx() {
        audioSource.clip = collisionSFX;
        audioSource.Play();
    }

    public void playSlidingSfx() {
        audioSource.clip = slidingSFX;
        audioSource.Play();
    }

	void OnMouseDown() {
		if (selectedRock != gameObject) {
			unselectRock (); // unselect that rock
			selectRock (); // select this rock
		} else {
			unselectRock (); // clicking selected rock unselects it
		}
	}

	public void setManager(Init_Rocks manager) {
		this.manager = manager;
	}

	public void setPosition(Vector2Int p) {
		this.position = p;
		this.targetPos = manager.getTransformPos (p, 25);
	}

    public void Appear()
    {
        gameObject.SetActive(true);
    }

	public void selectRock() {
		selectedRock = gameObject;
		Vector3 temp = selectedRock.transform.position;
		temp.y = 35;
        selectedRock.transform.position = temp;
	}

	public void unselectRock() {
		if (selectedRock != null) {
			Vector3 temp = selectedRock.transform.position;
			temp.y = 22;
			selectedRock.transform.position = temp;
		}
		selectedRock = null;
	}

	public void slideRock(Direction direction) {
		if (selectedRock != gameObject)
			return; // invalid, can only move self
		// selectedRock is this.gameObject, so just move self
		int dir = (int)direction;
		setPosition(manager.moveRock (this.position, delta_x [dir], delta_z [dir]));

        // register slide start and play sound
        isMoving = true;
        playSlidingSfx();

	}
}
