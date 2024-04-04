using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PouchManager : MonoBehaviour
{
    [SerializeField] private float distanceFromHead = 1f;
    private CharacterController characterController;
    private Vector3 initialOffset;
    // Start is called before the first frame update
    void Start()
    {
        characterController = GameObject.FindWithTag("Player").GetComponent<CharacterController>();
        initialOffset = characterController.center;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 offset = characterController.center - initialOffset;
        transform.localPosition += offset;
    }

    public void SetDistanceFromHead(float distance)
    {
        distanceFromHead = distance;
    }
}
