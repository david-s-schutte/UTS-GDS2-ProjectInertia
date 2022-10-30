using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using Surfer.Input;
using Surfer.Managers;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class PlayerController : MonoBehaviour
{

    // if my future employers see this region i am fucked
    #region variables

    static PlayerController Instance;
    public enum MovementMode {Walking, Surfer, Grinding, Stopped};

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
    
    #region Events for Audio Purposes
    public delegate void ModeChanged(float currentSpeed, bool isSurfer);
    public ModeChanged OnModeChanged;

    public delegate void GrindState(bool grindStarted);
    public GrindState OnGrindStateUpdated;
    
    public delegate void JumpOccured();
    public JumpOccured OnJumpOccured;

    
    public CharacterController Controller => cc;


    #endregion
    
    // # Movement
    [Header("Movement")]
    // Speed of player walking
    [SerializeField] float walkSpeed = 5;
    // Current movement input (left stick)
    Vector2 movementInput;
    // Current velocity. Modifying this value will instantly change velocity (but please use static function SetVelocity)
    private static Vector3 velocity;
    // Velocity we were travelling at last frame
    static Vector3 lastFrameVelocity = new Vector3();

    [SerializeField] float runningStartBoost = 1.5f;

    [SerializeField] float groundStickingForce = 4.0f;

    // Railgrind stuff
    // TODO: I reckon this should be somewhere else and this script should mostly be reserved for direct player movement
    [SerializeField] Vector3 railOffset = new(0, 1, 0);
    [SerializeField] float railSpeed = 5;
    [SerializeField] float railPushAmount = 5; // Amount you can speed up on rails by holding forward
    [SerializeField] float railLeaveBoost = 5;

    // Speed carried between frames
    float surferModeCarriedSpeed = 0;
    float surferModeCurrentThrust = 0;
    [SerializeField] float surferModeThrustEaseOff = 0.95f;
    // Speed to accelerate the player in surfer mode (units/s^2)
    [SerializeField] float surferModeAcceleration = 2.0f;
    // braking divisor innit
    [SerializeField] float surferModeBrakeAmount = 2.0f;
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
    [SerializeField] bool onlySingleJumpInSurfer = true;
    // The jump we're currently on
    int jumpCount = 0;

    Vector3 cumulativeMidairRotation;

    // # Air movement (WALKING)
    // Carried velocity whilst in air
    Vector2 airHorizontalMomentumVel;
    // The speed the player was moving when we last left the ground
    float lastGroundedSpeed;
    // Were we grounded last frame
    bool wasGrounded;
    [Header("Air Control (WALKING)")]
    // Mutliplier for maneuverability whilst in mid-air
    [SerializeField] float airControl = 6;
    // Speed that air control builds up when you hold a key
    [SerializeField] float airControlSpeed = 12.0f;
    // Direct input transfered to air movement - this means you can still move when you jump from a stand-still
    [SerializeField] float directAirControl = 3;
    // Speed at which initial jump/fall momentum "wears off"
    [SerializeField] float airSlowTime = 0.5f;
    // Time that the player has been in the air (not grounded)
    float airTime = 0;
    // Total air control input over this jump
    // Is set where relevant but currently isn't used.
    Vector2 cumulativeAirControl = new Vector2();

    [Header("Collisions")]
    // Distance for raycast targetting the floor
    [SerializeField] float floorCastDist = 4;
    // Values for boxcast targetting floor obstacles, like railgrinds
    [SerializeField] float boxCastSize = 1;
    [SerializeField] float boxCastDistance = 1;
    [SerializeField] float maxRampDegrees; // maximum degrees to allow player to continue sliding up a ramp (otherwise will treat like a wall)

    [Header("Controls")]
    // "floatiness" of input - higher values will be snappier, lower values will be smoother.
    [SerializeField] float inputGravity;
    // Sensitivity for placeholder camera control
    // FIXME: remove this once we're on the proper camera controls
    [SerializeField] float mouseSensitivity = 1.0f;

    // Other
    static MovementMode mode;
    RaycastHit floorCast;


    // TODO: This should not be chillin in playercontroller lmao
    [Header("Tricks")]
    [SerializeField] Trick trick180;
    [SerializeField] Trick trick360;
    [SerializeField] Trick trickFrontFlip;
    [SerializeField] Trick trickBackFlip;
    [SerializeField] Trick trickGrind;
    [SerializeField] Trick trickAir;
    // the frequency to input held tricks like grinding or airtime
    // TODO: tricksystem should handle this
    [SerializeField] int timedTricksFreq = 1;
    float airTimeTrickTimer;
    [SerializeField] float airTimeTrickThreshold = 4;
    // Number of degrees off from a full flip that still counts
    [SerializeField] float frontFlipFudgeAngle = 20;
    // Angle at which the player has cocked up and had a crash innit
    [SerializeField] float crashAngle = 90;

    [SerializeField] string[] flipTrickPrefixes = { "", "Double", "Triple", "Quadruple", "Quintuple", "Sextuple", "Septuple", "Octuple", "Nonuple", "Decuple" };

    bool collisionThisAirTime = false;

    TrickSystem trickSystem;

    #endregion

    #region Just adding on it really, love it

    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _currentPauseMenuInstance;

    #endregion

    private void Awake() {
        Instance = this;
        cc = GetComponent<CharacterController>();

        playerCamera = Camera.main;
        velocity = lastFrameVelocity = Vector3.zero;
        PlayerInputController.CheckInitialised();
        trickSystem = Managers.ManagerLocator.Get<TrickSystem>();
    }

    private void Start() {
        GameCameraController.InitialiseGameCamera();
        SetMovementMode(MovementMode.Walking);
        this.enabled = true;
        PlayerInputController.playerControls.Player.Pause.performed += OpenClosePauseMenu;
    }
    
    private void OnEnable()
    {
        PlayerInputController.CheckInitialised();

    }

    private void OnDisable()
    {
        
        PlayerInputController.playerControls.Player.Pause.performed -= OpenClosePauseMenu;
        PlayerInputController.UnInitialiseInput();

    }

    private void Update() {

        print("test");
        // Perform ground raycast at start of frame so it is only performed once.
        Physics.Raycast(transform.position, -transform.up, out floorCast, cc.height / 2 + floorCastDist);
        // Perform box cast for floor obstacles omg what a sick function name
        if ((IsSurferMode() || IsWalkingMode()) && IsFalling()) {
            BoxCastForFloorObstacles();
        }

        // INPUT COLLECTION
        // Get left stick / WSAD input
        Vector2 directMovementInput = GetMoveInput();
        // Glide movement towards the current input value
        movementInput.x = Mathf.MoveTowards(movementInput.x, directMovementInput.x, inputGravity * Time.deltaTime);
        movementInput.y = Mathf.MoveTowards(movementInput.y, directMovementInput.y, inputGravity * Time.deltaTime);

        PlayerFeedbackController.UpdateMoveAmount(movementInput.sqrMagnitude);

        // Slowly rotate character towards forward motion direction
        characterObject.transform.rotation = Quaternion.RotateTowards(characterObject.transform.rotation, forwardDirection, characterModelTurnRate * Time.deltaTime);

        if (mode == MovementMode.Grinding)
        {
            PlayerFeedbackController.UpdateGrounded(true);
            PlayerFeedbackController.SetGrind(true);
            return;
        }
        else PlayerFeedbackController.SetGrind(false);

        PlayerFeedbackController.UpdateGrounded(IsGrounded());

        if (PlayerInputController.GetSwitchDown()) {
            SetMovementMode((IsWalkingMode()) ? MovementMode.Surfer : MovementMode.Walking);
            if (IsSurferMode()) EnterSurfer(velocity.magnitude * (1 + runningStartBoost / 10.0f));
            if (IsWalkingMode()) EnterWalking();
        }

        // MOVEMENT HANDLING
        if (IsGrounded()) {
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

            airTimeTrickTimer += Time.deltaTime;
            if (airTimeTrickTimer > timedTricksFreq) {
                if (airTime > airTimeTrickThreshold) trickSystem.InputTrick(trickAir); // this is kinda dumb ay
                airTimeTrickTimer = 0;
            }
            // movementInput = cumulativeAirControl;

        }
        // Apply movement functions for each mode
        ApplyGravity();
        if (IsWalkingMode()) {
            MoveWalking(movementInput);
        } else if (IsSurferMode()) { // redundancy here in case there is eventually a "null" mode where movement is stopped entirely
            MoveSurfer(movementInput);
        }

        if (GetJumpDown()) {
            TryJump();
        }

        if (!IsGrounded()) {
            // Clamp horizontal movement so that no excessive air control is possible
            Vector2 finalHorizontalMovement = Vector2.ClampMagnitude(new Vector2(velocity.x, velocity.z), lastGroundedSpeed);
            velocity.x = finalHorizontalMovement.x;
            velocity.z = finalHorizontalMovement.y;
        }

        // Update wasGrounded in advance of applying movement so we can catch if the CC was grounded before jumping
        // Also store this frame's velocity
        wasGrounded = IsGrounded();
        lastFrameVelocity = velocity;

        // Apply final movement
        cc.Move(velocity * Time.deltaTime);

    }

    public void OpenClosePauseMenu(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed)
            return;

        if (_currentPauseMenuInstance == null)
         _currentPauseMenuInstance = Instantiate(_pauseMenu);
    }

  
    void SetMovementMode (MovementMode newMode)
    {
        MovementMode currentMode = mode;

        if (currentMode == MovementMode.Grinding)
            OnGrindStateUpdated?.Invoke(false);
        
        mode = newMode;
        switch (mode) {
            case MovementMode.Walking :
                EnterWalking(); break;
            case MovementMode.Surfer :
                EnterSurfer(); break;
            case MovementMode.Grinding : 
                OnGrindStateUpdated?.Invoke(true);
                break;
            case MovementMode.Stopped :
                break;
        }
        PlayerFeedbackController.OnChangeMovementMode();
    }

    // TODO: this function is mostly just so jumppads can set the player's mode to walking
    // this probs shouldn't be how this is handled if we do any further development
    public static void SetWalkingMode() {
        Instance.SetMovementMode(MovementMode.Walking);
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

    public static void SurfBoost (float boostSpeed, Quaternion forward, bool additive) {
        if (!IsSurferMode()) {
            Instance.SetMovementMode(MovementMode.Surfer);
        }
        Instance.forwardDirection = forward;
        velocity = lastFrameVelocity = Vector3.zero;
        // set to boost speed or add current thrust if additive mode
        Instance.surferModeCurrentThrust = boostSpeed + (additive ? Instance.surferModeCurrentThrust : 0);
    }

    public static bool IsFalling () {
        if (mode == MovementMode.Grinding) return false;
        return velocity.y <= 0;
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
        return mode == MovementMode.Surfer || mode == MovementMode.Grinding;
    }

    public static bool IsWalkingMode() {
        return mode == MovementMode.Walking;
    }

    #endregion

    #region Walking

    void EnterWalking() {
        GameCameraController.EnterWalkCam();
        // If we aren't grounded then act like a liftoff so that momentum is properly carried
        if (!IsGrounded()) {
            HandleLiftoff();
        }
    }

    // Movement function for adventure mode to be run on Update
    public void MoveWalking(Vector2 input) {

        OnModeChanged?.Invoke(cc.velocity.magnitude, false);

        
        // Start by applying friction if we're on the ground
        if (IsGrounded()) {
            WalkApplyFriction();
        } else {
            // cumulativeAirControl = CreateCameraRelativeMotionVector(input) * airControlSpeed;
            cumulativeAirControl += CreateCameraRelativeMotionVector(input) * Time.deltaTime * airControlSpeed;
        }

        // Create motion vector
        Vector2 motion = CreateCameraRelativeMotionVector(input) * walkSpeed;
        if (!IsGrounded()) {
            motion *= directAirControl;
            motion += cumulativeAirControl * airControl;
        }
        Vector3 motion3d = new Vector3(motion.x, 0, motion.y);
        // Set target forward quaternion for character model to rotate towards
        // Only do this when magnitude is non-zero so that the rotation doesn't reset to world forward
        if (input.sqrMagnitude > characterTurnThreshold)
            forwardDirection = Quaternion.LookRotation(motion3d, Vector3.up);

        if (IsGrounded()) {
            if (velocity.y <= 0) velocity.y = 0;
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
        EnterSurfer(velocity.magnitude);
    }
    void EnterSurfer (float overrideCarriedSpeed) {
        // Force forward direction to be camera forward.
        if (IsGrounded()) forwardDirection = Quaternion.LookRotation(FlattenAndNormalise3D(GetCameraForwardVector()), Vector3.up);

        GameCameraController.EnterSurfCam();
        surferModeCarriedSpeed = overrideCarriedSpeed;
        surferModeCurrentThrust = 0;
    }

    // WARNING: THIS BIT OF CODE WAS ACTUALLY WRITTEN BY A SMALL ALBINO GERBIL AND IS NOT VERY GOOD AT ALL WHEN COMPARED TO THAT WRITTEN BY HUMANS
    void MoveSurfer(Vector2 input) {

        // Vector3 motionUnprojected = new();

        OnModeChanged?.Invoke(cc.velocity.magnitude, true);

        // Perform y rotation (turning) to turnRotation that will carry the total controlled turn for this frame.
        Quaternion turnRotation = Quaternion.identity;
        turnRotation *= Quaternion.Euler(0, input.x * surferTurnRate * Time.deltaTime, 0);
        cumulativeMidairRotation += Vector3.up * input.x * surferTurnRate * Time.deltaTime; // Add to the cumulative rotation for tricks
        
        // If we're in range of the floor then either rotate to follow the surface or prepare to land
        if (floorCast.transform != null) {
            Vector3 groundNormal = floorCast.normal;
            Vector3 groundForward = Vector3.Cross(forwardDirection * Vector3.right, groundNormal);
            // Get downhill vector
            Vector3 downhillDirection = Vector3.Cross(Vector3.Cross(groundNormal, Vector3.down), groundNormal).normalized;

            float slopeAngle = Mathf.Deg2Rad * Vector3.Angle(downhillDirection, Vector3.down);
            float slideAmount = Mathf.Abs(surferSlideAmount * Mathf.Cos(slopeAngle));

            forwardDirection = Quaternion.LookRotation(groundForward, groundNormal);

            //add a force to slide downhill - disabled to get something workable for the sprint submission
            // motionUnprojected += downhillDirection * slideAmount;
            
        } else {

            // Rotating on the X axis when we're not in range of the floor
            if (!IsGrounded() && GetGrabButton()) {
                turnRotation *= Quaternion.Euler(input.y * surferTurnRate * Time.deltaTime, 0, 0);
                // Note: while Y is always counted for cumulative rotation, X is only counted when in floor range.
                // This is cause Y rotation isn't affected by the floorCast result while X rotation is
                cumulativeMidairRotation += Vector3.right * input.y * surferTurnRate * Time.deltaTime;
            }

        }

        // Apply final turn rotation
        forwardDirection *= turnRotation;


        // Debug.DrawRay(transform.position, forwardDirection * Vector3.forward, Color.green);

        Vector3 lateralForward = forwardDirection * Vector3.forward;
        lateralForward.y = 0;
        lateralForward.Normalize();
        // motionUnprojected += forwardDirection * Vector3.forward * input.y * surferModeAcceleration* Time.deltaTime;

        // This is a total mess due to getting it workable for sprint 3

        if (!IsGrounded()) lateralForward = FlattenAndNormalise3D(lastLiftoffDirection);

        Vector3 motion = new();
        motion += lateralForward * surferModeCurrentThrust;

        // Preconfigure thrust and brake input amounts
        float thrustInput = Mathf.Max(0, input.y);
        float brakeInput = Mathf.Abs(Mathf.Min(0, input.y));
        
        surferModeCurrentThrust *= Mathf.Lerp(Mathf.Pow(surferModeThrustEaseOff, Time.deltaTime), 1, Mathf.Abs(thrustInput));
        


        if (IsGrounded()){
            if (surferModeCarriedSpeed > 0) {
                // Debug.Log("applied carried speed of " + surferModeCarriedSpeed);
                surferModeCurrentThrust = surferModeCarriedSpeed;
                surferModeCarriedSpeed = 0;
            }
            surferModeCurrentThrust += thrustInput * surferModeAcceleration * Time.deltaTime;
            if (brakeInput > 0) surferModeCurrentThrust -= Time.deltaTime * Mathf.Log(surferModeBrakeAmount) * surferModeCurrentThrust;
            surferModeCurrentThrust = Mathf.Max(surferModeCurrentThrust , 0);
            velocity = Vector3.MoveTowards(velocity, Vector3.zero, surferModeFriction * Time.deltaTime);
            // motionUnprojected.x += velocity.x;
            // motionUnprojected.y += velocity.y;
            // motionUnprojected.z += velocity.z;
            velocity.x = 0;
            velocity.y = -groundStickingForce;
            velocity.z = 0;
        } else {
            Debug.DrawRay(transform.position, Vector3.up * 2, Color.magenta);
        }

        // motion += Vector3.Project(motionUnprojected, lateralForward);

        if (IsGrounded()) {
            motion = Vector3.Project(motion, forwardDirection * Vector3.forward);
            // this is dreadful BUT
            // applies the speed when we switched to surfer mode then immediately resets that speed so it's only applied once 8-)
        } else {
            // if we haven't jumped, use last liftoff as the motion direction
            if (jumpCount == 0)
                motion = Vector3.Project(motion, lastLiftoffDirection);
        }

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
        if (IsGrounded() && velocity.y < 0) {
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
        lastGroundedSpeed = Mathf.Max(new Vector2(airHorizontalMomentumVel.x, airHorizontalMomentumVel.y).magnitude, directAirControl);

        lastLiftoffDirection = lastFrameVelocity.normalized;

        // reset airtime timer
        airTimeTrickTimer = 0;

        if (jumpCount == 0) jumpCount++;
    }

    // Handle changes for when the player hits the ground.
    // Should be called whenever the character controller becomes grounded.
    void HandleLanding() {
        jumpCount = 0;
        airTime = 0;

        LandingTricks(cumulativeMidairRotation);
        // Reset midair rotation value
        cumulativeMidairRotation = Vector3.zero;
    }

    void LandingTricks (Vector3 rotation) {
        // If we've had a collision, ignore this landing
        if (collisionThisAirTime) {
            collisionThisAirTime = false;
            return;
        }

        // 180s, 360s, and beyond
        // The number of turns (180s) we've done this landing
        int halfTurns = Mathf.Abs(Mathf.FloorToInt(rotation.y / 180.0f));
        if (halfTurns >= 1) {
            Trick rotationTrick = new Trick();
            rotationTrick.BaseScore = trick180.BaseScore * halfTurns;
            rotationTrick.TrickName = (halfTurns * 180).ToString();
            trickSystem.InputTrick(rotationTrick);
        }

        // Flips
        // If any, the number of flips we achieved this jump
        int flips = Mathf.FloorToInt(Mathf.Abs(rotation.x - frontFlipFudgeAngle) / 360.0f);
        if (flips >= 1) {
            bool frontFlip = rotation.x > 0;
            Trick baseFlipTrick = frontFlip ? trickFrontFlip : trickBackFlip;

            Trick flipTrick = new Trick();
            flipTrick.BaseScore = baseFlipTrick.BaseScore * flips;

            // Generate the trick's name with a prefix based on how many flips were achieved
            string trickName = "";
            if (flips > 1) {
                if (flipTrickPrefixes.GetLength(0) > flips - 1) {
                    trickName = flipTrickPrefixes[flips - 1] + " ";
                } else {
                    trickName = flips + "x ";
                }
            }
            trickName += baseFlipTrick.name;
            flipTrick.TrickName = trickName;
            // Debug.Log(trickName);
    
            trickSystem.InputTrick(flipTrick);
        }
    }

    void TryJump () {
        // Disable multi-jumping if in surfer mode
        if (IsSurferMode() && onlySingleJumpInSurfer) {
            if (jumpCount > 0) return;
        }

        if (jumpCount < maxJumps) {
            jumpCount++;
            Jump();
        }
    }

    void Jump() {
        OnJumpOccured?.Invoke();
        PlayerFeedbackController.OnJump();
        velocity.y = jumpImpulse;  //TODO: experiment with making jump relative to local up instead of world up (e.g. for half pipes)
        HandleLiftoff();
    }

    #endregion

    #region Collisions

    bool IsGrounded() {
        return cc.isGrounded;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit) {
        // forward direction converted into player normal
        Vector3 normalDirection = (forwardDirection * Quaternion.Euler(-90 * transform.right)) * Vector3.forward;
        // Debug.DrawRay(transform.position, hit.normal, Color.yellow);
        // Debug.DrawRay(transform.position, normalDirection, Color.red);
        // Angle that we'd need to change by if we treat this is a ramp
        float angleDif = Vector3.Angle(hit.normal, normalDirection);
        // Debug.Log(angleDif);

        if (hit.moveDirection.y > -0.3f && angleDif > maxRampDegrees) { // FIXME: arbitrary value for min move dir
            collisionThisAirTime = true;
            if (IsSurferMode()) {
                // Speed of thrust
                // Note DIVIDING by deltaTime to convert the single frame translation into a speed
                float hitSpeed = (hit.moveLength / Time.deltaTime);
                // The speed we'll retain from hitting the wall
                // On a direct hit with a wall (i.e. surfer perpendicular to wall) this should be 1.0
                // This value will approach 0 as the angle is closer to parallel.
                float reboundAmount = Mathf.Cos(Vector3.Angle(FlattenAndNormalise3D(hit.normal), FlattenAndNormalise3D(hit.moveDirection)) * Mathf.Deg2Rad);
                // Debug.Log(reboundAmount + ", " + hitSpeed);
                // Adjust the current thrust by the speed of impact multiplied by the amount retained based on the angle
                surferModeCurrentThrust += hitSpeed * reboundAmount;
                // Restrict thrust to not go below 0. This means backing into a wall will instantly stop
                // Mostly this is to prevent a weird chain reaction issue I was having. Could probably fix more properly
                surferModeCurrentThrust = Mathf.Max(surferModeCurrentThrust, 0);
                // Set direction to run along the wall in the closest direction to forward
                // FIXME: there's almost definitely a less scuffed way of doing this, this is just a way i know will give predictableish results
                Quaternion alongLeft = Quaternion.LookRotation(hit.normal, Vector3.up) * Quaternion.Euler(0, -90, 0);
                Quaternion alongRight = Quaternion.LookRotation(hit.normal, Vector3.up) * Quaternion.Euler(0, 90, 0);
                if (Quaternion.Angle(alongLeft, forwardDirection) > Quaternion.Angle(alongRight, forwardDirection)) {
                    forwardDirection = alongRight;
                } else {
                    forwardDirection = alongLeft;
                }
            }
        }
    }

    #endregion

    #region Camera

    // Converts user-relative input (e.g. wsad) to a world-space vector that compensates for the angle of both the camera and character
    Vector2 CreateCameraRelativeMotionVector (Vector2 input) { return CreateCameraRelativeMotionVector(input.x, input.y); }
    Vector2 CreateCameraRelativeMotionVector (float x, float y) {
        // Get camera right and forward vectors to be used to determine the forward vector relative to the camera
        // For both of these vectors, we want to ignore the y axis and normalise
        Vector2 forward =       FlattenAndNormalise2D(Vector3.ProjectOnPlane(GetCameraForwardVector(), Vector3.up));
        Vector2 right =         FlattenAndNormalise2D(GetCameraRightVector());
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

    #region Interaction

    void BoxCastForFloorObstacles() {
        RaycastHit boxHit;
        if (Physics.BoxCast(transform.position - Vector3.up * cc.height/2, new(boxCastSize, boxCastSize, boxCastSize), Vector3.down, out boxHit, forwardDirection, boxCastDistance)) {
            if (boxHit.transform.tag == "GrindrailNode") {
                GrindRailController grindRailController = boxHit.collider.gameObject.GetComponentInParent<GrindRailController>();
                if (grindRailController) EnterGrindRail(grindRailController);
            }
        }
    }

    public static void EnterGrindRail(GrindRailController grindRail) {
        if (grindRail) Instance.StartCoroutine(Instance.RailGrindCoroutine(grindRail));
    }

    // alpha - this is probs not how it'll work in the final game
    // hi me in six months finding this comment and facepalming :D
    // me later: actually this is kinda valid i might keep it like this........
    IEnumerator RailGrindCoroutine(GrindRailController grindRail) {
        SetMovementMode(MovementMode.Grinding);
        GameCameraController.EnterSurfCam();
        velocity = Vector3.zero;

        List<Transform> railPoints = grindRail.GetRemainingNodes(transform);
        // Debug.Log("landed on grindrail with " + railPoints.Count + " nodes");

        int node = 0;
        float grindTime = 0;
        
        // Initial rail grind trick reward
        trickSystem.InputTrick(trickGrind);

        while (node < railPoints.Count) {
            Vector3 startPosition = transform.position;
            float t = 0;
            Transform target = railPoints[node];
            // Calculate speed multiplier for this node transition (ensures node density doesn't affect speed)
            // divided by a value to make values similar to when i first designed the system innit
            float speedMultiplier = Vector3.SqrMagnitude(startPosition - target.position) / 10;
            // Debug.Log("on my way to node number " + node + " at " + target.position);
            while (t < 1) {
                transform.position = Vector3.Lerp(startPosition, target.position + railOffset, t);

                Vector3 nextNodeDirection = ((target.position + railOffset) - transform.position).normalized;
                if (node == 0) nextNodeDirection = FlattenAndNormalise3D(nextNodeDirection);
                forwardDirection = Quaternion.LookRotation(nextNodeDirection, Vector3.up);

                // Rail speed = base speed on rails
                // Push speed is added multiplied by the forward movement vector.
                t += Time.deltaTime * (railSpeed + railPushAmount * Mathf.Max(GetMoveInput().y, 0));
            
                // Handle awarding of trick
                // TODO: This won't give perfect results, ideally we should be using a system on TrickSystem where you can begin and end repeating tricks
                grindTime += Time.deltaTime;
                if (grindTime > timedTricksFreq) {
                    trickSystem.InputTrick(trickGrind);
                    grindTime = 0;
                }

                
                // Allow jumps to cancel the whole Coroutine
                if (GetJumpDown()) {
                    velocity = lastFrameVelocity = Vector3.zero;
                    HandleLiftoff();
                    Jump();
                    break;
                }
                yield return null;
            }
            if (GetJumpDown()) {
                break;
            }
            transform.position = target.position + railOffset;
            node++;
        }

        // If we've just run out of nodes and not jumped off, we'll change to surfer mode. If we've jumped off, change to walking.
        SetMovementMode(!GetJumpDown() ? MovementMode.Surfer : MovementMode.Walking);
        if (!GetJumpDown()) surferModeCarriedSpeed = railLeaveBoost;

    }

    #endregion


    Vector3 FlattenAndNormalise3D (Vector3 v) {
        v.y = 0;
        v.Normalize();
        return v;
    }
    // Ignores the y component and normalizes to a VECTOR 2.
    // DOES NOT apply a max value - watch out for sqrt(2) diagonal motion
    Vector2 FlattenAndNormalise2D (Vector3 v) {
        v.y = 0;
        v.Normalize();
        return new(v.x, v.z);
    }

    #region Input functions

    // Gets left stick / WSAD equivalent input as a Vector2.
    // Note already clamps so diagonal motion should never be >1 - don't do this again later pls.
    Vector2 GetMoveInput() {
        return PlayerInputController.GetMoveInput();
    }

    bool GetJumpDown () {
        // return Input.GetKeyDown(KeyCode.Space);
        return PlayerInputController.GetJumpDown();
    }

    bool GetGrabButton () {
        return Input.GetKey(KeyCode.LeftControl);
    }

    #endregion
}
