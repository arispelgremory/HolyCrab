using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItems : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    // Detect Collision Trigger
    void OnTriggerEnter(Collider other)
    {
        // Check if the object that collided with the player is a coin
        if (other.gameObject.CompareTag("LevelUpItem"))
        {
            // Destroy the coin
            Destroy(other.gameObject);
            
            // Trigger the level up UI
            
            
        }
    }
    
}
