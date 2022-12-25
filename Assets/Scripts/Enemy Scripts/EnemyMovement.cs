using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public NavMeshAgent agent;
    public LayerMask whatIsGround, whatIsPlayer;
    
    // Patrolling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;
    
    // Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    
    // States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;
    
    public Transform targetTransform;
    
    public bool hadTakenDamage = false;

    private int hp = 1;
    
    // Enemy Animation
    private Animator anim;
    // Enemy's rigidbody
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(targetTransform);
        AnimateMovement();

        if (hp < 1)
        {
            Destroy(gameObject);
        }

    }
    
    // Animate which direction to walk based on it's velocity
    public void AnimateMovement()
    {
        // anim.SetBool("IsWalk", true);
    }
    
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "ClawCollider")
        {
            // Debug.Log("Kena Hit");
            TakeDamage();
        }
    }

    public void TakeDamage()
    {
        hp--;
    }
    
}
