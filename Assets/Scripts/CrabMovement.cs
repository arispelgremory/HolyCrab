using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using UnityEngine;

public class CrabMovement : MonoBehaviour
{

    public float speed = 15;
    public float friction = 0.5f;
    public float frictionDuringDash = 3.0f;

    private Rigidbody rb;
    private Animator anim;
    
    
    private bool attackable = true;
    
    // Jump properties
    bool isJumping = false;
    public int jumpForce = 2;
    public float jumpTime = 0.5f;
    public int jumpDelay = 500; // jump time delay in milliseconds
    public float jumpFallMultiplier = 2.5f;
    public float jumpLowMultiplier = 2f;
    
    // Dash properties
    private bool isDodging = false;
    public float dashCooldownTime = 10.0f; // The cooldown time in seconds
    private float dashRemainingCooldownTime; // The remaining cooldown time
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        dashRemainingCooldownTime = dashCooldownTime;
    }

    // Update is called once per frame
    void Update()
    {   
        // Cannot dodge while attack
        // Jump dodge is ok
        // cannot attack while jump

        // TODO: set timeouts for dodge and attack
        if (Input.GetButtonDown("Fire1") && attackable)
        {
            // Trigger attack animation
            anim.SetTrigger("IsAttack");
        } else if (Input.GetButtonDown("Fire2") && attackable)
        {
            // Trigger heavy attack animation
            anim.SetTrigger("IsHeavyAttack");
        } else if (Input.GetButtonDown("Shift") && !isDodging)
        {
            if (rb.velocity.z >= 0)
            {
                // Trigger dodge animation
                anim.SetTrigger("IsDodging");
            }
            else
            {
                anim.SetTrigger("IsDodgingBackwards");
            }
            
            setDashFriction();
            isDodging = true;
            
        }
        
        // Movement
        if (rb.velocity.z > 0)
        {
            anim.SetBool("IsForward", true);
            anim.SetBool("IsRight", false);
            anim.SetBool("IsLeft", false);
            // Debug.Log("Moving Forward");
        } else if (rb.velocity.z < 0)
        {
            anim.SetBool("IsBackward", true);
            anim.SetBool("IsRight", false);
            anim.SetBool("IsLeft", false);
            // Debug.Log("Moving Backwards");
        } else if (rb.velocity.x > 0)
        {
            anim.SetBool("IsRight", true);
            anim.SetBool("IsLeft", false);
            // Debug.Log("Moving Right");
        } else if (rb.velocity.x < 0)
        {
            anim.SetBool("IsLeft", true);
            anim.SetBool("IsRight", false);
            // Debug.Log("Moving Left");
        }
        else
        {
            anim.SetBool("IsForward", false);
            anim.SetBool("IsBackward", false);
            anim.SetBool("IsRight", false);
            anim.SetBool("IsLeft", false);
            // Debug.Log("Not Moving");
            
        }
    }

    private void FixedUpdate()
    {
        
        // Add diagonal movement
        if(Input.GetAxis("Vertical") > 0)
        {
            rb.AddForce(transform.forward * (speed * Time.deltaTime), ForceMode.Acceleration);
            // Dodge
            if (isDodging)
            {
                rb.AddForce(transform.forward * (speed * jumpForce), ForceMode.Impulse);
                // anim.SetBool("IsDodging", true);
            }
        }
        else if (Input.GetAxis("Vertical") < 0)
        {
            rb.AddForce(-transform.forward * (speed * Time.deltaTime), ForceMode.Acceleration);
            // Dodge
            if (isDodging)
            {
                
                rb.AddForce(-transform.forward * (speed * jumpForce), ForceMode.Impulse);
                // anim.SetBool("IsDodging", true);
            }
        }

        
        if (Input.GetAxis("Horizontal") > 0)
        {
            rb.AddForce(transform.right * (speed * Time.deltaTime), ForceMode.Acceleration);
            // Dodge
            if (isDodging)
            {
                rb.AddForce(transform.right * (speed * jumpForce), ForceMode.Impulse);
                // anim.SetBool("IsDodging", true);
            }
        } else if (Input.GetAxis("Horizontal") < 0)
        {
            rb.AddForce(-transform.right * (speed * Time.deltaTime), ForceMode.Acceleration);
            // Dodge
            if (isDodging)
            {
                
                rb.AddForce(-transform.right * (speed * jumpForce), ForceMode.Impulse);
                // anim.SetBool("IsDodging", true);
            }
        }
        
        // Not moving dodge
        if (isDodging && Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
        {
            
            rb.AddForce(0, 0, 1 * (speed * jumpForce), ForceMode.Impulse);
            // anim.SetBool("IsDodging", true);
        }

        if (isDodging)
        {
            isDodging = false;
            Invoke("setNormalFriction", 0.5f);
        }
        
        // anim.SetBool("IsDodging", false);
    }

    private async void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Terrain")
        {
            await Task.Delay(jumpDelay);
            isJumping = false;
            attackable = true;
        }
    }


    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Terrain")
        {
            isJumping = true;
            // Cannot attack while jumping
            attackable = false;
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
