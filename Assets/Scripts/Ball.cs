using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ball
{
    public float catchModifier;

    // TODO: Particle effects or VFX when throwing / holding the ball
}

public class NiceBall : Ball
{
    public NiceBall()
    {
        catchModifier = 1;
    }
}
