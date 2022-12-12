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
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {   
        // Movement
        if (rb.velocity.z > 0)
        {
            anim.SetBool("IsForward", true);
            Debug.Log("Moving Forward");
        } else if (rb.velocity.z < 0)
        {
            anim.SetBool("IsBackward", true);
            Debug.Log("Moving Backwards");
        } else if (rb.velocity.x > 0)
        {
            anim.SetBool("IsRight", true);
            Debug.Log("Moving Right");
        } else if (rb.velocity.x < 0)
        {
            anim.SetBool("IsLeft", true);
            Debug.Log("Moving Left");
        }
        else
        {
            anim.SetBool("IsForward", false);
            anim.SetBool("IsBackward", false);
            anim.SetBool("IsRight", false);
            anim.SetBool("IsLeft", false);
            Debug.Log("Not Moving");
        }
        
        // TODO: fix slow at start fast at end
        
        
    }

    private void FixedUpdate()
    {
        
        // Add diagonal movement
        if(Input.GetAxis("Vertical") > 0)
        {
            rb.AddForce(transform.forward * speed, ForceMode.Acceleration);
        }
        else if (Input.GetAxis("Vertical") < 0)
        {
            rb.AddForce(-transform.forward * speed, ForceMode.Acceleration);
        }

        
        if (Input.GetAxis("Horizontal") > 0)
        {
            rb.AddForce(transform.right * speed, ForceMode.Acceleration);
        } else if (Input.GetAxis("Horizontal") < 0)
        {
            rb.AddForce(-transform.right * speed, ForceMode.Acceleration);
            
        }
    }
}
