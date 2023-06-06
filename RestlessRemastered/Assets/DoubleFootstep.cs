using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleFootstep : MonoBehaviour
{
    public GameObject player;
    private void Start()
    {
        player = GameObject.Find("Player");
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            player.GetComponent<PlayerMovementGrappling>().inFootstepArea = true;
        }
       
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            player.GetComponent<PlayerMovementGrappling>().inFootstepArea = false;
        }
    }

}
