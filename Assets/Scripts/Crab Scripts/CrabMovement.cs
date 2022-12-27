using System.Collections;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;


public class CrabMovement : MonoBehaviour
{
    // Animations
    private static readonly string IsForward = "IsForward";
    private static readonly string IsBackward = "IsBackward";
    private static readonly string IsLeft = "IsLeft";
    private static readonly string IsRight = "IsRight";
    private static readonly string IsAttack = "IsAttack";
    private static readonly string IsHeavyAttack = "IsHeavyAttack";
    private static readonly string IsDashing = "IsDodging";
    private static readonly string IsDashingBackwards = "IsDodgingBackwards";

    private Rigidbody rb;
    private Animator anim;
    
    [Header("Character Settings")]
    public float friction = 2.5f;
    
    // Attack
    [Header("Attack Settings")]
    public static float attackCoolDownTime = 1.5f;
    public static bool attackable = true;
    private float attackTimer = 0.0f;
    private bool isAttacking = false;
    
    private float heavyAttackTimer = 0.0f;
    public static float heavyAttackCoolDownTime = 5.0f;
    private bool isHeavyAttacking = false;
    public static bool heavyAttackable = true;
    
    // Movements
    [Header("Player Settings")]
    public float velocityThreshold = 0.1f;
    private float timer = 0.0f;
    private bool isMovingForward = false;
    private bool isMovingBackward = false;
    private bool isMovingLeft = false;
    private bool isMovingRight = false;
    public float movingSpeed = 15;
    public static float actionInterval = 0.5f;

    // Jump properties
    [Header("Jump Settings")]
    private bool isJumping = false;
    private bool isGrounded = true;
    public float jumpForce = 50.0f;
    private float gravity;
    public static readonly float jumpCoolDownTime = 1.5f;
    private float jumpTimer = 1.5f;
    
    // Dash properties
    [Header("Dash Settings")]
    public float frictionDuringDash = 5.0f;
    public float dashForce = 1.2f;
    private bool isDashing = false;
    private float dashTimer = 5.0f; // The remaining cooldown time
    public static bool dashable = true;
    public static float dashCooldownTime = 2.0f; // Dash cooldown time in seconds

    [Header("Claw Collider")]
    private GameObject clawCollider;

    // Input Settings
    private float horizontalInput;
    private float verticalInput;
    
    private Camera m_Camera;
    [SerializeField] private MouseLook m_MouseLook;

    [Header("Controlling Settings")] 
    public float mouseSensitivity;
    
    private float rotationY = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        setNormalFriction();
    
        // World Settings

        dashTimer = dashCooldownTime;
        jumpTimer = jumpCoolDownTime;
        attackTimer = attackCoolDownTime;
        heavyAttackTimer = heavyAttackCoolDownTime;
        
