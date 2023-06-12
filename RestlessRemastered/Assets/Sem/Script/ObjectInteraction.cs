using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SubsystemsImplementation;

public class ObjectInteraction : MonoBehaviour
{
    private Transform mainCamera;
    public GameObject crossHair;
    public bool lookingAtObject;
    public Renderer rend;
    
    private Transform carriedObject;
    public bool carrying = false;
    public float pickupDistance = 3f;
    public float finalForce;
    public float minThrowForce = 0;
    public float maxThrowForce = 30;
    public LayerMask layer;
    public bool throwing;
    public LayerMask holdingLayer;
    public LayerMask droppedlayer;
    public Shader defaultShader;
    public Shader pickedUpShader;
    public Transform holdingTransform;
    public Transform throwingTransform;
    public Vector3 newPos;
    public bool inPosition;
    public bool inThrowingPosition;

    void Start()
    {
        mainCamera = Camera.main.transform;
    }

    void Update()
    {
        CrosshairFade();
        Pickup();

    }
    
    public void CrosshairFade()
    {
        Image image = crossHair.GetComponent<Image>();
        float maxValue = 0.8f;
        float minValue = 0.3f;
        Color color = image.color;  
        if(lookingAtObject == true)
        {
            color.a += 3f * Time.deltaTime;
            if (color.a >= maxValue)
            {
                color.a = maxValue;
            }
            image.color = color;
        }
        else
        {
            color.a -= 3f * Time.deltaTime;
            if(color.a <= minValue)
            {
                color.a = minValue;
            }
            image.color = color;
        }
    }

    void Pickup()
    {
        RaycastHit hit;

        if (Physics.Raycast(mainCamera.position, mainCamera.forward, out hit, pickupDistance))
        {

            if (hit.transform.gameObject.CompareTag("Pin"))
            {
                lookingAtObject = true;
            }

        }

        if (Physics.Raycast(mainCamera.position, mainCamera.forward, out hit, pickupDistance))
        {

            if (hit.transform.gameObject.CompareTag("Pic"))
            {
                lookingAtObject = true;
            }

        }

        if (Physics.Raycast(mainCamera.position, mainCamera.forward, out hit, pickupDistance))
        {

            if (hit.transform.gameObject.CompareTag("Lock"))
            {
                lookingAtObject = true;
            }

        }

        if (!Physics.Raycast(mainCamera.position, mainCamera.forward, out hit, pickupDistance, layer))
        {
            lookingAtObject = false;            

        }

    }

    public IEnumerator Dissolve()
    {
        yield return new WaitForSeconds(1);
        float t =0;
        while (t < 1f)
        {
            t += Time.deltaTime / 2;
            rend.material.SetFloat("_Dissolve", t);
            yield return null;

        }
        Destroy(rend.gameObject);
        
    }
}