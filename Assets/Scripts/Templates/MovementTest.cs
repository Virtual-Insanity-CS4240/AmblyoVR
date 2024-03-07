using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementTest : MonoBehaviour
{
    private GameObject player;
    [SerializeField] private Transform lookat;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    void OnTriggerEnter(Collider other)
    {
        print(other.gameObject.layer);
        if (other.gameObject.layer == 7)
        {   
            print("movement");
            Rigidbody rb = player.GetComponent<Rigidbody>();
            Vector3 direction = (player.transform.position - lookat.position).normalized;
            Vector3 newDirection = new Vector3(direction.x, 0, direction.z);
            rb.AddForce(newDirection * 1f, ForceMode.Impulse);
        }
    }
}
