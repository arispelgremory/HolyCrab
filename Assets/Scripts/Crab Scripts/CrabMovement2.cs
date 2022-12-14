using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class CrabMovement2 : MonoBehaviour
{
    // Game UI stuffs
    private InGameUI gameUI;
    
    private CharacterController _controller;
    private Vector3 _moveDirection;
    private Rigidbody _rb;
    
    [Header("UI Settings")]
    [SerializeField] private Image _normalAttackImage;
    [SerializeField] private Image _heavyAttackImage;
    [SerializeField] private Image _dashImage;
    
    [Header("Character Movement")]
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _moveSpeedMultipler = 1.0f;

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
    [SerializeField] private float _jumpCDMultiplier = 1.0f;
    
    
    // Dash
    [Header("Dash Settings")]
    [SerializeField] private float _dashCd;
    [SerializeField] private float _dashCDMultiplier = 1.0f;
    [SerializeField] private float _dashForce;
    private float _dashTimer;
    private bool isDashing = false;
    
    // Attack
    [Header("Attack Settings")]
    private float _attackTimer;
    [SerializeField] private float _attackCd;
    [SerializeField] private float _attackCDMultiplier = 1.0f;
    private bool isAttacking = false;
    
    
    [Header("Claw Collider")]
    private GameObject clawCollider;
    
    // Heavy Attack
    [Header("Heavy Settings")]
    private float _heavyAttackTimer;
    [SerializeField] private float _heavyAttackCd;
    [SerializeField] private float _heavyAttackCDMultiplier = 1.0f;
    private bool isHeavyAttacking = false;
    
    [Header("Action Delays Settings")]
    [SerializeField] private float _actionDelay;
    
    // Stun
    [Header("Stun Settings")]
    [SerializeField] private float _stunTime;
    private bool _isStunned = false;

    private bool newlyFeverTime = true;

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
        gameUI = InGameUI.Instance;
        
        _attackTimer = _attackCd;
        _heavyAttackTimer = _heavyAttackCd;
        _dashTimer = _dashCd;
        
        // Prevent the UI fill from being filled at the start
        _normalAttackImage.fillAmount = 0;
        _heavyAttackImage.fillAmount = 0;
        _dashImage.fillAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // If either win or lose, then return
        if (gameUI.IsWin() || gameUI.IsGameOver()) return;
        
        CoolDownProperties();
        GetInput();
        FeverTimeBuffs();
        AnimatePlayerMovement();
        Jump();
        Attack();
        HeavyAttack();
        Dash();
        CalculateMovement();
        ApplyGravity();
        ApplyMovement();
        UpdateUI();
        
        
        // Reset Input to avoid multiple inputs
        ResetInput();
    }

    private void ResetInput()
    {
        
        jumpInput = false;
        isAttacking = false;
        isHeavyAttacking = false;
        isDashing = false;
    }

    private void FeverTimeBuffs()
    {
        if (gameUI.isFeverTime)
        {
            _jumpCDMultiplier = 0.5f;
            _dashCDMultiplier = 0.5f;
            _attackCDMultiplier = 0.5f;
            _heavyAttackCDMultiplier = 0.5f;
            _moveSpeedMultipler = 2.0f;
            if (newlyFeverTime)
            {
                // Reset UI fill
                _normalAttackImage.fillAmount = 0;
                _heavyAttackImage.fillAmount = 0;
                _dashImage.fillAmount = 0;
                
                // Reset timers
                _attackTimer = _attackCd;
                _heavyAttackTimer = _heavyAttackCd;
                _dashTimer = _dashCd;
                
                newlyFeverTime = false;
            }
        }
        else
        {
            _jumpCDMultiplier = 1.0f;
            _dashCDMultiplier = 1.0f;
            _attackCDMultiplier = 1.0f;
            _heavyAttackCDMultiplier = 1.0f;
            _moveSpeedMultipler = 1.0f;
            newlyFeverTime = true;
        }
        
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
        // If stunned, do not allow any input
        if (!_isStunned)
        {
            // Check if the player is pressing the button
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");
            jumpInput = Input.GetKeyDown(KeyCode.Space);

            isAttacking = (Input.GetKeyDown(KeyCode.Mouse0) && _attackTimer > (_attackCd * _attackCDMultiplier));
            isHeavyAttacking = Input.GetKeyDown(KeyCode.Mouse1) && _heavyAttackTimer > (_heavyAttackCd * _heavyAttackCDMultiplier);
            isDashing = Input.GetKeyDown(KeyCode.LeftShift) && _dashTimer > (_dashCd * _dashCDMultiplier);
        }

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
        _moveDirection *= (_moveSpeed * _moveSpeedMultipler);

        // If stunned, do not allow any movement
        if (_isStunned)
        {
            _moveDirection = Vector3.zero;
        }
        
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
        if (!_controller.isGrounded || !jumpInput || jumpTimer < (_jumpCd * _jumpCDMultiplier)) return;

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
        if ((_attackTimer <= (_attackCd * _attackCDMultiplier)) || isDashing || isHeavyAttacking || !isAttacking || !_controller.isGrounded) return;
        
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
        yield return new WaitForSeconds((_dashCd * _dashCDMultiplier) - _actionDelay);
        isAttacking = false;
    }
    
    private void HeavyAttack()
    {
        if ((_heavyAttackTimer <= (_heavyAttackCd * _heavyAttackCDMultiplier)) || isDashing || !isHeavyAttacking || isAttacking || !_controller.isGrounded) return;
        
        
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
        if ((_dashTimer <= (_dashCd * _dashCDMultiplier)) || !isDashing || isHeavyAttacking || isAttacking || (horizontalInput == 0 && verticalInput == 0)) return;
        
        _dashTimer = 0;
        
        StartCoroutine(PerformDash());
    }

    IEnumerator PerformDash()
    {
        _animator.SetTrigger(IsDashing);
        // UI CoolDown
        _dashImage.fillAmount = 1;
        
        yield return new WaitForSeconds(0.25f);
        // Dash Movements
        Vector3 impact = Vector3.zero;
        impact.Normalize();
        impact += _moveDirection * _dashForce;
        
        _controller.Move(impact * Time.deltaTime);
        
        yield return new WaitForSeconds(_actionDelay - 0.25f);
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
        if (!isAttacking && _attackTimer <= (_attackCd * _attackCDMultiplier))
        {
            _normalAttackImage.fillAmount -= Time.deltaTime /  (_attackCd * _attackCDMultiplier) ;
        }
        
        if (_normalAttackImage.fillAmount <= 0.01f)
        {
            _normalAttackImage.fillAmount = 0;
        }

    }

    private void HeavyAttackUI()
    {
        if (_heavyAttackTimer <= (_heavyAttackCd * _heavyAttackCDMultiplier) && !isHeavyAttacking)
        {
            _heavyAttackImage.fillAmount -= Time.deltaTime /  (_heavyAttackCd * _heavyAttackCDMultiplier);
        }
        
        if (_heavyAttackImage.fillAmount <= 0.01f)
        {
            _heavyAttackImage.fillAmount = 0;
        }
    }

    private void DashUI()
    {

        if (_dashTimer <= (_dashCd * _dashCDMultiplier) && !isDashing)
        {
            _dashImage.fillAmount -= Time.deltaTime / (_dashCd * _dashCDMultiplier);
        }
        
        if (_dashImage.fillAmount <= 0.01f)
        {
            _dashImage.fillAmount = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("EnemyAttacker") && !_isStunned)
        {
            // Disable the claw collider when stunned
            clawCollider.SetActive(false);
            
            _isStunned = true;
            _animator.SetBool("IsStunned", true);
            StartCoroutine(Stun());
        }

        if (other.gameObject.name == "OceanBorderWarningCollider")
        {
            gameUI._isWarning = true;
            gameUI.warningPanel.SetActive(true);
            StartCoroutine(Death());
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "OceanBorderWarningCollider")
        {
            gameUI._isWarning = false;
            gameUI. warningText.text = gameUI.warningText.ToString();
            gameUI.warningPanel.SetActive(false);
        }
    }

    IEnumerator Stun()
    {
        // Stop down player
        _velocity = 0.0f;
        
        yield return new WaitForSeconds(_stunTime);
        _animator.SetBool("IsStunned", false);
        _isStunned = false;
    }
    
    IEnumerator Death()
    {
        for (int i = gameUI.warningTime; i > 0; i--)
        {
            gameUI.warningText.text = i.ToString();
            yield return new WaitForSeconds(1);
            if(!gameUI._isWarning) break;
        }
        
        // Dead
        gameUI.HasLost();
    }
    
}
