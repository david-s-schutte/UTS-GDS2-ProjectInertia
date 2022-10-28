using System;
using System.Collections;
using System.Collections.Generic;
using Surfer.Player;
using UnityEngine;

public class PlayerFeedbackController : MonoBehaviour
{
    static PlayerFeedbackController Instance;
    [SerializeField] Animator animator;
    [SerializeField] PlayerAnimationController _animationController;

    public PlayerController playerController;
    
    private static readonly int Intro = Animator.StringToHash("Intro");
    private static readonly int InputAmount = Animator.StringToHash("InputAmount");
    private static readonly int InTheAir = Animator.StringToHash("InTheAir");
    private static readonly int Boarding = Animator.StringToHash("Boarding");
    private static readonly int Jump = Animator.StringToHash("Jump");
    private static readonly int Property = Animator.StringToHash("Double Jump");
    private static readonly int Grinding = Animator.StringToHash("Grinding");
    private static readonly int Grind = Animator.StringToHash("Grind");

    private void Awake() {
        if (!animator) {
            Debug.Log("Player animator unset in inspector");
            animator = GetComponent<Animator>();
        }

        if (Instance) {
            Debug.LogWarning("Multiple instances of PlayerFeedbackController???? Tim Allen grunt?????");
            Destroy(Instance);
        }
        Instance = this;

        playerController.enabled = false;

    }

    private void Start()
    {
        _animationController.OnPlayerIntroCompleted += (() =>
        {
            playerController.enabled = true;
            Instance.animator.SetBool(Intro, false);
        });
    }


    public static void UpdateMoveAmount (float inputSqrMagnitude) {
        Instance.animator.SetFloat(InputAmount, inputSqrMagnitude);
    }

    public static void UpdateGrounded(bool isGrounded) {
        Instance.animator.SetBool(InTheAir, !isGrounded);
    }

    public static void OnChangeMovementMode() {
        Instance.animator.SetBool(Boarding, PlayerController.IsSurferMode());
    }

    public static void OnJump() {
        if (!Instance.animator.GetBool(InTheAir))
            Instance.animator.SetTrigger(Jump);
        if (Instance.animator.GetBool(InTheAir))
            Instance.animator.SetTrigger(Property);
    }

    public static void SetGrind(bool isGrinding)
    {
        if (isGrinding)
        {
            if (!Instance.animator.GetBool(Grinding))
                Instance.animator.SetTrigger(Grind);
        }
        Instance.animator.SetBool(Grinding, isGrinding);
    }

}
