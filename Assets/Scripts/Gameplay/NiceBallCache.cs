using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NiceBallCache : MonoBehaviour
{
    public int restockValue;

    public void Restock()
    {
        if (PlayerInventory.IsReady)
            PlayerInventory.ChangeBallCount(restockValue);
    }
}
