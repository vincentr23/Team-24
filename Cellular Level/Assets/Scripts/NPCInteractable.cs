using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPCInteractable : MonoBehaviour
{
    [SerializeField] private GameObject dialogueCanvas;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private float textDisplayTime = 5f;
    private Animator animator;
    private Coroutine dialogueCoroutine;
    private int currentLineIndex = 0;

    private string[] dialogueLines = {
        "Assistance is required! Please Help!",
        "Oxygen suppliers have lost contact in the south-east.",
        "Follow the light to locate and retrieve all oxygen...",
        "If you don't hurry, the body will die in a matter of minutes!", 
        "Stay safe, and may our home hold out..."
    };

    private void Awake() {
        animator = GetComponentInParent<Animator>();
    }

    private void Start() {
        if (dialogueCanvas != null) {
            dialogueCanvas.SetActive(false);
        }
    }

    public void Interact() {
        Debug.Log("Interact!");

        if (dialogueCanvas != null) {
            dialogueCanvas.SetActive(true);

            if (dialogueCoroutine != null) {
                StopCoroutine(dialogueCoroutine);
                ShowNextLine();
            } else {
                currentLineIndex = 0; 
                ShowNextLine();
            }

            if (animator != null) {
                animator.ResetTrigger("Attack");
                animator.Play("Attack", 0, 0f);
            }
        }
    }

    public void EndInteraction() {
        if (dialogueCanvas != null) {
            dialogueCanvas.SetActive(false);
            if (dialogueCoroutine != null) {
                StopCoroutine(dialogueCoroutine);
                dialogueCoroutine = null;
            }
        }
    }

    private void ShowNextLine() {
        if (currentLineIndex < dialogueLines.Length) {
            dialogueText.text = dialogueLines[currentLineIndex];
            currentLineIndex++;
            dialogueCoroutine = StartCoroutine(ChangeTextAutomatically());
        } else {
            EndInteraction();
        }
    }

    private IEnumerator ChangeTextAutomatically() {
        yield return new WaitForSeconds(textDisplayTime);
        ShowNextLine();
    }

    /*private IEnumerator ResetAttackTrigger() {
        yield return new WaitForSeconds(0.01f); // Small delay to allow trigger to activate
        animator.ResetTrigger("Attack"); 
    }*/
}
