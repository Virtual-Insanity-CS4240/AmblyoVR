using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTriggerArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            NPC.StartDialogue();
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            NPC.EndDialogue();
        }
    }
}
