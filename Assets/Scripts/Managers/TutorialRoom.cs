using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialRoom : MonoBehaviour
{
    [SerializeField] private int totalGhosts;
    private int niceGhostCount = 0;
    [SerializeField] private bool isFirstRoom;

    public void GhostHit()
    {
        niceGhostCount++;
        if (niceGhostCount == totalGhosts)
        {
            if (isFirstRoom)
            {
                TutorialManager.UpdateTutorialFlag(4);
            }
            else
            {
                // Instantiate Boss in Room

            }
        }
    }
}
