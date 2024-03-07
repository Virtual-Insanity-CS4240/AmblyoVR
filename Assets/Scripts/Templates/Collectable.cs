using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] private string collectorTag;
    void OnTriggerEnter(Collider other)
    {
        // if (other.gameObject.CompareTag(collectorTag) 
        //     && gameObject.transform.parent != null 
        //     && gameObject.transform.parent.tag == "Hand")
        // {
        //     print("Collected " + gameObject.tag);
        //     if (gameObject.tag == "WhiteChicken")
        //     {
        //         PlayerInventory.CollectChicken?.Invoke(ChickenType.White);
        //     }
        //     else if (gameObject.tag == "BlackChicken")
        //     {
        //         PlayerInventory.CollectChicken?.Invoke(ChickenType.Black);
        //     }
        //     Destroy(gameObject);
        // }
    }
}
