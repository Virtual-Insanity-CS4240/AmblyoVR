using UnityEngine;

public class CustomGrab : MonoBehaviour
{
    public OVRInput.Controller Controller;

    public string grabButtonName;
    public float grabRadius;
    [SerializeField] private Vector3 grabOffset;
    public LayerMask grabMask;
    [SerializeField] private string collectorTag;

    private GameObject currGrabbedObject;
    private bool isGrabbing;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
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
        //print(other.gameObject.name);
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

    // For debugging
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + grabOffset, grabRadius);
    }

    void GrabObject()
    {
        RaycastHit[] hits;

        hits = Physics.SphereCastAll(transform.position + grabOffset, grabRadius, transform.forward, 100.0f, grabMask);

        if (hits.Length > 0)
        {
            isGrabbing = true;

            int closestHit = 0;

            for (int i = 0; i < hits.Length; i++)
            {
                if ((hits[i]).distance < hits[closestHit].distance)
                {
                    closestHit = i;
                }
            }

            currGrabbedObject = hits[closestHit].transform.gameObject; // grab the closest object
            currGrabbedObject.GetComponent<Rigidbody>().isKinematic = true; // the grabbed object should not have gravity
            currGrabbedObject.layer = gameObject.layer;

            // grab object will follow our hands
            currGrabbedObject.transform.position = transform.position + grabOffset;
            currGrabbedObject.transform.parent = transform;
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
