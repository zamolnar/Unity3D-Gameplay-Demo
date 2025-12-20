using System.Runtime.CompilerServices;
using TMPro.EditorUtilities;
using Unity.Mathematics;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]

public class Player_Movemet : MonoBehaviour
{
    #region Variables: Movement

    private CharacterController _PlayerController;
    [SerializeField] private float Speed;
    private Vector2 _Input;
    private Vector3 _Movement;

    #endregion
    #region Variables: Rotation

    [SerializeField] private float RotationSpeed = 500f;
    private Camera _MainCamera;

    #endregion
    #region Variables: Gravity

    private float _Gravity = -9.81f;
    [SerializeField] private float GravityMultiplier = 1.0f;
    private float _Velocity;

    #endregion
    #region Variables: Jump

    [SerializeField] private float JumpPower;

    #endregion

    private void Awake()
    {
        _PlayerController = GetComponent<CharacterController>();
        _MainCamera = Camera.main;
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

        _Movement = Quaternion.Euler(0.0f, _MainCamera.transform.eulerAngles.y, 0.0f) * new Vector3(_Input.x, 0.0f, _Input.y);
        var targetrotation = Quaternion.LookRotation(_Movement, Vector3.up);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetrotation, RotationSpeed * Time.deltaTime);
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

        PlayerRotation();
        PlayerGravity();
        PlayerMovement();
        
    }
}
