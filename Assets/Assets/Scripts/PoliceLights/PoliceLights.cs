using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceLights : MonoBehaviour
{
    [SerializeField] private Light blueLight;
    [SerializeField] private Light redLight;
    [SerializeField] private float flashInterval = 0.5f;

    private bool isFlashing = true;

    private void Start()
    {
        StartCoroutine(FlashLights());
    }

    private IEnumerator FlashLights()
    {
        while (isFlashing)
        {
            blueLight.enabled = true;
            redLight.enabled = false;
            yield return new WaitForSeconds(flashInterval);

            blueLight.enabled = false;
            redLight.enabled = true;
            yield return new WaitForSeconds(flashInterval);
        }
    }

    public void StopFlashing()
    {
        isFlashing = false;

        blueLight.enabled = false;
        redLight.enabled = false;
    }

    public void StartFlashing()
    {
        isFlashing = true;
        StartCoroutine(FlashLights());
    }
}