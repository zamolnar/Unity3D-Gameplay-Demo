using System.Runtime.CompilerServices;
using TMPro.EditorUtilities;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]

public class Player_Movemet : MonoBehaviour
{
    
    private CharacterController _PlayerController;
    [SerializeField] private float Speed;
    private Vector2 _Input;
    private Vector3 _Movement;

    [SerializeField] private float TimeSmooth = 0.05f;
    private float _CurrentVel;
    
    private float _Gravity = -9.81f;
    [SerializeField] private float GravityMultiplier = 1.0f;
    private float _Velocity;

    [SerializeField] private float JumpPower;

    private void Awake()
    {
        _PlayerController = GetComponent<CharacterController>();
    }

    public void Move(InputAction.CallbackContext context)
    {
        _Input = context.ReadValue<Vector2>();
        _Movement = new Vector3(_Input.x, 0.0f, _Input.y);
    }
    public void Jump(InputAction.CallbackContext context) 
    {
        if (!context.started) return;
        if (!_PlayerController.isGrounded) return;
        _Velocity += JumpPower;
    }

    private void PlayerMovement() 
    {
        _PlayerController.Move(_Movement * Time.deltaTime * Speed);
    }
    private void PlayerRotation() 
    {
        if (_Input.sqrMagnitude == 0) return;

        var _TargetAngle = Mathf.Atan2(_Movement.x, _Movement.z) * Mathf.Rad2Deg;
        var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, _TargetAngle, ref _CurrentVel, TimeSmooth);
        transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
    }
    private void PlayerGravity()
    {
        if (_PlayerController.isGrounded && _Velocity < 0.0f)
        {
            _Velocity = -1.0f;
        }
        else
        {
            _Velocity += _Gravity * GravityMultiplier * Time.deltaTime;
            
        }
        _Movement.y = _Velocity;
    }


    // Update is called once per frame
    void Update()
    {

        PlayerGravity();
        PlayerRotation();
        PlayerMovement();
        
    }
}
