using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PouchManager : MonoBehaviour
{
    [SerializeField] private float handPouchPositionOffset = 0.05f;
    [SerializeField] private GameObject ball1;
    [SerializeField] private GameObject ball2;
    public void SetPosition(Vector3 leftHand, Vector3 rightHand)
    {
        float lowPointY = Mathf.Min(leftHand.y, rightHand.y);
        transform.position = new Vector3(transform.position.x, lowPointY - handPouchPositionOffset, transform.position.z);
    }
    public void SetScale(Vector3 leftHand, Vector3 rightHand)
    {
        ball1.transform.parent = null;
        ball2.transform.parent = null;
        float distance = Math.Abs(leftHand.x - rightHand.x);
        Debug.Log("Distance: " + distance);
        transform.localScale = new Vector3(distance * 2, transform.localScale.y, transform.localScale.z);
        ball1.transform.parent = transform;
        ball2.transform.parent = transform;
    }
}
