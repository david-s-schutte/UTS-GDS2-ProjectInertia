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

    [Header("Component References")]
    [SerializeField] private Rigidbody rb;

    private void Start()
    {
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        //Determine direction from player input
        Vector3 movementDirection = GetDirectionFromInput(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        //Move player relevant to camera
        Vector3 cameraForward = camera.transform.forward;
        Vector3 cameraRight = camera.transform.right;
        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward = cameraForward.normalized;
        cameraRight = cameraRight.normalized;

        transform.position += (cameraForward*movementDirection.z + cameraRight*movementDirection.x)*Time.deltaTime*movementSpeed;
        //rb.AddForce((cameraForward * movementDirection.z + cameraRight * movementDirection.x) * movementSpeed);

        //Rotate the player in the given direction
        if (movementDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(cameraForward * movementDirection.z + cameraRight * movementDirection.x, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            //transform.Translate(transform.forward * magnitude * movementSpeed * Time.deltaTime, Space.World);
        }
    }

    public Vector3 GetDirectionFromInput(float horizontalInput, float verticalInput)
    {
        Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput);
        movementDirection.Normalize();
        return movementDirection;
    }
}
