using Delegates;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BallColor
{
    Red = 0, Green = 1, Purple = 2, Yellow = 3
}

public class PlayerInventory : SimpleSingleton<PlayerInventory>
{
    public static BallColor equippedBallColor = BallColor.Red;
    public static int ballCount = 0;

    public static BallColorReturnEvent EquipBall;
    public static BallColorEvent BallColorChange;
    public static IntEvent ChangeBallCount;

    private void OnEnable()
    {
        EquipBall += HandleEquipBall;
        ChangeBallCount += HandleChangeBallCount;
        BallColorChange += HandleBallColorChange;
    }

    private void OnDisable()
    {
        EquipBall -= HandleEquipBall;
        ChangeBallCount -= HandleChangeBallCount;
        BallColorChange -= HandleBallColorChange;
    }

    private BallColor? HandleEquipBall()
    {
        if (ballCount <= 0)
            return null;
        else
        {
            ballCount--;
            return equippedBallColor;
        }
    }

    private void HandleChangeBallCount(int i)
    {
        if (ballCount + i <= 0)
            Debug.LogWarning("Ball count now <= 0! Should not even be able to equip!");

        ballCount += i;
    }

    private void HandleBallColorChange(BallColor color)
    {
        equippedBallColor = color;
    }
}
