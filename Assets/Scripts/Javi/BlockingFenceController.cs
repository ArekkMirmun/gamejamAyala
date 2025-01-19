using UnityEngine;

public class BlockingFenceController : MonoBehaviour
{
    public GameObject objectToActivate;
    public GameObject signToActivate;
    public GameObject signToDesactivate;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerInteract playerInteract = other.GetComponent<PlayerInteract>();
            playerInteract.blockingFenceController = this;

            if (objectToActivate != null)
            {
                objectToActivate.SetActive(true);
            }

            if (signToActivate != null)
            {
                signToActivate.SetActive(true);
            }

            if (signToDesactivate != null)
            {
                signToDesactivate.SetActive(false);
            }
        }
    }
}
