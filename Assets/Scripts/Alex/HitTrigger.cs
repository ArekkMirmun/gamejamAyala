using System.Collections;
using UnityEngine;

public class HitTrigger : MonoBehaviour
{
    private static readonly int Hit = Animator.StringToHash("Hit");
    public Collider hitCollider;
    [SerializeField] private Animator animator; // Reference to the player's Animator
    [SerializeField] private AudioSource attackSound; // Reference to the player's attack sound
    [SerializeField] private AudioSource cowHit1; // Reference to the cow hit sound
    [SerializeField] private AudioSource cowHit2; // Reference to the cow hit sound
    private bool _isAttacking = false;
    
   public void OnTriggerEnter(Collider other)
   {
       if (CheckInBattle()) return;
       
       if (other.gameObject.CompareTag("Enemy"))
       {
           GetComponent<Collider>().enabled = true;
           PlayerInfo.Instance.EnemyEncountered(other.gameObject);
       }

       if (other.gameObject.CompareTag("Barrel"))
       {
           print("Barrel hit");
           GetComponent<Collider>().enabled = true;
           BarrelController barrel = other.gameObject.GetComponent<BarrelController>();
           barrel.Break();
       }
       //if cow is hit
         if (other.gameObject.CompareTag("Cow"))
         {
             //random to choose between two sounds
                int random = Random.Range(0, 2);
                if (random == 0)
                {
                    cowHit1.Play();
                }
                else
                {
                    cowHit2.Play();
                }
         }
   }
   
   // OnAttack is called when the player attacks
    public void OnAttack()
    {
        if (CheckInBattle() || _isAttacking) return;
        attackSound.Play();
        animator.SetTrigger(Hit);
        StartCoroutine(Attack());
    }

    private static bool CheckInBattle()
    {
        return BattleSystem.Instance.state != BattleState.WAIT;
    }

    private IEnumerator Attack()
    {
        _isAttacking = true;
        //Wait 0.5 seconds for animation to hit
        yield return new WaitForSeconds(0.5f);
        //enable the collider
        GetComponent<Collider>().enabled = true;
        //wait for 0.5 seconds
        yield return new WaitForSeconds(0.5f);
        _isAttacking = false;
        //disable the collider
        GetComponent<Collider>().enabled = false;
    }
}
