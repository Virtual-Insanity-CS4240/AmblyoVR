using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStart : MonoBehaviour
{
    private bool isSpawned = false;
    // Start is called before the first frame update
    void Start()
    {
        // GameObject player = GameObject.Find("Player");
        // player.transform.position = transform.position;
        // player.transform.rotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null && !isSpawned)
        {
            player.transform.position = transform.position;
            player.transform.rotation = transform.rotation;
            isSpawned = true;
        }
    }
}
