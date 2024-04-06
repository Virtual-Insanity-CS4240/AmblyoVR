using System.Collections;
using System.Collections.Generic;
using Delegates;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    [SerializeField] private bool oneWayDoor = true;
    [SerializeField] private bool isOtherWay = false;
    [SerializeField] private Animator animator;
    [SerializeField] private bool isLocked = false;
    private List<string> animations = new List<string>();
    private enum DoorAnimations { DoorOpen = 0, DoorClose = 1, DoorOpenOther = 2, DoorCloseOther = 3 }
    private bool doorOpen = false;

    // Start is called before the first frame update
    void Start()
    {
        animator.SetInteger("State", -1);
    }

    public void HandleDoorTriggered(bool isEntered)
    {
        if (isEntered && !doorOpen && !isLocked && !(oneWayDoor && isOtherWay))
        {
            animator.SetInteger("State", (int)DoorAnimations.DoorOpen);
            doorOpen = true;
        }
        else if (!isEntered && doorOpen && (!oneWayDoor || isOtherWay))
        {
            animator.SetInteger("State", (int)DoorAnimations.DoorCloseOther);
            doorOpen = false;
        }
    }
    
    public void HandleDoorTriggeredOther(bool isEntered)
    {
        if (isEntered && !doorOpen && !isLocked && !(oneWayDoor && !isOtherWay))
        {
            animator.SetInteger("State", (int)DoorAnimations.DoorOpenOther);
            doorOpen = true;
        }
        else if (!isEntered && doorOpen && (!oneWayDoor || !isOtherWay))
        {
            animator.SetInteger("State", (int)DoorAnimations.DoorClose);
            doorOpen = false;
        }
    }

    public void OpenDoor()
    {
        if (!doorOpen)
        {
            animator.SetInteger("State", isOtherWay ? (int)DoorAnimations.DoorOpenOther : (int)DoorAnimations.DoorOpen);
            doorOpen = true;
        }
    }

    public void CloseDoor()
    {
        if (doorOpen)
        {
            animator.SetInteger("State", isOtherWay ? (int)DoorAnimations.DoorCloseOther : (int)DoorAnimations.DoorClose);
            doorOpen = false;
        }
    }

    public void LockDoor()
    {
        isLocked = true;
    }

    public void UnlockDoor()
    {
        isLocked = false;
    }
}
