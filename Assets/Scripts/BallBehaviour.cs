using System.Collections;
using UnityEngine;

public class BallBehaviour : MonoBehaviour
{
    public ParticleSystem[] particleSystems;
    public BallColor color;


    // TODO after HOTO: Probably make this an interface to prevent DRY
    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ghost"))
        {
            GhostMovement ghostMovement = other.gameObject.GetComponent<GhostMovement>();

            if (ghostMovement.ghostColor == color)
            {
                ghostMovement.GhostHit();
                foreach (ParticleSystem ps in particleSystems)
                {
                    if (ps != null)
                    {
                        ps.Play();
                    }
                }
                StartCoroutine(DestroyBall(0.5f));
            }
            else
            {
                // TODO: Handle negative case
            }
        }

        if (other.gameObject.CompareTag("TutorialGhost"))
        {
            TutorialGhost tutorialGhost = other.gameObject.GetComponent<TutorialGhost>();

            if (tutorialGhost.ghostColor == color)
            {
                tutorialGhost.GhostHit();
                foreach (ParticleSystem ps in particleSystems)
                {
                    if (ps != null)
                    {
                        ps.Play();
                    }
                }
                StartCoroutine(DestroyBall(0.5f));
            }
            else
            {
                // TODO: Handle negative case
            }
        }
    }

    public IEnumerator DestroyBall(float time)
    {
        GetComponent<Rigidbody>().useGravity = false;
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
