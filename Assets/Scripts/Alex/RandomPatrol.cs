using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class RandomPatrol : MonoBehaviour
{
    private NavMeshAgent agent;

    public float wanderRadius = 10f;
    public float wanderInterval = 5f;
    public bool shouldBePaused;

    private float timer;

    private float speed;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        timer = wanderInterval;
        
        //gets speed from agent and sets private speed variable to it
        speed = agent.speed;
        //start at random timer value to avoid all NPCs moving at the same time
        timer = Random.Range(0, wanderInterval);

        // Disable automatic rotation by the NavMeshAgent
        agent.updateRotation = false;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= wanderInterval)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            agent.SetDestination(newPos);
            timer = 0;
        }

        // Lock rotation to a specific value (e.g., initial rotation)
        LockRotation();
        //We should flip the NPC model based on the direction it is moving to look left or right
        //This is done by checking the movement on the x axis, and, if it is negative, we rotate the model to look left
        //with x rotation of -59 and z rotation of -180
        
        if (agent.velocity.x > 0)
        {
            transform.rotation = Quaternion.Euler(-45, 0, 180);
        }
        else
        {
            transform.rotation = Quaternion.Euler(3, 0, 0);
        }
        
    }

    private void FixedUpdate()
    {
        //checks if time is paused
        TimeChangeScript timeChangeScript = TimeChangeScript.Instance;
        
        
        if (timeChangeScript.isPaused)
        {
            //sets speed to 0
            agent.speed = 0;
            agent.velocity = Vector3.zero;
        }
        else
        {
            agent.speed = speed;
        }
    }


    void LockRotation()
    {
        // Keep the NPC's rotation constant
        transform.rotation = Quaternion.identity; // Change to desired rotation if needed
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float distance, int layerMask)
    {
        Vector3 randomDirection = Random.insideUnitSphere * distance;
        randomDirection += origin;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection, out navHit, distance, layerMask);

        return navHit.position;
    }
}