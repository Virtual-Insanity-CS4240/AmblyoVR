using System.Collections;
using System.Collections.Generic;
using Delegates;
using UnityEngine;

public class TeleportTrajectory : MonoBehaviour
{
    
    private LineRenderer lineRenderer;
    [SerializeField] private int linePoints = 175;
    private Vector3[] linePositions;
    [SerializeField] private string teleportButtonName;
    private Vector3 telelocation;
    private bool canTeleport = false;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        linePositions = new Vector3[linePoints];
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (lineRenderer != null)
        {
            DrawTrajectory();
        }
        if (Input.GetAxis(teleportButtonName) < 0)
        {
            lineRenderer.enabled = true;
        }
        else if (lineRenderer.enabled)
        {
            lineRenderer.enabled = false;
            if (canTeleport)
            {
                player.transform.position = new Vector3(telelocation.x, player.transform.position.y, telelocation.z);
                canTeleport = false;
            }
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }

    void DrawTrajectory()
    {
        // Draw parabolic trajectory towards the ground, but limited to a certain distance and boundary
        Vector3 origin = transform.position;
        Vector3 velocity = transform.forward * 5.0f;
        lineRenderer.positionCount = linePoints;
        float time = 0;
        float timeStep = 0.01f;
        lineRenderer.SetPosition(0, origin);
        time += timeStep;
        linePositions[0] = origin;
        for (int i = 1; i < linePoints; i++)
        {
            Vector3 position = origin + velocity * time + Physics.gravity * time * time / 2;
            lineRenderer.SetPosition(i, position);
            linePositions[i] = position;
            RaycastHit hit;
            if (Physics.Raycast(linePositions[i - 1], position - linePositions[i - 1], out hit, (position - linePositions[i - 1]).magnitude))
            {
                if (hit.collider.gameObject.layer != 2 && hit.collider.gameObject.layer != 3) // ignore raycast and grabbable
                {
                    if (hit.collider.gameObject.layer == 6)
                    {
                        // Teleportable
                        lineRenderer.endColor = Color.green;
                        canTeleport = true;
                        telelocation = hit.point;
                    }
                    else 
                    {
                        lineRenderer.endColor = Color.red;
                        canTeleport = false;
                        
                    }
                    lineRenderer.positionCount = i + 1;
                    lineRenderer.SetPosition(i, hit.point);
                    break;
                }
                Debug.DrawLine(linePositions[i - 1], position, Color.red, 0.0f, true);
            }
            time += timeStep;
        }
    }
}
