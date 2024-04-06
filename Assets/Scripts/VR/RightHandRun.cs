using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightHandRun : SimpleSingleton<RightHandRun>
{
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Right Hand Entered");
        if (other.gameObject.name == "Plane1")
        {
            HandRunManager.rightHandEntered?.Invoke(2);
            Debug.Log("2");
        }
        else if (other.gameObject.name == "Plane2")
        {
            HandRunManager.rightHandEntered?.Invoke(3);
            Debug.Log("3");
        }
    }
}