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
    private static readonly string IsDodgingBackwards = "IsDodgingBackwards";
    
    // Boolean
    private static readonly bool True = true;
    private static readonly bool False = false;

    private Rigidbody rb;
    private Animator anim;
    
    private float friction;
    
    [Header("Attack Settings")]
    // Attack
    private float attackTimer = 0.0f;
    public static float attackCoolDownTime = 1.5f;
    private bool isAttacking = false;
    public static bool attackable = true;
    
    private float heavyAttackTimer = 0.0f;
    public static float heavyAttackCoolDownTime = 5.0f;
    private bool isHeavyAttacking = false;
    public static bool heavyAttackable = true;
    
    
    [Header("Player Settings")]
    // Movements
    public float intervals = 1.0f;
    private float timer = 0.0f;
    private bool isMovingForward = false;
    private bool isMovingBackward = false;
    private bool isMovingLeft = false;
    private bool isMovingRight = false;
    public float movingSpeed = 15;
    public static float actionInterval = 1.0f;

    [Header("Jump Settings")]
    // Jump properties
    private bool isJumping = False;
    private bool isGrounded = True;
    private float jumpForce;
    private float gravity;
    public static readonly float jumpCoolDownTime = 1.5f;
    public float jumpHeight = 2.0f;
    private float jumpTimer = 1.5f;

    [Header("Dash Settings")]
    // Dash properties
    private bool isDashing = false;
    private float dashTimer = 5.0f; // The remaining cooldown time
    public static float dashCooldownTime = 1.0f; // Dash cooldown time in seconds
    public float dashForce = 1.2f;
    public float frictionDuringDash = 5.0f;
    public static bool dashable = true;
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        setNormalFriction();
    
        // World Settings
        gravity = GameManager.gravity;
        friction = GameManager.friction;

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

        if (dashTimer >= dashCooldownTime
            && !isAttacking
            && !isHeavyAttacking)
        {
            dashable = True;
        }
        
        if (attackTimer >= attackCoolDownTime
            && !isDashing
            && !isHeavyAttacking)
        {
            attackable = True;
        }
        
        if (heavyAttackTimer >= heavyAttackCoolDownTime
            && !isDashing
            && !isAttacking)
        {
            heavyAttackable = True;
        }

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
        }

        // Check if the player is pressing the button
        if (Input.GetAxis("Vertical") > 0)
        {
            anim.SetBool(IsForward, True);
            isMovingBackward = False;
            isMovingForward = True;
            // If the player is pressing the button, reset the timer
            timer = intervals;
            
            if (Input.GetAxis("Horizontal") > 0)
            {
                anim.SetBool(IsRight, True);
                isMovingLeft = False;
                isMovingRight = True;
                // If the player is pressing the button, reset the timer
                timer = intervals;
            } else if (Input.GetAxis("Horizontal") < 0)
            {
                anim.SetBool(IsLeft, True);
                isMovingRight = False;
                isMovingLeft = True;
                // If the player is pressing the button, reset the timer
                timer = intervals;
            }
            else
            {
                isMovingRight = False;
                isMovingLeft = False;
                anim.SetBool(IsRight, False);
                anim.SetBool(IsLeft, False);
            }
            
        } else if (Input.GetAxis("Vertical") < 0)
        {
            anim.SetBool(IsBackward, True);
            isMovingForward = False;
            isMovingBackward = True;
            // If the player is pressing the button, reset the timer
            timer = intervals;
            
            if (Input.GetAxis("Horizontal") > 0)
            {
                anim.SetBool(IsRight, True);
                isMovingLeft = False;
                isMovingRight = True;
                // If the player is pressing the button, reset the timer
                timer = intervals;
            } else if (Input.GetAxis("Horizontal") < 0)
            {
                anim.SetBool(IsLeft, True);
                isMovingRight = False;
                isMovingLeft = True;
                // If the player is pressing the button, reset the timer
                timer = intervals;
            }
            else
            {
                isMovingRight = False;
                isMovingLeft = False;
                anim.SetBool(IsRight, False);
                anim.SetBool(IsLeft, False);
            }
            
            
        } else if (Input.GetAxis("Horizontal") > 0)
        {
            anim.SetBool(IsRight, True);
            isMovingLeft = False;
            isMovingRight = True;
            // If the player is pressing the button, reset the timer
            timer = intervals;
        } else if (Input.GetAxis("Horizontal") < 0)
        {
            anim.SetBool(IsLeft, True);
            isMovingRight = False;
            isMovingLeft = True;
            // If the player is pressing the button, reset the timer
            timer = intervals;
        }
        else
        {
            timer = intervals;
            isMovingForward = False;
            isMovingBackward = False;
            isMovingRight = False;
            isMovingLeft = False;
            anim.SetBool(IsForward, False);
            anim.SetBool(IsBackward, False);
            anim.SetBool(IsLeft, False);
            anim.SetBool(IsRight, False);
        }
    }


    private void FixedUpdate()
    {
        
    }

    IEnumerator Dash()
    {
        anim.SetTrigger(IsDashing);
        
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
