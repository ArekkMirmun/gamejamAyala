using UnityEngine;
using UnityEngine.AI;

public class AnimalMovement : MonoBehaviour
{
    
    [SerializeField] private Animator animator;
    [SerializeField] private NavMeshAgent agent;

    void FixedUpdate()
    {
        // Check if the Animal NPC is moving and update animator speed variable accordingly
        if (agent.velocity.sqrMagnitude > 0.1f) // Check if the agent is moving
        {
            animator.SetFloat("speed", agent.velocity.magnitude);
        }
        else
        {
            animator.SetFloat("speed", 0f);
        }
    }
}
