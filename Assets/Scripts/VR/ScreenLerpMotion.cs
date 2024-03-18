using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenLerpMotion : MonoBehaviour
{
    [SerializeField] private GameObject camera;
    [SerializeField] private float speed = 0.01f;
    [SerializeField] private float thresholdDistance = 0.1f;
    private float timeCount = 0.0f;

    void Start()
    {
        transform.position = camera.transform.position;
        transform.rotation = camera.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Vector3.Distance(transform.position, camera.transform.position));
        if (Vector3.Distance(transform.position, camera.transform.position) > thresholdDistance)
        {
            transform.position = camera.transform.position;
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, camera.transform.position, speed);
        }
        transform.rotation = Quaternion.Lerp(transform.rotation, new Quaternion(camera.transform.rotation.x, camera.transform.rotation.y, 0, camera.transform.rotation.w), speed);
        // transform.rotation = Quaternion.Lerp(transform.rotation, camera.transform.rotation, timeCount * speed);
        timeCount += Time.deltaTime;
    }
}
