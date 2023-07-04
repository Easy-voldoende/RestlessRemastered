using UnityEngine;

public class DoorController : MonoBehaviour
{
    private Quaternion initialRotation;
    private Quaternion targetRotation;
    private bool isOpen = false;
    RaycastHit hit;
    [SerializeField] private float openAngle = 90f;
    [SerializeField] private float openSpeed = 2f;

    private void Start()
    {
        initialRotation = transform.rotation;
        //targetRotation = initialRotation * Quaternion.Euler(0f, openAngle, 0f);
    }

    private void Update()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            if(Physics.Raycast(Camera.main.transform.position,Camera.main.transform.forward,out hit, 5f))
            {
                if(hit.transform.gameObject == gameObject)
                {
                    ToggleDoor();
                }
            }
        }

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, openSpeed * Time.deltaTime);
    }

    private void ToggleDoor()
    {
        isOpen = !isOpen;
        targetRotation = isOpen ? initialRotation * Quaternion.Euler(0f, openAngle, 0f) : initialRotation;
    }
}