using UnityEngine;
using UnityEngine.InputSystem;

public class EightDirectionMovement3DWASDRotate : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 90f; // Degrees per second

    private Vector3 moveDirection;
    public float rotationDirection;
    private PlayerInput playerInput;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        playerInput.actions["Move"].performed += OnMove;
        playerInput.actions["Move"].canceled += OnMove;
        playerInput.actions["Rotate"].performed += OnRotate;
        playerInput.actions["Rotate"].canceled += OnRotate;
    }

    private void OnDisable()
    {
        playerInput.actions["Move"].performed -= OnMove;
        playerInput.actions["Move"].canceled -= OnMove;
        playerInput.actions["Rotate"].performed -= OnRotate;
        playerInput.actions["Rotate"].canceled -= OnRotate;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        Vector2 inputVector = context.ReadValue<Vector2>();
        moveDirection = new Vector3(inputVector.x, 0f, inputVector.y);
    }

    private void OnRotate(InputAction.CallbackContext context)
    {
        rotationDirection = context.ReadValue<float>();
    }

    private void Update()
    {
        Move();
        Rotate();
    }

    private void Move()
    {
        // Normalize the input vector to prevent faster diagonal movement
        moveDirection.Normalize();

        // Move the object in the determined direction
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.Self);
    }

    private void Rotate()
    {
        // Rotate the object based on the rotation input
        transform.Rotate(Vector3.up * rotationDirection * rotationSpeed * Time.deltaTime, Space.Self);
    }
}