using System;
using System.Collections;
using Oculus.Interaction.Input;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class SelfDirectionDetector : MonoBehaviour
{
    public float requiredDistance = 0.2f; // Distance for continuous upward movement
    public float resetThreshold = -0.1f;  // Threshold to reset if the object moves downward
    public float timeLimit = 5.0f;        // Maximum time allowed for the raise to complete
    public float rayLength = 10f;         // Length of the raycast

    private Vector3 startPosition;
    private bool isRaising = false;
    private float elapsedTime = 0f;
    private GameObject targetedObject;
    private LineRenderer lineRenderer;
    private bool rotated = false;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
    }

    void Update()
    {
        DetectHandGesture();
    }

    public void DetectHandGesture()
    {
        if (!isRaising)
        {
            startPosition = transform.position;
            elapsedTime = 0f; // Reset timer
            isRaising = true;
        }

        // Increment elapsed time
        elapsedTime += Time.deltaTime;

        // Calculate the upward distance traveled
        float distanceRaised = transform.position.y - startPosition.y;

        // Check if the object has moved upward by the required distance within the time limit
        if (distanceRaised >= requiredDistance && elapsedTime <= timeLimit)
        {
            DebugLogToCanvasTMP.Instance.UpdateLog("Raised");
            DoRayCast();
            isRaising = false; // Reset or handle the action once the requirement is met
        }
        // Reset if the time limit is exceeded or if the object moves downward
        else if(distanceRaised < requiredDistance && elapsedTime > timeLimit)
        {
            DebugLogToCanvasTMP.Instance.UpdateLog("Raise failed");
            DebugLogToCanvasTMP.Instance.UpdateLog("hand posY: " + transform.position.y.ToString());
            DebugLogToCanvasTMP.Instance.UpdateLog("total distance: " + distanceRaised.ToString());
            isRaising = false;
        }
    }

    public void DoRayCast()
    {
        Vector3 palmDirection = transform.forward;
        Ray ray = new Ray(transform.position, palmDirection);
        RaycastHit hit;

        // Set the LineRenderer start and end points
        lineRenderer.SetPosition(0, ray.origin);

        if (Physics.Raycast(ray, out hit, rayLength))
        {
            lineRenderer.SetPosition(1, hit.point);

            // Check if the object hit has the tag "Flower"
            if (hit.collider.CompareTag("Flower"))
            {
                DebugLogToCanvasTMP.Instance.UpdateLog("find flower " + hit.collider.name);
                targetedObject = hit.collider.gameObject;
                RotateObject(targetedObject);
            }
        }
        else
        {
            // Extend the line to full ray length if no hit
            lineRenderer.SetPosition(1, ray.origin + palmDirection * rayLength);
        }
    }

    private IEnumerator RotateObject(GameObject obj)
    {
        if (obj != null && !rotated)
        {
            // Rotate around the Y-axis
            obj.transform.Rotate(0, 90, 0);
            rotated = true;
        }
        yield return new WaitForSeconds(5.0f);
        obj.transform.Rotate(0, -90, 0);
        rotated = false;
    }
}