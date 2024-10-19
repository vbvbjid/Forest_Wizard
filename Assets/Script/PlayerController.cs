using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector3 moveDirection;
    private Rigidbody rb;
    public float rotationSpeed = 100f; // Speed of rotation
    public float groundCheckAngleThreshold = 45f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPosition = transform.position; // Save the initial position
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetPosition();
        }

        if (transform.position.y < -10) // Example of falling off the map
        {
            ResetPosition();
        }
        ProcessInputs();
        RotateCharacter();
        Jump();
        Move();
    }

    void ProcessInputs()
    {
        float moveZ = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector3(0, 0, moveZ).normalized;
    }

    void Move()
    {
        if (moveDirection.z != 0) // Only move when there is input on the Vertical axis
        {
            Vector3 forwardMovement = transform.forward * moveDirection.z * moveSpeed; // Move in the direction the player is facing
            rb.velocity = new Vector3(forwardMovement.x, rb.velocity.y, forwardMovement.z); // Maintain Y velocity (for gravity/jumping)
        }
    }

    void RotateCharacter()
    {
        float rotateY = Input.GetAxisRaw("Horizontal"); // Use horizontal input to rotate
        transform.Rotate(Vector3.up * rotateY * rotationSpeed * Time.deltaTime); // Rotate the character left/right
    }
    public float jumpForce = 7f;
    private bool isGrounded;
    void Jump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z); // Jump upwards
            isGrounded = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Block")){
            //Debug.Log("player touch block");
        }
        // Check if the collision is with a surface that should be considered the ground
        foreach (ContactPoint contact in collision.contacts)
        {
            Vector3 contactNormal = contact.normal;
            float angle = Vector3.Angle(contactNormal, Vector3.up); // Calculate the angle between the contact normal and the upward vector

            if (angle <= groundCheckAngleThreshold) // Consider this a ground collision if the angle is below the threshold
            {
                isGrounded = true;
                break; // We found a ground collision, so no need to check further
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        /*if (other.gameObject.CompareTag("Carrot"))
        {
            Destroy(other.gameObject); // The carrot disappears
        }*/
    }
    public Vector3 startPosition;

    void ResetPosition()
    {
        rb.velocity = Vector3.zero; // Reset velocity
        transform.position = startPosition;
    }
}