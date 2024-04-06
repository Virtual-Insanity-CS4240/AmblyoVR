using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialFlagTrigger : MonoBehaviour
{
    [SerializeField] private int neededFlagValue;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered " + neededFlagValue);
        if (other.CompareTag("Player") && TutorialManager.Instance.tutorialStep == neededFlagValue)
        {
            Debug.Log("Tutorial Flag Triggered");
            TutorialManager.UpdateTutorialFlag(neededFlagValue + 1);
        }
    }
}
