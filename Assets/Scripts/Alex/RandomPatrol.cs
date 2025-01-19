using UnityEngine;
using UnityEngine.AI;

public class RandomPatrol : MonoBehaviour
{
    private NavMeshAgent agent;

    public float wanderRadius = 10f;
    public float wanderInterval = 5f;

    private float timer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        timer = wanderInterval;
        
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