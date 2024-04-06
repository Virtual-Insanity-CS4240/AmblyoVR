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
    private Color[] colors = { Color.red, Color.green, Color.magenta, Color.yellow };
    private bool isJoystickReleased = true;

    private GameObject currGrabbedObject;
    private bool isGrabbing;
    private List<GameObject> touchedObjects = new List<GameObject>();

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

    void OnTriggerEnter(Collider other)
    {
        if (grabbableLayer.Contains(other.gameObject.layer) && !isGrabbing)
        {
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
                PlayerInventory.ChangeBallCount(10);
            }
            if (closestObject.CompareTag("Pouch"))
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
                
            // if (closestObject.transform.parent != null && closestObject.transform.parent.gameObject.CompareTag("Hand"))
            // {
            //     otherHand.GetComponent<CustomGrab>().DropObject();
            // }
            // // normal grab
            if (haveObject)
            {
                currGrabbedObject = closestObject; // grab the closest object
                currGrabbedObject.GetComponent<Rigidbody>().isKinematic = true; // the grabbed object should not have gravity

                // grab object will follow our hands
                currGrabbedObject.transform.SetParent(attachAchor.transform, true); // attach the grabbed object to our attachAnchor
                currGrabbedObject.transform.localPosition = Vector3.zero;
            }
            Debug.Log("grabcomplete");
        }
    }

    void DropObject()
    {
        isGrabbing = false;
        
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
            currGrabbedObject.GetComponent<Rigidbody>().velocity = OVRInput.GetLocalControllerVelocity(Controller);
            currGrabbedObject.GetComponent<Rigidbody>().angularVelocity = OVRInput.GetLocalControllerAngularVelocity(Controller);
            // Debug.Log("velocity:" + currGrabbedObject.GetComponent<Rigidbody>().velocity);
            // Debug.Log(currGrabbedObject.GetComponent<Rigidbody>().angularVelocity);

            currGrabbedObject = null;
        }
    }
}
