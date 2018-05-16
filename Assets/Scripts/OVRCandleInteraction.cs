using System.Collections;

using System.Collections.Generic;

using UnityEngine;


public class OVRCandleInteraction : OVRInteractable
{

    public Transform OVRCameraRig;
    public GameObject candles;

    protected int numPressed;
    protected int resetNumPressed;

    protected Vector3 candleWorldPos;

    protected Vector3 leftStartPos;

    protected Vector3 rightStartPos;

    protected Vector3 hitPoint;

    protected float minDist = 0.15f;

    protected float maxAngle = 45.0f;
    protected float maxResetAngle = 45.0f;

    private CandleController controller;

    protected Material oldHoverMat;

    public Material yellowMat;
    public Material greenMat;


    private float multiplier = 1.4F;

    private bool firstPunch = false;

    public GameObject exitPortal;

    private void resetCandles()
    {
        candles.GetComponent<Init_Candles>().setCandleConfig(false);
    }

    public void OnHoverEnter(Transform t)
    {
        if (t.gameObject.tag == "Candle")
        {
            // highlight candle
            if (numPressed == 0)
            {
                oldHoverMat = t.gameObject.transform.GetChild(1).gameObject.GetComponent<Renderer>().material;
                if (yellowMat != null) { t.gameObject.transform.GetChild(1).gameObject.GetComponent<Renderer>().material = yellowMat; }
            }

        }
    }

    public void OnHoverExit(Transform t)
    {
        if (t.gameObject.tag == "Candle")
        {
            if (numPressed == 0) t.gameObject.transform.GetChild(1).gameObject.GetComponent<Renderer>().material = oldHoverMat;
        }
    }

    public void OnSelect(Transform t)
    {
        resetNumPressed++;

        StoreHandPositions();
        if (t.gameObject.tag == "Candle")
        {
            numPressed++;
            if (numPressed == 1)
            {
                controller = t.gameObject.GetComponent<CandleController>();             
                firstPunch = true;
                candleWorldPos = t.transform.position;
                candleWorldPos.y += 1;
                hitPoint = t.position;

                if (greenMat != null) { t.gameObject.transform.GetChild(1).gameObject.GetComponent<Renderer>().material = greenMat; }
            }
        }

        if (t.gameObject.tag == "ExitPortal")
        {
            exitPortal.GetComponent<ExitPortalScript>().leaving = true;
        }
    }

    public void OnDeselect(Transform t)
    {
        resetNumPressed--;

        if (t.gameObject.tag == "Candle")
        {
            numPressed--;
            if (numPressed == 0)
            {
                t.gameObject.transform.GetChild(1).gameObject.GetComponent<Renderer>().material = oldHoverMat;
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


    private Vector3 getHandWorldPos(Vector3 localPos)
    {
        return OVRCameraRig.Find("TrackingSpace").transform.TransformPoint(localPos);
    }


    // Update is called once per frame
    void Update()
    {
        Vector3 leftHandPos = GetLeftHandPos();
        Vector3 rightHandPos = GetRightHandPos();

        float leftDistance = Vector3.Distance(leftStartPos, leftHandPos);
        float rightDistance = Vector3.Distance(rightStartPos, rightHandPos);

        if (numPressed != 0 && firstPunch)
        {
            if (leftDistance > minDist || rightDistance > minDist)
            {
                Vector3 headWorldPos = OVRCameraRig.transform.position;

                Vector3 handStartPos = getHandWorldPos(leftDistance >= rightDistance ? leftStartPos : rightStartPos);
                Vector3 handEndPos = getHandWorldPos(leftDistance >= rightDistance ? leftHandPos : rightHandPos);

                Vector3 startToEnd = handEndPos - handStartPos;
                Vector3 startToCandle = candleWorldPos - handStartPos;

                if (Vector3.Angle(startToEnd, startToCandle) < maxAngle)
                {
                    firstPunch = false;
                    controller.toggle();
                }
            }
        }
        if(resetNumPressed != 0)
        {
            if (leftDistance > minDist && rightDistance > minDist)
            {
                Vector3 leftHandVector = leftHandPos - leftStartPos;
                Vector3 rightHandVector = rightHandPos - rightStartPos;

                if (Vector3.Angle(Vector3.up, leftHandVector) < maxResetAngle && Vector3.Angle(Vector3.up, rightHandVector) < maxResetAngle)
                {
                    firstPunch = false;
                    resetCandles();
                }
            }
        }
    }
}
