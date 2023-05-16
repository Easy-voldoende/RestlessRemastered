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
    private float pickupDistance = 3f;
    private float holdDistance = 1f;
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
        if (carrying)
        {
            
            if(carriedObject != null)
            {
                Carry(carriedObject);
                CheckThrow(carriedObject);
            }
        }
        else
        {
            Pickup();
        }
        if(finalForce > maxThrowForce)
        {
            finalForce = maxThrowForce;
        }
        CrosshairFade();

    }
    
    void Carry(Transform obj)
    {
        if(throwing == false &&inPosition == false)
        {
            obj.position = Vector3.Lerp(obj.position, holdingTransform.position, Time.deltaTime *10);
            obj.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            
        }
        if(Vector3.Distance(obj.position, holdingTransform.position) < 0.05f)
        {
            inPosition = true;
        }
        if (inPosition == true && throwing == false)
        {
            obj.position = holdingTransform.position;
        }
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
    public void AddToInventory()
    {
        if(carriedObject != null)
        {
            ItemManager manager = GetComponentInParent<ItemManager>();
            
        }
    }
    void Pickup()
    {
        RaycastHit hit;
        if (Physics.Raycast(mainCamera.position, mainCamera.forward, out hit, pickupDistance, layer))
        {
            lookingAtObject = true;
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (hit.rigidbody != null)
                {

                    carriedObject = hit.transform;
                    carriedObject.GetComponent<Renderer>().material.SetInt("Outline", true ? 1 : 0); ;
                    carrying = true;
                    finalForce = minThrowForce;
                    carriedObject.gameObject.layer = LayerMask.NameToLayer("PickedUp");
                    
                }
            }
            
        }

        if (!Physics.Raycast(mainCamera.position, mainCamera.forward, out hit, pickupDistance, layer))
        {
            lookingAtObject = false;            

        }

    }

    void CheckThrow(Transform obj)
    {
        
        if (Input.GetMouseButton(0))
        {

            if(Vector3.Distance(obj.position, throwingTransform.position) > 0.05f && inThrowingPosition == false)
            {
                inThrowingPosition = false;
                obj.position = Vector3.Lerp(obj.position, throwingTransform.position, Time.deltaTime * 10);
            }
            else if (Vector3.Distance(obj.position, throwingTransform.position) <= 0.05f)
            {
                inThrowingPosition = true;
                
            }
            throwing = true;
            if(inThrowingPosition == true && throwing == true)
            {
                obj.position = throwingTransform.position;
            }
            finalForce += 17.5f * Time.deltaTime;
            
        }
        else if (throwing == true && !Input.GetMouseButton(0))
        {
            carriedObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            carriedObject.GetComponent<Rigidbody>().isKinematic = false;
            carriedObject.GetComponent<Rigidbody>().AddForce(mainCamera.forward * finalForce, ForceMode.Impulse);
            carriedObject.gameObject.layer = LayerMask.NameToLayer("PickUp");
            rend = carriedObject.GetComponent<Renderer>();
            StartCoroutine(nameof(Dissolve));
            carriedObject = null;
            inPosition = false;
            inThrowingPosition = false;

            carrying = false;
            throwing=false;
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