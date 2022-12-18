using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class CrabMovement : MonoBehaviour
{
    // Animations
    public static readonly string IsForward = "IsForward";
    public static readonly string IsBackward = "IsBackward";
    public static readonly string IsLeft = "IsLeft";
    public static readonly string IsRight = "IsRight";
    public static readonly string IsAttack = "IsAttack";
    public static readonly string IsHeavyAttack = "IsHeavyAttack";
    public static readonly string IsDodging = "IsDodging";
    public static readonly string IsDodgingBackwards = "IsDodgingBackwards";
    
    // Boolean
    public static readonly bool  True = true;
    public static readonly bool  False = false;

   

    private Rigidbody rb;
    private Animator anim;
    
    
    private bool attackable = true;
    
    // Movements
    public float intervals = 1.0f;
    private float timer = 0.0f;
    private bool isMovingForward = false;
    private bool isMovingBackward = false;
    private bool isMovingLeft = false;
    private bool isMovingRight = false;
    public float movingSpeed = 15;
    public float friction = 2.5f;
    public float frictionDuringDash = 5.0f;
    
    // Jump properties
    bool isJumping = False;
    bool isGrounded = True;
    public float jumpForce = 2;
    public int jumpDelay = 500; // jump time delay in milliseconds
    public float jumpCoolDown = 1.5f;
    private float jumpTimer = 1.5f;
    public float jumpHeight = 2.0f;
    public float gravity = 9.81f;   // Float can't negative???

    // Dash properties
    private bool isDashing = false;
    public float dashCooldownTime = 1.0f; // Dash cooldown time in seconds
    private float dashTimer = 5.0f; // The remaining cooldown time
    public float dashForce = 1.2f;
    public int dashDelayInMilliseconds = 500;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        setNormalFriction();
    }

    // Update is called once per frame
    void Update()
    {   
        // Cannot dodge while attack
        // Jump dodge is ok
        // cannot attack while jump
        jumpTimer += Time.deltaTime;
        dashTimer += Time.deltaTime;

        if (dashTimer >= dashCooldownTime)
        {
            // Debug.Log("can dash: " + dashTimer);
        }
        
        // Attack
        if (Input.GetButtonDown("Fire1"))
        {
            // Trigger attack animation
            anim.SetTrigger("IsAttack");
        } 
        else if (Input.GetButtonDown("Fire2"))
        {
            // Trigger heavy attack animation
            anim.SetTrigger("IsHeavyAttack");
        }
        else
        {
            // Jump
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded && jumpTimer > jumpCoolDown)
            {
                Debug.Log("Jumping");
                isJumping = True;
            }
            
            // Dash
            if (Input.GetKeyDown(KeyCode.LeftShift) && (dashTimer > dashCooldownTime))
            {
                Debug.Log("Dashing");
                isDashing = True;
                animateDodge();
            }
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
    
    void animateDodge()
    {
        if (isMovingForward)
        {
            anim.SetTrigger(IsDodging);
        }
        else if(isMovingBackward)
        {
            anim.SetTrigger(IsDodgingBackwards);
        }

        dashTimer = 0.0f;
    }

    private void calculateMoving(Vector3 direction, ForceMode forceMode)
    {
        Debug.Log("Movement: " + direction * (movingSpeed * Time.deltaTime));
        rb.AddForce(direction * (movingSpeed * Time.deltaTime), forceMode);
    }

    public IEnumerator calculateDash(Vector3 direction, ForceMode forceMode)
    {
        yield return new WaitForSeconds(dashDelayInMilliseconds);
        Debug.Log("Dash: " + (movingSpeed * dashForce * Time.deltaTime));
        rb.AddForce(direction * (movingSpeed * dashForce * Time.deltaTime), forceMode);
    }

    private void FixedUpdate()
    {

        if (isJumping && isGrounded)
        {
            // Jump
            jumpForce = Mathf.Sqrt(-2 * -gravity * jumpHeight) * rb.mass;
            rb.AddForce(new Vector3(0, jumpForce, 0) * Time.deltaTime, ForceMode.Impulse);
        } else if (isDashing)
        {
            setDashFriction();
        }
        
        // Add vertical movement velocity
        
        // TODO: fix forward and backward velocity is not the same
        if (isMovingForward)
        {
            calculateMoving(transform.forward, ForceMode.Acceleration);
            if (isDashing)
            {
                StartCoroutine(calculateDash(transform.forward, ForceMode.Impulse));
            }
            else
            {
                calculateMoving(transform.forward, ForceMode.Acceleration);
            }
        } else if (isMovingBackward)
        {
            if (isDashing)
            {
                StartCoroutine(calculateDash(-transform.forward, ForceMode.Impulse));
            }
            else
            {
                calculateMoving(-transform.forward, ForceMode.Acceleration);
            }
        } 
        
        if (isMovingLeft)
        {
            if (isDashing)
            {
                StartCoroutine(calculateDash(-transform.right, ForceMode.Impulse));
            }
            else
            {
                calculateMoving(-transform.right, ForceMode.Acceleration);
            }
        } else if (isMovingRight)
        {
            if (isDashing)
            {
                StartCoroutine(calculateDash(transform.right, ForceMode.Impulse));
            }
            else
            {
                calculateMoving(transform.right, ForceMode.Acceleration);
            }
        }
        else if (isDashing)
        {
            isDashing = False;
        }

        // Not moving dodge
        // if (isDashing && Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
        // {
        // }


        isDashing = False;
        setNormalFriction();
    }

    private async void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Terrain")
        {
            isGrounded = True;
            // attackable = true;
        }
    }


    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Terrain")
        {
            isGrounded = False;
            isJumping = False;
            // Cannot attack while jumping
            // attackable = false;
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
