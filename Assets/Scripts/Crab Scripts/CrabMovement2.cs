using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class CrabMovement2 : MonoBehaviour
{
    private CharacterController _controller;
    private Vector3 _moveDirection;
    private Rigidbody _rb;
    
    [Header("UI Settings")]
    [SerializeField] private Image _normalAttackImage;
    [SerializeField] private Image _heavyAttackImage;
    [SerializeField] private Image _dashImage;
    
    [Header("Character Movement")]
    [SerializeField] private float _moveSpeed = 5f;

    // Input Settings
    private float horizontalInput;
    private float verticalInput;
    private bool jumpInput;
    
    // Gravity
    private float _gravity = -9.81f;
    [SerializeField] private float gravityMultiplier = 1f;
    private float _velocity = 0.0f;
    
    // Animations
    private Animator _animator;
    private static readonly string IsForward = "IsForward";
    private static readonly string IsAttack = "IsAttack";
    private static readonly string IsHeavyAttack = "IsHeavyAttack";
    private static readonly string IsDashing = "IsDodging";

    // Jump
    [Header("Jump Settings")]
    [SerializeField] private float _jumpPower;
    private float jumpTimer;
    [SerializeField] private float _jumpCd;
    
    
    // Dash
    [Header("Dash Settings")]
    [SerializeField] private float _dashCd;
    [SerializeField] private float _dashForce;
    private float _dashTimer;
    private bool isDashing = false;
    
    // Attack
    [Header("Attack Settings")]
    private float _attackTimer;
    [SerializeField] private float _attackCd;
    private bool isAttacking = false;
    [Header("Claw Collider")]
    private GameObject clawCollider;
    
    // Heavy Attack
    [Header("Heavy Settings")]
    private float _heavyAttackTimer;
    [SerializeField] private float _heavyAttackCd;
    private bool isHeavyAttacking = false;
    
    [Header("Action Delays Settings")]
    [SerializeField] private float _actionDelay;
    
    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
        
        clawCollider = GameObject.FindWithTag("PlayerAttacker");
    }
    
    
    // Start is called before the first frame update
    void Start()
    {
        _attackTimer = _attackCd;
        _heavyAttackTimer = _heavyAttackCd;
        _dashTimer = _dashCd;
    }

    // Update is called once per frame
    void Update()
    {
        CoolDownProperties();
        GetInput();
        AnimatePlayerMovement();
        Jump();
        Attack();
        HeavyAttack();
        Dash();
        CalculateMovement();
        ApplyGravity();
        ApplyMovement();
        UpdateUI();
        
        
        // Reset Input to avoid multiple inputs (NOT WORKING)
        // ResetInput();
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

        isAttacking = (Input.GetKeyDown(KeyCode.Mouse0) && _attackTimer > _attackCd);
        isHeavyAttacking = Input.GetKeyDown(KeyCode.Mouse1) && _heavyAttackTimer > _heavyAttackCd;
        isDashing = Input.GetKeyDown(KeyCode.LeftShift) && _dashTimer > _dashCd;

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
        
        _attackTimer = 0;
        
        StartCoroutine(PerformAttack());
    }

    IEnumerator PerformAttack()
    {
        _animator.SetTrigger(IsAttack);
        
        // UI CoolDown
        _normalAttackImage.fillAmount = 1;
        clawCollider.SetActive(true);
        yield return new WaitForSeconds(_actionDelay);
        clawCollider.SetActive(false);
        yield return new WaitForSeconds(_dashCd - _actionDelay);
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
        
        // UI CoolDown
        _heavyAttackImage.fillAmount = 1;

        clawCollider.SetActive(true);
        yield return new WaitForSeconds(_actionDelay);
        clawCollider.SetActive(false);
        isHeavyAttacking = false;
    }
    
    private void Dash()
    {
        if ((_dashTimer < _dashCd) || !isDashing || isHeavyAttacking || isAttacking) return;
        
        _dashTimer = 0;
        
        StartCoroutine(PerformDash());
    }

    IEnumerator PerformDash()
    {
        _animator.SetTrigger(IsDashing);
        // Dash Movements
        Vector3 impact = Vector3.zero;
        impact.Normalize();
        impact += _moveDirection * _dashForce;
        
        _controller.Move(impact * Time.deltaTime);
        
        // UI CoolDown
        _dashImage.fillAmount = 1;
        
        yield return new WaitForSeconds(_actionDelay);
        isDashing = false;
    }

    private void UpdateUI()
    {
        NormalAttackUI();
        HeavyAttackUI();
        DashUI();
    }
    
    private void NormalAttackUI()
    {
        if (!isAttacking && _attackTimer <= _attackCd)
        {
            _normalAttackImage.fillAmount -= Time.deltaTime /  _attackCd ;
        }
        
        if (_normalAttackImage.fillAmount <= 0.01f)
        {
            _normalAttackImage.fillAmount = 0;
        }

    }

    private void HeavyAttackUI()
    {
        if (_heavyAttackTimer <= _heavyAttackCd && !isHeavyAttacking)
        {
            _heavyAttackImage.fillAmount -= Time.deltaTime /  _heavyAttackCd;
        }
        
        if (_heavyAttackImage.fillAmount <= 0.01f)
        {
            _heavyAttackImage.fillAmount = 0;
        }
    }

    private void DashUI()
    {

        if (_dashTimer <= _dashCd && !isDashing)
        {
            _dashImage.fillAmount -= Time.deltaTime / _dashCd;
        }
        
        if (_dashImage.fillAmount <= 0.01f)
        {
            _dashImage.fillAmount = 0;
        }
    }

    
}
