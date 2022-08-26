using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] private Animator animator;

    [Header("Adventure Control Animations")]
    [SerializeField] private AnimationClip idleAnim;
    [SerializeField] private AnimationClip walkAnim;

    private void Start()
    {
        animator = GameObject.FindWithTag("Player").GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        //Get player input
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput);
        //Set playerInput in animatorController
        animator.SetFloat("playerInput", movementDirection.magnitude);
    }
}
