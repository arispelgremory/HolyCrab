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
    public Transform player;
    
    Vector3 destination;

    private bool hasCrab = false;

    private int hp = 1;
    
    // Enemy Animation
    private Animator anim;
    // Enemy's rigidbody
    private Rigidbody rb;
    // Run away position
    private Vector3 runAwayPos;

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
        if (!hasCrab && hp > 0)
        {
            // Debug.Log("Going to allied base");
            target = alliedBase;
            agent.SetDestination(alliedBase.position);
        } else if (hasCrab && hp > 0)
        {
            // Debug.Log("Going to enemy base");
            target = enemyBase;
            agent.SetDestination(enemyBase.position);
        }
        else
        {
            // Debug.Log("Allied base Pos: " + alliedBase.position);
            Debug.Log("Destination Pos: " + destination);
            // Set destination to calculated run away position
            destination = runAwayPos;
            agent.destination = runAwayPos;
        }

        transform.LookAt(target);
        AnimateMovement();

        if (!Object.ReferenceEquals(destination, null))
        {
            // Update destination if the target moves one unit
            if (Vector3.Distance(destination, target.position) > 1.0f && hp > 0)
            {
                destination = target.position;
                agent.destination = destination;
            } 
        
            // If the target's unit is smaller than 1, slow down
            if (Vector3.Distance(destination, target.position) < 1.0f && hp > 0)
            {
                agent.speed = 5.0f;
            }
        }
        
    }
    
    // Animate which direction to walk based on it's velocity
    public void AnimateMovement()
    {
        if (hp >= 1)
        {
            anim.SetBool("IsWalk", true);
        }
    }
    
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "ClawCollider")
        {
            Debug.Log("Kena Hit");
            TakeDamage();
        }
        
        if (other.gameObject.name == "Crab Base" && hp > 0)
        {
            agent.isStopped = true;
            if (agent.velocity != Vector3.zero)
            {
                agent.velocity = Vector3.zero;
            }

            anim.SetBool("IsWalk", false);
            StartCoroutine(EnemyTakingCrab());
        }
        
        if (other.gameObject.name == "Enemy Tent" && hp > 0)
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
        // Enemy's hp is 0, will run away from the player and if the enemy has a crab, will drop it
        if (hp < 1)
        {
            // Pause to calculate the direction to run
            agent.speed = 0;
            // cancel walk animation
            anim.SetBool("IsWalk", false);
            StartCoroutine(EnemyDead());
        }
    }

    IEnumerator EnemyDead()
    {
        Vector3 runAwayDirection = -(transform.position - player.transform.position);
        runAwayDirection.y = 0; // Ignore y-axis
        runAwayPos = transform.position + runAwayDirection.normalized * 50; // 50 units away from the player

        Debug.Log("163: Run away pos: " + runAwayPos);

        // Set destination to calculated run away position
        destination = runAwayPos;
        agent.destination = runAwayPos;
        // Resume movement
        anim.SetBool("IsDefeat", true);
        agent.speed = 1000;
        
        // Wait for 0.5 second
        yield return new WaitForSeconds(5.0f);
            
        // Destroy the enemy if ran away from the player after 5 seconds
        Destroy(gameObject);
        

    }
    
}
