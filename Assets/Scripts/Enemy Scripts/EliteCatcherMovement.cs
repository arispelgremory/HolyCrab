using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EliteCatcherMovement : EnemyMovement
{
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        
        hp = 2;
        xp = 20;
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }
}
