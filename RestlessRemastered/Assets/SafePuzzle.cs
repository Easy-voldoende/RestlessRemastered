using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class SafePuzzle : MonoBehaviour
{
    public AudioSource source;
    public GameObject ai;
    public Animator door;
    public AudioSource source2;
    public Animator numbers;
    public string enteredLine;
    public int numbersEntered;
    public TextMeshProUGUI text;
    public TextMeshProUGUI text2;
    RaycastHit hit;
    GameObject cam;
    public LayerMask mask;
    public float maxRot;
    public bool canRot;
    public int[] correctCombination = new int[4];
    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("Main Camera");
        GenerateRandomCombination();
    }

    void GenerateRandomCombination()
    {
        for (int i = 0; i < 4; i++)
        {
            int digit = Random.Range(1, 9);
            
            correctCombination[i] = digit;
        }
        text.text = correctCombination[0].ToString() + correctCombination[1].ToString() + correctCombination[2].ToString() + correctCombination[3].ToString();
    }

    private void Update()
    {
        text2.text = enteredLine;
        CheckNumber();
        if (numbersEntered == correctCombination.Length)
        {
            OpenSafe();
        }
        //if (Input.GetKeyDown(KeyCode.F))
        //{
        //    GenerateRandomCombination();
        //    enteredLine = "";
        //}



    }
    public void CheckNumber()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 3f, mask))
            {
                if(hit.transform.gameObject.tag == "Button")
                {
                    enteredLine += hit.transform.gameObject.name.ToString();
                    if (hit.transform.gameObject.name.ToString() == correctCombination[numbersEntered].ToString())
                    {
                        numbersEntered++;
                        
                        Debug.Log("entered correct number" + hit.transform.gameObject.name.ToString());
                        if (numbersEntered < 4)
                        {
                            PlayOnce(source, 1);
                            
                        }

                    }
                    else
                    {
                        enteredLine = "";
                        numbersEntered = 0;
                        PlayOnce(source, 0.6f);
                        ai.GetComponent<EnemyPathfinding>().target = transform.position;
                        ai.GetComponent<EnemyPathfinding>().agent.SetDestination(transform.position);
                        Debug.Log("entered Wrong number");
                    }
                }
            }
        }
    }
    public void PlayOnce(AudioSource source, float pitch)
    {
        source.pitch = pitch;
        source.Play();

    }
    public void OpenSafe()
    {
        Debug.Log("Open the noor");
        PlayOnce(source, 1.4f);
        door.SetTrigger("Door");
        numbersEntered = 0;
    }
}
