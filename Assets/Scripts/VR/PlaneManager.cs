using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneManager : MonoBehaviour
{
    [SerializeField] private float distanceFromHead = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y - distanceFromHead, Camera.main.transform.position.z);
    }

    public void SetDistanceFromHead(float distance)
    {
        distanceFromHead = distance;
    }
}
