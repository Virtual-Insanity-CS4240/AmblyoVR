using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialRoom : MonoBehaviour
{
    [SerializeField] private int totalGhosts;
    [SerializeField] private Transform bossSpawnPosition;
    [SerializeField] private GameObject bossPrefab;
    private int niceGhostCount = 0;
    public bool isFirstRoom;

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
                GameObject bossGhost = Instantiate(bossPrefab, bossSpawnPosition.position, bossSpawnPosition.rotation);
                bossGhost.transform.localScale = Vector3.one * 2;
                TutorialManager.UpdateTutorialFlag(6);
                SoundManager.Instance.PlayBossFightMusic();
            }
        }
    }

    public void BossDone()
    {
        SoundManager.Instance.PlayCasualMusic();
    }
}
