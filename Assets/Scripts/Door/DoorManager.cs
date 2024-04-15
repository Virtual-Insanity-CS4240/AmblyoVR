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
    [SerializeField] private DoorManager connectedDoor;
    private List<string> animations = new List<string>();
    private enum DoorAnimations { DoorOpen = 0, DoorClose = 1, DoorOpenOther = 2, DoorCloseOther = 3 }
    private bool doorOpen = false;
    private bool isEnteredOtherWay = false;

    // Start is called before the first frame update
    void Start()
    {
        animator.SetInteger("State", -1);
    }

    public void HandleDoorTriggered(bool isEntered)
    {
        if (isEntered && !doorOpen && !isLocked && !(oneWayDoor && isOtherWay))
        {
            if (!oneWayDoor)
                isOtherWay = false;
                isEnteredOtherWay = false;
            OpenDoor();
        }
        else if (!isEntered && doorOpen && ((!oneWayDoor && isEnteredOtherWay) || isOtherWay))
        {
            if (!oneWayDoor)
                isOtherWay = false;
            CloseDoor();
        }
    }
    
    public void HandleDoorTriggeredOther(bool isEntered)
    {
        if (isEntered && !doorOpen && !isLocked && !(oneWayDoor && !isOtherWay))
        {
            if (!oneWayDoor)
                isOtherWay = true;
                isEnteredOtherWay = true;
            OpenDoor();
        }
        else if (!isEntered && doorOpen && ((!oneWayDoor && !isEnteredOtherWay) || !isOtherWay))
        {
            if (!oneWayDoor)
                isOtherWay = true;
            CloseDoor();
        }
    }

    public void OpenDoor()
    {
        if (!doorOpen)
        {
            animator.SetInteger("State", isOtherWay ? (int)DoorAnimations.DoorOpenOther : (int)DoorAnimations.DoorOpen);
            doorOpen = true;
            SoundManager.Instance.PlayDoorOpeningSound();
            if (connectedDoor != null)
            {
                connectedDoor.OpenDoor();
            }
        }
    }

    public void CloseDoor()
    {
        if (doorOpen)
        {
            animator.SetInteger("State", isOtherWay ? (int)DoorAnimations.DoorCloseOther : (int)DoorAnimations.DoorClose);
            doorOpen = false;
            SoundManager.Instance.PlayDoorOpeningSound();
            if (connectedDoor != null)
            {
                connectedDoor.CloseDoor();
            }
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
