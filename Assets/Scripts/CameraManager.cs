using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class CameraManager : MonoBehaviour 
{
    [SerializeField] private Transform Target;
    private float _DistanceToPlayer;
    private Vector2 _Input;

    [SerializeField] private MouseSensitivity MouseSens;
    [SerializeField] private CameraAngle CameraAngle;
    private CameraRotation _CameraRotation;
    private void Awake()
    {
        _DistanceToPlayer = Vector3.Distance(transform.position, Target.position);
    }

    public void Look(InputAction.CallbackContext context)
    {
        _Input = context.ReadValue<Vector2>();
    }

    private void Update()
    {
        _CameraRotation.yaw += _Input.x * MouseSens.Horizontal * Time.deltaTime;
        _CameraRotation.pitch += _Input.y * MouseSens.Vertical * Time.deltaTime;
        _CameraRotation.pitch = Mathf.Clamp(_CameraRotation.pitch, CameraAngle.min, CameraAngle.max);
    }

    private void LateUpdate()
    {
        transform.eulerAngles = new Vector3(_CameraRotation.pitch, _CameraRotation.yaw, 0.0f);
        transform.position = Target.position - transform.forward * _DistanceToPlayer;
    }
}

[Serializable]
public struct MouseSensitivity
{
    public float Horizontal;
    public float Vertical;

}

public struct CameraRotation
{
    public float pitch;
    public float yaw;
}

[Serializable]
public struct CameraAngle
{
    public float max;
    public float min;
}