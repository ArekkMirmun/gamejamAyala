using UnityEngine;
using TMPro;
using System.Collections;

public class DialogueController : MonoBehaviour
{
    public static DialogueController instance;
    [SerializeField] private GameObject dialogueMark;
    [SerializeField] private GameObject dialogueCanvas;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField, TextArea(3, 6)] private string[] dialogueLines;

    public bool isPlayerInRange;
    public bool isDialogueActive;
    public int lineIndex;

    private float typingTime = 0.05f;
    private bool isTyping;

    private void Start()
    {
        instance = this;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerInteract playerInteract = other.GetComponent<PlayerInteract>();
            playerInteract.dialogueController = this;

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
            PlayerInfo.Instance.FreezeMovement();
            isDialogueActive = true;
            dialogueCanvas.SetActive(true);
            dialogueMark.SetActive(false);
            lineIndex = 0;
            StartCoroutine(ShowLine());
        }
    }

    private IEnumerator ShowLine()
    {
        isTyping = true;
        typingTime = 0.05f;

        dialogueText.text = string.Empty;

        foreach (char ch in dialogueLines[lineIndex])
        {
            dialogueText.text += ch;
            yield return new WaitForSeconds(typingTime);
        }
        isTyping = false;
    }

    public void NextText()
    {
        if (isTyping)
        {
            typingTime = 0;
        }
        else
        {
            lineIndex++;
            if (lineIndex < dialogueLines.Length)
            {
                StartCoroutine(ShowLine());
            }
            else
            {
                dialogueCanvas.SetActive(false);
                isDialogueActive = false;
                lineIndex = 0;
                typingTime = 0.05f;
                dialogueMark.SetActive(true);
                isDialogueActive = false;
                PlayerInfo.Instance.UnfreezeMovement();
            }
        }
    }
}
