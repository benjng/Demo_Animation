using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float crouchSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float jumpDuration;
    [SerializeField] private float gravity;

    private CharacterController characterController;
    private Quaternion targetRotation;

    private float moveSpeed;
    private float jumpTime; // The time elapsed since the jump started
    private float jumpStartVelocity;
    private bool jumpPressed;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // Get the horizontal and vertical input axes
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Check if the player has pressed the jump button
        if (Input.GetButtonDown("Jump"))
        {
            if (characterController.isGrounded)
            {
                jumpPressed = true;
                jumpTime = 0f;
                jumpStartVelocity = 2f * jumpSpeed;
            }
        }

        // Check if player is running or not
        if (Input.GetKey("left shift"))
        {
            moveSpeed = runSpeed;
        } else
        {
            moveSpeed = crouchSpeed;
        }

        // Calculate the movement direction based on the camera's orientation
        Vector3 cameraForward = cameraTransform.forward;        
        Vector3 cameraRight = cameraTransform.right;

        // The direction in which the character should move based on the user's input
        Vector3 movementDirection = verticalInput * cameraForward + horizontalInput * cameraRight;
        movementDirection.Normalize();

        // Rotate the character towards the movement direction (Character facing direction)
        if (movementDirection.magnitude != 0)
        {
            targetRotation = Quaternion.LookRotation(movementDirection);
            targetRotation.eulerAngles = new Vector3(0f, targetRotation.eulerAngles.y, 0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        
        // Check if the jump button has been pressed
        if (jumpPressed)
        {
            float jumpAcceleration = jumpStartVelocity / jumpDuration;
            float jumpVelocity = jumpAcceleration * jumpTime - 0.5f * gravity * jumpTime * jumpTime;
            movementDirection.y = jumpVelocity;
            jumpTime += Time.deltaTime;
            if (jumpTime > jumpDuration)
            {
                jumpPressed = false;
            }
        } else {
            movementDirection.y -= gravity; 
        }

        movementDirection *= moveSpeed;
        characterController.Move(movementDirection * Time.deltaTime);
    }
}
