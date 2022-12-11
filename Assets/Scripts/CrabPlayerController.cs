using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabPlayerController : MonoBehaviour
{
    Animator anim;
    public float speed;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float x;
        float z;

        x = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        z = Input.GetAxis("Vertical") * Time.deltaTime * speed;


        // Check if the up or down button is press or not
        if(Input.GetAxis("Vertical") != 0)
        {
            anim.SetBool("IsForward", true);
        }else{
            anim.SetBool("IsForward", false);
        }

        // TO-DO: Movement Direction
        if(Input.GetAxis("Horizontal") != 0)
        {
            speed = 7;
            anim.SetBool("IsLeft", true);
        }else{
            speed = 0;
            anim.SetBool("IsLeft", false);
        }

        //  Go Forward
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            speed = 7;
            anim.SetBool("IsForward", true);
            transform.Translate(x, 0, z++);
        }else{
            speed = 0;
            anim.SetBool("IsForward", false);
        }

        //  Go Backward
        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            speed = 7;
            anim.SetBool("IsForward", true);
        }else{
            speed = 0;
            anim.SetBool("IsForward", false);
        }

        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            speed = 7;
            anim.SetBool("IsRunning", true);
        } 

        if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            anim.SetBool("IsRunning", false);
        } 
        if(Input.GetButtonDown("Fire1"))
        {
            anim.SetTrigger("IsAttack");
        } 

        transform.Translate(x, 0, z);
        }
}
