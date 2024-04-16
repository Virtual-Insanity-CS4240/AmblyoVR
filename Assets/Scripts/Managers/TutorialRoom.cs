using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialRoom : MonoBehaviour
{
    [SerializeField] private int totalGhosts;
    [SerializeField] private Transform bossSpawnPosition;
    [SerializeField] private GameObject bossPrefab;
    public List<GameObject> spawnedGhosts;
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
                StartCoroutine(BossSpawn());
            }
        }
    }

    public void BossDone()
    {
        SoundManager.Instance.PlayCasualMusic();
        TutorialManager.UpdateTutorialFlag(7);
    }

    IEnumerator BossSpawn()
    {
        SoundManager.Instance.PlayWarningSound();
        foreach (GameObject ghost in spawnedGhosts)
        {
            Destroy(ghost);
        }
        yield return new WaitForSeconds(2);
        SoundManager.Instance.StopMusic();
        GameObject bossGhost = Instantiate(bossPrefab, bossSpawnPosition.position, bossSpawnPosition.rotation);
        bossGhost.transform.localScale = Vector3.one * 1.5f;
        bossGhost.GetComponent<BossMovement>().tutRoomReference = this;
        TutorialManager.UpdateTutorialFlag(6);
        Debug.Log("Boss Spawned!");
        SoundManager.Instance.PlayBossFightMusic();
    }
}
