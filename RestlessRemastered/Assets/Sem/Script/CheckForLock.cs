using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckForLock : MonoBehaviour
{
    public GameObject maincam;
    public GameObject mainHolder;
    public Transform lastPos;
    public bool pickedUpPin;
    public bool pickedUpScrewDriver;

    public GameObject player;
    RaycastHit hit;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if(Physics.Raycast(maincam.transform.position, maincam.transform.forward, out hit,3f))
            {
                if (hit.transform.gameObject.CompareTag("Lock"))
                {
                    LockPick pick = GameObject.Find("LockPickObj").GetComponent<LockPick>();
                    if (pickedUpPin && pickedUpScrewDriver&& pick.picked == false)
                    {
                        hit.transform.gameObject.tag = "Door";
                        StartPicking();
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Physics.Raycast(maincam.transform.position, maincam.transform.forward, out hit, 3f))
            {
                if (hit.transform.gameObject.CompareTag("Pin"))
                {
                    pickedUpPin = true;
                    hit.transform.gameObject.SetActive(false);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Physics.Raycast(maincam.transform.position, maincam.transform.forward, out hit, 3f))
            {
                if (hit.transform.gameObject.CompareTag("Screwdriver"))
                {
                    pickedUpScrewDriver = true;
                    hit.transform.gameObject.SetActive(false);
                }
            }
        }
    }
    public void StartPicking()
    {
        lastPos = transform;
        hit.transform.gameObject.transform.GetComponent<GetGameObject>().obj.GetComponent<LockPick>().startedPicking = true;
        hit.transform.gameObject.transform.GetComponent<GetGameObject>().obj.GetComponent<LockPick>().pin.SetActive(true);
        hit.transform.gameObject.transform.GetComponent<GetGameObject>().obj.GetComponent<LockPick>().screwDriver.SetActive(true);
        player.GetComponent<PlayerMovementGrappling>().enabled = false;
        maincam.GetComponent<CustomizableCamera>().enabled = false;
        player.transform.position = hit.transform.gameObject.GetComponent<GetGameObject>().obj.GetComponent<LockPick>().camPos.transform.position;
        player.GetComponent<Rigidbody>().isKinematic = true;
        maincam.transform.LookAt(hit.transform.gameObject.GetComponent<GetGameObject>().obj.GetComponent<LockPick>().innerLock.transform.position);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    
}
