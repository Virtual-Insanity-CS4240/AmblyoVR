using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ScreenFaceCamera : MonoBehaviour
{
    [SerializeField] private GameObject hand;
    [SerializeField] private Vector3 offset;
    [SerializeField] private InputActionProperty MenuButton;
    [SerializeField] private GameObject screen;
    private bool menuButtonPressed = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(Camera.main.transform);
        transform.position = hand.transform.position + offset;
        float MenuButtonValue = MenuButton.action.ReadValue<float>();
        if (MenuButtonValue == 1 && !menuButtonPressed)
        {
            screen.SetActive(!screen.activeSelf);
            menuButtonPressed = true;
            Debug.Log("Menu Button Pressed");
        }
        else if (MenuButtonValue == 0)
        {
            menuButtonPressed = false;
        }
    }
}
