using Delegates;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{    
    private int totalGhosts;
    [SerializeField] private bool isBossRoom;
    [SerializeField] private Transform bossSpawnPosition;
    [SerializeField] private GameObject bossPrefab;
    [SerializeField] private GameObject[] spawnedGhosts;
    private int niceGhostCount = 0;

    public bool roomActive = true;

    void Start()
    {
        totalGhosts = spawnedGhosts.Length;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && roomActive)
        {
            SoundManager.Instance.PlayBattleMusic();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && roomActive)
        {
            SoundManager.Instance.PlayCasualMusic();
        }
    }

    public void GhostHit() 
    {
        niceGhostCount++;
        if (niceGhostCount == totalGhosts)
        {            
            if (isBossRoom)
            {
                // Instantiate Boss in Room if have, turn light spookier
                StartCoroutine(BossSpawn());
            }
            else
            {
                // TODO: Remove spooky light from the room
                SoundManager.Instance.PlayCasualMusic();
                roomActive = false;
                EndGameManager.AddRoomComplete?.Invoke();
            }
        }
    }

    public void BossDied()
    {
        // TODO: Remove spooky light from the room
        SoundManager.Instance.PlayCasualMusic();
        roomActive = false;
        EndGameManager.AddRoomComplete?.Invoke();
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
        bossGhost.GetComponent<BossMovement>().roomReference = this;
        Debug.Log("Boss Spawned!");
        SoundManager.Instance.PlayBossFightMusic();
    }
}
