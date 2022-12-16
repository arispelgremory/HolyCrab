using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterJump : MonoBehaviour
{
    private Rigidbody rb;
    public float jumpForce = 20f;
    public float jumpTime = 0.5f;
    public float jumpDelay = 0.5f;
    public float jumpFallMultiplier = 2.5f;
    public float jumpLowMultiplier = 2f;
    
    bool isJumping = false;
    // CrabMovement crabMovement;
    
    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        // crabMovement = GetComponent<CrabMovement>();
    }

    // Update is called once per frame
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }


    private void FixedUpdate()
    {
        
    }
    
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Terrain")
        {
            isJumping = false;
            // crabMovement.attackable = true;
        }
    }


    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Terrain")
        {
            isJumping = true;
            // Cannot attack while jumping
            // crabMovement.attackable = false;
        }
    }
}
