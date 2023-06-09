using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class SafePuzzle : MonoBehaviour
{
    public AudioSource source;
    public GameObject ai;
    public AudioSource source2;
    public int enteredNumbers;
    public TextMeshProUGUI text;
    RaycastHit hit;
    GameObject cam;
    public LayerMask mask;
    public int[] correctCombination = new int[4];
    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("Main Camera");
        GenerateRandomCombination();
        source = GetComponent<AudioSource>();
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
        CheckNumber();
        if(enteredNumbers == correctCombination.Length)
        {
            OpenSafe();
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            GenerateRandomCombination();
        }
    }
    public void CheckNumber()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 3f, mask))
            {
                if(hit.transform.gameObject.tag == "Button")
                {
                    if (hit.transform.gameObject.name.ToString() == correctCombination[enteredNumbers].ToString())
                    {
                        enteredNumbers++;
                        Debug.Log("entered correct number" + hit.transform.gameObject.name.ToString());
                        if (enteredNumbers < 4)
                        {
                            PlayOnce(source, 1);
                        }

                    }
                    else
                    {
                        enteredNumbers = 0;
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
        enteredNumbers = 0;
    }
}
