using UnityEngine;

public class CharacterAttack : MonoBehaviour
{
    public HitTrigger hitTrigger;
    
    public void OnAttack()
    {
        // Call the OnAttack method
        hitTrigger.OnAttack();
    }
}
