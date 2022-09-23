using System.Collections;
using System.Collections.Generic;
using Surfer.Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Surfer.Player
{
    public class PlayerAnimationController : MonoBehaviour
    {
        [Header("Component References")]
        //[SerializeField] private PlayerCharacter playerCharacter;
        [Header("Cinemachine Controls")]
        [SerializeField] private Animator cinemachineController;
        private bool isPlatforming;
        private PlayerControls controls;
        private CharacterController controller;

        [Header("Model Controls")]
        private Vector3 movementDirection;
        [SerializeField] private float rotateSpeed;
        [SerializeField] private Animator playerAnimationController;


        [Header("TEMPORARY DEFAULT UNITY AUDIO SHIT")]
        [SerializeField] private AudioSource playerAudio;
        [SerializeField] private AudioClip walkSFX;
        [SerializeField] private AudioClip jumpSFX;


        //Input Actions
        private InputAction switchCameras;
        private InputAction jumping;
        private InputAction running;

        private void OnEnable()
        {
            //controls.Player.ChangeMode.started += ChangeCamera;
            switchCameras = controls.Player.ChangeMode;
            switchCameras.Enable();
            switchCameras.performed += SwitchCamera;
            jumping = controls.Player.Jump;
            jumping.Enable();
            jumping.performed += Jump;
            jumping.performed += PlayJumpSFX;
            running = controls.Player.Move;
            running.Enable();
            running.performed += PlayWalkSFX;

            isPlatforming = true;
            movementDirection = Vector3.zero;
            //playerCharacter = GetComponent<PlayerCharacter>();
            controller = GetComponent<CharacterController>();
        }

        private void OnDisable()
        {
            switchCameras.Disable();
        }

        private void Awake()
        {
            controls = new PlayerControls();
        }

        private void Update()
        {
            //Set InTheAir parameter
            playerAnimationController.SetBool("InTheAir", !controller.isGrounded);
            if (isPlatforming)
            {
                playerAnimationController.SetBool("Running", running.IsInProgress());
            }
        }

        private void FixedUpdate()
        {
            //movementDirection = playerCharacter.GetCameraRelevantInput();
            //movementDirection.y = 0;
            //if (movementDirection != Vector3.zero)
            //{
            //    Quaternion rot = Quaternion.LookRotation(movementDirection);
            //    gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, rot, rotateSpeed * Time.deltaTime);
            //}
            
        }

        private void SwitchCamera(InputAction.CallbackContext ctx)
        {
            isPlatforming = !isPlatforming;
            //Change camera
            cinemachineController.SetBool("isPlatforming", isPlatforming);
            //Update player state
            if (!isPlatforming)
            {
                playerAnimationController.SetBool("Running", false);
                playerAnimationController.SetBool("Boarding", true);
            }
            else
            {
                playerAnimationController.SetBool("Running", true);
                playerAnimationController.SetBool("Boarding", false);
            }
        }

        private void Jump(InputAction.CallbackContext ctx)
        {
            playerAnimationController.SetBool("Jumping", true);
        }

        /*TEMP FUNCTIONS - BASIC UNITY AUDIO FOR NOW*/
        private void PlayWalkSFX(InputAction.CallbackContext ctx)
        {
            if (controller.isGrounded && isPlatforming)
            {
                if (!playerAudio.isPlaying)
                {
                    playerAudio.clip = walkSFX;
                    playerAudio.Play();
                }
            }
        }

        private void PlayJumpSFX(InputAction.CallbackContext ctx)
        {
            if (controller.isGrounded)
            {
                if ((!playerAudio.isPlaying) || (playerAudio.isPlaying && playerAudio.clip == walkSFX))
                {
                    playerAudio.clip = jumpSFX;
                    playerAudio.Play();
                }
            }
        }
    }
}