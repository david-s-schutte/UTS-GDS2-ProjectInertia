using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Surfer.Input;
namespace Surfer.UI
{
    public class NewPauseMenu : MonoBehaviour
    {
        private PlayerControls playerControls;
        private InputAction pause;

        public static bool isPaused;
        [SerializeField] private GameObject pauseUI;
    
    // Start is called before the first frame update
        void Awake()
        {
            playerControls = new PlayerControls();
        }

        private void OnEnable()
        {
            pause = playerControls.Player.Pause;
            pause.Enable();

            pause.performed += Pause;
        }

        private void OnDisable()
        {
            pause.Disable();
        }

        public void Pause(InputAction.CallbackContext context)
        {
            isPaused = !isPaused;
            if (isPaused)
            {
                ActivateMenu();
            }
            else
            {
                DeactivateMenu();
            }
        }

        public void ActivateMenu()
        {
            Time.timeScale = 0;
            pauseUI.SetActive(true);
        }

        public void DeactivateMenu()
        {
            Time.timeScale = 1;
            pauseUI.SetActive(false);
            isPaused = false;
        }
    }
}