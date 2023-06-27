using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CheckForLock : MonoBehaviour
{
    public GameObject maincam;
    public GameObject mainHolder;
    public Transform lastPos;
    public Vector3 lastPosVector;
    public bool pickedUpPin;
    public bool pickedUpScrewDriver;
    public AudioSource doorSound;
    public GameObject UI;
    public TextMeshProUGUI text;
    public bool hidden;

    public GameObject player;
    RaycastHit hit;
    void Start()
    {
        StartCoroutine(nameof(FlashLightUI));
    }
    public IEnumerator FlashLightUI()
    {
        yield return new WaitForSeconds(12);
        UI.GetComponent<Animator>().SetTrigger("Text");
        text.text = "Press 'F' to activate your flashlight";
    }
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
                    else
                    {
                        AudioSource door = hit.transform.gameObject.GetComponent<AudioSource>();
                        PlayOnce(door, Random.Range(0.9f, 1.1f));
                        text.text = "Find something to open the noor with...";
                        UI.GetComponent<Animator>().SetTrigger("Text");
                    }
                }
                if (hit.transform.CompareTag("Hide") && hidden ==false)
                {
                    lastPosVector = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
                    hidden = true;
                    
                    player.GetComponent<Rigidbody>().isKinematic = true;
                    player.transform.position = hit.transform.gameObject.transform.GetChild(0).transform.position;

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

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Physics.Raycast(maincam.transform.position, maincam.transform.forward, out hit, 3f))
            {
                if (hit.transform.gameObject.CompareTag("Screen"))
                {

                    TurnOffScreen screen = hit.transform.gameObject.GetComponent<TurnOffScreen>();

                    if(screen.switched == true)
                    {
                        screen.switched = false;
                    }
                    else
                    {
                        screen.switched = true;
                    }

                    
                }
            }
        }
        if (hidden == true && Input.GetKeyDown(KeyCode.Escape))
        {
            //UnHide();
        }
        if(player.GetComponent<Rigidbody>().isKinematic == true)
        {
            hidden = true;
        }
    }
    public void UnHide()
    {
        if(hidden == true)
        {
            player.GetComponent<Rigidbody>().isKinematic = false;
            player.transform.position = lastPosVector;
            hidden = false;
        }

    }
    public void PlayOnce(AudioSource source, float pitch)
    {
        source.pitch = pitch;
        source.Play();

    }
    public void StartPicking()
    {
        lastPos = player.transform;
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
