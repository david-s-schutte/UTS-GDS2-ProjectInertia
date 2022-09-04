using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayStyleController : MonoBehaviour
{
    public Camera cam;
    [SerializeField] private bool isPlatforming;
    //Adventure Style Control Components
    private AdventureControls adventureControls;
    private CharacterController characterController;
    //Surfer Style Control Components
    private SurferControls surferControls;
    [SerializeField] private Collider capsuleCollider;
    [SerializeField] private float rotateSpeed;
    private Rigidbody rb;
    
    [Header("Child References")]
    [SerializeField] private Transform playerModel;
    public Animator animator;

    private void Start()
    {
        cam = Camera.main;
        adventureControls = GetComponent<AdventureControls>();
        surferControls = GetComponent<SurferControls>();
        characterController = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        surferControls.InitialiaseSurferControls(capsuleCollider);
    }

    // Update is called once per frame
    void Update()
    {
        //Switch styles first if the player decides to
        if (Input.GetButtonDown("Switch Styles"))
        {
            //Swap its value
            isPlatforming = !isPlatforming;
            animator.SetBool("isPlatforming", isPlatforming);
            //If the player wants to use the adventure control scheme
            if (isPlatforming)
            {
                //Enable adventure control scheme components
                adventureControls.enabled = true;
                characterController.enabled = true;
                //Disable surfer control scheme components
                rb.useGravity = false;
                rb.angularVelocity = Vector3.zero;
                capsuleCollider.enabled = false;
            }
            //If the player wants to use the surfer control scheme
            else
            {
                //Enable surfer control scheme components
                rb.useGravity = true;
                capsuleCollider.enabled = true;
                //Disable adventure control scheme components
                adventureControls.enabled = false;
                characterController.enabled = false;
            }
        }

        //Get the player's input
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        //Get variables associated with the camera
        Vector3 cameraForward = cam.transform.forward;
        Vector3 cameraRight = cam.transform.right;
        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward = cameraForward.normalized;
        cameraRight = cameraRight.normalized;

        Vector3 movementDirection = (cameraRight * horizontalInput) + (cameraForward * verticalInput);
        movementDirection.Normalize();

        //Execute the player's movement based on their chosen control scheme
        if (isPlatforming)
        {
            //Rotate the player's model in their given direction accounting for camera placement
            //Vector3 movementDirection = (cameraRight * horizontalInput) + (cameraForward * verticalInput);
            if (movementDirection != Vector3.zero)
            {
                Quaternion rot = Quaternion.LookRotation(movementDirection);
                playerModel.rotation = Quaternion.Slerp(playerModel.rotation, rot, rotateSpeed * Time.deltaTime);
            }

            adventureControls.MovePlayer(movementDirection);
        }
        else
        {
            //Rotate the player's model in their given direction accounting for camera placement
            //Vector3 movementDirection = (cameraRight * horizontalInput) + (cameraForward * verticalInput);
            surferControls.MovePlayer(movementDirection, playerModel);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }
}
