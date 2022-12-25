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
        
        // Vector3 destination = new Vector3(0, 0, 0);
        // agent.SetDestination(destination);

        // Update destination if the target moves one unit
        // if (Vector3.Distance(destination, target.position) > 1.0f)
        // {
        //     destination = target.position;
        //     agent.destination = destination;
        // }

        
        if (agent.pathStatus == NavMeshPathStatus.PathComplete) {
            // The agent has a valid path
            Debug.Log("NavMeshAgent have a valid path");
        } else {
            // The agent does not have a valid path
            Debug.Log("NavMeshAgent does not have a valid path");
        }



    }

}
