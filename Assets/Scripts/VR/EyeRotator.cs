using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class EyeRotator : MonoBehaviour
{
    [SerializeField] private GameObject leftEyeAnchor;
    [SerializeField] private GameObject rightEyeAnchor;
    // [SerializeField] private InputActionReference leftRotate;
    // [SerializeField] private InputActionReference rightRotate;
    // [SerializeField] private InputActionReference resetLeftRotation;
    // [SerializeField] private InputActionReference resetRightRotation;
    [SerializeField] private Slider leftXSlider;
    [SerializeField] private Slider leftYSlider;
    [SerializeField] private Slider rightXSlider;
    [SerializeField] private Slider rightYSlider;
    // private bool allowJoystick;

    // Update is called once per frame
    void Update()
    {
        // if (allowJoystick)
        // {
        //     leftXSlider.value = leftEyeAnchor.transform.rotation.x;
        //     leftYSlider.value = leftEyeAnchor.transform.rotation.y;
        //     rightXSlider.value = rightEyeAnchor.transform.rotation.x;
        //     rightYSlider.value = rightEyeAnchor.transform.rotation.y;
        //     leftEyeAnchor.transform.rotation = new Quaternion((Mathf.Abs(leftRotate.action.ReadValue<Vector2>().y) > 0.5 ? (leftRotate.action.ReadValue<Vector2>().y)/180 : 0) + leftEyeAnchor.transform.rotation.x, leftEyeAnchor.transform.rotation.y, leftEyeAnchor.transform.rotation.z, leftEyeAnchor.transform.rotation.w);
        //     leftEyeAnchor.transform.rotation = new Quaternion(leftEyeAnchor.transform.rotation.x, (Mathf.Abs(leftRotate.action.ReadValue<Vector2>().x) > 0.5 ? (leftRotate.action.ReadValue<Vector2>().x)/180 : 0) + leftEyeAnchor.transform.rotation.y, leftEyeAnchor.transform.rotation.z, leftEyeAnchor.transform.rotation.w);
        //     rightEyeAnchor.transform.rotation = new Quaternion((Mathf.Abs(rightRotate.action.ReadValue<Vector2>().y) > 0.5 ? (rightRotate.action.ReadValue<Vector2>().y)/180 : 0) + rightEyeAnchor.transform.rotation.x, rightEyeAnchor.transform.rotation.y, rightEyeAnchor.transform.rotation.z, rightEyeAnchor.transform.rotation.w);
        //     rightEyeAnchor.transform.rotation = new Quaternion(rightEyeAnchor.transform.rotation.x, (Mathf.Abs(rightRotate.action.ReadValue<Vector2>().x) > 0.5 ? (rightRotate.action.ReadValue<Vector2>().x)/180 : 0) + rightEyeAnchor.transform.rotation.y, rightEyeAnchor.transform.rotation.z, rightEyeAnchor.transform.rotation.w);
        //     Debug.Log("Left: " + leftRotate.action.ReadValue<Vector2>().x + " " + leftRotate.action.ReadValue<Vector2>().y);
        //     Debug.Log("Right: " + rightRotate.action.ReadValue<Vector2>().x + " " + rightRotate.action.ReadValue<Vector2>().y);
        //     if (resetLeftRotation.action.ReadValue<float>() > 0)
        //     {
        //         leftEyeAnchor.transform.rotation = new Quaternion(0, 0, 0, 0);
        //     }
        //     if (resetRightRotation.action.ReadValue<float>() > 0)
        //     {
        //         rightEyeAnchor.transform.rotation = new Quaternion(0, 0, 0, 0);
        //     }
        // }
    }
    public void LeftRotateY(float amount)
    {
        leftEyeAnchor.transform.rotation = new Quaternion(amount, leftEyeAnchor.transform.rotation.y, leftEyeAnchor.transform.rotation.z, leftEyeAnchor.transform.rotation.w);
    }
    public void LeftRotateX(float amount)
    {
        leftEyeAnchor.transform.rotation = new Quaternion(leftEyeAnchor.transform.rotation.x, amount, leftEyeAnchor.transform.rotation.z, leftEyeAnchor.transform.rotation.w);
    }
    public void RightRotateY(float amount)
    {
        rightEyeAnchor.transform.rotation = new Quaternion(amount, rightEyeAnchor.transform.rotation.y, rightEyeAnchor.transform.rotation.z, rightEyeAnchor.transform.rotation.w);
    }
    public void RightRotateX(float amount)
    {
        rightEyeAnchor.transform.rotation = new Quaternion(rightEyeAnchor.transform.rotation.x, amount, rightEyeAnchor.transform.rotation.z, rightEyeAnchor.transform.rotation.w);
    }
    // public void AllowJoystick(bool allow)
    // {
    //     allowJoystick = allow;
    // }
    public void ResetAllRotation()
    {
        leftEyeAnchor.transform.rotation = new Quaternion(0, 0, 0, 0);
        rightEyeAnchor.transform.rotation = new Quaternion(0, 0, 0, 0);
        leftXSlider.value = 0;
        leftYSlider.value = 0;
        rightXSlider.value = 0;
        rightYSlider.value = 0;
    }
}
