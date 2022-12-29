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
    private float _dashTimer;
    [SerializeField] private float _dashCd;
    private bool isDashing = false;
    
    // Attack
    private float _attackTimer;
    [SerializeField] private float _attackCd;
    private bool isAttacking = false;
    [Header("Claw Collider")]
    private GameObject clawCollider;
    
    // Heavy Attack
    private float _heavyAttackTimer;
    [SerializeField] private float _heavyAttackCd;
    private bool isHeavyAttacking = false;
    
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
        AnimatePlayerMovement();
        // Reset Input to avoid multiple inputs (NOT WORKING)
        ResetInput();
    }

    private void FixedUpdate()
    {
        CalculateMovement();
        ApplyGravity();
        Jump();
        Attack();
        HeavyAttack();
        Dash();
        ApplyMovement();
    }

    private void CoolDownProperties()
    {
        jumpTimer += Time.deltaTime;
        _dashTimer += Time.deltaTime;
        _attackTimer += Time.deltaTime;
        _heavyAttackTimer += Time.deltaTime;
    }

    private void GetInput()
    {
        // Check if the player is pressing the button
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        jumpInput = Input.GetKeyDown(KeyCode.Space);
        isAttacking = Input.GetMouseButtonDown(0);
        isHeavyAttacking = Input.GetMouseButtonDown(1);
        isDashing = Input.GetKeyDown(KeyCode.LeftShift);
    }

    private void ResetInput()
    {
        jumpInput = false;
        isAttacking = false;
        isHeavyAttacking = false;
    }

    private void AnimatePlayerMovement()
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

    private void Jump()
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
    
    private void Attack()
    {
        if ((_attackTimer < _attackCd) || isDashing || isHeavyAttacking || !isAttacking || !_controller.isGrounded) return;
        StartCoroutine(PerformAttack());
    }

    IEnumerator PerformAttack()
    {
        _animator.SetTrigger(IsAttack);
        clawCollider.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        clawCollider.SetActive(false);
        isAttacking = false;
    }
    
    private void HeavyAttack()
    {
        if ((_heavyAttackTimer < _heavyAttackCd) || isDashing || !isHeavyAttacking || isAttacking || !_controller.isGrounded) return;
        
        _heavyAttackTimer = 0;
        StartCoroutine(PerformHeavyAttack());
    }

    IEnumerator PerformHeavyAttack()
    {
        _animator.SetTrigger(IsHeavyAttack);
        clawCollider.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        clawCollider.SetActive(false);
        isHeavyAttacking = false;
    }
    
    private void Dash()
    {
        if ((_heavyAttackTimer < _heavyAttackCd) || !isDashing || isHeavyAttacking || !isAttacking) return;
        
        _dashTimer = 0;
        StartCoroutine(PerformDash());
    }

    IEnumerator PerformDash()
    {
        _animator.SetTrigger(IsDashing);
        // TBD: Add Dash Movement
        
        yield return new WaitForSeconds(0.5f);
        isDashing = false;
    }
    
}
