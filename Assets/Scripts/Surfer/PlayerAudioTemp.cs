using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using Surfer.Input;
using UnityEngine;

/*THIS ENTIRE SCRIPT IS BODGE AND SHOULD BE DELETED ONCE FMOD IS FULLY INTEGRATED - CHECK UR DISCORD FOR ONCE FRANCISCO*/

public class PlayerAudioTemp : MonoBehaviour
{
    [SerializeField] AudioClip walkSFX;
    [SerializeField] AudioClip surfSFX;
    [SerializeField] AudioClip jumpSFX;
    [SerializeField] AudioClip switchSFX;
    [SerializeField] AudioSource source;

    PlayerControls playerControls;
    InputAction leftStickMove;
    InputAction jump;
    InputAction switchStyles;

    private CharacterController cc;

    // Start is called before the first frame update
    void Awake()
    {
        InitiateInputActions();
        cc = GetComponent<CharacterController>();
    }

    private void OnDisable()
    {
        leftStickMove.Disable();
        jump.Disable();
        jump.performed -= PlayJumpSFX;
        switchStyles.Disable();
        switchStyles.performed -= PlaySwitchSFX;
    }

    private void Update()
    {
        if(leftStickMove.ReadValue<Vector2>().magnitude > 0)
        {
            PlayMoveSFX();
        }
    }

    private void InitiateInputActions()
    {
        playerControls = new PlayerControls();

        leftStickMove = playerControls.Player.Move;
        leftStickMove.Enable();

        jump = playerControls.Player.Jump;
        jump.Enable();
        jump.performed += PlayJumpSFX;

        switchStyles = playerControls.Player.ChangeMode;
        switchStyles.Enable();
        switchStyles.performed += PlaySwitchSFX;
    }

    private void PlayMoveSFX()
    {
        if (!source.isPlaying && cc.isGrounded)
        {
            if (PlayerController.IsSurferMode())
            {
                source.clip = surfSFX;
                source.Play();
            }
            else
            {
                source.clip = walkSFX;
                source.Play();
            }
        }
        
    }

    private void PlayJumpSFX(InputAction.CallbackContext context)
    {
        if (cc.isGrounded)
        {
            if (!source.isPlaying)
            {
                source.clip = jumpSFX;
                source.Play();
            }
            else
            {
                source.Stop();
                source.clip = jumpSFX;
                source.Play();
            }
        }
    }

    private void PlaySwitchSFX(InputAction.CallbackContext context)
    {
        if (!source.isPlaying)
        {
            source.clip = switchSFX;
            source.Play();
        }
        else
        {
            source.Stop();
            source.clip = switchSFX;
            source.Play();
        }
    }

    public static void PlayClip(AudioSource source, AudioClip clip)
    {
        source.clip = clip;
        source.Play();
    }
}
