using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenLerpMotion : MonoBehaviour
{
    [SerializeField] private GameObject cameraObject;
    [SerializeField] private float speed = 0.01f;
    [SerializeField] private float thresholdDistance = 0.1f;
    private float timeCount = 0.0f;

    void Start()
    {
        transform.position = cameraObject.transform.position;
        transform.rotation = cameraObject.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {

        transform.rotation = Quaternion.Lerp(transform.rotation, new Quaternion(cameraObject.transform.rotation.x, cameraObject.transform.rotation.y, 0, cameraObject.transform.rotation.w), speed);
        // transform.rotation = Quaternion.Lerp(transform.rotation, camera.transform.rotation, timeCount * speed);
        timeCount += Time.deltaTime;
    }
    void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, cameraObject.transform.position) > thresholdDistance)
        {
            transform.position = cameraObject.transform.position;
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, cameraObject.transform.position, speed);
        }
    }
}
