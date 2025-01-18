using UnityEngine;

public class Unit : MonoBehaviour
{
    public string unitName;
    public int unitLevel;

    public int damage;
    
    public int maxHP;
    public int currentHP;
    
    public bool TakeDamage(int damage)
    {
        currentHP -= damage;

        return currentHP <= 0;
    }
    
    public void Heal(int amount)
    {
        currentHP += amount;
        if (currentHP > maxHP)
            currentHP = maxHP;
    }
}
