using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdventureControls : MonoBehaviour
{
    [Header("External Game Objects")]
    [SerializeField] private Camera camera;
    private Vector3 cameraForward;
    private Vector3 cameraRight;

    [Header("Movement Variables")]
    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float jumpHeight;
    [SerializeField] private int numberOfJumps;
    [SerializeField] private float gravity;
    private bool isGrounded;
    private int jumpsLeft;

    [Header("Component References")]
    [SerializeField] private Rigidbody rb;

    private void Start()
    {
        camera = Camera.main;
        isGrounded = true;
        jumpsLeft = numberOfJumps;
    }

    // Update is called once per frame
    void Update()
    {
        //Determine direction from player input
        Vector3 movementDirection = GetDirectionFromInput(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        //Move and rotate the player
        MovePlayer(movementDirection);
        //Let them jump if they input a jump
        PlayerJump();

    }

    public Vector3 GetDirectionFromInput(float horizontalInput, float verticalInput)
    {
        Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput);
        movementDirection.Normalize();
        return movementDirection;
    }

    public void MovePlayer(Vector3 movementDirection)
    {
        //Determine the transform of the camera
        Vector3 cameraForward = camera.transform.forward;
        Vector3 cameraRight = camera.transform.right;
        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward = cameraForward.normalized;
        cameraRight = cameraRight.normalized;

        //Move the player based on input and camera transform
        transform.position += (cameraForward * movementDirection.z + cameraRight * movementDirection.x) * Time.deltaTime * movementSpeed;

        //If the given direction isn't zero
        if (movementDirection != Vector3.zero)
        {
            //Rotate the player to face ther given direction
            Quaternion toRotation = Quaternion.LookRotation(cameraForward * movementDirection.z + cameraRight * movementDirection.x, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }

    public void PlayerJump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            Debug.Log("Jump!");
            if (isGrounded)
            {
                rb.velocity = new Vector3(rb.velocity.x, jumpHeight, rb.velocity.z);
                isGrounded = false;
                jumpsLeft--;
            }
            else if(numberOfJumps > 0)
            {
                rb.velocity = new Vector3(rb.velocity.x, jumpHeight, rb.velocity.z);
                jumpsLeft--;
            }
        }
    }
}
