using System;
using UnityEngine;
using TMPro;
using System.Collections;

//Enum for different types of Consumibles
public enum ConsumibleType
{
    Heal,
    Sword,
    Rifle,
    Priest,
    Farmer,
    None
}
public class PickableController : MonoBehaviour
{
    public static PickableController Instance;
    [SerializeField] private GameObject dialogueMark;
    [SerializeField] private GameObject dialogueCanvas;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private AudioSource pickableSound;
    [SerializeField, TextArea(3, 6)] private string[] dialogueLines;
    
    public bool shouldDestroy;
    public bool isPlayerInRange;
    public bool isDialogueActive;
    public int lineIndex;
    public ConsumibleType consumibleType;
    private bool _hasBetterWeapon;

    private float _typingTime = 0.05f;
    private bool _isTyping;

    private void Start()
    {
        Instance = this;
        //Disable the dialogue canvas
        dialogueCanvas.SetActive(false);
        
        //Instantiates the dialogue canvas
        if (!dialogueText)
        {
            dialogueCanvas = Instantiate(dialogueCanvas, new Vector3(0,0,0), Quaternion.identity);
            
            //Sets the dialogue text
            dialogueText = dialogueCanvas.transform.GetChild(0)
                .gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerInteract playerInteract = other.GetComponent<PlayerInteract>();
            playerInteract.pickableController = this;

            isPlayerInRange = true;
            dialogueMark.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerInRange = false;
            dialogueMark.SetActive(false);
        }
    }
    
    private void CheckPossibleExtraDialogue()
    {
        if (this.consumibleType == ConsumibleType.Heal)
        {
            //Adds a new line to the dialogue with the number of heals the player will had
            //With the following format: "Now you have X heals"
            Array.Resize(ref dialogueLines, dialogueLines.Length + 1);
            dialogueLines[^1] = "Now you have " + (PlayerInfo.Instance.currentHeals + 1) + " heals";
        }
        
        if (this.consumibleType == ConsumibleType.Sword || 
            this.consumibleType == ConsumibleType.Rifle || 
            this.consumibleType == ConsumibleType.Farmer)
        {
            // Map consumibleType to the appropriate WeaponType
            WeaponType weaponToEquip = consumibleType switch
            {
                ConsumibleType.Sword => WeaponType.Sword,
                ConsumibleType.Rifle => WeaponType.Rifle,
                ConsumibleType.Farmer => WeaponType.Gun, // Assuming Farmer maps to Gun; adjust if needed
                _ => WeaponType.Hand // Default fallback
            };

            // Check if the weapon should be equipped
            if (PlayerInfo.Instance.ShouldEquipWeapon(weaponToEquip))
            {
                PlayerInfo.Instance.ChangeWeapon(weaponToEquip);

                // Add a new line to the dialogue indicating the weapon was equipped
                Array.Resize(ref dialogueLines, dialogueLines.Length + 1);
                dialogueLines[^1] = $"You have equipped the {weaponToEquip.ToString().ToLower()}";
            }
            else
            {
                _hasBetterWeapon = true;

                // Add a new line to the dialogue indicating the player has a better weapon
                Array.Resize(ref dialogueLines, dialogueLines.Length + 1);
                dialogueLines[^1] = "You already have a better weapon";
            }
        }

    }

    public void StartDialogue()
    {
        if (!isDialogueActive)
        {
            pickableSound.Play();
            CheckPossibleExtraDialogue();
            PlayerInfo.Instance.FreezeMovement();
            isDialogueActive = true;
            dialogueCanvas.SetActive(true);
            dialogueMark.SetActive(false);
            lineIndex = 0;
            StartCoroutine(ShowMessage());
        }
    }

    private IEnumerator ShowMessage()
    {
        _isTyping = true;
        _typingTime = 0.05f;

        dialogueText.text = string.Empty;

        foreach (char ch in dialogueLines[lineIndex])
        {
            dialogueText.text += ch;
            yield return new WaitForSeconds(_typingTime);
        }
        _isTyping = false;
    }

    public void NextText()
    {
        if (!isDialogueActive)
        {
            return;
        }

        if (_isTyping)
        {
            _typingTime = 0;
        }
        else
        {
            lineIndex++;
            if (lineIndex < dialogueLines.Length)
            {
                StartCoroutine(ShowMessage());
            }
            else
            {
                //Switch case for the different types of consumibles
                //Take into account if the player has a better weapon
                switch (consumibleType)
                {
                    case ConsumibleType.Heal:
                        PlayerInfo.Instance.currentHeals++;
                        break;
                    case ConsumibleType.Sword:
                        if (!_hasBetterWeapon)
                        {
                            PlayerInfo.Instance.ChangeWeapon(WeaponType.Sword);
                        }
                        break;
                    case ConsumibleType.Rifle:
                        if (!_hasBetterWeapon)
                        {
                            PlayerInfo.Instance.ChangeWeapon(WeaponType.Rifle);
                        }
                        break;
                    case ConsumibleType.Priest:
                        PlayerInfo.Instance.currentHeals++;
                        break;
                    case ConsumibleType.Farmer:
                        if (!_hasBetterWeapon)
                        {
                            PlayerInfo.Instance.ChangeWeapon(WeaponType.Gun);
                        }
                        break;
                }
                
                dialogueCanvas.SetActive(false);
                isDialogueActive = false;
                lineIndex = 0;
                _typingTime = 0.05f;
                dialogueMark.SetActive(true);
                isDialogueActive = false;
                if (shouldDestroy)
                {
                    Destroy(dialogueCanvas);
                    Destroy(this.gameObject);
                }
                PlayerInfo.Instance.UnfreezeMovement();
            }
        }
    }
}
