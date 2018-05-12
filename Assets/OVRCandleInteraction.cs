using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class OVRCandleInteraction : OVRInteractable {
    protected int numPressed;
    protected Vector3 leftStartPos;
    protected Vector3 rightStartPos;
    protected Vector3 hitPoint;
    protected float minDist = 0.15f;
    protected float maxAngle = 45.0f;
    private enum DIRECTION { LEFT, RIGHT, UP, DOWN, FRONT, BACK, NONE };
    private CandleController controller;

    public float multiplier = 1.4F;

    public new void OnSelect(Transform t)
    {
        if (t.gameObject.tag == "Candle")
        {            
            numPressed++;

            if (numPressed == 1)
            {
                controller = t.gameObject.GetComponent<CandleController>();
                StoreHandPositions();
                hitPoint = t.position;
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

                if (leftDistance >= rightDistance)
                {
                    DIRECTION leftDir = getDirection(leftStartPos, leftHandPos);
                    handleBending(leftDir);
                    numPressed = 0;
                }
                else
                {
                    DIRECTION rightDir = getDirection(rightStartPos, rightHandPos);
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

    DIRECTION getDirection(Vector3 s, Vector3 e)
    {
        Vector3 v = e - s;
        /*if (Vector3.Angle(v, Vector3.up) < maxAngle)
        {
            return DIRECTION.UP;
        }
        if (Vector3.Angle(v, Vector3.down) < maxAngle)
        {
            return DIRECTION.DOWN;
        }
        if (Vector3.Angle(v, Vector3.left) < maxAngle)
        {
            return DIRECTION.LEFT;
        }
        if (Vector3.Angle(v, Vector3.right) < maxAngle)
        {
            return DIRECTION.RIGHT;
        }*/
        //else if (Vector3.Angle(v, Vector3.forward) < maxAngle)
        //{

        /*if (Vector3.Angle(e, s) < 4.0F)
        {
            Debug.Log("Angle Used " + Vector3.Angle(e, s));
            return DIRECTION.FRONT;
            
        }*/
        if (Mathf.Abs(Vector3.Angle(hitPoint, s) - Vector3.Angle(e, hitPoint)) < 10F)
        {
            Debug.Log("Angle Used " + Vector3.Angle(e, s));
            return DIRECTION.FRONT;

        }

        //return getPunch(s, e);
        //}

        return DIRECTION.NONE;
    }
    

    //This function is not working becuase the candles and hands are in different coordinate spaces
    DIRECTION getPunch(Vector3 t, Vector3 c)
    {
        //will return DIRECTION.FRONT if punch detected
        //will return DIRECTION.NONE by default
        // hitpoint is the other vector used. It is assigned on select
        // multiplier used for math. Public to change/test
        Vector3 f = hitPoint;

        //distance from start to end hand
        Debug.Log("Candle point, f " + f + " start point, t " + t + " current point, c " + c);


        float tc = Mathf.Sqrt(Mathf.Pow(t.x - c.x, 2F) + Mathf.Pow(t.y-c.y, 2F) + Mathf.Pow(t.z - c.z, 2F));
        Debug.Log("tc " + tc);

        //distance from end to candle
        double cf = Mathf.Sqrt(Mathf.Pow(Mathf.Abs(c.x) - Mathf.Abs(f.x), 2F) + Mathf.Pow(Mathf.Abs(c.y) - Mathf.Abs(f.y), 2F) + Mathf.Pow(Mathf.Abs(c.z) - Mathf.Abs(f.z), 2F));
        Debug.Log("cf " + cf);

        //distance from start to candle
        double tf = Mathf.Sqrt(Mathf.Pow(Mathf.Abs(t.x) - Mathf.Abs(f.x), 2F) + Mathf.Pow(Mathf.Abs(t.y) - Mathf.Abs(f.y), 2F) + Mathf.Pow(Mathf.Abs(t.z) - Mathf.Abs(f.z), 2F));
        Debug.Log("tf " + tf);

        if (tf > cf && (tc + cf) <= (multiplier * tf))
        {
            return DIRECTION.FRONT;
        }

        return DIRECTION.NONE;
    }
}
