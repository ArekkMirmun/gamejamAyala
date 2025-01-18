using UnityEngine;

//Enum for the different enemy types
public enum EnemyType
{
    Sword,
    Gun,
    Rifle,
    Boss
}
public class EnemyInfo : MonoBehaviour
{
    public EnemyType enemyType;
}
