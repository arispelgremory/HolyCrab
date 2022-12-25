using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]

public class FollowTarget : MonoBehaviour
{
    public Transform target;
    Vector3 destination;
    NavMeshAgent agent;
    
    

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        destination = agent.destination;
    }

    // Update is called once per frame
    void Update()
    {
        // Update destination if the target moves one unit
        if (Vector3.Distance(destination, target.position) > 1.0f)
        {
            destination = target.position;
            agent.destination = destination;
        }
    }

}
