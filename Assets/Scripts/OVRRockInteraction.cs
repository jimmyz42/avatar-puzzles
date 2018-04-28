using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace RockInteractionNameSpace
{
	public class OVRRockInteraction : OVRInteractable
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

        new public void OnHoverEnter(Transform t)
        {
            if (t.gameObject.tag == "Movable_Rock")
            {
                // highlight rock
                oldHoverMat = t.gameObject.GetComponent<Renderer>().material;
                if (yellowMat != null) { t.gameObject.GetComponent<Renderer>().material = yellowMat;}
            }
        }

        new public void OnHoverExit(Transform t)
        {
            if (t.gameObject.tag == "Movable_Rock")
            {
                t.gameObject.GetComponent<Renderer>().material = oldHoverMat;
            }
        }

        public new void OnSelect(Transform t)
        {
            if (t.gameObject.tag == "Movable_Rock")
            {
                oldHoverMat = t.gameObject.GetComponent<Renderer>().material;
                if (yellowMat != null) { t.gameObject.GetComponent<Renderer>().material = yellowMat; }
                numPressed++;

                if (numPressed == 1)
                {
                    //t.gameObject.GetComponent<InteractableRockController>().selectRock();
                    controller = t.gameObject.GetComponent<InteractableRockController>();
                    StoreHandPositions();
                }

                
            }
        }


        new public void OnDeselect(Transform t)
        {
            if (t.gameObject.tag == "Movable_Rock")
            {

                numPressed--;
                if (numPressed == 0)
                {
                    t.gameObject.GetComponent<InteractableRockController>().unselectRock();
                    controller = null;
                }
                t.gameObject.GetComponent<Renderer>().material = oldHoverMat;
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

                Vector3 leftHandPos = GetLeftHandPos();
                Vector3 rightHandPos = GetRightHandPos();

                float leftDistance = Vector3.Distance(leftStartPos, leftHandPos);
                float rightDistance = Vector3.Distance(rightStartPos, rightHandPos);

                  
                if (leftDistance > minDist && rightDistance > minDist) {
                    
                    DIRECTION leftDir = getDirection(leftHandPos - leftStartPos);
                    DIRECTION rightDir = getDirection(rightHandPos - rightStartPos);
                    
                    if (leftDir == rightDir)
                    {
                        handleBending(leftDir);
                    }
                }
            }
            
        }

        void handleBending(DIRECTION dir)
        {
            if (dir == DIRECTION.NONE || controller == null) return;
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
                    controller.unselectRock();
                    break;
                case DIRECTION.RIGHT:
                    controller.slideRock(InteractableRockController.Direction.RIGHT);
                    controller.unselectRock();
                    break;
                case DIRECTION.FRONT:
                    controller.slideRock(InteractableRockController.Direction.UP);
                    controller.unselectRock();
                    break;
                case DIRECTION.BACK:
                    controller.slideRock(InteractableRockController.Direction.DOWN);
                    controller.unselectRock();
                    break;
            }
        }

        DIRECTION getDirection(Vector3 v)
        {
           
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
