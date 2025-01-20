using JetBrains.Annotations;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public DialogueController dialogueController;
    public PickableController pickableController;
    public BlockingFenceController blockingFenceController;

    public SettingsMenu settingsMenu;
    private bool _isPaused = false;
    

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
        _isPaused = true;
        settingsMenu.OpenSettings();
        if (BossAnimation.instance)
        {
            BossAnimation.instance.PauseAnimation();
        }
    }
    
    public void OnUnpause()
    {
        _isPaused = false;
        settingsMenu.CloseSettings();
        if (BossAnimation.instance)
        {
            BossAnimation.instance.ResumeAnimation();
        }
    }
}
