using Delegates;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : SimpleSingleton<PlayerManager>
{
    private Queue<Ball> ballQueue = new Queue<Ball>();

    public static BallEvent AddBallToQueue;
    public static BallReturnEvent GetBallFromQueue;

    private void OnEnable()
    {
        AddBallToQueue += HandleAddBallToQueue;
        GetBallFromQueue += HandleGetBallFromQueue;
    }

    private void OnDisable()
    {
        AddBallToQueue -= HandleAddBallToQueue;
        GetBallFromQueue -= HandleGetBallFromQueue;
    }

    private void HandleAddBallToQueue(Ball ball)
    {
        ballQueue.Enqueue(ball);

        // TODO: Update UI
    }

    private Ball HandleGetBallFromQueue()
    {
        // TODO: Link up UI

        if (ballQueue.Count > 0)
        {
            return ballQueue.Dequeue();
        }
        else 
        {
            return null;
        }
    }
}
