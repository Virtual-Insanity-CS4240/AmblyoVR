using System.Collections;
using UnityEngine;

public class BallBehaviour : MonoBehaviour
{
    public ParticleSystem[] particleSystems;
    public BallColor color;
    public Vector3 curveDirection;
    public bool isDisappear = true;

    private bool hasHit = false;

    void FixedUpdate()
    {
        if (curveDirection != Vector3.zero && !hasHit)
        {
            GetComponent<Rigidbody>().AddForce(curveDirection, ForceMode.Impulse);
        }
    }

    // TODO after HOTO: Probably make this an interface to prevent DRY
    public void OnCollisionEnter(Collision other)
    {

        if (other.gameObject.CompareTag("Ghost"))
        {
            GhostMovement ghostMovement = other.gameObject.GetComponent<GhostMovement>();

            if (ghostMovement.ghostColor == color)
            {
                hasHit = true;
                ghostMovement.GhostHit();
                SoundManager.Instance.PlayPoofSound();
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
        else if (other.gameObject.CompareTag("Boss"))
        {
            BossMovement bossGhostMovement = other.gameObject.GetComponent<BossMovement>();

            if (bossGhostMovement.ghostColor == color)
            {
                hasHit = true;
                bossGhostMovement.GhostHit();
                SoundManager.Instance.PlayPoofSound();
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
            }
        }
        else if (other.gameObject.CompareTag("TutorialGhost"))
        {
            TutorialGhost tutorialGhost = other.gameObject.GetComponent<TutorialGhost>();

            if (tutorialGhost.ghostColor == color)
            {
                hasHit = true;
                tutorialGhost.GhostHit();
                SoundManager.Instance.PlayPoofSound();
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
        else if (!hasHit)
        {
            hasHit = true;
            SoundManager.Instance.PlayBallLandingSound();
            StartCoroutine(DestroyBallGround(0.5f));
        }
    }

    public IEnumerator DestroyBall(float time)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        GetComponent<Collider>().isTrigger = true;
        yield return new WaitForSeconds(time);
        if (isDisappear)
        {
            Destroy(gameObject);
        }
    }

    public IEnumerator DestroyBallGround(float time)
    {
        yield return new WaitForSeconds(time);
        if (isDisappear)
        {
            Destroy(gameObject);
        }
    }
}
