using System;
using UnityEngine;

public class InitialDialogue : MonoBehaviour
{
    public PickableController pickable;
    
    public PlayerInteract playerInteract;

    
    //Start with delay
    private void Start()
    {
        Invoke(nameof(StartDialogue), 1);
    }
    
    private void StartDialogue()
    {
        playerInteract.pickableController = pickable;
        pickable.StartDialogue();
    }
}
