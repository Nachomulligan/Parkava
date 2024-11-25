using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayHud : MonoBehaviour
{
    [SerializeField] public GameObject GameplayHUDUI;

    private void Update()
    {
            if (GameManager.Instance.GetCurrentState() is GameplayState)
            {
              Show();
            }
            else
            {
              Hide();
            }
    }
    public void Show()
    {
        GameplayHUDUI.SetActive(true);
    }

    public void Hide()
    {
        GameplayHUDUI.SetActive(false);
    }
}
