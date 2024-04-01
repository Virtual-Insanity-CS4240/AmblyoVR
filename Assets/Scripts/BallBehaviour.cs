using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehaviour : MonoBehaviour
{
    public ParticleSystem[] particleSystems;

    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ghost"))
        {
            Debug.Log("Hit Ghost");
            foreach (ParticleSystem ps in particleSystems)
            {
                if (ps != null)
                {
                    ps.Play();
                }
            }
            StartCoroutine(DestroyBall(2f));
        }
    }

    public IEnumerator DestroyBall(float time)
    {
        Debug.Log("Destroying Ball");
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
