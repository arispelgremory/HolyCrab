using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]

public class EnemyMovement : MonoBehaviour
{
    public NavMeshAgent agent;

    protected Transform target;
    public Transform enemyBase;
    public Transform alliedBase;
    public Transform player;
    
    protected Vector3 destination;

    protected bool hasCrab = false;

    protected int hp = 1;
    
    [SerializeField] protected float _movementSpeed = 5;
    
    // Crab Amount
    protected InGameUI gameUI;
    
    // Enemy Animation
    protected Animator anim;
    // Enemy's rigidbody
    protected Rigidbody rb;
    
    // Enemy's XP
    protected int xp;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        
        destination = agent.destination;

        // Default enemy is 5%
        xp = 5;

        agent.speed = _movementSpeed;
        gameUI = InGameUI.Instance;
    }

    // Update is called once per frame
    private void Update()
    {
        // If the enemy don't has a crab, go to the allied base
        if (!hasCrab && hp > 0)
        {
            target = alliedBase;
            agent.SetDestination(alliedBase.position);
        } else if (hasCrab && hp > 0)
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
            if (Vector3.Distance(destination, target.position) < 1.0f && hp > 0)
            {
                agent.speed = 5.0f;
            }
        }
        
    }
    
    // Animate which direction to walk based on it's velocity
    protected void AnimateMovement()
    {
        if ((agent.velocity.z == 0 && agent.velocity.x == 0) || hp < 1)
        {
            anim.SetBool("IsWalk", false);
            return;
        }
        anim.SetBool("IsWalk", true);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "ClawCollider")
        {
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
        // Player's crab amount will decrease
        gameUI.crabAmount--;
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
            StartCoroutine(EnemyDead());
        }
    }

    IEnumerator EnemyDead()
    {
        // If enemy has crab, will drop it
        if (hasCrab)
        {
            gameUI.crabAmount++;
        }
        
        // Pause to calculate the direction to run
        agent.speed = 0;
        // cancel walk animation
        anim.SetBool("IsWalk", false);
        anim.SetBool("IsDefeat", true);
        
        // Add player's FeverTime XP
        gameUI.SetSliderValue(xp);
        
        // Wait for 0.5 second
        yield return new WaitForSeconds(2.5f);
        // Destroy the enemy if ran away from the player about 50 units
        Destroy(gameObject);
        

    }
    
}
