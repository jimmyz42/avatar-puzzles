using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OVRPipeInteraction : MonoBehaviour {

    protected Material oldHoverMatLeft;
    protected Material oldHoverMatRight;

    public Material yellowMat;
    public Material greenMat;

    public GameObject waterPipesObj;

    protected bool leftPressed, rightPressed;
    private Init_Pipes pipeManager;

    Vector3 leftStartPos;
    Vector3 rightStartPos;

    float THRES = .2f;



    // Use this for initialization
    void Start () {
        pipeManager = waterPipesObj.GetComponent<Init_Pipes>();
	}
	
	// Update is called once per frame
	void Update () {

        if (leftPressed && rightPressed) {
            Vector3 leftHandPos = GetLeftHandPos();
            Vector3 rightHandPos = GetRightHandPos();

            float rightHandToLeftStartDistance = Vector3.Distance(leftStartPos, rightHandPos);
            float leftHandToRightStartDistance = Vector3.Distance(rightStartPos, leftHandPos);

            Debug.Log("rightHandToLeftStartDistance = " + rightHandToLeftStartDistance);
            Debug.Log("leftHandToRightStartDistance = " + leftHandToRightStartDistance);

            if (rightHandToLeftStartDistance < THRES)
            {
                Debug.Log("retracting pipe");
                pipeManager.retractPipe();
            }
            else if (leftHandToRightStartDistance < THRES)
            {
                Debug.Log("retracting pipe");
                pipeManager.extendPipe();
            }

        }
		
	}

    public void OnHoverEnter(Transform t, bool isLeft)
    {
        Debug.Log("on hover enter ");
        Debug.Log(t.gameObject.tag + " isleft: "+isLeft);

        if (isLeft)
        {
            if (t.gameObject.tag == "WaterPipe" && !leftPressed)
            {
                oldHoverMatLeft = t.gameObject.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Renderer>().material;
                if (yellowMat != null)
                {
                    t.gameObject.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Renderer>().material = yellowMat;
                }
            }
        } else
        {
            if (t.gameObject.tag == "GoalHole" && !rightPressed)
            {
                oldHoverMatRight = t.gameObject.transform.gameObject.GetComponent<Renderer>().material;
                if (yellowMat != null)
                {
                     t.gameObject.transform.gameObject.GetComponent<Renderer>().material = yellowMat;
                }
            }
        }
    }

    public void OnHoverExit(Transform t, bool isLeft)
    {
        Debug.Log("on hover exit ");
        Debug.Log(t.gameObject.tag + " isleft: " + isLeft);

        if (isLeft)
        {
            if (t.gameObject.tag == "WaterPipe" && !leftPressed)
            {
                t.gameObject.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Renderer>().material = oldHoverMatLeft;
            }
        }
        else
        {
            if (t.gameObject.tag == "GoalHole" && !rightPressed)
            {
                t.gameObject.transform.gameObject.GetComponent<Renderer>().material = oldHoverMatRight;
            }
        }
    }

    protected Vector3 GetLeftHandPos()
    {
        return OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTrackedRemote);
    }

    protected Vector3 GetRightHandPos()
    {
        return OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTrackedRemote);
    }

    public void OnSelect(Transform t, bool isLeft)
    {
        Debug.Log("on select ");
        Debug.Log(t.gameObject.tag + " isleft: " + isLeft);

        if (isLeft)
        {
            leftPressed = true;
            leftStartPos = GetLeftHandPos();  
            if (t.gameObject.tag == "WaterPipe")
            {
                if (greenMat != null) { t.gameObject.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Renderer>().material = greenMat; }
                t.gameObject.GetComponent<WaterPipeScript>().selectPipe();
            }
        }
        else
        {
            rightPressed = true;
            rightStartPos = GetRightHandPos();
            if (t.gameObject.tag == "GoalHole")
            {
                if (greenMat != null) { t.gameObject.transform.gameObject.GetComponent<Renderer>().material = greenMat; }
                t.gameObject.GetComponent<GoalHoleScript>().selectHole();
                // DELETE LATER
                //pipeManager.extendPipe();
            }
        }
    }

    public void OnDeselect(Transform t, bool isLeft)
    {
        Debug.Log("on deselect ");
        //Debug.Log(t.gameObject.tag + " isleft: " + isLeft);

        if (isLeft)
        {
            leftPressed = false;
            if (t.gameObject.tag == "WaterPipe")
            {
                t.gameObject.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Renderer>().material = oldHoverMatLeft;
                t.gameObject.GetComponent<WaterPipeScript>().unselectPipe();
            }
        }
        else
        {
            rightPressed = false;
            if (t.gameObject.tag == "GoalHole")
            {
                t.gameObject.transform.gameObject.GetComponent<Renderer>().material = oldHoverMatRight;
                t.gameObject.GetComponent<GoalHoleScript>().unselectHole();
            }
        }
    }

    public void changeHand() {
    }

}
