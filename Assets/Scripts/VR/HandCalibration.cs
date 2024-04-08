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
    [SerializeField] private GameObject plane;
    [SerializeField] private GameObject pouch;
    [SerializeField] private GameObject eyeObject;
    private enum CalibrationState { None, Hand, Run, Pouch, Grab, Eyes};
    private GameObject player;
    private bool isAHold = false;
    private bool isAReleased = true;
    private bool isBHold = false;
    private bool isBReleased = true;
    [SerializeField] private Text calibrationText;
    [SerializeField] private CalibrationState calibrationState = CalibrationState.Hand;
    private string[] calibrationTexts = {
        @"Welcome to AmblyoVR
This is the Calibration Scene.

Firstly, put both of your hands in front of you and hold the 'A' button.",
@"Great! This is your running position.
You can move your hands alternately up and down near this position to move forward.

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
            float lowPointY = Mathf.Min(leftHand.transform.position.y, rightHand.transform.position.y);
            pouch.transform.position = new Vector3(pouch.transform.position.x, lowPointY, pouch.transform.position.z);
            // PouchManager pouchManager = pouch.GetComponent<PouchManager>();
            // pouchManager.SetDistanceFromHead(Camera.main.transform.position.y - pouch.transform.position.y);
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
}
