using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Delegates;
using UnityEngine.InputSystem;

public class HandRunManager : SimpleSingleton<HandRunManager>
{
    public static IntEvent leftHandEntered;
    public static IntEvent rightHandEntered;
    [SerializeField] private GameObject leftHand;
    [SerializeField] private GameObject rightHand;
    [SerializeField] private string movementYAxisName;
    [SerializeField] private float timeoutThreshold = 0.3f;
    [SerializeField] private float speed = 1f;
    [SerializeField] private float stoppingSpeed = 2f;
    private Vector3 movement = Vector3.zero;
    private CharacterController characterController;
    private GameObject player;
    private float leftTimer;
    private float rightTimer;
    private int directionForward = 1;
    private int leftHandValue = 0;
    private int rightHandValue = 0;
    private int tempLeftHandValue = 0;
    private int tempRightHandValue = 0;

    private void OnEnable()
    {
        leftHandEntered += HandleLeftHandEntered;
        rightHandEntered += HandleRightHandEntered;
    }
    private void OnDisable()
    {
        leftHandEntered -= HandleLeftHandEntered;
        rightHandEntered -= HandleRightHandEntered;
    }
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        characterController = player.GetComponent<CharacterController>();
    }
    // Update is called once per frame
    void Update()
    {
        // Debug.Log(leftHandValue + " " + rightHandValue);
        // Debug.Log("move:" + movement);
        if (Input.GetAxis(movementYAxisName) > 0)
        {
            directionForward = -1;
        }
        else
        {
            directionForward = 1;
        }
        leftTimer += Time.deltaTime;
        rightTimer += Time.deltaTime;
        if (leftTimer > timeoutThreshold)
        {
            // Debug.Log("Left Timeout");
            leftHandValue = 0;
        }
        if (rightTimer > timeoutThreshold)
        {
            // Debug.Log("Right Timeout");
            rightHandValue = 0;
        }
    }

    void FixedUpdate()
    {
        if (!characterController.isGrounded)
        {
            movement.y += Physics.gravity.y * Time.fixedDeltaTime;
        }
        characterController.Move(movement * Time.fixedDeltaTime);
        movement = Vector3.Lerp(movement, Vector3.zero, Time.fixedDeltaTime * stoppingSpeed);
    }
    private void HandleLeftHandEntered(int value)
    {
        if (tempLeftHandValue == 0)
        {
            tempLeftHandValue = value;
        }
        else
        {
            leftHandValue = value - tempLeftHandValue;
            if (leftHandValue != 0)
            {
                leftTimer = 0;
            }
            if (((rightHandValue < 0 && leftHandValue > 0) || (rightHandValue > 0 && leftHandValue < 0)) && rightHandValue != 0)
            {
                // leftHandValue = 0;
                // rightHandValue = 0;
                Move();
            }
            tempLeftHandValue = value;
        }
        // Debug.Log("LEFT: leftHandValue:" + leftHandValue + ", rightHandValue:" + rightHandValue);
    }
    private void HandleRightHandEntered(int value)
    {
        if (tempRightHandValue == 0)
        {
            tempRightHandValue = value;
        }
        else
        {
            rightHandValue = value - tempRightHandValue;
            if (rightHandValue != 0)
            {
                rightTimer = 0;
            }
            if (((rightHandValue < 0 && leftHandValue > 0) || (rightHandValue > 0 && leftHandValue < 0)) && leftHandValue != 0)
            {
                // leftHandValue = 0;
                // rightHandValue = 0;
                Move();
            }
            tempRightHandValue = value;
        }
        // Debug.Log("RIGHT: leftHandValue:" + leftHandValue + ", rightHandValue:" + rightHandValue);
    }

    private void Move()
    {
        StartCoroutine(VRControllerUtility.VibrateController(0.1f, 0.5f, 0.5f, OVRInput.Controller.All));
        SoundManager.Instance.PlaySteppingSound();
        Vector3 direction = Camera.main.transform.forward.normalized * speed * directionForward * (OVRInput.GetLocalControllerVelocity(OVRInput.Controller.LTouch).magnitude + OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RTouch).magnitude) / 2;
        movement = movement + new Vector3(direction.x, 0, direction.z);
        // Debug.Log("movement:" + movement);
    }
}
