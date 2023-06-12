using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterGate : MonoBehaviour
{
    public Animator anim;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            anim.SetTrigger("CloseGate");
        }
    }
}
