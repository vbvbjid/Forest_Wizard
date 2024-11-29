using UnityEngine;

public class HandRotationController : MonoBehaviour
{
    public Transform handTransform; // Assign your VR hand transform here
    public LayerMask interactableLayer; // Only objects in this layer will respond to hand gestures
    public float rotationSpeed = 50f; // Adjust the rotation speed
    private float elapsedTime = 0f;
    private Vector3 startPosition;
    public float requiredDistance = 0.1f; // Distance for continuous upward movement
    public float resetThreshold = -0.1f;  // Threshold to reset if the object moves downward
    public float timeLimit = 3.0f;        // Maximum time allowed for the raise to complete
    private bool isRaising = false;
    //private bool PawnUp = false;

    private GameObject targetedObject;

    void Update()
    {
        
        DetectHandGesture();
    }

    private void DetectHandGesture()
    {
        if (!isRaising)
        {
            startPosition = handTransform.position;
            elapsedTime = 0f; // Reset timer
            isRaising = true;
        }

        // Increment elapsed time
        elapsedTime += Time.deltaTime;

        // Calculate the upward distance traveled
        float distanceRaised = handTransform.position.y - startPosition.y;
        DebugLogToCanvasTMP.Instance.UpdateLog(distanceRaised.ToString());
        DebugLogToCanvasTMP.Instance.UpdateLog(handTransform.position.y.ToString());
        // Check if the object has moved upward by the required distance within the time limit
        if (distanceRaised >= requiredDistance && elapsedTime <= timeLimit)
        {
            DebugLogToCanvasTMP.Instance.UpdateLog("pawn up");
            //Debug.Log("pawn up");
            Vector3 palmDirection = handTransform.forward;
            Ray ray = new Ray(handTransform.position, palmDirection);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, interactableLayer))
            {
                // Check if the object hit has the tag "Block"
                if (hit.collider.CompareTag("Flower"))
                {
                    DebugLogToCanvasTMP.Instance.UpdateLog("find flower");
                    DebugLogToCanvasTMP.Instance.UpdateLog("find flower");
                    targetedObject = hit.collider.gameObject;
                    // Rotate the object around the Y-axis
                    RotateObject(targetedObject);
                }
            }
            DebugLogToCanvasTMP.Instance.UpdateLog("raised");
            isRaising = false; // Reset or handle the action once the requirement is met
        }
        else if (elapsedTime > timeLimit || distanceRaised < resetThreshold)
        {
            isRaising = false;
            Debug.Log("Raise failed (either due to timeout or downward movement)");
        }
    }

    private void RotateObject(GameObject obj)
    {
        if (obj != null)
        {
            // Rotate around the Y-axis
            obj.transform.Rotate(Vector3.up, rotationSpeed);
        }
    }
}