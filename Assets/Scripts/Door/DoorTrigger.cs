using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] private bool isOtherWay = false;
    [SerializeField] private GameObject doorManager;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (isOtherWay)
            {
                doorManager.GetComponent<DoorManager>().HandleDoorTriggeredOther(true);
                Debug.Log("Triggered other way");
            }
            else
            {
                doorManager.GetComponent<DoorManager>().HandleDoorTriggered(true);
                Debug.Log("Triggered");
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (isOtherWay)
            {
                doorManager.GetComponent<DoorManager>().HandleDoorTriggeredOther(false);
                Debug.Log("Exited other way");
            }
            else
            {
                doorManager.GetComponent<DoorManager>().HandleDoorTriggered(false);
                Debug.Log("Exited");
            }
        }
    }
}