        clawCollider = GameObject.FindWithTag("PlayerAttacker");
        
    }

    // Update is called once per frame
    void Update()
    {
        // Mouse rotate input
        rotationY += Input.GetAxis("Mouse X") * mouseSensitivity;
        rb.rotation = Quaternion.Euler(0, rotationY, 0);
        // Debug.Log(rotationY);

        // Cannot dodge while attack
        // Jump dodge is ok
        // cannot attack while jump
        jumpTimer += Time.deltaTime;
        dashTimer += Time.deltaTime;
        attackTimer += Time.deltaTime;
        heavyAttackTimer += Time.deltaTime;
        

        // TODO: Pressing two keys and it will play it after the other
        dashable = (dashTimer >= dashCooldownTime
                    && !isAttacking
                    && !isHeavyAttacking);

        attackable = (attackTimer >= attackCoolDownTime
                      && !isDashing
                      && !isHeavyAttacking
                      && !isJumping
                      && isGrounded);

        heavyAttackable = (heavyAttackTimer >= heavyAttackCoolDownTime
                           && !isDashing
                           && !isAttacking
                           && !isJumping
                           && isGrounded);
        // Debug.Log("Dashable: " + dashable + " Attackable: " + attackable + " Heavy Attackable: " + heavyAttackable);
        
        
        if (Input.GetButtonDown("Shift") && 
            dashTimer >= dashCooldownTime 
            && !isAttacking
            && !isHeavyAttacking
            )
        {
            // Dashable
            // Debug.Log("can dash: " + dashTimer);
            isDashing = true;
            dashable = false;
            attackable = false;
            heavyAttackable = false;
            dashTimer = 0.0f;
            StartCoroutine(Dash());
        }

        if (
            Input.GetButtonDown("Fire1") && 
            attackTimer >= attackCoolDownTime && 
            !isHeavyAttacking && 
            !isJumping &&
            isGrounded
            )
        {
            // Attackable
            isAttacking = true;
            dashable = false;
            attackable = false;
            heavyAttackable = false;
            attackTimer = 0.0f;
            StartCoroutine(Attack());
        } else if (Input.GetButtonDown("Fire2") && 
            heavyAttackTimer >= heavyAttackCoolDownTime && 
            !isAttacking && 
            !isJumping && isGrounded)
        {
            // Heavy Attackable
            isHeavyAttacking = true;
            heavyAttackTimer = 0.0f;
            dashable = false;
            attackable = false;
            heavyAttackable = false;
            StartCoroutine(HeavyAttack());
        }
        
        if (Input.GetKeyDown(KeyCode.Space) && 
            isGrounded && 
            jumpTimer > jumpCoolDownTime && 
            !isAttacking)
        {
            isJumping = true;
            attackable = false;
            heavyAttackable = false;
            StartCoroutine(Jump());
        }

        // Movement
        // Check if the player is pressing the button
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        // Check if the player is moving
        if (verticalInput != 0)
        {
            anim.SetBool(IsForward, verticalInput > 0);
            anim.SetBool(IsBackward, verticalInput < 0);
            isMovingForward = verticalInput > 0;
            isMovingBackward = verticalInput < 0;
        }
        else
        {
            anim.SetBool(IsForward, false);
            anim.SetBool(IsBackward, false);
            isMovingForward = false;
            isMovingBackward = false;
        }

        if (horizontalInput != 0 && !anim.GetBool(IsForward) && !anim.GetBool(IsBackward))
        {
            // Debug.Log("Animating left or right");
            anim.SetBool(IsRight, horizontalInput > 0);
            anim.SetBool(IsLeft, horizontalInput < 0);
        }

        if (horizontalInput != 0)
        {
            isMovingLeft = horizontalInput < 0;
            isMovingRight = horizontalInput > 0;
        } else {
            anim.SetBool(IsLeft, false);
            anim.SetBool(IsRight, false);
            isMovingLeft = false;
            isMovingRight = false;
        }
        
    }


    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(0, 0, 0);
        
        // Forward
        if (verticalInput > 0)
        {
            movement += transform.forward;
            
        } else if (verticalInput < 0)
        {
            movement += (transform.forward * -1);
        }
        
        // Right
        if (horizontalInput > 0)
        {
            movement += transform.right;
        } else if (horizontalInput < 0)
        {
            movement += (transform.right * -1);
        }
        
        movement.Normalize();
        movement *= movingSpeed;
        movement *= Time.deltaTime;
        rb.AddForce(movement, ForceMode.Acceleration);
    }

    IEnumerator Dash()
    {
        Vector3 movement = new Vector3(0, 0, 0);
        
        // Horizontal velocity
        if (horizontalInput > 0)
        {
            movement += transform.right;
        } else if (horizontalInput < 0)
        {
            movement += (transform.right * -1);
        }
        
        // Vertical velocity
        if (verticalInput > 0)
        {
            movement += transform.forward;
        } else if (verticalInput < 0)
        {
            // Debug.Log("Dashing backward");
            anim.SetTrigger(IsDashingBackwards);
            movement += (transform.forward * -1);
        }
        
        if(verticalInput > 0 || horizontalInput != 0)
        {
            anim.SetTrigger(IsDashing);
        }

        movement.Normalize();
        movement *= movingSpeed;
        movement *= Time.deltaTime;
        movement *= dashForce;
        rb.drag = frictionDuringDash;
        rb.AddForce(movement, ForceMode.Impulse);

        // Wait for attack animation to finish before allow to dash
        yield return new WaitForSeconds(actionInterval);
        rb.drag = friction;
        isDashing = false;
    }
    
    IEnumerator Attack()
    {
        anim.SetTrigger(IsAttack);
        
        clawCollider.SetActive(true);
        // Wait for attack animation to finish before allow to attack
        yield return new WaitForSeconds(actionInterval);
        clawCollider.SetActive(false);
        isAttacking = false;
    }
    
    IEnumerator HeavyAttack()
    {
        anim.SetTrigger(IsHeavyAttack);
        
        clawCollider.SetActive(true);
        // Wait for attack animation to finish before allow to heavy attack
        yield return new WaitForSeconds(actionInterval);
        clawCollider.SetActive(false);
        isHeavyAttacking = false;
    }

    IEnumerator Jump()
    {
        Vector3 movement = new Vector3(0, 1, 0);
        movement.Normalize();
        movement *= jumpForce;
        movement *= Time.deltaTime;
        // Debug.Log(movement);
        rb.AddForce(movement, ForceMode.Impulse);
        
        yield return new WaitForSeconds(actionInterval);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Terrain")
        {
            isGrounded = true;
        }
    }


    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Terrain")
        {
            isGrounded = false;
            isJumping = false;
            jumpTimer = 0;
        }
    }
    
    
    void setNormalFriction()
    {
        rb.drag = friction;
    }
    
    void setDashFriction()
    {
        rb.drag = frictionDuringDash;
    }
    
    
}
