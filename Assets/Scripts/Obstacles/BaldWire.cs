using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaldWire : MonoBehaviour
{
    [SerializeField] private float rotationDuration = 2f;
    [SerializeField] private float rotationAngle = 45f; 
    [SerializeField] private float rotationSpeed = 1f; 
    [SerializeField] private float startDelay = 0f; 

    private float elapsedTime;
    private bool rotatingRight = true;

    private void Start()
    {
        StartCoroutine(StartRotationWithDelay());
    }
    /// <summary>
    /// Starts the rotation of the GameObject after an initial delay, creating a continuous 
    /// back-and-forth rotation effect. The method uses a coroutine to apply the delay and 
    /// handle the rotation over time. The rotation alternates between positive and 
    /// negative angles to achieve a left-to-right movement.
    /// </summary>
    private IEnumerator StartRotationWithDelay()
    {
        yield return new WaitForSeconds(startDelay);

        while (true)
        {
            float fraction = elapsedTime / rotationDuration;
            float currentAngle = Mathf.Lerp(-rotationAngle, rotationAngle, rotatingRight ? fraction : 1 - fraction);
            transform.localRotation = Quaternion.Euler(0f, 0f, currentAngle);
            elapsedTime += Time.deltaTime * rotationSpeed;

            if (elapsedTime >= rotationDuration)
            {
                elapsedTime = 0f;
                rotatingRight = !rotatingRight; 
            }

            yield return null; 
        }
    }
}
