using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ControlStyle {Surfer, Adventure}
public class PlayerController : MonoBehaviour {
    // Static variables
    public static ControlStyle currentStyle;

    // Component references
    [SerializeField] Animator animator; //TODO: we should be using playeranimatorcontroller so this isn't here???
    CharacterController cc;
    AdventureControls adventureControls;
    SurferControls surferControls;

    // Private variables
    private Vector3 velocity;

    // Serialized variables
    [SerializeField] float turnLerpValue;

        // Adventure style
    [SerializeField] float advMoveSpeed = 5;
    [SerializeField] float advJumpVelocity = 50;

    private void Awake() {
        // Setup component references that aren't already set
        if (!cc) cc                                 = GetComponent<CharacterController>();
        if (!animator) animator                     = GetComponentInChildren<Animator>();
        if (!adventureControls) adventureControls   = GetComponent<AdventureControls>();
        if (!surferControls) surferControls         = GetComponent<SurferControls>();
        // Default values
        currentStyle = ControlStyle.Adventure;
        SetStyleTo(ControlStyle.Adventure);
    }

    private void Update() {
        // Switch styles when style switch key pressed
        // TODO: New input system
        if (Input.GetButtonDown("Switch Styles")) {
            SwitchStyle();
        }

        // TODO: New input system
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        // If there is *any* movement input, enable walking bool on animator
        // Note no dead zone is considered here as the input system is assumed to deal with that
        // TODO: Add a speed variable so that the walk animation plays at a different speed when walking slower
        animator.SetBool("walking", moveInput.sqrMagnitude > 0);

        // Determine camera-relative forward direction
        // Get right and forward vectors of camera
        Vector3 camRight    = Camera.main.transform.right;
        Vector3 camForward  = Camera.main.transform.forward;
        // Cancel the Y axis
        camRight.y   = 0;
        camForward.y = 0;
        // Normalize camera axes
        camRight.Normalize();
        camForward.Normalize();
        // Add forward and right camera vector multiplied by respective inputs to get the movement direction 
        Vector3 movementDirection = (camRight * moveInput.x) + (camForward * moveInput.y);
        movementDirection.y = 0;
        // Clamp movement direction to be below 1
        Vector3.ClampMagnitude(movementDirection, 1.0f);

        // PROCESSING MOVEMENT
        Vector3 movement = Vector3.zero;

        if (currentStyle == ControlStyle.Adventure) {
            // Turn to look in the direction of input
            if (movementDirection.sqrMagnitude > 0) {
                Quaternion targetRotation = Quaternion.LookRotation(movementDirection);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, turnLerpValue * Time.deltaTime);
            }
            
            movement += AdventureStyle.Move(movementDirection) * advMoveSpeed;
        } else {
            surferControls.MovePlayer(movementDirection, transform);
        }


        // Apply gravity
        if (cc.isGrounded) {
            velocity.y = 0;
        } else {
            velocity += Physics.gravity * Time.deltaTime;
        }

        // Input Jumping - not yet working
        // if (Input.GetButtonDown("Jump")) {
        //     if (currentStyle == ControlStyle.Adventure) {
        //         velocity += AdventureStyle.Jump(moveInput) * advJumpVelocity;
        //     }
        // }

        cc.Move(movement * Time.deltaTime + velocity);

    }

    Vector3 MoveAdventureStyle (Vector3 input) {
        Vector3 movement = input * advMoveSpeed;
        return movement;
    }

    // Sets the style to the requested style
    // Might be a good idea to make this a queue system using a coroutine, so that there can be a slight delay to prevent rapid switches
    public void SetStyleTo (ControlStyle newStyle) {
        currentStyle = newStyle;
        // TODO: Animator bool has inconsistent naming
        animator.SetBool("isPlatforming", currentStyle == ControlStyle.Adventure);
    }

    void SwitchStyle() {
        if (currentStyle == ControlStyle.Surfer) 
            currentStyle = ControlStyle.Adventure;
        else 
            currentStyle = ControlStyle.Surfer;
    }

    // This defs needs work
    // i have in mind a really smart way to do this but i can't quite get my head around it
    class AdventureStyle {
        public static Vector3 Move (Vector3 input) {
            Vector3 motion = input;
            return motion;
        }
        public static Vector3 Jump (Vector3 input) {
            Vector3 motion = new Vector3(0, 1, 0);
            return motion;
        }
    }
}
