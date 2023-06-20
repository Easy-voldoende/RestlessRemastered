using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightIntensity : MonoBehaviour
{


    public float intensity;
    public float maxInt =80;
    public float minInt=0;
    public float minDistance;
    public float distDifference;
    public float distance;
    public float shiftSmoothness;
    public float desiredIntensity;
    public float currentIntensity;
    RaycastHit hit;
    void Update()
    {
        if (Physics.Raycast(Camera.main.gameObject.transform.position, Camera.main.gameObject.transform.forward, out hit, minDistance))
        {

            Debug.Log(hit.distance);
            distance = hit.distance;
            desiredIntensity = maxInt - (minDistance - distance) * 8;
            currentIntensity = GetComponent<Light>().intensity;

        }
        else
        {
            desiredIntensity = maxInt;
            currentIntensity = GetComponent<Light>().intensity;
        }
        GetComponent<Light>().intensity = intensity;
        SmoothFade();
    }
    public void SmoothFade()
    {
        intensity = Mathf.Lerp(currentIntensity, desiredIntensity, Time.deltaTime*15);
    }
}
