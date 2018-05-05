using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OVRCandleInteraction : OVRInteractable {
    protected int numPressed;
    protected Vector3 leftStartPos;
    protected Vector3 rightStartPos;
    protected float minDist = 0.02f;
    protected float maxAngle = 45.0f;
    private enum DIRECTION { LEFT, RIGHT, UP, DOWN, FRONT, BACK, NONE };
    private CandleController controller;

    protected Material oldHoverMat;
    public Material yellowMat;

    public new void OnSelect(Transform t)
    {
        if (t.gameObject.tag == "Candle")
        {            
            numPressed++;

            if (numPressed == 1)
            {
                controller = t.gameObject.GetComponent<CandleController>();
                StoreHandPositions();
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
        return 
             OVRCandleInteraction`tbu5rgOVRInput.Controller.LTrackedRemote);
    }

    protected Vector3 GetRightHandPos()
    {
        return OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTrackedRemote);
    }

	
	// Update is called once per frame
	void Update () {
        if (numPressed != 0)
        {

            Vector3 leftHandPos = GetLeftHandPos();
            Vector3 rightHandPos = GetRightHandPos();

            float leftDistance = Vector3.Distance(leftStartPos, leftHandPos);
            float rightDistance = Vector3.Distance(rightStartPos, rightHandPos);


            if (leftDistance > minDist || rightDistance > minDist)
            {

                DIRECTION leftDir = getDirection(leftHandPos - leftStartPos);
                DIRECTION rightDir = getDirection(rightHandPos - rightStartPos);

                if (leftDir >= rightDir)
                {
                    handleBending(leftDir);
                    numPressed = 0;
                }
                else
                {
                    handleBending(rightDir);
                    numPressed = 0;
                }
            }

        }
    }

    void handleBending(DIRECTION dir)
    {
        if (dir == DIRECTION.NONE || controller == null) return;
        switch (dir)
        {
            case DIRECTION.FRONT:
                controller.toggle ();
                break;
            // Would like to add a case where there is lava react or firebolt if you miss/are not toggling

        }
    }

    DIRECTION getDirection(Vector3 v)
    {

        if (Vector3.Angle(v, Vector3.up) < maxAngle)
        {
            return DIRECTION.UP;
        }
        else if (Vector3.Angle(v, Vector3.down) < maxAngle)
        {
            return DIRECTION.DOWN;
        }
        //else if (Vector3.Angle(v, Vector3.forward) < maxAngle)
        //{
            return DIRECTION.FRONT;
        //}
        
        return DIRECTION.NONE;
    }
}
