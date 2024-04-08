using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialGhost : MonoBehaviour
{
    public TutorialRoom roomReference;
    [SerializeField] private Material niceMaterial;
    [SerializeField] private GameObject ghostModel;

    public BallColor ghostColor;

    public void GhostHit()
    {
        if (roomReference != null)
            roomReference.GhostHit();
        ghostModel.GetComponent<SkinnedMeshRenderer>().material = niceMaterial;
        ghostColor = BallColor.White;
    }
}
