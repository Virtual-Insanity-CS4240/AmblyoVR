using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftHandRun : SimpleSingleton<LeftHandRun>
{
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Left Hand Entered");
        if (other.gameObject.name == "Plane0")
        {
            HandRunManager.leftHandEntered?.Invoke(5);
            Debug.Log("5");
        }
        else if (other.gameObject.name == "Plane1")
        {
            HandRunManager.leftHandEntered?.Invoke(6);
            Debug.Log("6");
        }
        else if (other.gameObject.name == "Plane2")
        {
            HandRunManager.leftHandEntered?.Invoke(7);
            Debug.Log("7");
        }
        else if (other.gameObject.name == "Plane3")
        {
            HandRunManager.leftHandEntered?.Invoke(8);
            Debug.Log("8");
        }
    }
}
