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
        [SerializeField] private PlayerCharacter playerCharacter;
        [Header("Cinemachine Controls")]
        [SerializeField] private Animator cinemachineController;
        private bool isPlatforming;
        private PlayerControls controls;

        [Header("Model Controls")]
        private Vector3 movementDirection;
        [SerializeField] private float rotateSpeed;

        //Input Actions
        private InputAction switchCameras;

        private void OnEnable()
        {
            //controls.Player.ChangeMode.started += ChangeCamera;
            switchCameras = controls.Player.ChangeMode;
            switchCameras.Enable();
            switchCameras.performed += SwitchCamera;

            isPlatforming = true;
            movementDirection = Vector3.zero;
            playerCharacter = GetComponent<PlayerCharacter>();
        }

        private void OnDisable()
        {
            switchCameras.Disable();
        }

        private void Awake()
        {
            controls = new PlayerControls();
        }

        private void FixedUpdate()
        {
            movementDirection = playerCharacter.GetCameraRelevantInput();
            movementDirection.y = 0;
            if (movementDirection != Vector3.zero)
            {
                Quaternion rot = Quaternion.LookRotation(movementDirection);
                gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, rot, rotateSpeed * Time.deltaTime);
            }
            
        }

        private void SwitchCamera(InputAction.CallbackContext ctx)
        {
            isPlatforming = !isPlatforming;
            cinemachineController.SetBool("isPlatforming", isPlatforming);
        }
    }
}