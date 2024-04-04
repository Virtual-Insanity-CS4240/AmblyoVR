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
    [SerializeField] private InputActionProperty movementYAxis;
    [SerializeField] private float timeoutThreshold = 0.3f;
    [SerializeField] private float timeoutMove = 0.8f;
    [SerializeField] private float speed = 1f;
    private Vector3 movement = Vector3.zero;
    private IEnumerator coroutine;
    private CharacterController characterController;
    private GameObject player;
    private bool isTimeout = false;
    private bool isMove = false;
    private int leftHandValue = 0;
    private int rightHandValue = 0;
    private Vector3 direction;

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
        if (movement != Vector3.zero && Input.GetAxis(movementYAxisName) != 0)
        {
            Vector3 forward = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z);
            direction = forward * Input.GetAxis(movementYAxisName) * -1;
        }
        if (isMove)
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
            coroutine = TimeMove();
            StartCoroutine(coroutine);
            isMove = false;
        }
    }

    void FixedUpdate()
    {
        characterController.Move(direction * speed * Time.deltaTime);
    }
    private void HandleLeftHandEntered(int value)
    {
        if (leftHandValue == 0)
        {
            leftHandValue = value;
            if (!isTimeout)
            {
                StartCoroutine(Timeout());
                isTimeout = true;
            }
        }
        else
        {
            leftHandValue -= value;
            if (rightHandValue + leftHandValue == 0)
            {
                leftHandValue = 0;
                rightHandValue = 0;
                isTimeout = false;
                Move();
            }
        }
    }
    private void HandleRightHandEntered(int value)
    {
        if (rightHandValue == 0)
        {
            rightHandValue = value;
            if (!isTimeout)
            {
                StartCoroutine(Timeout());
                isTimeout = true;
            }
        }
        else
        {
            rightHandValue -= value;
            if (rightHandValue + leftHandValue == 0)
            {
                leftHandValue = 0;
                rightHandValue = 0;
                isTimeout = false;
                Move();
            }
        }
    }

    private void Move()
    {
        StartCoroutine(VRControllerUtility.VibrateController(0.1f, 0.5f, 0.5f, OVRInput.Controller.All));
        Vector3 midpoint = transform.position = Vector3.Lerp(leftHand.transform.position, rightHand.transform.position, 0.5f);
        Vector3 direction = (midpoint - characterController.transform.position).normalized;
        movement = new Vector3(direction.x, 0, direction.z);
        isMove = true;
        Debug.Log("movement:" + movement);
    }

    private IEnumerator Timeout()
    {
        yield return new WaitForSeconds(timeoutThreshold);
        if (isTimeout)
        {
            leftHandValue = 0;
            rightHandValue = 0;
            isTimeout = false;
        }
    }
    private IEnumerator TimeMove()
    {
        yield return new WaitForSeconds(timeoutMove);
        movement = Vector3.zero;
    }
}
