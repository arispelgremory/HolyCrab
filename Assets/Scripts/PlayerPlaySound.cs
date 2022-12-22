using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class PlayerPlaySound : MonoBehaviour
{
    public AudioSource actionClip;
    public AudioClip atkSE, hAtkSE, dashSE, jumpSE, walkSE;

    public void AttackSound() {
        actionClip.clip = atkSE;
        actionClip.Play();
    }

    public void HeavyAttackSound() {
        actionClip.clip = hAtkSE;
        actionClip.Play();
    }

    public void DashSound() {
        actionClip.clip = dashSE;
        actionClip.Play();
    }

    public void JumpSound() {
        actionClip.clip = jumpSE;
        actionClip.Play();
    }

    public void WalkSound() {
        actionClip.clip = walkSE;
        actionClip.Play();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            JumpSound();
        }
        else if (Input.GetButtonDown("Shift"))
        {
            DashSound();
        }
        else if (Input.GetButtonDown("Fire1"))
        {
            AttackSound();
        }
        else if (Input.GetButtonDown("Fire2"))
        {
            HeavyAttackSound();
        }
        // else if (Input.GetAxis("Horizontal"))
        // {
        //     WalkSound();
        // }
    }
}
