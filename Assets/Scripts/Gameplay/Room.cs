using Delegates;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    private int niceGhostCount = 0;
    public int totalGhosts = 3;

    public void GhostHit() 
    {
        niceGhostCount++;
        if (niceGhostCount == totalGhosts) {
            Debug.Log("All ghosts have been hit!");
            // Instantiate Boss in Room
        }
    }
}
