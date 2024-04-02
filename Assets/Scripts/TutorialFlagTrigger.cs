using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialFlagTrigger : MonoBehaviour
{
    [SerializeField] private int neededFlagValue;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && TutorialManager.Instance.tutorialStep == neededFlagValue)
        {
            Debug.Log("CUM");
            TutorialManager.UpdateTutorialFlag(neededFlagValue + 1);
        }
    }
}
