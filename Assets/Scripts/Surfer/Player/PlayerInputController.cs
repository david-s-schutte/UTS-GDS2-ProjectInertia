using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Surfer.Input;

public class PlayerInputController : MonoBehaviour {
    static PlayerControls playerControls;
    static InputAction leftStickMove;
    static InputAction lookInputAction;
    static InputAction jump;
    static InputAction switchStyles;

    static Vector2 lastMove;

    private void Awake() {
        InitiateInputActions();
    }

    public static void CheckInitialised() {
        if (playerControls == null) {
            InitiateInputActions();
        }
    }

    static void InitiateInputActions()
    {
        playerControls = new PlayerControls();

        leftStickMove = playerControls.Player.Move;
        leftStickMove.Enable();

        lookInputAction = playerControls.Player.Look;
        lookInputAction.Enable();

        jump = playerControls.Player.Jump;
        jump.Enable();

        switchStyles = playerControls.Player.ChangeMode;
        switchStyles.Enable();
    }

    private void OnDisable()
    {
        leftStickMove.Disable();
        lookInputAction.Disable();
        jump.Disable();
        switchStyles.Disable();
    }

    public static Vector2 GetMoveInput() {
        // sometimes this doesn't work and i don't know why
        CheckInitialised();
        // idfk why but this doesn't work unless i read the value twice.....
        Vector2 input = Vector2.ClampMagnitude(leftStickMove.ReadValue<Vector2>(), 1);
        //Debug.Log("returning: " + Vector2.ClampMagnitude(leftStickMove.ReadValue<Vector2>(), 1) );//+ ", playercontrols: " + playerControls );//+ ", move action: " + leftStickMove);
        return Vector2.ClampMagnitude(leftStickMove.ReadValue<Vector2>(), 1);
    }

    public static Vector2 GetLookInput() {
        CheckInitialised();
        return lookInputAction.ReadValue<Vector2>();
    }

    public static bool GetJumpDown() {
        return jump.WasPressedThisFrame();
    }

    public static bool GetSwitchDown() {
        return switchStyles.WasPressedThisFrame();
    }


}
