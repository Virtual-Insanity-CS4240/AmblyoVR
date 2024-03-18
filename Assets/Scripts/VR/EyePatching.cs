using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyePatching : MonoBehaviour
{
    [SerializeField] private GameObject leftOccluder;
    [SerializeField] private GameObject rightOccluder;
    private Color leftColor;
    private Color rightColor;
    private Renderer leftRenderer;
    private Renderer rightRenderer;

    // Start is called before the first frame update
    void Start()
    {
        leftRenderer = leftOccluder.GetComponent<Renderer>();
        rightRenderer = rightOccluder.GetComponent<Renderer>();
        leftColor = leftRenderer.material.color;
        rightColor = rightRenderer.material.color;
    }

    public void LeftOcclusion(float amount)
    {
        leftRenderer.material.color = new Color(leftColor.r, leftColor.g, leftColor.b, amount);
    }

    public void RightOcclusion(float amount)
    {
        rightRenderer.material.color = new Color(rightColor.r, rightColor.g, rightColor.b, amount);
    }
}
