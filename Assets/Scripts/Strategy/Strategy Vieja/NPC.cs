using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPC : MonoBehaviour, IInteractable
{
    public InteractionPriority InteractionPriority => InteractionPriority.High;

    [SerializeField, TextArea] private string dialogue;
    [SerializeField] private Canvas dialogueCanvas;
    [SerializeField] private TextMeshProUGUI dialogueText;
    
    public void Interact()
    {
        ToggleCanvas();
        dialogueText.SetText(dialogue);
    }

    private void ToggleCanvas()
    {
        dialogueCanvas.gameObject.SetActive(!dialogueCanvas.gameObject.activeInHierarchy);
    }
}
