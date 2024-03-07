using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootTrigger : MonoBehaviour
{
    [SerializeField] private string shootButtonName;
    [SerializeField] private GameObject whiteChickenProjectile;
    [SerializeField] private GameObject blackChickenProjectile;
    [SerializeField] private float shootForce = 10f;
    [SerializeField] private Transform origin;
    private bool isShooting = false;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // if you hold button
        if (!isShooting && Input.GetAxis(shootButtonName) == 1)
        {
            print("Shoot");
            isShooting = true;
            Queue<ChickenType> chickenQueue = PlayerInventory.GetChickenQueue?.Invoke();
            if (chickenQueue == null || (chickenQueue != null && chickenQueue.Count == 0))
            {
                print("No chickens in queue");
            }
            else
            {
                ChickenType? chicken = PlayerInventory.ShootChicken?.Invoke();
                print(chicken);
                print("Chickens in queue: " + chickenQueue.Count);
                ShootObject((ChickenType)chicken);
            }
        }

        if (isShooting && Input.GetAxis(shootButtonName) < 1)
        {
            isShooting = false;
        }
    }

    void ShootObject(ChickenType chicken)
    {
        GameObject chickenPrefab = chicken == ChickenType.White ? whiteChickenProjectile : blackChickenProjectile;

        GameObject projectile = Instantiate(chickenPrefab, origin.position, origin.rotation);

        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        rb.AddForce(origin.forward * shootForce, ForceMode.Impulse);

        audioSource.Play();     

    }
}
