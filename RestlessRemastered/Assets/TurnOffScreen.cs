using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TurnOffScreen : MonoBehaviour
{
    public GameObject[] screens;
    public AudioSource source;
    public int i;
    public bool switched;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(switched == true)
        {
            i = 1;
        }

        if(switched == false)
        {
            i = 0;
        }
        SwitchScreens();
    }

    public void SwitchScreens()
    {
        if (i == 0)
        {
            source.volume = 0.1f;
            screens[0].GetComponent<MeshRenderer>().enabled = true;
        }

        if (i == 1)
        {
            source.volume = 0;
            screens[0].GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
