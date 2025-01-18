using JetBrains.Annotations;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public DialogueController dialogueController;

    public void OnInteract()
    {
        print("Interacting with NPC");
        if (dialogueController.isPlayerInRange)
        {
            
            dialogueController.StartDialogue();
        }
    }

    public void OnNextText()
    {
        if (dialogueController.isPlayerInRange)
        {
            dialogueController.NextText();
        }
    }
}
