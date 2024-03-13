using System.Collections.Generic;
using UnityEngine;

public class CustomGrab : MonoBehaviour
{
    [SerializeField] private OVRInput.Controller Controller;

    [SerializeField] private string grabButtonName;
    [SerializeField] private LayerMask grabMask;
    private int grabbableLayer;
    [SerializeField] private GameObject attachAchor;

    private GameObject currGrabbedObject;
    private bool isGrabbing;
    private AudioSource audioSource;
    private List<GameObject> touchedObjects = new List<GameObject>();

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        grabbableLayer = (int) Mathf.Log(grabMask.value, 2);
    }
    // Update is called once per frame
    void Update()
    {
        // if you hold button
        if (!isGrabbing && Input.GetAxis(grabButtonName) == 1)
        {
            GrabObject();
        }

        // if you let go of button
        if (isGrabbing && Input.GetAxis(grabButtonName) < 1)
        {
            DropObject();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        print(other.gameObject.name);
        if (other.gameObject.layer == grabbableLayer && !isGrabbing)
        {
            OVRInput.SetControllerVibration(0.5f, 0.5f, Controller);
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
        GameObject closestObject = null;
        foreach (GameObject touchedObject in touchedObjects)
        {
            if (touchedObject.layer == grabbableLayer 
                && (closestObject == null || Vector3.Distance(transform.position, touchedObject.transform.position) < Vector3.Distance(transform.position, closestObject.transform.position)))
            {
                closestObject = touchedObject;
            }
        }

        if (closestObject)
        {
            isGrabbing = true;

            currGrabbedObject = closestObject; // grab the closest object
            currGrabbedObject.GetComponent<Rigidbody>().isKinematic = true; // the grabbed object should not have gravity
            grabbableLayer = currGrabbedObject.layer;
            currGrabbedObject.layer = gameObject.layer;

            // grab object will follow our hands
            currGrabbedObject.transform.SetParent(attachAchor.transform, true); // attach the grabbed object to our attachAnchor
        }
    }

    void DropObject()
    {
        isGrabbing = false;

        // we are currently grabbing something, let it go
        if (currGrabbedObject != null)
        {
            currGrabbedObject.transform.parent = null;
            currGrabbedObject.GetComponent<Rigidbody>().isKinematic = false; // enable gravity again for the object
            currGrabbedObject.layer = grabbableLayer;
            /**
             * Throw the object based on how hard we 'swing' our hand
             * 
             * HINT FOR ASSIGNMENT 2: 
             * if you would like to change to shooting instead of throwing, you can use rigidbody AddForce
             * AddForce() requires a direction vector (what direction the object should move towards), there is a way to get what direction this object is pointing towards, can you find it?
             */
            currGrabbedObject.GetComponent<Rigidbody>().velocity = OVRInput.GetLocalControllerVelocity(Controller);
            currGrabbedObject.GetComponent<Rigidbody>().angularVelocity = OVRInput.GetLocalControllerAngularVelocity(Controller);

            currGrabbedObject = null;
        }
    }
}
