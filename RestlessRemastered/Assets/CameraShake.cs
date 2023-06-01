using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public Transform cameraTransform;
    public float shakeDuration = 0.1f;
    public float shakeMagnitude = 0.1f;
    public float dampingSpeed = 1f;

    private Vector3 originalPosition;
    private float currentShakeDuration = 0f;
    private Vector3 shakeVelocity = Vector3.zero;
    public bool canShake;

    private void Awake()
    {
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Shake();
        }

        if (canShake)
        {
            Vector3 shakeOffset = Random.insideUnitSphere * shakeMagnitude;
            Vector3 targetPosition = originalPosition + shakeOffset;
            cameraTransform.localPosition = Vector3.SmoothDamp(cameraTransform.localPosition, targetPosition, ref shakeVelocity, dampingSpeed);
        }
        else
        {
            cameraTransform.localPosition = originalPosition;
        }
    }

    public void Shake()
    {
        originalPosition = cameraTransform.localPosition;
        if (canShake == true)
        {
            canShake = false;
        }
        else
        {
            canShake = true;
        }

    }
}
