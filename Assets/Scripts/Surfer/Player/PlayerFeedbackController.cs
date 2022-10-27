using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFeedbackController : MonoBehaviour
{
    static PlayerFeedbackController Instance;
    [SerializeField] Animator animator;
    public PlayerController playerController;

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

    public void Update()
    {
        if (animator.GetBool("Intro") == true)
        {
            playerController.enabled = false;
        }
        if (animator.GetBool("Intro") == false)
        {
            playerController.enabled = true;
        }
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

    public static void SetGrind(bool isGrinding)
    {
        if (isGrinding)
        {
            if (!Instance.animator.GetBool("Grinding"))
                Instance.animator.SetTrigger("Grind");
        }
        Instance.animator.SetBool("Grinding", isGrinding);
    }

}
