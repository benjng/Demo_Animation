using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float jumpHeight = 3f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private Transform groundChecker;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Camera myCam;

    private Vector3 velocity;
    private bool isGrounded;

    void Update()
    {
        GroundedCheck();
        JumpCheck();

        // Gather Keyboard inputs
        float xInput = Input.GetAxis("Horizontal");
        float zInput = Input.GetAxis("Vertical");

        MoveCharacter(xInput, zInput);
        ApplyGravity();
        
    }

    void MoveCharacter(float xInput, float zInput){
        // Main cam reference
        Transform camTransform = myCam.transform;
        camTransform.eulerAngles = new Vector3(0, camTransform.eulerAngles.y, camTransform.eulerAngles.z); // set x back to horizontal level

        // Character moving direction, according to camera's orientation
        Vector3 moveDir = camTransform.right * xInput + camTransform.forward * zInput;
        controller.Move(moveDir * speed * Time.deltaTime);

        // Character orientation
        if (moveDir.normalized != Vector3.zero) {
            Debug.Log("Moving");
            Quaternion q = Quaternion.LookRotation(moveDir, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, q, 0.05f);
        }
    }

    void ApplyGravity(){
        // y = 0.5*g * t^2
        velocity.y += gravity * (Time.deltaTime);
        controller.Move(velocity * Time.deltaTime);
    }

    void GroundedCheck(){
        isGrounded = Physics.CheckSphere(groundChecker.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0){
            velocity.y = -2f; // constantly forcing the player falls
        }
    }

    void JumpCheck(){
        if (Input.GetButtonDown("Jump") && isGrounded){
            // v = √h * -2 * g
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(groundChecker.position, groundDistance);
    }
}
