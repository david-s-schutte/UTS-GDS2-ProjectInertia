using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] private Animator cinemachineController;

    [Header("Adventure Control Animations")]
    [SerializeField] private AnimationClip idleAnim;
    [SerializeField] private AnimationClip walkAnim;

    private void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
