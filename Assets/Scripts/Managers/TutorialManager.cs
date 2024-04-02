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

    private bool inFirstShootingStage = false;

    [Header("Second Shooting Range")]
    [SerializeField] private TutorialRoom room2;

    private bool inSecondShootingStage = false;

    public static IntEvent UpdateTutorialFlag;    
    public static VoidEvent EndTutorial;

    [Header("Billboards")]
    public MeshRenderer screen1;
    public MeshRenderer screen2;

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
        if (inFirstShootingStage)
        {
            // TODO: Hook up events to ghosts dying to go to flag 4
        }

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
            default:
                Debug.LogWarning("Weird flag detected: " + tutorialStep);
                break;
        }
    }    
    
    // Start: Enter room
    // Next: End of Dialogue
    private IEnumerator Flag0Event()
    {
        // TODO: Show the string on screen
        string message = "Welcome to Nice Ghosts Inc.! As our newest recruit, we’ll show you how to make ghosts nice through this interactive tutorial.";
        yield return new WaitForSeconds(1);
        HandleUpdateTutorialFlag(1);
    }

    // Start: <>
    // Next: Pick up Nice Balls
    private IEnumerator Flag1Event()
    {
        string message = "First, you’ll see that there’s a box in front of you on the right of the table. Try using <CONTROL HERE> to pick up some balls!";
        yield return new WaitForSeconds(1);
    }

    // Start: <>
    // Next: <>
    private IEnumerator Flag2Event()
    {
        string message = "Great! Notice how you only pick up 10 Nice Balls at a time and you only can have a maximum of 20 in your inventory!";
        yield return new WaitForSeconds(1);
        HandleUpdateTutorialFlag(3);
    }

    // Start: <>
    // Next: Kill all Ghosts
    private IEnumerator Flag3Event()
    {
        string message1 = "Now let’s move on to the fun part: Nice-ifying ghosts!";
        yield return new WaitForSeconds(1);

        string message2 = "In your arsenal, there are 3 different types of Nice Balls: Red, Green, Purple and Yellow. You can switch between them using the Right Joystick!";
        yield return new WaitForSeconds(1);

        string message3 = "Try nice-ifying these ghosts! Use the same colour of the nice ball on the ghost of the same type! Remember, you can switch between the balls using the Right Joystick and can pick up more Nice Balls anytime using <CONTROL HERE>!";
        // Spawn the four ghosts
        GameObject ghost1 = Instantiate(redGhost, firstRoomGhostSpawnTransforms[0].position, Quaternion.identity);
        GameObject ghost2 = Instantiate(greenGhost, firstRoomGhostSpawnTransforms[1].position, Quaternion.identity);
        GameObject ghost3 = Instantiate(yellowGhost, firstRoomGhostSpawnTransforms[2].position, Quaternion.identity);
        GameObject ghost4 = Instantiate(purpleGhost, firstRoomGhostSpawnTransforms[2].position, Quaternion.identity);

        ghost1.GetComponent<TutorialGhost>().roomReference = room1;
        ghost2.GetComponent<TutorialGhost>().roomReference = room1;
        ghost3.GetComponent<TutorialGhost>().roomReference = room1;
        ghost4.GetComponent<TutorialGhost>().roomReference = room1;
    }

    // Start: <>
    // Next: Walk to second shooting range
    private IEnumerator Flag4Event()
    {
        string message = "Good job! Now it’s time to take it up a notch. Move to the next range please!";
        yield return new WaitForSeconds(1);
        //TODO: Open door to second range
        inFirstShootingStage = false;
    }

    // Start: <>
    // Next: Kill all the ghosts
    private IEnumerator Flag5Event()
    {
        inSecondShootingStage = true;
        string message = "Alright…now let’s see how you handle it…when the ghosts are moving around! Remember, you can switch between the balls using the Right Joystick and can pick up more Nice Balls anytime using <CONTROL HERE>!";
        yield return new WaitForSeconds(1);
        // TODO: Spawn 3 ghosts
    }

    private IEnumerator Flag6Event()
    {
        // TODO: Change to spooky room lighting
        yield return new WaitForSeconds(1);
        string message1 = "Boo! Did we scare you? Sometimes, rooms might have bosses that appear if you nice - ify all ghosts in a room. They can also fight back: if you get hit, you’ll lose some Nice Balls!";
        yield return new WaitForSeconds(1);
        string message2 = "But don’t be scared: it works just like any other ghost, just needs more Nice Balls to nice-ify it! Go ahead! Nice-ify this ghoul!";
    }

    private IEnumerator Flag7Event()
    {
        inSecondShootingStage = false;
        string message1 = "Phew, that must’ve stressed you out! Don’t worry, the real thing is just like this training. You’ll do great!";
        yield return new WaitForSeconds(1);
        // TODO: Door opens
        string message2 = "Head on back to the starting room to start nice-ifying ghosts for real, or do the tutorial again!";
    }

    private void HandleBallCountIncreased(int i)
    {
        if (i > 0 && tutorialStep == 1)
            HandleUpdateTutorialFlag(2);
    }

    private void HandleEndTutorial()
    {
        tutorialStep = -1;
        // TODO: Shut door
    }    
}
