using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class CrabMovement : MonoBehaviour
{

    public float speed = 15;
    public float friction = 0.5f;
    public float frictionDuringDash = 2.5f;

    private Rigidbody rb;
    private Animator anim;
    
    private bool isDodging = false;
    public bool attackable = true;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
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
            // Trigger dodge animation
            anim.SetTrigger("IsDodging");
            isDodging = true;
            setDashFriction();
            
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
        //
        // // TODO: fix slow at start fast at end
    }

    private void FixedUpdate()
    {
        
        // Add diagonal movement
        if(Input.GetAxis("Vertical") > 0)
        {
            rb.AddForce(transform.forward * speed, ForceMode.Acceleration);
            // Dodge
            if (isDodging)
            {
                
                rb.AddForce(transform.forward * (speed * 2), ForceMode.Impulse);
                // anim.SetBool("IsDodging", true);
            }
        }
        else if (Input.GetAxis("Vertical") < 0)
        {
            rb.AddForce(-transform.forward * speed, ForceMode.Acceleration);
            // Dodge
            if (isDodging)
            {
                
                rb.AddForce(-transform.forward * (speed * 2), ForceMode.Impulse);
                // anim.SetBool("IsDodging", true);
            }
        }

        
        if (Input.GetAxis("Horizontal") > 0)
        {
            rb.AddForce(transform.right * speed, ForceMode.Acceleration);
            // Dodge
            if (isDodging)
            {
                rb.AddForce(transform.right * (speed * 2), ForceMode.Impulse);
                // anim.SetBool("IsDodging", true);
            }
        } else if (Input.GetAxis("Horizontal") < 0)
        {
            rb.AddForce(-transform.right * speed, ForceMode.Acceleration);
            // Dodge
            if (isDodging)
            {
                
                rb.AddForce(-transform.right * (speed * 2), ForceMode.Impulse);
                // anim.SetBool("IsDodging", true);
            }
        }
        
        // Not moving dodge
        if (isDodging && Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
        {
            
            rb.AddForce(0, 0, 1 * (speed * 2), ForceMode.Impulse);
            // anim.SetBool("IsDodging", true);
        }

        if (isDodging)
        {
            isDodging = false;
            Invoke("setNormalFriction", 0.5f);
        }
        
        // anim.SetBool("IsDodging", false);
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
