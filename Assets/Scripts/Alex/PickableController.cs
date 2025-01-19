using System;
using UnityEngine;
using TMPro;
using System.Collections;

//Enum for different types of Consumibles
public enum ConsumibleType
{
    Heal,
    Sword,
    Rifle
}
public class PickableController : MonoBehaviour
{
    public static PickableController Instance;
    [SerializeField] private GameObject dialogueMark;
    [SerializeField] private GameObject dialogueCanvas;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField, TextArea(3, 6)] private string[] dialogueLines;

    public bool isPlayerInRange;
    public bool isDialogueActive;
    public int lineIndex;
    public ConsumibleType consumibleType;

    private float _typingTime = 0.05f;
    private bool _isTyping;

    private void Start()
    {
        Instance = this;
        //Disable the dialogue canvas
        dialogueCanvas.SetActive(false);
        
        //Instantiates the dialogue canvas
        dialogueCanvas = Instantiate(dialogueCanvas, new Vector3(0,0,0), Quaternion.identity);

        //Sets the dialogue text
        dialogueText = dialogueCanvas.transform.GetChild(0)
            .gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        
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

    public void StartDialogue()
    {
        if (!isDialogueActive)
        {
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
                switch (consumibleType)
                {
                    case ConsumibleType.Heal:
                        PlayerInfo.Instance.PickUpHeal();
                        break;
                    case ConsumibleType.Sword:
                        PlayerInfo.Instance.ChangeWeapon(WeaponType.Sword);
                        break;
                    case ConsumibleType.Rifle:
                        PlayerInfo.Instance.ChangeWeapon(WeaponType.Rifle);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                dialogueCanvas.SetActive(false);
                isDialogueActive = false;
                lineIndex = 0;
                _typingTime = 0.05f;
                dialogueMark.SetActive(true);
                isDialogueActive = false;
                Destroy(dialogueCanvas);
                Destroy(this.gameObject);
                
            }
        }
    }
}
