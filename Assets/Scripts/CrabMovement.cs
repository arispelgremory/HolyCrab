using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class CrabMovement : MonoBehaviour
{

    public float speed;

    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");
        
        
        
        
    }

    private void FixedUpdate()
    {
        if(Input.GetAxis("Vertical") > 0)
        {
            rb.AddForce(transform.forward * speed, ForceMode.Acceleration);
        }
        else if (Input.GetAxis("Vertical") < 0)
        {
            rb.AddForce(-transform.forward * speed, ForceMode.Acceleration);
        }

        // TODO: Fix over rotation
        if (Input.GetAxis("Horizontal") > 0)
        {
            rb.AddForce(transform.right * speed, ForceMode.Acceleration);
        } else if (Input.GetAxis("Horizontal") < 0)
        {
            rb.AddForce(-transform.right * speed, ForceMode.Acceleration);
            
        }
    }
}
