using Delegates;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/*
 * Tutorial Steps:
 * -1 = Havent begun tutorial
 * 0 = Entered the tutorial room
 * 1 = Pick up nice balls
 * 2 = Intermmediate Dialogue
 * 3 = Hit all the ghosts in the first shooting range
 * 4 = Enter second shooting range
 * 5 = Shoot moving ghosts
 * 6 = Boss is in
 * 7 = Boss died
 */

public class TutorialManager : SimpleSingleton<TutorialManager>
{
    public int tutorialStep = -1;

    [Header("First Shooting Range")]
    [SerializeField] private GameObject redGhost;
    [SerializeField] private GameObject greenGhost;
    [SerializeField] private GameObject yellowGhost;
    [SerializeField] private GameObject purpleGhost;
    [SerializeField] private TutorialRoom room1;

    [SerializeField] private Transform[] firstRoomGhostSpawnTransforms;

    [Header("Second Shooting Range")]
    [SerializeField] private TutorialRoom room2;

    private bool inSecondShootingStage = false;

    public static IntEvent UpdateTutorialFlag;    
    public static VoidEvent EndTutorial;

    [Header("Billboards")]
    public MeshRenderer screen1;
    public MeshRenderer screen2;

    [Header("Materials")]
    public Material[] billboardMaterials;

    private void OnEnable()
    {        
        UpdateTutorialFlag += HandleUpdateTutorialFlag;
        PlayerInventory.ChangeBallCount += HandleBallCountIncreased;
        EndTutorial += HandleEndTutorial;
        /* TODO:
         * Link the following actions:
         * Go To Flag 4: Kill all ghosts
         * Go To Flag 5: Move to second range
         */

    }

    private void OnDisable()
    {
        UpdateTutorialFlag -= HandleUpdateTutorialFlag;
        PlayerInventory.ChangeBallCount -= HandleBallCountIncreased;
        EndTutorial -= HandleEndTutorial;
    }

    private void Update()
    {
        if (inSecondShootingStage)
        {
            // TODO: Check if ghosts died to go to flag 6
            

            // TODO: Check if boss died to go to flag 7
        }
    }

    private void HandleUpdateTutorialFlag(int i)
    {
        if (i < -1 || i > 7)
            Debug.LogError("Invalid step value: " + i);

        tutorialStep = i;
        TriggerStuff();
    }

    private void TriggerStuff()
    {
        switch (tutorialStep)
        {
            case 0:
                StartCoroutine(Flag0Event());
                break;
            case 1:
                StartCoroutine(Flag1Event());
                break;
            case 2:
                StartCoroutine(Flag2Event());
                break;
            case 3:
                StartCoroutine(Flag3Event());
                break;
            case 4:
                StartCoroutine(Flag3Event());
                break;
            case 5:
                StartCoroutine(Flag3Event());
                break;
            case 6:
                StartCoroutine(Flag3Event());
                break;
            case 7:
                StartCoroutine(Flag3Event());
                break;
            default:
                Debug.LogWarning("Weird flag detected: " + tutorialStep);
                break;
        }
    }    
    
    // Start: Enter room
    // Next: End of Dialogue
    private IEnumerator Flag0Event()
    {
        screen1.material = billboardMaterials[0];
        yield return new WaitForSeconds(3);
        HandleUpdateTutorialFlag(1);
    }

    // Start: <>
    // Next: Pick up Nice Balls
    private IEnumerator Flag1Event()
    {
        yield return new WaitForSeconds(0.1f);
        screen1.material = billboardMaterials[1];
    }

    // Start: <>
    // Next: <>
    private IEnumerator Flag2Event()
    {
        screen1.material = billboardMaterials[2];
        yield return new WaitForSeconds(2);
        HandleUpdateTutorialFlag(3);
    }

    // Start: <>
    // Next: Kill all Ghosts
    private IEnumerator Flag3Event()
    {
        screen1.material = billboardMaterials[3];
        yield return new WaitForSeconds(1.5f);

        screen1.material = billboardMaterials[4];
        yield return new WaitForSeconds(2f);

        screen1.material = billboardMaterials[5];
        // Spawn the four ghosts
        GameObject ghost1 = Instantiate(redGhost, firstRoomGhostSpawnTransforms[0].position, Quaternion.identity);
        GameObject ghost2 = Instantiate(greenGhost, firstRoomGhostSpawnTransforms[1].position, Quaternion.identity);
        GameObject ghost3 = Instantiate(yellowGhost, firstRoomGhostSpawnTransforms[2].position, Quaternion.identity);
        GameObject ghost4 = Instantiate(purpleGhost, firstRoomGhostSpawnTransforms[3].position, Quaternion.identity);

        ghost1.GetComponent<TutorialGhost>().roomReference = room1;
        ghost2.GetComponent<TutorialGhost>().roomReference = room1;
        ghost3.GetComponent<TutorialGhost>().roomReference = room1;
        ghost4.GetComponent<TutorialGhost>().roomReference = room1;
    }

    // Start: <>
    // Next: Walk to second shooting range
    private IEnumerator Flag4Event()
    {
        screen1.material = billboardMaterials[6];
        yield return new WaitForSeconds(1);
        //TODO: Open door to second range
    }

    // Start: <>
    // Next: Kill all the ghosts
    private IEnumerator Flag5Event()
    {
        screen1.material = billboardMaterials[0]; // Reset billboard
        inSecondShootingStage = true;
        screen2.material = billboardMaterials[7];
        yield return new WaitForSeconds(1);
        // TODO: Spawn 3 ghosts
    }

    private IEnumerator Flag6Event()
    {
        // TODO: Change to spooky room lighting
        yield return new WaitForSeconds(1);
        screen2.material = billboardMaterials[8];
        yield return new WaitForSeconds(3);
        screen2.material = billboardMaterials[9];
    }

    private IEnumerator Flag7Event()
    {
        inSecondShootingStage = false;
        screen2.material = billboardMaterials[10];
        yield return new WaitForSeconds(2);
        // TODO: Door opens
        screen2.material = billboardMaterials[11];
    }

    private void HandleBallCountIncreased(int i)
    {
        if (i > 0 && tutorialStep == 1)
            HandleUpdateTutorialFlag(2);
    }

    private void HandleEndTutorial()
    {
        tutorialStep = -1;
        screen2.material = billboardMaterials[7]; // Reset billboard 2
        // TODO: Shut door
    }    
}
