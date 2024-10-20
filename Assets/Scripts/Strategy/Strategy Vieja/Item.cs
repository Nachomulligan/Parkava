using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, IInteractable
{
    public InteractionPriority InteractionPriority => InteractionPriority.Medium;
    public void Interact()
    {
        Debug.Log("Add to Inventory");
        Destroy(gameObject);
    }
}
