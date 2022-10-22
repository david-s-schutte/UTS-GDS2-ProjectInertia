using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Surfer.Input;

public class PlayerInputController {
    static PlayerControls playerControls;
    static InputAction leftStickMove;
    static InputAction jump;
    static InputAction switchStyles;

    static Vector2 lastMove;

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

        jump = playerControls.Player.Jump;
        jump.Enable();

        switchStyles = playerControls.Player.ChangeMode;
        switchStyles.Enable();
    }

    static void ProcessMoveInput(InputAction.CallbackContext ctx) {
        lastMove = ctx.ReadValue<Vector2>();
    }

    public static Vector2 GetMoveInput() {
        return Vector2.ClampMagnitude(lastMove, 1);
    }

    public static bool GetJumpDown() {
        return jump.WasPressedThisFrame();
    }


}
