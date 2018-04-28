using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace MirrorInteractionNameSpace
{
	public class OVRMirrorInteraction : MonoBehaviour
	{

		protected Material oldHoverMat;
		public Material yellowMat;

		protected int numPressed;
		protected Vector3 leftStartPos;
		protected Vector3 rightStartPos;
		protected float minDist = 0.02f;
		protected float minAngle = 20.0f;
		protected float maxAngle = 45.0f;
		private enum DIRECTION { UP, DOWN, RIGHT_FRONT_SLOW, RIGHT_FRONT_FAST, LEFT_FRONT_SLOW, LEFT_FRONT_FAST, NONE };
		private InteractableMirrorController controller;

		public void OnHoverEnter(Transform t)
		{
			if (t.gameObject.tag == "Movable_Mirror")
			{
				// highlight mirror
				oldHoverMat = t.gameObject.GetComponent<Renderer>().material;
				if (yellowMat != null) { t.gameObject.GetComponent<Renderer>().material = yellowMat;}
			}
		}

		public void OnHoverExit(Transform t)
		{

			if (t.gameObject.tag == "Movable_Mirror")
			{
				t.gameObject.GetComponent<Renderer>().material = oldHoverMat;
			}
		}

		public void OnSelect(Transform t)
		{
			if (t.gameObject.tag == "Movable_Mirror")
			{
				numPressed++;

				if (numPressed == 1)
				{
					//t.gameObject.GetComponent<InteractableMirrorController>().selectMirror();
					controller = t.gameObject.GetComponent<InteractableMirrorController>();
					StoreHandPositions();
				}


			}
		}


		public void OnDeselect(Transform t)
		{
			if (t.gameObject.tag == "Movable_Mirror")
			{

				numPressed--;

				if (numPressed == 0)
				{
					t.gameObject.GetComponent<InteractableMirrorController>().unselectMirror();
					controller = null;
				}
			}
		}

		protected void StoreHandPositions() {
			leftStartPos = GetLeftHandPos();
			rightStartPos = GetRightHandPos();
		}

		protected Vector3 GetLeftHandPos ()
		{
			return OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTrackedRemote);
		}

		protected Vector3 GetRightHandPos()
		{
			return OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTrackedRemote);
		}

		// Use this for initialization
		void Start()
		{

		}

		// Update is called once per frame
		void Update()
		{
			if (numPressed != 0)
			{
				//Debug.Log("get direction called");

				Vector3 leftHandPos = GetLeftHandPos();
				Vector3 rightHandPos = GetRightHandPos();

				float leftDistance = Vector3.Distance(leftStartPos, leftHandPos);
				float rightDistance = Vector3.Distance(rightStartPos, rightHandPos);

				//Debug.Log("left distance: " + leftDistance + " right dist: " + rightDistance);   
				if (leftDistance > minDist && rightDistance > minDist) {
					Debug.Log(leftDistance + " " + rightDistance + " " + minDist);
					DIRECTION leftDir = left_getDirection(leftHandPos - leftStartPos);
					DIRECTION rightDir = right_getDirection(rightHandPos - rightStartPos);
					Debug.Log("left dir: " + leftDir + " right dir: " + rightDir);
				}
			}
			//Debug.Log("update done");
		}

		void handleBending(DIRECTION dir)
		{
			if (dir == DIRECTION.NONE || controller == null) return;
			Debug.Log("handle bending called " + dir);
			switch(dir)
			{
			case DIRECTION.UP:
				controller.selectMirror();
				break;
			case DIRECTION.DOWN:
				controller.unselectMirror();
				break;
			case DIRECTION.RIGHT_FRONT_SLOW:
				controller.spinMirror(true, 1);
				break;
			case DIRECTION.RIGHT_FRONT_FAST:
				controller.spinMirror(true, 10);
				break;
			case DIRECTION.LEFT_FRONT_SLOW:
				controller.spinMirror(false, 1);
				break;
			case DIRECTION.LEFT_FRONT_FAST:
				controller.spinMirror(false, 10);
				break;
			}
		}

		DIRECTION left_getDirection(Vector3 v)
		{
			Debug.Log("get direction: " + v);
			Debug.Log("angle to up: " + Vector3.Angle(v, Vector3.up));
			if(Vector3.Angle(v, Vector3.up) < maxAngle)
			{
				return DIRECTION.UP;
			}
			else if (Vector3.Angle(v, Vector3.down) < maxAngle)
			{
				return DIRECTION.DOWN;
			}
			else if (Vector3.Angle(v, Vector3.forward) < minAngle)
			{
				return DIRECTION.LEFT_FRONT_SLOW;
			}
			else if (Vector3.Angle(v, Vector3.forward) < maxAngle)
			{
				return DIRECTION.LEFT_FRONT_FAST;
			}
			return DIRECTION.NONE;
		}

		DIRECTION right_getDirection(Vector3 v)
		{
			Debug.Log("get direction: " + v);
			Debug.Log("angle to up: " + Vector3.Angle(v, Vector3.up));
			if(Vector3.Angle(v, Vector3.up) < maxAngle)
			{
				return DIRECTION.UP;
			}
			else if (Vector3.Angle(v, Vector3.down) < maxAngle)
			{
				return DIRECTION.DOWN;
			}
			else if (Vector3.Angle(v, Vector3.forward) < minAngle)
			{
				return DIRECTION.RIGHT_FRONT_SLOW;
			}
			else if (Vector3.Angle(v, Vector3.forward) < maxAngle)
			{
				return DIRECTION.RIGHT_FRONT_FAST;
			}
			return DIRECTION.NONE;
		}
	}
}
