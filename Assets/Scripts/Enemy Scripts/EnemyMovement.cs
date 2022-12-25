using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyMovement : MonoBehaviour
{
    public NavMeshAgent agent;

    private Transform target;
    public Transform enemyBase;
    public Transform alliedBase;
    
    Vector3 destination;

    private bool hasCrab = false;

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
        destination = agent.destination;
    }

    // Update is called once per frame
    void Update()
    {
        
        // If the enemy don't has a crab, go to the allied base
        if (!hasCrab)
        {
            target = alliedBase;
            agent.SetDestination(alliedBase.position);
        } else
        {
            target = enemyBase;
            agent.SetDestination(enemyBase.position);
        }
        
        transform.LookAt(target);
        AnimateMovement();

        if (!Object.ReferenceEquals(destination, null))
        {
            // Update destination if the target moves one unit
            if (Vector3.Distance(destination, target.position) > 1.0f)
            {
                destination = target.position;
                agent.destination = destination;
            } 
        
            // If the target's unit is smaller than 1, slow down
            if (Vector3.Distance(destination, target.position) < 1.0f)
            {
                agent.speed = 5.0f;
            }
        }
        
    }
    
    // Animate which direction to walk based on it's velocity
    public void AnimateMovement()
    {
        anim.SetBool("IsWalk", true);
    }
    
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "ClawCollider")
        {
            // Debug.Log("Kena Hit");
            TakeDamage();
        }
        
        if (other.gameObject.name == "Crab Base")
        {
            agent.isStopped = true;
            if (agent.velocity != Vector3.zero)
            {
                agent.velocity = Vector3.zero;
            }

            anim.SetBool("IsWalk", false);
            StartCoroutine(EnemyTakingCrab());
        }
        
        if (other.gameObject.name == "Enemy Tent")
        {
            agent.isStopped = true;
            if (agent.velocity != Vector3.zero)
            {
                agent.velocity = Vector3.zero;
            }
            anim.SetBool("IsWalk", false);
            StartCoroutine(EnemyDroppingCrab());
        }
    }

    IEnumerator EnemyTakingCrab()
    {
        hasCrab = true;
        yield return new WaitForSeconds(2);
        anim.SetBool("IsWalk", true);
        agent.isStopped = false;
    }

    IEnumerator EnemyDroppingCrab()
    {
        yield return new WaitForSeconds(2);
        // Will destroy this AI after back to tent
        Destroy(gameObject);
    }

    public void TakeDamage()
    {
        hp--;
    }
    
}
