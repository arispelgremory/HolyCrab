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
    
    // Boolean
    private static readonly bool True = true;
    private static readonly bool False = false;

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
    private bool isJumping = False;
    private bool isGrounded = True;
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


    // Input Settings
    private float horizontalInput;
    private float verticalInput;
    
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

    }

    // Update is called once per frame
    void Update()
    {   
        // Cannot dodge while attack
        // Jump dodge is ok
        // cannot attack while jump
        jumpTimer += Time.deltaTime;
        dashTimer += Time.deltaTime;
        attackTimer += Time.deltaTime;
        heavyAttackTimer += Time.deltaTime;

        // TODO: Pressing two keys and it will play it after the other
        if (dashTimer >= dashCooldownTime
            && !isAttacking
            && !isHeavyAttacking)
        {
            dashable = True;
        }

        attackable = (attackTimer >= attackCoolDownTime
                      && !isDashing
                      && !isHeavyAttacking);
        heavyAttackable = (heavyAttackTimer >= heavyAttackCoolDownTime
                           && !isDashing
                           && !isAttacking);
        
        
        
        if (Input.GetButtonDown("Shift") && 
            dashTimer >= dashCooldownTime 
            && !isAttacking
            && !isHeavyAttacking
            )
        {
            // Dashable
            // Debug.Log("can dash: " + dashTimer);
            isDashing = True;
            dashable = False;
            attackable = False;
            heavyAttackable = False;
            dashTimer = 0.0f;
            StartCoroutine(Dash());
        }
        
        
        
        if (
            Input.GetButtonDown("Fire1") && 
            attackTimer >= attackCoolDownTime && 
            !isAttacking && 
            !isJumping)
        {
            // Attackable
            isAttacking = true;
            dashable = False;
            attackable = False;
            heavyAttackable = False;
            attackTimer = 0.0f;
            StartCoroutine(Attack());
        } 
        
        if (Input.GetButtonDown("Fire2") && 
            heavyAttackTimer >= heavyAttackCoolDownTime && 
            !isHeavyAttacking && 
            !isJumping)
        {
            // Heavy Attackable
            isHeavyAttacking = true;
            heavyAttackTimer = 0.0f;
            dashable = False;
            attackable = False;
            heavyAttackable = False;
            StartCoroutine(HeavyAttack());
        }
        
        if (Input.GetKeyDown(KeyCode.Space) && 
            isGrounded && 
            jumpTimer > jumpCoolDownTime && 
            !isAttacking)
        {
            isJumping = True;
            attackable = False;
            heavyAttackable = False;
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

        if (horizontalInput != 0 && !anim.GetBool(IsForward) && !anim.GetBool(IsBackward))
        {
            // Debug.Log("Animating left or right");
            anim.SetBool(IsRight, verticalInput > 0);
            anim.SetBool(IsLeft, verticalInput < 0);
        }

        if (horizontalInput != 0)
        {
            Debug.Log("Left:" + (horizontalInput < 0) + " Right:" + (horizontalInput > 0)); 
            isMovingLeft = horizontalInput < 0;
            isMovingRight = horizontalInput > 0;
        }
        
        if (rb.velocity.z == 0 && verticalInput == 0)
        {
            anim.SetBool(IsForward, False);
            anim.SetBool(IsBackward, False);
            isMovingForward = False;
            isMovingBackward = False;
        }

        if (rb.velocity.x == 0 && horizontalInput == 0)
        {
            Debug.Log("Canceling animation of left or right");
            anim.SetBool(IsLeft, False);
            anim.SetBool(IsRight, False);
            isMovingLeft = False;
            isMovingRight = False;
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
            Debug.Log("Dashing backward");
            anim.SetTrigger(IsDashingBackwards);
            movement += (transform.forward * -1);
        }

        if (verticalInput > 0 || horizontalInput != 0)
        {
            anim.SetTrigger(IsDashing);
        }

        movement.Normalize();
        movement *= movingSpeed;
        movement *= Time.deltaTime;
        movement *= dashForce;
        rb.drag = frictionDuringDash;
        rb.AddForce(movement, ForceMode.Impulse);
        
        
        // Perform physics calculation and apply damage to any colliders within range
        RaycastHit hit;
        // if (Physics.Raycast(transform.position, transform.forward, out hit, range))
        // {
        //     if (hit.collider.CompareTag("Enemy"))
        //     {
        //         hit.collider.GetComponent<EnemyScript>().TakeDamage(damage);
        //         hit.rigidbody.AddForce(transform.forward * 50f);
        //     }
        // }

        // Wait for attack animation to finish before allow to dash
        yield return new WaitForSeconds(actionInterval);
        rb.drag = friction;
        isDashing = False;
    }
    
    IEnumerator Attack()
    {
        anim.SetTrigger(IsAttack);
        
        // Perform physics calculation and apply damage to any colliders within range
        RaycastHit hit;
        // if (Physics.Raycast(transform.position, transform.forward, out hit, range))
        // {
        //     if (hit.collider.CompareTag("Enemy"))
        //     {
        //         hit.collider.GetComponent<EnemyScript>().TakeDamage(damage);
        //         hit.rigidbody.AddForce(transform.forward * 50f);
        //     }
        // }

        // Wait for attack animation to finish before allow to attack
        yield return new WaitForSeconds(actionInterval);
        isAttacking = False;
    }
    
    IEnumerator HeavyAttack()
    {
        anim.SetTrigger(IsHeavyAttack);
        // Perform physics calculation and apply damage to any colliders within range
        RaycastHit hit;
        // if (Physics.Raycast(transform.position, transform.forward, out hit, range))
        // {
        //     if (hit.collider.CompareTag("Enemy"))
        //     {
        //         hit.collider.GetComponent<EnemyScript>().TakeDamage(damage);
        //         hit.rigidbody.AddForce(transform.forward * 50f);
        //     }
        // }

        // Wait for attack animation to finish before allow to heavy attack
        yield return new WaitForSeconds(actionInterval);
        isHeavyAttacking = False;
    }

    IEnumerator Jump()
    {
        Vector3 movement = new Vector3(0, 1, 0);
        movement.Normalize();
        movement *= jumpForce;
        movement *= Time.deltaTime;
        Debug.Log(movement);
        rb.AddForce(movement, ForceMode.Impulse);
        
        yield return new WaitForSeconds(actionInterval);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Terrain")
        {
            isGrounded = True;
        }
    }


    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Terrain")
        {
            isGrounded = False;
            isJumping = False;
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
