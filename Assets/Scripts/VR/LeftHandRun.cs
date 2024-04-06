using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftHandRun : SimpleSingleton<LeftHandRun>
{
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Left Hand Entered");
        if (other.gameObject.name == "Plane1")
        {
            HandRunManager.leftHandEntered?.Invoke(4);
            Debug.Log("4");
        }
        else if (other.gameObject.name == "Plane2")
        {
            HandRunManager.leftHandEntered?.Invoke(5);
            Debug.Log("5");
        }
    }
}
