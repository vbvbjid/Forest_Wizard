using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;       // The object the camera will follow (e.g., player)
    public Vector3 offset;         // Offset position of the camera relative to the target
    public float smoothSpeed = 0.125f;  // The speed of the smooth follow
 
    private void LateUpdate()
    {
        if (target != null)
        {
            // Desired position for the camera
            Vector3 desiredPosition = target.position + target.rotation * offset; // Use target's rotation for the offset

            // Smoothly interpolate between the current camera position and the desired position
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            // Update camera position
            transform.position = smoothedPosition;

            // Keep the camera looking at the target
            transform.LookAt(target);
        }
    }
}