using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CustomGrab : MonoBehaviour
{
    [SerializeField] private OVRInput.Controller Controller;

    [SerializeField] private string grabButtonName;
    [SerializeField] private LayerMask[] grabMasks;
    private List<int> grabbableLayer = new List<int>();
    [SerializeField] private GameObject attachAchor;
    [SerializeField] private InputActionProperty changeColor;
    [SerializeField] private GameObject[] hands;
    [SerializeField] private int colorIndex = 0;
    [SerializeField] private GameObject[] ballPrefabs;
    [SerializeField] private float throwThreshold = 1f;
    [SerializeField] private int totalTrackedPositions = 25;
    [SerializeField] private int positionsBeforeRelease = 15;
    [SerializeField] private int rotationsBeforeRelease = 10;
    [SerializeField] private float throwStrength = 10f;
    [SerializeField] private float rotationStrength = 0.1f;
    [SerializeField] private float curveStrength = 0.1f;
    private Color[] colors = { Color.red, Color.green, Color.magenta, Color.yellow };
    private bool isJoystickReleased = true;

    private GameObject currGrabbedObject;
    private Rigidbody grabbedRigidbody;
    private bool isGrabbing;
    private List<GameObject> touchedObjects = new List<GameObject>();
    private bool isTracking = false;
    private List<Vector3> trackedPositions = new List<Vector3>();
    private List<Quaternion> trackedRotations = new List<Quaternion>();

    void Start()
    {
        foreach (LayerMask grabMask in grabMasks)
        {
            grabbableLayer.Add((int) Mathf.Log(grabMask.value, 2));
        }
        if (attachAchor == null)
        {
            attachAchor = new GameObject("GrabAttachAnchor");
            attachAchor.transform.parent = transform;
            attachAchor.transform.localPosition = new Vector3(0f, 0f, 0.15f);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (changeColor != null)
        {
            Vector2 changeColorValue = changeColor.action.ReadValue<Vector2>();
            if (isJoystickReleased && Math.Abs(changeColorValue.x) == 1 )
            {
                isJoystickReleased = false;
                // Mathf.Clamp((int)changeColorValue.x + colorIndex, 0, 3); 
                colorIndex = ((int)changeColorValue.x + colorIndex) < 0 ? 3 : ((int)changeColorValue.x + colorIndex) % 4;
                Color colorHand = colors[colorIndex];
                PlayerInventory.BallColorChange?.Invoke((BallColor)colorIndex);
                foreach (GameObject hand in hands)
                {
                    hand.GetComponent<SkinnedMeshRenderer>().material.SetColor("_ColorTop", colorHand);
                }
                Debug.Log(colorHand);
                Debug.Log(colorIndex);
                Debug.Log("colorchange:" + changeColorValue.x);
            }
            if (changeColorValue == Vector2.zero)
            {
                isJoystickReleased = true;
            }
        }
    }

    void FixedUpdate()
    {
        // if you hold button
        if (!isGrabbing && Input.GetAxis(grabButtonName) == 1)
        {
            GrabObject();
            isGrabbing = true;
        }

        // if you let go of button
        if (isGrabbing && Input.GetAxis(grabButtonName) < 1)
        {
            DropObject();
        }
        if (isTracking)
        {
            Debug.Log("position:" + OVRInput.GetLocalControllerPosition(Controller));
            Debug.Log("rotation:" + Quaternion.Euler(grabbedRigidbody.rotation.x, grabbedRigidbody.rotation.y, grabbedRigidbody.rotation.z));
            if (trackedRotations.Count > 2)
            {
                Quaternion throwRotation = trackedRotations[trackedRotations.Count-1] * Quaternion.Inverse(trackedRotations[trackedRotations.Count-2]);
                float angle;
                Vector3 axis;
                throwRotation.ToAngleAxis(out angle, out axis);
                if (angle > 180)
                {
                    angle -= 360;
                }
                Debug.Log("torque" + angle * axis);
            }

            if (trackedPositions.Count <= totalTrackedPositions)
            {
                trackedPositions.Add(OVRInput.GetLocalControllerPosition(Controller));
                trackedRotations.Add(Quaternion.Euler(grabbedRigidbody.rotation.x, grabbedRigidbody.rotation.y, grabbedRigidbody.rotation.z));
            }
            else
            {
                trackedPositions.Add(OVRInput.GetLocalControllerPosition(Controller));
                trackedRotations.Add(Quaternion.Euler(grabbedRigidbody.rotation.x, grabbedRigidbody.rotation.y, grabbedRigidbody.rotation.z));
                if (trackedPositions.Count > totalTrackedPositions)
                {
                    trackedPositions.RemoveAt(0);
                }
                if (trackedRotations.Count > totalTrackedPositions)
                {
                    trackedRotations.RemoveAt(0);
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (grabbableLayer.Contains(other.gameObject.layer) && !isGrabbing)
        {
            Debug.Log("touched");
            if (!other.gameObject.CompareTag("Pouch"))
            {
                StartCoroutine(VRControllerUtility.VibrateController(0.1f, 0.2f, 0.2f, Controller));
            }
            touchedObjects.Add(other.gameObject);
        }
        //if (other.gameObject.CompareTag(collectorTag) 
        //    && isGrabbing)
        //{
        //    if (currGrabbedObject.tag == "WhiteChicken" || currGrabbedObject.tag == "WhiteChickenMeat")
        //    {
        //        PlayerInventory.CollectChicken?.Invoke(ChickenType.White);
        //        audioSource.Play();
        //    }
        //    else if (currGrabbedObject.tag == "BlackChicken" || currGrabbedObject.tag == "BlackChickenMeat")
        //    {
        //        PlayerInventory.CollectChicken?.Invoke(ChickenType.Black);
        //        audioSource.Play();
        //    }
        //    Destroy(currGrabbedObject);
        //}
    }

    void OnTriggerExit(Collider other)
    {
        if (touchedObjects.Contains(other.gameObject))
        {
            touchedObjects.Remove(other.gameObject);
        }
    }

    void GrabObject()
    {

        Debug.Log("grab");
        GameObject closestObject = null;
        foreach (GameObject touchedObject in touchedObjects)
        {
            if (closestObject == null || Vector3.Distance(transform.position, touchedObject.transform.position) < Vector3.Distance(transform.position, closestObject.transform.position))
            {
                closestObject = touchedObject;
            }
        }

        if (closestObject)
        {
            isGrabbing = true;
            bool haveObject = false;
            if (closestObject.CompareTag("Cache"))
            {
                StartCoroutine(VRControllerUtility.VibrateController(0.2f, 0.5f, 0.5f, Controller));
                Debug.Log("Cache");
                if (PlayerInventory.ballCount <= PlayerInventory.maxBallCount - 10)
                {
                    PlayerInventory.ChangeBallCount(10);
                }
                else if (PlayerInventory.ballCount < PlayerInventory.maxBallCount)
                {
                    PlayerInventory.ChangeBallCount(PlayerInventory.maxBallCount - PlayerInventory.ballCount);
                }
                else
                {
                    StartCoroutine(VRControllerUtility.VibrateController(0.3f, 0.7f, 0.7f, Controller));
                    Debug.LogWarning("Cache is full");
                }
            }
            else if (closestObject.CompareTag("Pouch"))
            {
                BallColor? ballColor = PlayerInventory.EquipBall();
                Debug.Log("Color: " + ballColor);
                if (ballColor != null)
                {
                    StartCoroutine(VRControllerUtility.VibrateController(0.1f, 0.5f, 0.5f, Controller));
                    int ballType = (int)ballColor;
                    closestObject = Instantiate(ballPrefabs[ballType]);
                    haveObject = true;
                    // currGrabbedObject.GetComponent<Rigidbody>().isKinematic = true; // the grabbed object should not have gravity

                    // // grab object will follow our hands
                    // currGrabbedObject.transform.SetParent(attachAchor.transform, true); // attach the grabbed object to our attachAnchor
                    // currGrabbedObject.transform.localPosition = Vector3.zero;
                    // Debug.Log("CUM");
                }
                else
                {
                    StartCoroutine(VRControllerUtility.VibrateController(0.3f, 0.7f, 0.7f, Controller));
                }
            }
            else
            {
                haveObject = true;
            }
                
            // if (closestObject.transform.parent != null && closestObject.transform.parent.gameObject.CompareTag("Hand"))
            // {
            //     otherHand.DropObject();
            // }
            // // normal grab
            if (haveObject)
            {
                currGrabbedObject = closestObject; // grab the closest object
                grabbedRigidbody = currGrabbedObject.GetComponent<Rigidbody>();
                currGrabbedObject.GetComponent<Rigidbody>().isKinematic = true; // the grabbed object should not have gravity

                // grab object will follow our hands
                currGrabbedObject.transform.SetParent(attachAchor.transform, true); // attach the grabbed object to our attachAnchor
                currGrabbedObject.transform.localPosition = Vector3.zero;
                isTracking = true;
            }
            Debug.Log("grabcomplete");
        }
    }

    void DropObject()
    {
        isGrabbing = false;
        isTracking = false;
        
        // we are currently grabbing something, let it go
        if (currGrabbedObject != null && currGrabbedObject.transform.parent != null && currGrabbedObject.transform.parent.gameObject == attachAchor)
        {
            currGrabbedObject.transform.parent = null;
            currGrabbedObject.GetComponent<Rigidbody>().isKinematic = false; // enable gravity again for the object
            /**
             * Throw the object based on how hard we 'swing' our hand
             * 
             * HINT FOR ASSIGNMENT 2: 
             * if you would like to change to shooting instead of throwing, you can use rigidbody AddForce
             * AddForce() requires a direction vector (what direction the object should move towards), there is a way to get what direction this object is pointing towards, can you find it?
             */
            Vector3 torque = CalculateTorque();
            Vector3 velocity = CalculateThrowVelocity();
            float strength = CalculateStrength();
            Debug.Log("torque:" + torque);
            Debug.Log("torque positions:" + trackedRotations.Count);
            Debug.Log("velocity:" + velocity);
            Debug.Log("velocity positions:" + trackedPositions.Count);
            Debug.Log("strength:" + strength);
            Debug.Log("controller velocity:" + OVRInput.GetLocalControllerVelocity(Controller));
            Debug.Log("controller angular velocity:" + OVRInput.GetLocalControllerAngularVelocity(Controller));
            Rigidbody rb = currGrabbedObject.GetComponent<Rigidbody>();
            rb.AddForce(velocity * strength * throwStrength, ForceMode.Impulse);
            rb.constraints = RigidbodyConstraints.None;
            rb.AddTorque(torque * rotationStrength, ForceMode.Impulse);
            if (currGrabbedObject.CompareTag("Ball"))
            {
                currGrabbedObject.GetComponent<BallBehaviour>().curveDirection = Quaternion.AngleAxis(90, velocity) * torque * rotationStrength * curveStrength;
                Debug.Log("curveDirection:" + currGrabbedObject.GetComponent<BallBehaviour>().curveDirection);
                // rb.AddForce(currGrabbedObject.GetComponent<BallBehaviour>().curveDirection, ForceMode.Force);
            }
            // currGrabbedObject.GetComponent<Rigidbody>().velocity = OVRInput.GetLocalControllerVelocity(Controller);
            // currGrabbedObject.GetComponent<Rigidbody>().angularVelocity = OVRInput.GetLocalControllerAngularVelocity(Controller);
            if (OVRInput.GetLocalControllerVelocity(Controller).magnitude > throwThreshold)
            {
                SoundManager.Instance.PlayBallThrowingSound();
            }
            
            // Debug.Log("velocity:" + currGrabbedObject.GetComponent<Rigidbody>().velocity);
            // Debug.Log(currGrabbedObject.GetComponent<Rigidbody>().angularVelocity);

            currGrabbedObject = null;
            trackedPositions.Clear();
            trackedRotations.Clear();
        }
    }

    private Vector3 CalculateThrowVelocity()
    {
        Vector3 throwVelocity = Vector3.zero;
        int minCount = trackedPositions.Count - positionsBeforeRelease < 0 ? 0 : trackedPositions.Count - positionsBeforeRelease;
        for (int i = trackedPositions.Count - 1; i > minCount + 1; i--)
        {
            throwVelocity += trackedPositions[i] - trackedPositions[i - 1];
        }
        return throwVelocity;
    }

    private Vector3 CalculateTorque()
    {
        Vector3 torque = Vector3.zero;
        int minCount = trackedRotations.Count - rotationsBeforeRelease < 0 ? 0 : trackedRotations.Count - rotationsBeforeRelease;
        for (int i = trackedRotations.Count - 1; i > minCount + 1; i--)
        {
            Quaternion throwRotation = trackedRotations[i] * Quaternion.Inverse(trackedRotations[i - 1]);
            float angle;
            Vector3 axis;
            throwRotation.ToAngleAxis(out angle, out axis);
            if (angle > 180)
            {
                angle -= 360;
            }
            torque += angle * axis;
        }
        return torque;
    }

    private float CalculateStrength()
    {
        Vector3 strength = Vector3.zero;
        for (int i = 0; i < trackedPositions.Count - 1; i++)
        {
            strength += trackedPositions[i + 1] - trackedPositions[i];
        }
        return strength.magnitude;
    }
}
