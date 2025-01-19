using System.Collections;
using UnityEngine;

public class HitTrigger : MonoBehaviour
{
    public Collider hitCollider;
   public void OnTriggerEnter(Collider other)
   {
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
   }
   
   // OnAttack is called when the player attacks
    public void OnAttack()
    {
        StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        //enable the collider
        GetComponent<Collider>().enabled = true;
        //wait for 0.5 seconds
        yield return new WaitForSeconds(0.5f);
        //disable the collider
        GetComponent<Collider>().enabled = false;
    }
}
