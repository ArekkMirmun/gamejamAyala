using UnityEngine;
public class BarrelController : MonoBehaviour
{
    public GameObject healthPack;
    
    public void Break()
    {

        //Takes a random change of 66% to spawn a health pack
        if (Random.Range(0, 3) != 0)
        {
            Instantiate(healthPack, transform.position, Quaternion.identity);
        }
        //Destroy the barrel
        Destroy(gameObject);
    }
}
