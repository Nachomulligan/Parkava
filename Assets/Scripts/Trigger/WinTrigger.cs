using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var gameManager = GameManager.Instance;
            if (gameManager != null)
            {
                gameManager.GoToWinState();
            }
            else
            {
                Debug.LogError("GameManager instance not found.");
            }
        }
    }
}
