using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class CameraManager : MonoBehaviour 
{
    #region Variables: Camera

    [SerializeField] private Transform Player;
    [SerializeField] private float _DistanceToPlayer;
    //This is for camera collion using raycast
    //private float _MinDistanceToPlayer;
    //private float _MaxDistanceToPlayer;
    private Vector2 _MouseInput;

    //using the structs below to clean up code
    [SerializeField] private MouseSensitivity MouseSens;
    [SerializeField] private CameraAngle CameraAngle;
    private CameraRotation _CameraRotation;

    #endregion
    private void Awake()
    {
        //Distance camera is from player
        //also good to use if wanting to zoom in/out
        _DistanceToPlayer = Vector3.Distance(transform.position, Player.position);
    }

    public void Look(InputAction.CallbackContext context)
    {
        //Getting mouse value
        _MouseInput = context.ReadValue<Vector2>();
    }

    private void Update()
    {
        //How the Camera is updated via mouse inputs
        _CameraRotation.yaw += _MouseInput.x * MouseSens.Horizontal * Time.deltaTime;
        _CameraRotation.pitch += _MouseInput.y * MouseSens.Vertical * Time.deltaTime;
        //Clamps the camera to be between the min/max angles
        _CameraRotation.pitch = Mathf.Clamp(_CameraRotation.pitch, CameraAngle.min, CameraAngle.max);
    }

    private void LateUpdate()
    {
        //Sets camera rotation/position with values gotten from CameraRotation struct
        transform.eulerAngles = new Vector3(_CameraRotation.pitch, _CameraRotation.yaw, 0.0f);
        transform.position = Player.position - transform.forward * _DistanceToPlayer;
    }
}

//Mouse sensitivity for moving the camera
[Serializable]
public struct MouseSensitivity
{
    public float Horizontal;
    public float Vertical;

}

//moxing the camera along the x/y axis
public struct CameraRotation
{
    public float pitch;
    public float yaw;
}

//used to clamp min/max values for camera rotation
[Serializable]
public struct CameraAngle
{
    public float max;
    public float min;
}