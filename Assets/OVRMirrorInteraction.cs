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
        protected Material oldSelectMat;

        public Material yellowMat;
        public Material greenMat;

        public GameObject exitPortal;

        protected int numPressed;
        protected Vector3 leftStartPos;
        protected Vector3 rightStartPos;
        protected float minDist = 0.2f;
        protected float maxDist = 0.4f;
        protected float maxAngle = 20.0f;


        private enum DIRECTION { RIGHT_FRONT_SLOW, RIGHT_FRONT_FAST, LEFT_FRONT_SLOW, LEFT_FRONT_FAST, NONE };

        private InteractableMirrorController controller;



        public void OnHoverEnter(Transform t)

        {
            Debug.Log("mirror OnHoverEnter " + t.gameObject.tag);

            if (t.gameObject.tag == "GameMirror")

            {

                // highlight mirror

                if (numPressed == 0) {
                    oldHoverMat = t.gameObject.transform.GetChild(1).gameObject.GetComponent<Renderer>().material;
                    if (yellowMat != null) { t.gameObject.transform.GetChild(1).gameObject.GetComponent<Renderer>().material = yellowMat; }
                }

            }

        }



        public void OnHoverExit(Transform t)

        {
            //Debug.Log("mirror OnHoverExit");


            if (t.gameObject.tag == "GameMirror")
            {
                if(numPressed == 0) t.gameObject.transform.GetChild(1).gameObject.GetComponent<Renderer>().material = oldHoverMat;
            }

        }



        public void OnSelect(Transform t)
        {
            Debug.Log("mirror OnSelect " + t.gameObject.tag);
            if (t.gameObject.tag == "GameMirror")
            {
                numPressed++;
                if (numPressed == 1)
                {
                    oldSelectMat = t.gameObject.transform.GetChild(1).gameObject.GetComponent<Renderer>().material;
                    if (greenMat != null) { t.gameObject.transform.GetChild(1).gameObject.GetComponent<Renderer>().material = greenMat; }
                    t.gameObject.GetComponent<InteractableMirrorController>().selectMirror();
                    controller = t.gameObject.GetComponent<InteractableMirrorController>();
                    StoreHandPositions();
                }
            }

            if (t.gameObject.tag == "ExitPortal")
            {
                exitPortal.GetComponent<ExitPortalScript>().leaving = true;
            }
        }

        public void OnDeselect(Transform t)
        {
            Debug.Log("mirror OnDeselect");
            if (t.gameObject.tag == "GameMirror")
            {
                numPressed--;
                if (numPressed == 0)
                {
                    t.gameObject.transform.GetChild(1).gameObject.GetComponent<Renderer>().material = oldHoverMat;
                    t.gameObject.GetComponent<InteractableMirrorController>().unselectMirror();
                    controller = null;
                }
            }
        }

        protected void StoreHandPositions()
        {
            leftStartPos = GetLeftHandPos();
            rightStartPos = GetRightHandPos();
        }

        protected Vector3 GetLeftHandPos()
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

            Vector3 leftHandPos = GetLeftHandPos();
            Vector3 rightHandPos = GetRightHandPos();
            float leftDistance = Vector3.Distance(leftStartPos, leftHandPos);
            float rightDistance = Vector3.Distance(rightStartPos, rightHandPos);

           Debug.Log("left distance: " + leftDistance + " right dist: " + rightDistance);
            if (leftDistance > minDist || rightDistance > minDist)
            {
                //Debug.Log(leftDistance + " " + rightDistance + " " + minDist);

                DIRECTION leftDir = left_getDirection(leftHandPos - leftStartPos);

                DIRECTION rightDir = right_getDirection(rightHandPos - rightStartPos);

                //Debug.Log("left dir: " + leftDir + " right dir: " + rightDir);

                handleBending(leftDir);
                handleBending(rightDir);

                leftStartPos = leftHandPos;
                rightStartPos = rightHandPos;
            }


            //Debug.Log("update done");

        }



        void handleBending(DIRECTION dir)

        {

            if (dir == DIRECTION.NONE || controller == null) return;

            Debug.Log("handle bending called " + dir);

            switch (dir)

            {


                case DIRECTION.RIGHT_FRONT_SLOW:

                    controller.spinMirror(true, 1);

                    break;

                case DIRECTION.RIGHT_FRONT_FAST:

                    controller.spinMirror(true, 5);

                    break;

                case DIRECTION.LEFT_FRONT_SLOW:

                    controller.spinMirror(false, 1);

                    break;

                case DIRECTION.LEFT_FRONT_FAST:

                    controller.spinMirror(false, 5);

                    break;

            }

        }



        DIRECTION left_getDirection(Vector3 v)

        {

            //Debug.Log("get left direction: " + v);

            //Debug.Log("left angle to up: " + Vector3.Angle(v, Vector3.up));

            // needs to check with direction of raycast not FORWARD
            /*
            if (Vector3.Angle(v, Vector3.forward) > maxAngle) {
                return DIRECTION.NONE;
            }*/
            if (v.magnitude > maxDist)

            {

                return DIRECTION.LEFT_FRONT_FAST;

            }

            else if (v.magnitude > minDist)

            {

                return DIRECTION.LEFT_FRONT_SLOW;

            }

            return DIRECTION.NONE;

        }



        DIRECTION right_getDirection(Vector3 v)

        {

            // Debug.Log("get right direction: " + v);

            // Debug.Log("right angle to up: " + Vector3.Angle(v, Vector3.up));

            // needs to check with direction of raycast not FORWARD
            /*
            if (Vector3.Angle(v, Vector3.forward) > maxAngle)
            {
                return DIRECTION.NONE;
            }*/
            if (v.magnitude > maxDist)

            {

                return DIRECTION.RIGHT_FRONT_FAST;

            }

            else if (v.magnitude > minDist)

            {

                return DIRECTION.RIGHT_FRONT_SLOW;

            }

            return DIRECTION.NONE;

        }

    }

}