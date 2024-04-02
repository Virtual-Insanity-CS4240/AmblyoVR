using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialGhost : MonoBehaviour
{
    public TutorialRoom roomReference;

    public BallColor ghostColor;

    public void GhostHit()
    {
        roomReference.GhostHit();
    }
}
