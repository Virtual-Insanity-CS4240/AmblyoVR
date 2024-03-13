using Delegates;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BallColor
{
    Red = 0, Green = 1, Blue = 2
}

public class PlayerInventory : SimpleSingleton<PlayerInventory>
{
    private BallColor equippedBallColor = BallColor.Red;
    private int ballCount = 0;

    public static BallColorReturnEvent GetEquippedBallColor;
    public static IntEvent ChangeBallCount;

    private void OnEnable()
    {
        GetEquippedBallColor += HandleGetEquippedBallColor;
        ChangeBallCount += HandleChangeBallCount;
    }

    private void OnDisable()
    {
        GetEquippedBallColor -= HandleGetEquippedBallColor;
        ChangeBallCount -= HandleChangeBallCount;
    }

    private BallColor? HandleGetEquippedBallColor()
    {
        if (ballCount <= 0)
            return null;
        return equippedBallColor;
    }

    private void HandleChangeBallCount(int i)
    {
        if (ballCount + i <= 0)
            Debug.LogWarning("Ball count now <= 0! Should not even be able to equip!");

        ballCount += i;
    }
}
