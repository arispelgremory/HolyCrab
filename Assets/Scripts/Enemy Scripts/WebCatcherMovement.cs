using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class WebCatcherMovement : EnemyMovement
{
    private bool _isAttacking;
    
    [SerializeField] private float _attackRange;
    [SerializeField] private float _actionIntervals;
    [SerializeField] private float _attackDuration;
    private GameObject enemyWeapon;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        
        // Default enemy is 5%
        xp = 10;
        
        destination = agent.destination;
        gameUI = InGameUI.Instance;
        enemyWeapon =  GameObject.FindWithTag("EnemyAttacker");
        enemyWeapon.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
        // This enemy will chase down player
        if (hp > 0)
        {
            target = player;
            agent.SetDestination(player.position);
        }
        else
        {
            agent.isStopped = true;
        }
        
        // Update destination if the target moves one unit
        if (Vector3.Distance(destination, target.position) > _attackRange)
        {
            destination = target.position;
            agent.destination = destination;
        } 
        
        transform.LookAt(target);
        AnimateMovement();
        
        
        // If player is in range, stop then attack
        if (Vector3.Distance(transform.position, player.position) <= _attackRange && !_isAttacking)
        {
            // Stop
            agent.speed = 0;
            agent.isStopped = true;
            StartCoroutine(CatcherAttack());
        }
        
    }

    protected void AnimateMovement()
    {
        // Move Animation from parent class
        base.AnimateMovement();
        
    }

    IEnumerator CatcherAttack()
    {
        float animateInteraval = 0.5f;
        
        _isAttacking = true;
        // Attack animation
        anim.SetBool("IsWalk", false);
        anim.SetTrigger("IsAttack");

        
        yield return new WaitForSeconds(animateInteraval);
        
        enemyWeapon.SetActive(true);
        yield return new WaitForSeconds(_attackDuration);
        enemyWeapon.SetActive(false);

        // Stun for a while if enemy animated attack
        yield return new WaitForSeconds(_actionIntervals);
        _isAttacking = false;
        agent.isStopped = false;
        agent.speed = _movementSpeed;
    }

    protected void EnemyDead()
    {
        // Disable the weapon's collider before defeat
        enemyWeapon.SetActive(false);
        base.EnemyDead();
    }

}
