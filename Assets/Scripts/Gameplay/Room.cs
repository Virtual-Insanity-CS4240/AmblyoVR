using Delegates;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{    
    [SerializeField] private int totalGhosts;
    [SerializeField] private bool isBossRoom;
    [SerializeField] private Transform bossSpawnPosition;
    [SerializeField] private GameObject bossPrefab;
    private int niceGhostCount = 0;

    private bool roomActive = true;

    public void GhostHit() 
    {
        niceGhostCount++;
        if (niceGhostCount == totalGhosts)
        {            
            if (isBossRoom)
            {
                // Instantiate Boss in Room if have, turn light spookier
                SoundManager.Instance.PlayWarningSound();
                SoundManager.Instance.PlayBossFightMusic();
            }
            else
            {
                // TODO: Remove spooky light from the room
            }
        }
    }

    public void BossDied()
    {
        // TODO: Remove spooky light from the room
        SoundManager.Instance.PlayCasualMusic();
    }
}
