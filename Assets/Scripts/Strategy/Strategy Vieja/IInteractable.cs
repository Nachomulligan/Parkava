using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InteractionPriority
{
    Low,
    Medium,
    High
}
public interface IInteractable
{
    InteractionPriority InteractionPriority { get; }
    void Interact();
}
