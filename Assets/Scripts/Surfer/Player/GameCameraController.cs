using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Surfer.Input;
using Cinemachine;

public class GameCameraController : MonoBehaviour
{
    static CinemachineStateDrivenCamera stateDrivenCamera;
    static CinemachineFreeLook walkingCamera;
    static CinemachineFreeLook surfingCamera;
    static Vector3 cameraResetPos;
    //static PlayerController player;
    static Animator transitionController;

    //Temp shit
    bool isWalkingDebug;

    private void Start()
    {
        InitialiseGameCamera();
        DebugVariables();
        isWalkingDebug = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            ResetCamera();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isWalkingDebug = !isWalkingDebug;
            if (isWalkingDebug)
                EnterWalkCam();
            else
                EnterSurfCam();
        }
        Debug.Log("Current Active Camera: " + stateDrivenCamera.LiveChild);
    }

    public static void InitialiseGameCamera()
    {
        //Assign the state driven camera, vcams and transition controller
        stateDrivenCamera = GameObject.FindWithTag("GameCamera").GetComponent<CinemachineStateDrivenCamera>();
        walkingCamera = (CinemachineFreeLook)stateDrivenCamera.ChildCameras[0];
        surfingCamera = (CinemachineFreeLook)stateDrivenCamera.ChildCameras[1];
        transitionController = stateDrivenCamera.gameObject.GetComponent<Animator>();
        cameraResetPos = GameObject.FindWithTag("Player").transform.Find("Player_Logic").Find("Player_Base").Find("CameraResetPoint").position;
        //Initialise the camera to follow the player
        stateDrivenCamera.Follow = GameObject.FindWithTag("Player").transform;
        stateDrivenCamera.LookAt = GameObject.FindWithTag("Player").transform;
        //Automatically assign a CinemachineBrain to the main camera
        Camera camera = Camera.main;
        if(!camera.gameObject.GetComponent<CinemachineBrain>())
            camera.gameObject.AddComponent<CinemachineBrain>();
    }

    #region Switching Cameras

    //Call this when wanting to enter the surfer camera
    public static void EnterSurfCam()
    {
        SwitchCameras(false);
    }

    //Call this when entering to the walking camera
    public static void EnterWalkCam()
    {
        SwitchCameras(true);
    }

    //Switches cameras
    private static void SwitchCameras(bool isWalking)
    {
        transitionController.SetBool("isWalking", isWalking);
        ResetCamera();
    }

    private static void ResetCamera()
    {
        walkingCamera.ForceCameraPosition(cameraResetPos, Quaternion.identity);
        surfingCamera.ForceCameraPosition(cameraResetPos, Quaternion.identity);
    }

    private static void DebugVariables()
    {
        Debug.Log("stateDrivenCamera = " + stateDrivenCamera);
        Debug.Log("walkingCamera = " + walkingCamera);
        Debug.Log("surfingCamera = " + surfingCamera);
        Debug.Log("transitionController = " + transitionController);
        Debug.Log("cameraResetPos = " + cameraResetPos);

    }
    #endregion
}
