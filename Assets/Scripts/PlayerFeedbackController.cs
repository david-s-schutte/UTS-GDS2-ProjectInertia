using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFeedbackController : MonoBehaviour
{
    static PlayerFeedbackController Instance;
    [SerializeField] Animator animator;

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
    }

    public static void UpdateMoveAmount (float inputSqrMagnitude) {
        Instance.animator.SetFloat("InputAmount", inputSqrMagnitude);
    }

    public static void UpdateGrounded(bool isGrounded) {
        Instance.animator.SetBool("InTheAir", !isGrounded);
    }

    public static void OnChangeMovementMode() {
        Instance.animator.SetBool("Boarding", PlayerController.IsSurferMode());
    }

    public static void OnJump() {
        if (!Instance.animator.GetBool("InTheAir"))
            Instance.animator.SetTrigger("Jump");
        if (Instance.animator.GetBool("InTheAir"))
            Instance.animator.SetTrigger("Double Jump");
    }

}
