using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class CheckForLock : MonoBehaviour
{
    public AudioSource papersound;
    public AudioSource keySound;
    public GameObject maincam;
    public LayerMask layerMask;
    public GameObject mainHolder;
    public Transform lastPos;
    public Vector3 lastPosVector;
    public bool pickedUpPin;
    public bool pickedUpScrewDriver;
    public AudioSource doorSound;
    public GameObject mainMenu;
    public AudioSource pickUpSound;
    public GameObject UI;
    public TextMeshProUGUI text;
    public bool paper;
    public bool hidden;
    public float maxCooldown = 4;
    public bool canFade;
    public float cooldown;
    public bool canFadeOut;
    public GameObject canvas;
    public TextMeshProUGUI itemsText;
    public GameObject paperUI;
    public TextMeshProUGUI itemsText2;
    public bool gotKey;
    public GameObject player;
    RaycastHit hit;
    public int itemsPickedUp = 0;
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
        if (canFadeOut == true)
        {
            FadeOutText();
        }

        if (canFade == true)
        {
            FadeInText();
        }
        cooldown -= 1 * Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.E))
        {
            if(Physics.Raycast(maincam.transform.position, maincam.transform.forward, out hit,3f, layerMask))
            {
                if (hit.transform.gameObject.CompareTag("Pin"))
                {
                    pickedUpPin = true;
                    itemsPickedUp++;
                    pickUpSound.Play();
                    itemsText.text = itemsPickedUp.ToString() +"/2";
                    hit.transform.gameObject.SetActive(false);
                }

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
                        hit.transform.gameObject.transform.parent.GetComponent<Animator>().SetTrigger("Budge");
                        if (cooldown <= 0 && hit.transform.gameObject.name == "RealDoor")
                        {
                            
                            text.text = "Find something to open the door with...";
                            canFade = true;
                            UI.GetComponent<Animator>().SetTrigger("Text");
                            cooldown = maxCooldown;
                        }
                    }
                }

                if (hit.transform.gameObject.CompareTag("LockedDoor"))
                {                                        
                    {
                        AudioSource door = hit.transform.gameObject.GetComponent<AudioSource>();
                        PlayOnce(door, Random.Range(0.9f, 1.1f));
                        hit.transform.gameObject.transform.parent.GetComponent<Animator>().SetTrigger("Budge");
                        if (cooldown <= 0 )
                        {

                            text.text = "Find a way to open the door...";
                            UI.GetComponent<Animator>().SetTrigger("Text");
                            cooldown = maxCooldown;
                        }
                    }
                }
                if (hit.transform.CompareTag("Hide") && hidden ==false)
                {
                    lastPosVector = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
                    hidden = true;
                    
                    player.GetComponent<Rigidbody>().isKinematic = true;
                    player.transform.position = hit.transform.gameObject.transform.GetChild(0).transform.position;

                }

                if (hit.transform.gameObject.CompareTag("Screwdriver"))
                {
                    pickedUpScrewDriver = true;
                    itemsPickedUp++;
                    pickUpSound.Play();
                    itemsText.text = itemsPickedUp.ToString() + "/2";
                    hit.transform.gameObject.SetActive(false);
                }

                if (hit.transform.gameObject.CompareTag("Screen"))
                {

                    TurnOffScreen screen = hit.transform.gameObject.GetComponent<TurnOffScreen>();

                    if (screen.switched == true)
                    {
                        screen.switched = false;
                    }
                    else
                    {
                        screen.switched = true;
                    }


                }
                if (hit.transform.gameObject.CompareTag("SlotHanger"))
                {
                    if (gotKey == true)
                    {
                        hit.transform.gameObject.GetComponent<Rigidbody>().isKinematic = false;
                        GameObject.Find("Hek Gate").GetComponent<Animator>().SetTrigger("OpenGate");
                        
                    }
                    else 
                    {

                        text.text = "Find the gate key!";
                        hit.transform.gameObject.GetComponent<Animator>().SetTrigger("Jiggle");
                        if (cooldown <= 0)
                        {
                            UI.GetComponent<Animator>().SetTrigger("Text");
                            cooldown = maxCooldown;
                        }
                    }



                }
                
                if (hit.transform.gameObject.CompareTag("GateKey"))
                {
                    gotKey = true;
                    keySound.Play();
                    hit.transform.gameObject.SetActive(false);


                }
                if (hit.transform.gameObject.CompareTag("Curtain"))
                {
                    hit.transform.gameObject.GetComponentInParent<Animator>().SetTrigger("Curtain");

                }
                if (hit.transform.gameObject.CompareTag("Paper"))
                {
                    if(paper == false)
                    {
                        GameObject note = GameObject.Find("Note").gameObject;
                        note.GetComponent<MeshRenderer>().enabled = false;
                        paperUI.SetActive(true);
                        papersound.Play();
                        mainMenu.GetComponent<ActivateUI>().menuOpen = true;
                        StartCoroutine(PaperShit());
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
        if (paper == true && Input.GetKeyDown(KeyCode.E))
        {
            paperUI.SetActive(false);
            GameObject note = GameObject.Find("Note").gameObject;
            note.GetComponent<MeshRenderer>().enabled = true;
            mainMenu.GetComponent<ActivateUI>().menuOpen = false;
            paper = false;
        }
    }
    public IEnumerator PaperShit()
    {
        yield return new WaitForSeconds(0.1f);
        paper = true;
        
    }
    public void FadeInText()
    {
        
        if(itemsText.color.a < 0.99f)
        {
            Color color = itemsText.color;
            color.a += 1 * Time.deltaTime;
            itemsText.color = color;
            
            
        }
        if (itemsText2.color.a < 0.99f)
        {
            Color color = itemsText2.color;
            color.a += 1 * Time.deltaTime;
            itemsText2.color = color;
            
        }
        if (canvas.GetComponent<Image>().color.a < 0.65f)
        {
            Color color = canvas.GetComponent<Image>().color;
            color.a += 0.65f * Time.deltaTime;
            canvas.GetComponent<Image>().color = color;
            
        }

        if (itemsText.color.a >= 0.99f)
        {
            canFade = false;
        }
    }

    public void FadeOutText()
    {

        if (itemsText.color.a > 0f)
        {
            Color color = itemsText.color;
            color.a -= 1 * Time.deltaTime;
            itemsText.color = color;
        }
        if (itemsText2.color.a > 0f)
        {
            Color color = itemsText2.color;
            color.a -= 1 * Time.deltaTime;
            itemsText2.color = color;
        }
        if (canvas.GetComponent<Image>().color.a > 0)
        {
            Color color = canvas.GetComponent<Image>().color;
            color.a -= 0.65f * Time.deltaTime;
            canvas.GetComponent<Image>().color = color;
        }

        if (itemsText.color.a <= 0.01f)
        {
            canFadeOut = false;
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
        canFadeOut = true;
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
