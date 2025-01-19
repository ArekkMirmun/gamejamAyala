using System.Collections;
using UnityEngine;
public class BarrelController : MonoBehaviour
{
    public GameObject healthPack;
    public AudioSource breakSound;
    public void Break()
    {
        //Play the break sound
        StartCoroutine(PlaySoundAndDestroy());

        //Takes a random change of 66% to spawn a health pack
        if (Random.Range(0, 3) != 0)
        {
            Instantiate(healthPack, transform.position, Quaternion.identity);
        }
        
        //hides barrel for clip seconds
        gameObject.SetActive(false);
    }
    
    private IEnumerator PlaySoundAndDestroy()
    {
        //Play the break sound
        breakSound.Play();
        //Wait for the sound to finish
        yield return new WaitForSeconds(breakSound.clip.length);
        //Destroy the barrel
        Destroy(gameObject);
    }
}
