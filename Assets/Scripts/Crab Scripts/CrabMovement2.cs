using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class CrabMovement2 : MonoBehaviour
{
    private CharacterController _controller;
    private Vector3 _moveDirection;
    
    [Header("Character Movement")]
    [SerializeField] private float _moveSpeed = 5f;

    // Input Settings
    private float horizontalInput;
    private float verticalInput;
    private bool jumpInput;
    
    // Gravity
    private float _gravity = -9.81f;
    [SerializeField] private float gravityMultiplier = 1f;
    private float _velocity;
    
    // Animations
    private Animator _animator;
    private static readonly string IsForward = "IsForward";
    private static readonly string IsBackward = "IsBackward";
    private static readonly string IsLeft = "IsLeft";
    private static readonly string IsRight = "IsRight";
    private static readonly string IsAttack = "IsAttack";
    private static readonly string IsHeavyAttack = "IsHeavyAttack";
    private static readonly string IsDashing = "IsDodging";
    private static readonly string IsDashingBackwards = "IsDodgingBackwards";
    
    // Jump
    [Header("Jump Settings")]
    [SerializeField] private float _jumpPower;
    private float jumpTimer;
    [SerializeField] private float _jumpCd;
    
    
    // Dash
    private float dashTimer;
    [SerializeField] private float _dashCd;
    
    // Attack
    private float attackTimer;
    [SerializeField] private float _attackCd;
    
    // Heavy Attack
    private float heavyAttackTimer;
    [SerializeField] private float _heavyAttackCd;
    
    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
    }
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CoolDownProperties();
        GetInput();
        Animate();
        // TBD: Add attack, dash & heavy attack.
        
    }

    private void FixedUpdate()
    {
        CalculateMovement();
        ApplyGravity();
        Jump();
        ApplyMovement();
    }

    private void CoolDownProperties()
    {
        jumpTimer += Time.deltaTime;
        dashTimer += Time.deltaTime;
        attackTimer += Time.deltaTime;
        heavyAttackTimer += Time.deltaTime;
    }

    private void GetInput()
    {
        // Check if the player is pressing the button
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        jumpInput = Input.GetKeyDown(KeyCode.Space);
    }

    private void Animate()
    {
        // Check if the player is moving
        if (verticalInput != 0 || horizontalInput != 0)
        {
            _animator.SetBool(IsForward, true);
        }
        else
        {
            _animator.SetBool(IsForward, false);
        }
    }
    
    private void CalculateMovement()
    {
        _moveDirection = new Vector3(horizontalInput, 0, verticalInput);
        _moveDirection *= _moveSpeed;
        _moveDirection = transform.TransformDirection(_moveDirection);
    }

    private void ApplyGravity()
    {
        if (_controller.isGrounded && _velocity < 0.0f)
        {
            _velocity = -1.0f;
        }
        else
        {
            _velocity += _gravity * gravityMultiplier * Time.deltaTime;
        }
        
        _moveDirection.y = _velocity;
    }

    public void Jump()
    {
        if (!_controller.isGrounded || !jumpInput || jumpTimer < _jumpCd) return;

        _velocity += _jumpPower;
        // Reset Jump CD
        jumpTimer = 0;
    }

    private void ApplyMovement()
    {
        _controller.Move(_moveDirection * Time.deltaTime);
    }
    
}
