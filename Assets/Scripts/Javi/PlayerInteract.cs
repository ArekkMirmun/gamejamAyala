using JetBrains.Annotations;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public DialogueController dialogueController;
    public PickableController pickableController;
    public BlockingFenceController blockingFenceController;

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
}
