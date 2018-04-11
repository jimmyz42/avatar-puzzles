using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace RockInteractionNameSpace
{
    public class OVRRockInteraction : MonoBehaviour
    {

        protected Material oldHoverMat;
        public Material yellowMat;

        protected int numPressed;
        protected Vector3 leftStartPos;
        protected Vector3 rightStartPos;
        protected float minDist = 0.02f;
        protected float maxAngle = 45.0f;
        private enum DIRECTION { LEFT, RIGHT, UP, DOWN, FRONT, BACK, NONE };
        private InteractableRockController controller;

        public void OnHoverEnter(Transform t)
        {
            if (t.gameObject.tag == "Movable_Rock")
            {
                // highlight rock
                oldHoverMat = t.gameObject.GetComponent<Renderer>().material;
                if (yellowMat != null) { t.gameObject.GetComponent<Renderer>().material = yellowMat;}
            }
        }

        public void OnHoverExit(Transform t)
        {

            if (t.gameObject.tag == "Movable_Rock")
            {
                t.gameObject.GetComponent<Renderer>().material = oldHoverMat;
            }
        }

        public void OnSelect(Transform t)
        {
            if (t.gameObject.tag == "Movable_Rock")
            {
                numPressed++;

                if (numPressed == 1)
                {
                    //t.gameObject.GetComponent<InteractableRockController>().selectRock();
                    controller = t.gameObject.GetComponent<InteractableRockController>();
                    StoreHandPositions();
                }

                
            }
        }


        public void OnDeselect(Transform t)
        {
            if (t.gameObject.tag == "Movable_Rock")
            {

                numPressed--;

                if (numPressed == 0)
                {
                    t.gameObject.GetComponent<InteractableRockController>().unselectRock();
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
                    DIRECTION leftDir = getDirection(leftHandPos - leftStartPos);
                    DIRECTION rightDir = getDirection(rightHandPos - rightStartPos);
                    Debug.Log("left dir: " + leftDir + " right dir: " + rightDir);
                    if (leftDir == rightDir)
                    {
                        handleBending(leftDir);
                    }
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
                    controller.selectRock();
                    break;
                case DIRECTION.DOWN:
                    controller.unselectRock();
                    break;
                case DIRECTION.LEFT:
                    controller.slideRock(InteractableRockController.Direction.LEFT);
                    break;
                case DIRECTION.RIGHT:
                    controller.slideRock(InteractableRockController.Direction.RIGHT);
                    break;
                case DIRECTION.FRONT:
                    controller.slideRock(InteractableRockController.Direction.UP);
                    break;
                case DIRECTION.BACK:
                    controller.slideRock(InteractableRockController.Direction.DOWN);
                    break;
            }
        }

        DIRECTION getDirection(Vector3 v)
        {
            Debug.Log("get directoin  " + v);
            Debug.Log("angel to up: " + Vector3.Angle(v, Vector3.up));
            if(Vector3.Angle(v, Vector3.up) < maxAngle)
            {
                return DIRECTION.UP;
            }
            else if (Vector3.Angle(v, Vector3.down) < maxAngle)
            {
                return DIRECTION.DOWN;
            }
            else if (Vector3.Angle(v, Vector3.forward) < maxAngle)
            {
                return DIRECTION.FRONT;
            }
            else if (Vector3.Angle(v, Vector3.back) < maxAngle)
            {
                return DIRECTION.BACK;
            }
            else if (Vector3.Angle(v, Vector3.left) < maxAngle)
            {
                return DIRECTION.LEFT;
            }
            else if (Vector3.Angle(v, Vector3.right) < maxAngle)
            {
                return DIRECTION.RIGHT;
            }
            return DIRECTION.NONE;
        }
    }
}
