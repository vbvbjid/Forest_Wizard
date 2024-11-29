using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swing : MonoBehaviour
{

    public float rotationAngle = 60f; // Angle to swing
    private Rigidbody rb;
    private Vector3 originPos;
    public Transform parentObject;
    public Vector3 offset;
    public float rotationSpeed = 0.1f; // Speed of returning to original position
    public bool shouldResetRotation = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        originPos = gameObject.transform.position;
    }

    private void OnCollisionEnter(Collision other)
    {
        
    }

    void Update()
    {
        
        //rb.rotation, targetRotation, rotationSpeed * Time.deltaTime
        Quaternion newRotation = rb.rotation * Quaternion.Euler(rotationAngle, 0f, 0f);
        //Quaternion BackRotation = rb.rotation * Quaternion.Euler(0f, 0f, 0f);
        if (Input.GetKeyDown(KeyCode.P))
        {
            shouldResetRotation = false;
            // Apply torque to rotate forward along the X-axis
            rb.MoveRotation(newRotation);
        }
        if (Input.GetKeyUp(KeyCode.P))
        {
            //shouldResetRotation = true;
        }
        if (shouldResetRotation)
        {
            // Define the target rotation as Quaternion.identity (which is 0, 0, 0 in Euler angles)
            Quaternion targetRotation = Quaternion.identity;

            // Smoothly rotate from current rotation to the target (0, 0, 0)
            newRotation = Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Apply the rotation using MoveRotation
            rb.MoveRotation(newRotation);

            // Check if the object is close enough to the target rotation to stop further adjustment
            if (Quaternion.Angle(rb.rotation, targetRotation) < 0.1f)
            {
                shouldResetRotation = false; // Stop resetting rotation when it's close enough
                rb.MoveRotation(targetRotation); // Ensure it's perfectly aligned at the end
            }
        }

    }
}