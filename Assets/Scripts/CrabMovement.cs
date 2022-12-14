using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class CrabMovement : MonoBehaviour
{

    public float speed = 15;

    private Rigidbody rb;
    private Animator anim;
    
    private bool isDodging = false;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {   
        if (Input.GetButtonDown("Fire1"))
        {
            anim.SetTrigger("IsAttack");
        } else if (Input.GetButtonDown("Fire2"))
        {
            anim.SetTrigger("IsHeavyAttack");
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
            // Debug.Log("Moving Right");
        } else if (rb.velocity.x < 0)
        {
            anim.SetBool("IsLeft", true);
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
        
        // TODO: fix slow at start fast at end
        
        // Can dodge while jumping?
        // Dodge
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDodging)
        {
            isDodging = true;
            anim.SetTrigger("IsDodging");
            Debug.Log("Dodge");
        }
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
                anim.SetTrigger("IsDodging");
            }
        }
        else if (Input.GetAxis("Vertical") < 0)
        {
            rb.AddForce(-transform.forward * speed, ForceMode.Acceleration);
            // Dodge
            if (isDodging)
            {  
                
                rb.AddForce(-transform.forward * (speed * 2), ForceMode.Impulse);
                anim.SetTrigger("IsDodging");
            }
        }

        
        if (Input.GetAxis("Horizontal") > 0)
        {
            rb.AddForce(transform.right * speed, ForceMode.Acceleration);
            // Dodge
            if (isDodging)
            {
                rb.AddForce(transform.right * (speed * 2), ForceMode.Impulse);
                anim.SetTrigger("IsDodging");
            }
        } else if (Input.GetAxis("Horizontal") < 0)
        {
            rb.AddForce(-transform.right * speed, ForceMode.Acceleration);
            // Dodge
            if (isDodging)
            {
                rb.AddForce(-transform.right * (speed * 2), ForceMode.Impulse);
                anim.SetTrigger("IsDodging");
            }
        }
        
        // Not moving dodge
        if (isDodging && Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
        {
            rb.AddForce(0, 0, 1 * (speed * 2), ForceMode.Impulse);
            anim.SetTrigger("IsDodging");
        }
        
        isDodging = false;
    }
}
