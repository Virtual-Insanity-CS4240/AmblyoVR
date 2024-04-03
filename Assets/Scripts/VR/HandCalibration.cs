using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCalibration : MonoBehaviour
{
    [SerializeField] private string calibrateButtonName;
    [SerializeField] private GameObject hand;
    [SerializeField] private GameObject runningPlane;
    private enum CalibrationState { None, Hand, Pouch};
    [SerializeField] private CalibrationState calibrationState = CalibrationState.None;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (calibrationState == CalibrationState.Hand)
        {
            // if you hold button
            if (Input.GetAxis(calibrateButtonName) == 1)
            {
                runningPlane.transform.position = new Vector3(runningPlane.transform.position.x, hand.transform.position.y, runningPlane.transform.position.z);
                RunningManager runningManager = runningPlane.GetComponent<RunningManager>();
                runningManager.SetDistanceFromHead(Camera.main.transform.position.y - runningPlane.transform.position.y);
                Debug.Log("Hand Calibrated");
                NextState();
            }
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
}
