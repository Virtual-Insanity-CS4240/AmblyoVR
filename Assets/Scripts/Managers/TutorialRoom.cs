using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialRoom : MonoBehaviour
{
    [SerializeField] private int totalGhosts;
    private int niceGhostCount = 0;
    public static bool isFirstRoom;

    public void GhostHit()
    {
        niceGhostCount++;
        Debug.Log("Nice Ghosts: " + niceGhostCount);
        if (niceGhostCount == totalGhosts)
        {
            Debug.Log("All Ghosts Hit!");
            if (isFirstRoom)
            {
                Debug.Log("First Room Cleared!");
                TutorialManager.UpdateTutorialFlag(4);
            }
            else
            {
                // Instantiate Boss in Room

            }
        }
    }
}
