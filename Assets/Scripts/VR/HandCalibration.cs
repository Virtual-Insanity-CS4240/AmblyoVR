using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandCalibration : MonoBehaviour
{
    [SerializeField] private InputActionProperty Abutton;
    [SerializeField] private InputActionProperty Bbutton;
    [SerializeField] private float timeAPressed = 1.0f;
    [SerializeField] private float timeBPressed = 1.0f;
    [SerializeField] private GameObject leftHand;
    [SerializeField] private GameObject rightHand;
    [SerializeField] private GameObject plane;
    [SerializeField] private GameObject pouch;
    private enum CalibrationState { None, Hand, Pouch};
    private bool isAHold = false;
    private bool isAReleased = true;
    private bool isBHold = false;
    private bool isBReleased = true;
    [SerializeField] private CalibrationState calibrationState = CalibrationState.None;

    // Start is called before the first frame update
    void Start()
    {
        // Vector3 midPoint = (leftHand.transform.position + rightHand.transform.position) / 2;
        // BodyManager planeManager = plane.GetComponent<BodyManager>();
        // planeManager.SetOffsetFromCenter(midPoint);
        // planeManager.SetScale(leftHand.transform.position, rightHand.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
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
            // NextState();
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
            Vector3 midPoint = (leftHand.transform.position + rightHand.transform.position) / 2;
            pouch.transform.position = midPoint;
            pouch.transform.localPosition = new Vector3(0, pouch.transform.localPosition.y, 0);
            // PouchManager pouchManager = pouch.GetComponent<PouchManager>();
            // pouchManager.SetDistanceFromHead(Camera.main.transform.position.y - pouch.transform.position.y);
            // Debug.Log("Pouch Calibrated");
            StartCoroutine(VRControllerUtility.VibrateController(0.3f, 0.7f, 0.7f, OVRInput.Controller.All));
            // NextState();
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
            case CalibrationState.None:
                calibrationState = CalibrationState.Hand;
                break;
            case CalibrationState.Hand:
                calibrationState = CalibrationState.Pouch;
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
