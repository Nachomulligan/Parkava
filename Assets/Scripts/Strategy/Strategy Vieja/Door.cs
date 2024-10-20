using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    public InteractionPriority InteractionPriority => InteractionPriority.Low;
    [SerializeField] private Animator animator;

    private bool doorState;
    public void Interact()
    {
        animator.SetBool("IsOpen", !doorState);
    }
}
