using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using Surfer.Input;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum MovementMode {Walking, Surfer, Stopped};

    /*ADDITIONS MADE BY DAVID - new Inputs*/
    PlayerControls playerControls;
    InputAction leftStickMove;
    InputAction jump;
    InputAction switchStyles;

    // # Components    
    CharacterController cc;
    Camera playerCamera;
    [Header("Components")]
    [SerializeField] GameObject characterObject;
    [SerializeField] Transform cameraPivotTransform;

    // # Movement
    [Header("Movement")]
    // Speed of player walking
    [SerializeField] float walkSpeed = 5;
    // Current movement input (left stick)
    Vector2 movementInput;
    // Current velocity. Modifying this value will instantly change velocity (but please use static function SetVelocity)
    private static Vector3 velocity;
    // Velocity we were travelling at last frame
    Vector3 lastFrameVelocity = new Vector3();

    [SerializeField] float groundStickingForce = 4.0f;

    // Speed carried between frames
    float surferModeCarriedSpeed = 0;
    float surferModeCurrentThrust = 0;
    // Speed to accelerate the player in surfer mode (units/s^2)
    [SerializeField] float surferModeAcceleration = 2.0f;
    // Amount to slow surfer mode speed over time
    [SerializeField] float surferModeDrag = 2f;
    [SerializeField] float surferModeFriction = 0.2f;
    Vector3 lastLiftoffDirection = new Vector3();
    [SerializeField] float surferSlideAmount = 1;
    [SerializeField] float surferGroundClearance = 0.2f;

    // # Turning
    // Forward direction of motion
    // Used to allow the character model to gradually turn to face this direction
    Quaternion forwardDirection;
    [Header("Turning")]
    // Speed the player turns in Surfer mode
    [SerializeField] float surferTurnRate = 5;
    // Speed at which character model turns towards direction of movement
    [SerializeField] float characterModelTurnRate = 2;
    // Threshold of input needed to make the character turn towards direction of movement
    [SerializeField] float characterTurnThreshold = 0.2f;
    
    // # Jumping
    [Header("Jumping")]
    // Upward velocity of a jump
    [SerializeField] float jumpImpulse = 10;
    // Double-Jumping
    // Number of jumps allowed (set in inspector)
    [SerializeField] int maxJumps = 2;
    // The jump we're currently on
    int jumpCount = 0;
    
    // # Air movement (WALKING)
    // Carried velocity whilst in air
    Vector2 airHorizontalMomentumVel;
    // The speed the player was moving when we last left the ground
    float lastGroundedSpeed;
    // Were we grounded last frame
    bool wasGrounded;
    [Header("Air Control (WALKING)")]
    // Mutliplier for maneuverability whilst in mid-air 
    [SerializeField] float airControl = 1.0f;
    // Minimum air maneuverability - this means you can still move when you jump from a stand-still
    [SerializeField] float minAirControl = 0.2f;
    // Speed at which initial jump/fall momentum "wears off"
    [SerializeField] float airSlowTime = 0.5f;
    // Time that the player has been in the air (not grounded)
    float airTime = 0;
    // Total air control input over this jump
    // Is set where relevant but currently isn't used.
    Vector2 cumulativeAirControl = new Vector2();
    
    [Header("Controls")]
    // "floatiness" of input - higher values will be snappier, lower values will be smoother.
    [SerializeField] float inputGravity;
    // Sensitivity for placeholder camera control
    // FIXME: remove this once we're on the proper camera controls
    [SerializeField] float mouseSensitivity = 1.0f;

    // Other
    static MovementMode mode;
    RaycastHit floorCast;
    [SerializeField] float floorCastDist = 4;

    private void Awake() {
        cc = GetComponent<CharacterController>();
        playerCamera = Camera.main;
        velocity = Vector3.zero;
        mode = MovementMode.Walking;
    }

    private void Update() {
        // Perform ground raycast at start of frame so it is only performed once.
        Physics.Raycast(transform.position, -transform.up, out floorCast, cc.height / 2 + floorCastDist);


        // INPUT COLLECTION
        // Get left stick / WSAD input
        Vector2 directMovementInput = GetMoveInput();
        // Glide movement towards the current input value
        movementInput.x = Mathf.MoveTowards(movementInput.x, directMovementInput.x, inputGravity * Time.deltaTime);
        movementInput.y = Mathf.MoveTowards(movementInput.y, directMovementInput.y, inputGravity * Time.deltaTime);
        // Get right stick / mouse input
        Vector2 lookInput = GetLookInput();

        // TODO: this is placeholder, we're obvs using the new input system and stuffs
        if (Input.GetButtonDown("Fire1")) {
            mode = (IsWalkingMode()) ? MovementMode.Surfer : MovementMode.Walking;
            if (IsSurferMode()) EnterSurfer();
        }

        // MOVEMENT HANDLING
        if (cc.isGrounded) {
            if (!wasGrounded) HandleLanding();
        } else {
            if (wasGrounded) {
                HandleLiftoff();
            }
            // TODO: shove this into a function so it's less messy in update ay
            airTime += Time.deltaTime;
            velocity.x = airHorizontalMomentumVel.x;
            velocity.z = airHorizontalMomentumVel.y;
            airHorizontalMomentumVel.x = Mathf.MoveTowards(airHorizontalMomentumVel.x, 0, airSlowTime * Time.deltaTime);
            airHorizontalMomentumVel.y = Mathf.MoveTowards(airHorizontalMomentumVel.y, 0, airSlowTime * Time.deltaTime);
            
            cumulativeAirControl += movementInput * Time.deltaTime;
            // movementInput = cumulativeAirControl;
        }
        // Apply movement functions for each mode
        ApplyGravity();
        if (IsWalkingMode()) {
            MoveWalking(movementInput * (cc.isGrounded ? 1 : airControl));
        } else if (IsSurferMode()) { // redundancy here in case there is eventually a "null" mode where movement is stopped entirely
            MoveSurfer(movementInput);
        }
        
        if (Input.GetKeyDown(KeyCode.Space)) {
            TryJump();
        }
        // FIXME: remove this once there are proper camera controls
        RotateCamera(lookInput);
        // Slowly rotate character towards forward motion direction
        characterObject.transform.rotation = Quaternion.RotateTowards(characterObject.transform.rotation, forwardDirection, characterModelTurnRate * Time.deltaTime);
        
        if (!cc.isGrounded) {
            // Clamp horizontal movement so that no excessive air control is possible
            Vector2 finalHorizontalMovement = Vector2.ClampMagnitude(new Vector2(velocity.x, velocity.z), lastGroundedSpeed);
            velocity.x = finalHorizontalMovement.x;
            velocity.z = finalHorizontalMovement.y;

        }

        // Update wasGrounded in advance of applying movement so we can catch if the CC was grounded before jumping
        // Also store this frame's velocity
        wasGrounded = cc.isGrounded;
        lastFrameVelocity = velocity;

        // Apply final movement
        cc.Move(velocity * Time.deltaTime);
        
    }

    #region Static motion functions

    public static void ApplyVelocity(Vector3 v) {
        velocity += v;
    }
    public static void SetVelocity(Vector3 v) {
        velocity = v;
    }
    public static void SetYVelocity(float yVel) {
        velocity.y = yVel;
    }

    // Modes
    // Some of this is a bit redundant but hoping it'll allow more easily readable code 
    // Especially for people designing level architecture who aren't as familiar with the movement code
    // e.g. 
    // instead of:  PlayerController.GetMovementMode() == PlayerController.MovementMode.Walking     which is kinda unwieldly
    // or:          PlayerController.isSurferMode == false                                          which is slightly confusing
    // now:         PlayerController.IsWalkingMode()
    // Yes we could make MovementMode a global enum but I kinda hate that when it's a glorified boolean
    // If anyone has a better solution then let me know

    public static MovementMode GetMovementMode() {
        return mode;
    }

    public static bool IsSurferMode() {
        return mode == MovementMode.Surfer;
    }
    
    public static bool IsWalkingMode() {
        return mode == MovementMode.Walking;
    }

    #endregion
    
    #region Walking

    // Movement function for adventure mode to be run on Update
    // TODO: trajectory doesn't change over the course of the flight as a result of held input - i.e. you let go and keep going the direction you were
    // should be solvable by making input hold to a cumulative value somehow
    // TODO: minAirControl doesn't quite seem to be working properly.
    public void MoveWalking(Vector2 input) {

        // Start by applying friction if we're on the ground
        if (cc.isGrounded) {
            WalkApplyFriction();
        }

        // Create motion vector
        Vector2 motion = CreateCameraRelativeMotionVector(input) * walkSpeed;
        Vector3 motion3d = new Vector3(motion.x, 0, motion.y);
        // Set target forward quaternion for character model to rotate towards
        // Only do this when magnitude is non-zero so that the rotation doesn't reset to world forward
        if (motion.sqrMagnitude > characterTurnThreshold)
            forwardDirection = Quaternion.LookRotation(motion3d, Vector3.up); 
        
        if (cc.isGrounded) {
            velocity.y = 0;
            if (floorCast.transform != null) {
                motion3d = Vector3.ProjectOnPlane(motion3d, floorCast.normal);
                velocity.y -= groundStickingForce;
            }
        }

        // Apply motion to velocity
        velocity += motion3d;

    }


    // Apply friction from ground
    // I was planning to include a system where landing from a jump carried some amount of air velocity, but I couldn't quite get this right
    // so instead I'm just straight up setting the velocity to 0.
    void WalkApplyFriction () {
        airHorizontalMomentumVel = Vector2.zero;
        velocity.x = 0;
        velocity.z = 0;
    }

    #endregion

    #region Surfing
    
    void EnterSurfer () {
        surferModeCarriedSpeed = velocity.magnitude;
    }

    // WARNING: THIS BIT IS VERY IN PROGRESS
    // AND BROKEN
    // LIKE FOR SOME REASON YOU SLIDE FASTER ON FLATS THAN ON STEEP HILLS
    // I'M WORKING ON IT SMH
    void MoveSurfer(Vector2 input) {

        Vector3 motionUnprojected = new();

        if (floorCast.transform != null) {
            Vector3 groundNormal = floorCast.normal;
            Vector3 groundForward = Vector3.Cross(forwardDirection * Vector3.right, groundNormal);
            // Get downhill vector
            Vector3 downhillDirection = Vector3.Cross(Vector3.Cross(groundNormal, Vector3.down), groundNormal).normalized;

            float slopeAngle = Mathf.Deg2Rad * Vector3.Angle(downhillDirection, Vector3.down);
            float slideAmount = Mathf.Abs(surferSlideAmount * Mathf.Cos(slopeAngle));

            forwardDirection = Quaternion.LookRotation(groundForward, groundNormal);

            //add a force to slide downhill
            motionUnprojected += downhillDirection * slideAmount;
        }
        
        forwardDirection *= Quaternion.Euler(0, input.x * surferTurnRate * Time.deltaTime, 0);
        motionUnprojected += forwardDirection * Vector3.forward * input.y * surferModeAcceleration* Time.deltaTime;

        if (cc.isGrounded){
            surferModeCurrentThrust += input.y * surferModeAcceleration * Time.deltaTime;
            velocity = Vector3.MoveTowards(velocity, Vector3.zero, surferModeFriction * Time.deltaTime);
            motionUnprojected.x += velocity.x;
            motionUnprojected.z += velocity.z;
            velocity.x = 0;
            velocity.z = 0;
        } else {
            Debug.DrawRay(transform.position, Vector3.up * 2, Color.magenta);
        }

        Vector3 motion = Vector3.Project(motionUnprojected, forwardDirection * Vector3.forward);

        velocity += motion;
        // Drag - apply constant drag
        // surferModeCarriedSpeed = Mathf.MoveTowards(surferModeCarriedSpeed, 0, surferModeDrag * Time.deltaTime);
        // // Friction - 

        // // Acceleration - input.y in the transform.forward
        // float accelerationAmount = input.y * surferModeAcceleration;
        // surferModeCarriedSpeed += accelerationAmount * Time.deltaTime;

        // Vector3 actualMotionDirection = lastLiftoffDirection;

        // // Set forward direction to floor angle
        // if (cc.isGrounded) {
        // } 

        // // Turn - input.x to rotate left/right

        // if (!cc.isGrounded) {
        //     Debug.Log("NOT GROUNDED");
        //     // Turn forward/backward
        //     forwardDirection *= Quaternion.Euler(input.y * surferTurnRate * Time.deltaTime, 0, 0);
        // }

        // Vector3 forwardVelocity = actualMotionDirection * surferModeCarriedSpeed;
        // // forwardVelocity.y += currentGravity; 
        // velocity = forwardVelocity;
    }

    #endregion

    #region AirMotion

    void ApplyGravity () {
        
        float thisFrameGravity = Physics.gravity.y * Time.deltaTime;

        velocity.y += thisFrameGravity;

        // Ensure while grounded we never go beyond the base gravity rate
        if (cc.isGrounded && velocity.y < 0) {
            velocity.y = thisFrameGravity;
        }
        
    }

    // Handle changes for when the player leaves the ground.
    // Should be called whenever the player leaves the ground or air control needs to be reset.
    void HandleLiftoff() {
        // Reset air control
        cumulativeAirControl = Vector2.zero;
        // Set air horizontal momentum vel to the current velocity
        airHorizontalMomentumVel = new(lastFrameVelocity.x, lastFrameVelocity.z);
        // Set last grounded speed to either the actual speed we last left the ground, or the min air control value
        lastGroundedSpeed = Mathf.Max(new Vector2(airHorizontalMomentumVel.x, airHorizontalMomentumVel.y).magnitude, minAirControl);

        lastLiftoffDirection = lastFrameVelocity.normalized;

        if (jumpCount == 0) jumpCount++;
    }

    // Handle changes for when the player hits the ground.
    // Should be called whenever the character controller becomes grounded.
    void HandleLanding() {
        jumpCount = 0;
        airTime = 0;
    }

    void TryJump () {
        if (jumpCount < maxJumps) {
            jumpCount++;
            Jump();
        }
    }

    void Jump() {
        velocity.y = jumpImpulse;
        HandleLiftoff();
    }

    #endregion

    #region Camera

    // Placeholder - basic camera rotation system    
    // TODO: delet this
    public void RotateCamera(Vector2 input) {
        cameraPivotTransform.transform.Rotate(new(input.y * mouseSensitivity, input.x * mouseSensitivity, 0), Space.World);
    }

    // Converts user-relative input (e.g. wsad) to a world-space vector that compensates for the angle of both the camera and character
    Vector2 CreateCameraRelativeMotionVector (Vector2 input) { return CreateCameraRelativeMotionVector(input.x, input.y); }
    Vector2 CreateCameraRelativeMotionVector (float x, float y) {
        // Get camera right and forward vectors to be used to determine the forward vector relative to the camera
        // For both of these vectors, we want to ignore the y axis and normalise
        Vector2 forward =       FlattenAndNormaliseTo2D(Vector3.ProjectOnPlane(GetCameraForwardVector(), Vector3.up));
        Vector2 right =         FlattenAndNormaliseTo2D(GetCameraRightVector());
        // Create lateral motion vector
        Vector2 motion = forward * y + right * x;
        return motion;
    }

    Vector3 GetCameraForwardVector () {
        return playerCamera.transform.forward;
    }

    Vector3 GetCameraRightVector() {
        return playerCamera.transform.right;
    }

    #endregion

    // Ignores the y component and normalizes to a VECTOR 2.
    // DOES NOT apply a max value - watch out for sqrt(2) diagonal motion
    Vector2 FlattenAndNormaliseTo2D (Vector3 v) {
        v.y = 0;
        v.Normalize();
        return new(v.x, v.z);
    }

    #region Input functions
    
    // Gets left stick / WSAD equivalent input as a Vector2.
    // Note already clamps so diagonal motion should never be >1 - don't do this again later pls.
    Vector2 GetMoveInput() {
        return Vector2.ClampMagnitude(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")), 1);
    }

    // Gets right stick / mouse equivalent input as a Vector2.
    Vector2 GetLookInput() {
        return new Vector2(Input.GetAxis("Cam X"), -Input.GetAxis("Cam Y"));
    }

    /*ADDITIONS - initiate new input system*/
    private void InitiateInputActions()
    {
        leftStickMove = playerControls.Player.Move;
        leftStickMove.Enable();
        //leftStickMove.performed += SwitchCamera;
    }

    #endregion

}
