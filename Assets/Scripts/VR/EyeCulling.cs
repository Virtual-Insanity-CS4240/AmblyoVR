using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeCulling : MonoBehaviour
{
    [SerializeField] private Camera leftEyeCamera;
    [SerializeField] private Camera rightEyeCamera;
    [SerializeField] private String ghostLayerName;
    [SerializeField] private String ballLayerName;
    private int ghostLayer;
    private int ballLayer;
    void Start()
    {
        ghostLayer = LayerMask.NameToLayer(ghostLayerName);
        ballLayer = LayerMask.NameToLayer(ballLayerName);
    }

    public void NoGhost(String eye)
    {
        if (eye == "Left")
        {
            leftEyeCamera.cullingMask &=  ~(1 << ghostLayer);
            rightEyeCamera.cullingMask |= 1 << ghostLayer;
        }
        else if (eye == "Right")
        {
            rightEyeCamera.cullingMask &=  ~(1 << ghostLayer);
            leftEyeCamera.cullingMask |= 1 << ghostLayer;
        }
        else
        {
            leftEyeCamera.cullingMask |= 1 << ghostLayer;
            rightEyeCamera.cullingMask |= 1 << ghostLayer;
        }
    }

    public void NoBall(String eye)
    {
        if (eye == "Left")
        {
            leftEyeCamera.cullingMask &=  ~(1 << ballLayer);
            rightEyeCamera.cullingMask |= 1 << ballLayer;
        }
        else if (eye == "Right")
        {
            rightEyeCamera.cullingMask &=  ~(1 << ballLayer);
            leftEyeCamera.cullingMask |= 1 << ballLayer;
        }
        else
        {
            leftEyeCamera.cullingMask |= 1 << ballLayer;
            rightEyeCamera.cullingMask |= 1 << ballLayer;
        }
    }
}
