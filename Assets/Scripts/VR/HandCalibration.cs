using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class HandCalibration : MonoBehaviour
{
    [SerializeField] private InputActionProperty Abutton;
    [SerializeField] private InputActionProperty Bbutton;
    [SerializeField] private CustomGrab leftGrab;
    [SerializeField] private CustomGrab rightGrab;
    [SerializeField] private float timeAPressed = 1.0f;
    [SerializeField] private float timeBPressed = 1.0f;
    [SerializeField] private GameObject leftHand;
    [SerializeField] private GameObject rightHand;
    [SerializeField] private GameObject leftController;
    [SerializeField] private GameObject rightController;
    [SerializeField] private GameObject planeLeftHand;
    [SerializeField] private SkinnedMeshRenderer planeLeftHandMesh;
    [SerializeField] private GameObject planeRightHand;
    [SerializeField] private SkinnedMeshRenderer planeRightHandMesh;
    [SerializeField] private GameObject plane;
    [SerializeField] private GameObject pouchLeftBall;
    [SerializeField] private GameObject pouchRightBall;
    [SerializeField] private GameObject pouch;
    [SerializeField] private GameObject eyeObject;
    private enum CalibrationState { None, Hand, Run, Pouch, Grab, Eyes};
    private GameObject player;
    private bool isAHold = false;
    private bool isAReleased = true;
    private bool playHandAnimation = false;
    private bool isBHold = false;
    private bool isBReleased = true;
    [SerializeField] private Text calibrationText;
    [SerializeField] private CalibrationState calibrationState = CalibrationState.Hand;
    private string[] calibrationTexts = {
        @"Welcome to AmblyoVR
This is the Calibration Scene.

Firstly, put both of your hands in front of you and hold the 'A' button.",
@"Great! This is your running position.
You can move your hands alternately up and down near this position to move forward
in the direction you are facing.

Push the left joystick down while running to move backwards instead.

Once you are done trying, put your hands at your waist level and hold the 'B' button.",
@"Awesome! This is your pouch position.

Press and hold the grab trigger (middle finger) near the pouch position to get a ball.
You now have a lot of balls in you inventory for you to try.

Release the grab trigger to throw the ball.",
@"Good job! You are almost done with calibration.

Adjust your eye settings in front of you.
After you are done, you will be teleported to the tutorial scene."
    };

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (calibrationText != null)
        {
            calibrationText.text = calibrationTexts[0];
        }
        if (eyeObject != null)
        {
            eyeObject.SetActive(false);
        }
        planeLeftHand.SetActive(false);
        planeRightHand.SetActive(false);
        planeLeftHandMesh.material.SetColor("_ColorTop", Color.white);
        planeRightHandMesh.material.SetColor("_ColorTop", Color.white);
        pouchLeftBall.SetActive(false);
        pouchRightBall.SetActive(false);
        // Vector3 midPoint = (leftHand.transform.position + rightHand.transform.position) / 2;
        // BodyManager planeManager = plane.GetComponent<BodyManager>();
        // planeManager.SetOffsetFromCenter(midPoint);
        // planeManager.SetScale(leftHand.transform.position, rightHand.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (calibrationState == CalibrationState.Grab && (leftGrab.isBallThrown() || rightGrab.isBallThrown()))
        {
            NextState();
        }
        float AbuttonValue = Abutton.action.ReadValue<float>();
        float BbuttonValue = Bbutton.action.ReadValue<float>();
        // if you hold A button
        if (AbuttonValue == 1 && !isAHold && isAReleased)
        {
            isAReleased = false;
            StartCoroutine(TimeAPressed());
        }
        if (AbuttonValue == 1 && isAHold)
        {   
            Debug.Log("A pressed");
            planeLeftHand.SetActive(true);
            planeRightHand.SetActive(true);
            planeLeftHand.transform.position = leftController.transform.position;
            planeRightHand.transform.position = rightController.transform.position;
            planeLeftHandMesh.material.SetFloat("_Opacity", 0.6f);
            planeRightHandMesh.material.SetFloat("_Opacity", 0.6f);
            planeLeftHandMesh.material.SetFloat("_OutlineOpacity", 0.4f);
            planeRightHandMesh.material.SetFloat("_OutlineOpacity", 0.4f);
            StartCoroutine(HandAnimation());
            Vector3 midPoint = (leftHand.transform.position + rightHand.transform.position) / 2;
            BodyManager planeManager = plane.GetComponent<BodyManager>();
            planeManager.SetOffsetFromCenter(midPoint);
            planeManager.SetScale(leftHand.transform.position, rightHand.transform.position);
            Debug.Log("Hand Calibrated");
            StartCoroutine(VRControllerUtility.VibrateController(0.3f, 0.7f, 0.7f, OVRInput.Controller.All));
            if (calibrationState == CalibrationState.Hand)
            {
                NextState();
            }
            isAHold = false;
        }
        if (AbuttonValue == 0)
        {
            isAReleased = true;
        }

        // if you hold B button
        if (BbuttonValue == 1 && !isBHold && isBReleased)
        {
            isBReleased = false;
            StartCoroutine(TimeBPressed());
        }
        if (BbuttonValue == 1 && isBHold)
        {   
            Debug.Log("B pressed");
            PouchManager pouchManager = pouch.GetComponent<PouchManager>();
            pouchLeftBall.SetActive(true);
            pouchRightBall.SetActive(true);
            pouchManager.SetPosition(leftHand.transform.position, rightHand.transform.position);
            pouchManager.SetScale(leftHand.transform.position, rightHand.transform.position);
            pouchLeftBall.transform.position = leftController.transform.position + new Vector3(0.16f, 0, 0);
            pouchRightBall.transform.position = rightController.transform.position + new Vector3(0.16f, 0, 0);
            Material leftBallMaterial = pouchLeftBall.GetComponent<MeshRenderer>().material;
            Material rightBallMaterial = pouchRightBall.GetComponent<MeshRenderer>().material;
            leftBallMaterial.color = new Color(leftBallMaterial.color.r, leftBallMaterial.color.g, leftBallMaterial.color.b, 0.6f);
            rightBallMaterial.color = new Color(rightBallMaterial.color.r, rightBallMaterial.color.g, rightBallMaterial.color.b, 0.6f);
            StartCoroutine(BallAnimation());
            Debug.Log("Pouch Calibrated");
            StartCoroutine(VRControllerUtility.VibrateController(0.3f, 0.7f, 0.7f, OVRInput.Controller.All));
            if (calibrationState == CalibrationState.Pouch)
            {
                NextState();
            }
            isBHold = false;
        }
        if (BbuttonValue == 0)
        {
            isBReleased = true;
        }
    }

    public void NextState()
    {
        switch (calibrationState)
        {
            case CalibrationState.Hand:
                calibrationState = CalibrationState.Pouch;
                if (calibrationText != null)
                {
                    calibrationText.text = calibrationTexts[1];
                }
                break;
            case CalibrationState.Pouch:
                calibrationState = CalibrationState.Grab;
                if (calibrationText != null)
                {
                    calibrationText.text = calibrationTexts[2];
                }
                break;
            case CalibrationState.Grab:
                calibrationState = CalibrationState.Eyes;
                if (calibrationText != null)
                {
                    calibrationText.text = calibrationTexts[3];
                }
                if (eyeObject != null)
                {
                    eyeObject.SetActive(true);
                    eyeObject.transform.position = player.transform.position + player.transform.forward * 3 + Vector3.up * 1f;
                }
                break;
            default:
                calibrationState = CalibrationState.None;
                break;
        }
    }

    private IEnumerator TimeAPressed()
    {
        yield return new WaitForSeconds(timeAPressed);
        isAHold = true;
    }
    private IEnumerator TimeBPressed()
    {
        yield return new WaitForSeconds(timeBPressed);
        isBHold = true;
    }

    private IEnumerator HandAnimation()
    {
        while (planeLeftHandMesh.material.GetFloat("_Opacity") > 0)
        {
            planeLeftHandMesh.material.SetFloat("_Opacity", planeLeftHandMesh.material.GetFloat("_Opacity") - 0.01f);
            planeRightHandMesh.material.SetFloat("_Opacity", planeRightHandMesh.material.GetFloat("_Opacity") - 0.01f);
            planeLeftHandMesh.material.SetFloat("_OutlineOpacity", planeLeftHandMesh.material.GetFloat("_OutlineOpacity") - 0.01f);
            planeRightHandMesh.material.SetFloat("_OutlineOpacity", planeRightHandMesh.material.GetFloat("_OutlineOpacity") - 0.01f);
            yield return new WaitForSeconds(0.02f);
        }
        planeLeftHand.SetActive(false);
        planeRightHand.SetActive(false);
    }

    private IEnumerator BallAnimation()
    {
        Material leftBallMaterial = pouchLeftBall.GetComponent<MeshRenderer>().material;
        Material rightBallMaterial = pouchRightBall.GetComponent<MeshRenderer>().material;
        while (leftBallMaterial.color.a > 0)
        {
            leftBallMaterial.color = new Color(leftBallMaterial.color.r, leftBallMaterial.color.g, leftBallMaterial.color.b, leftBallMaterial.color.a - 0.01f);
            rightBallMaterial.color = new Color(rightBallMaterial.color.r, rightBallMaterial.color.g, rightBallMaterial.color.b, rightBallMaterial.color.a - 0.01f);
            yield return new WaitForSeconds(0.02f);
        }
        pouchLeftBall.SetActive(false);
        pouchRightBall.SetActive(false);
    }
}
