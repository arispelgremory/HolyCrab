using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class AttackScript : MonoBehaviour
{
    public LayerMask enemyLayer; // The layer of the enemy objects

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the collision is with an enemy object
        if (enemyLayer == (enemyLayer | (1 << collision.gameObject.layer)))
        {
            // If the collision is with an enemy, call the TakeDamage method of the Enemy script
            EnemyMovement enemy = collision.gameObject.GetComponent<EnemyMovement>();
            if (enemy != null)
            {
                enemy.TakeDamage();
            }
        }
    }
}

