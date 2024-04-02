using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialRoom : MonoBehaviour
{
    [SerializeField] private int totalGhosts;
    private int niceGhostCount = 0;

    public void GhostHit()
    {
        niceGhostCount++;
        if (niceGhostCount == totalGhosts)
        {
            Debug.Log("All ghosts have been hit!");
            // Instantiate Boss in Room if have
        }
    }
}
