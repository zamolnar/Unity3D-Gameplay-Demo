using System.Runtime.CompilerServices;
using TMPro.EditorUtilities;
using Unity.Mathematics;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

[RequireComponent(typeof(CharacterController))]

public class Player_Movemet : MonoBehaviour
{
    #region Variables: Movement

    private CharacterController _PlayerController;
    [SerializeField] private float PLayerSpeed;
    private Vector2 _WASDInput;
    private Vector3 _PlayerDirection;

    #endregion
    #region Variables: Rotation

    [SerializeField] private float RotationSpeed = 500f;
    private Camera _MainCamera;

    #endregion
    #region Variables: Gravity

    private float _Gravity = -9.81f;
    [SerializeField] private float GravityMultiplier = 1.0f;
    private float _GravityVelocity;

    #endregion
    #region Variables: Jump

    [SerializeField] private float JumpPower;

    #endregion

    private void Awake()
    {
        //Set values for camera and player
        _PlayerController = GetComponent<CharacterController>();
        _MainCamera = Camera.main;
    }

    public void Move(InputAction.CallbackContext context)
    {
        //setting the WASD inputs
        _WASDInput = context.ReadValue<Vector2>();
        //updating player movement
        _PlayerDirection = new Vector3(_WASDInput.x, 0.0f, _WASDInput.y);
    }
    public void Jump(InputAction.CallbackContext context) 
    {
        //if player is not grounded or not hitting space
        if (!context.started) return;
        if (!_PlayerController.isGrounded) return;
        _GravityVelocity += JumpPower;
    }

    private void PlayerMovement() 
    {
        //updating player controller with updated player position
        _PlayerController.Move(_PlayerDirection * Time.deltaTime * PLayerSpeed);
    }
    private void PlayerRotation() 
    {
        //makes the player not snap facing back forward
        if (_WASDInput.sqrMagnitude == 0) return;
        //Player rotates with camera
        _PlayerDirection = Quaternion.Euler(0.0f, _MainCamera.transform.eulerAngles.y, 0.0f) * new Vector3(_WASDInput.x, 0.0f, _WASDInput.y);
        var targetrotation = Quaternion.LookRotation(_PlayerDirection, Vector3.up);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetrotation, RotationSpeed * Time.deltaTime);
    }
    private void PlayerGravity()
    {
        //checks velocity if player is grounded or velocity is less than 0
        if (_PlayerController.isGrounded && _GravityVelocity < 0.0f)
        {
            //resets velocity
            _GravityVelocity = -1.0f;
        }
        //updates player po
        else
        {
            //updates velocity with grvity and its multiplier
            _GravityVelocity += _Gravity * GravityMultiplier * Time.deltaTime;
        }
        //set player y axis
        _PlayerDirection.y = _GravityVelocity;
    }


    // Update is called once per frame
    void Update()
    {

        PlayerRotation();
        PlayerGravity();
        PlayerMovement();
        
    }
}
