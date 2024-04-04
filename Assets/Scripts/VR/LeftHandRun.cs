using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftHandRun : SimpleSingleton<LeftHandRun>
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Plane1")
        {
            HandRunManager.leftHandEntered?.Invoke(2);
        }
        else if (other.gameObject.name == "Plane2")
        {
            HandRunManager.leftHandEntered?.Invoke(3);
        }
    }
}
