using JetBrains.Annotations;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public DialogueController dialogueController;
    public PickableController pickableController;
    public BlockingFenceController blockingFenceController;

    public GameObject SettingsHUD;
    public GameObject TimeHUD;

    public void OnInteract()
    {
        print("Interacting with NPC");
        if (dialogueController && dialogueController.isPlayerInRange)
        {
            dialogueController.StartDialogue();
        }
        else if (pickableController && pickableController.isPlayerInRange)
        {
            pickableController.StartDialogue();
        }
    }

    public void OnNextText()
    {
        if (dialogueController && dialogueController.isPlayerInRange)
        {
            dialogueController.NextText();
        }

        if (pickableController && pickableController.isPlayerInRange)
        {
            pickableController.NextText();
        }
    }

    public void OnPause()
    {
        Time.timeScale = 0f;
        TimeHUD.SetActive(false);
        SettingsHUD.SetActive(true);
    }
    
    public void OnUnpause()
    {
        Time.timeScale = 1f;
        TimeHUD.SetActive(true);
        SettingsHUD.SetActive(false);
    }
}
