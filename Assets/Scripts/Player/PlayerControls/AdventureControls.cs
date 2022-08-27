using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdventureControls : MonoBehaviour
{
    [Header("Control Variables")]
    private Vector3 movementDirection;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float jumpHeight;
    [SerializeField] private float doubleJumpModifier;
    [SerializeField] private int numberOfJumps;
    
    [Header("Physics Variables")]
    [SerializeField] private float gravityScale;

    //Variables used for calculations
    private int jumpsLeft;

    [Header("Component References")]
    [SerializeField] private CharacterController controller;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        jumpsLeft = numberOfJumps;
    }

    public void MovePlayer(Vector3 playerInput, float horizontalInput, float verticalInput)
    {
        //Store the player's current Y position
        float yStore = movementDirection.y;

        //Apply movement speed to the given input and reapply the player's current Y position
        movementDirection = playerInput * movementSpeed;
        movementDirection.y = yStore;
        
        //Check if the player wants to jump
        if (Input.GetButtonDown("Jump"))
        {
            //If they're grounded let them jump
            if (controller.isGrounded)
            {
                movementDirection.y = jumpHeight;
                jumpsLeft -= 1;
            }
            //If they're mid-air let them double jump if they have some jumps left
            else if (jumpsLeft > 0)
            {
                movementDirection.y = jumpHeight / doubleJumpModifier;
                jumpsLeft -= 1;
            }
            Debug.Log("jumps left = " + jumpsLeft);
        }

        

        //Reset jump count after landing
        if (controller.isGrounded)
            jumpsLeft = numberOfJumps;
        //Apply gravity to the player
        else
            movementDirection.y = movementDirection.y + (Physics.gravity.y * gravityScale * Time.deltaTime);

        //Move the player
        controller.Move(movementDirection * Time.deltaTime);
    }
}
