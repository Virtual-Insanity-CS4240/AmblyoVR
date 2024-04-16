using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyManager : MonoBehaviour
{
    [SerializeField] private float handPlaneSizeOffset = 0.5f;
    [SerializeField] private GameObject planes;
    [SerializeField] private GameObject offsetGameObject;
    [SerializeField] private GameObject cameraObject;
    private CharacterController characterController;
    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        characterController = player.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // offsetGameObject.transform.localPosition = characterController.center + offset;
        transform.localPosition = characterController.center;
        // Debug.Log("euler: " + cameraObject.transform.localEulerAngles.y);
        // Debug.Log("Local Rotation: " + cameraObject.transform.localRotation.y);
    }

    public void SetOffsetFromCenter(Vector3 midPoint)
    {
        transform.localRotation = Quaternion.Euler(0, cameraObject.transform.localEulerAngles.y, 0);
        offsetGameObject.transform.position = midPoint;
    }

    public void SetScale(Vector3 leftHand, Vector3 rightHand)
    {
        float distance = Math.Abs(leftHand.x - rightHand.x);
        Debug.Log("Distance: " + distance);
        planes.transform.localScale = new Vector3(distance * 2 + handPlaneSizeOffset, transform.localScale.y, transform.localScale.z);
    }
}
