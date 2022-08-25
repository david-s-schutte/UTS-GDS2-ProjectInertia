using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdventureControls : MonoBehaviour
{
    [Header("Movement Variables")]
    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationSpeed;

    [Header("Component References")]
    [SerializeField] private Rigidbody rb;

    // Update is called once per frame
    void Update()
    {
        //Get player input
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        //Determine direction from player input
        Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput);
        float magnitude = Mathf.Clamp01(movementDirection.magnitude);
        movementDirection.Normalize();

        Vector3 newVelocity = new Vector3(movementDirection.x * magnitude * movementSpeed, rb.velocity.y, movementDirection.z * magnitude * movementSpeed);
        rb.velocity = newVelocity;

        //Rotate the player in the given direction
        if (movementDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            //transform.Translate(transform.forward * magnitude * movementSpeed * Time.deltaTime, Space.World);
        }
    }
}
