using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightHandRun : SimpleSingleton<RightHandRun>
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Plane1")
        {
            HandRunManager.rightHandEntered?.Invoke(2);
        }
        else if (other.gameObject.name == "Plane2")
        {
            HandRunManager.rightHandEntered?.Invoke(3);
        }
    }
}