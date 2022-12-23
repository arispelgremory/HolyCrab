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
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        transform.LookAt(targetTransform);

        if (hadTakenDamage)
        {
            Destroy(gameObject);
        }

    }
    
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "ClawCollider")
        {
            // Debug.Log("Kena Hit");
            TakeDamage();
        }
    }


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    
    public void TakeDamage()
    {
        hadTakenDamage = true;
    }
    
}
