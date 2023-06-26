using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterGate : MonoBehaviour
{
    public Animator anim;
    public GameObject player;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            player = other.gameObject;
            StartCoroutine(nameof(StartJumpscare));
        }
    }
    
    public IEnumerator StartJumpscare()
    {
        anim.SetTrigger("CloseGate");
        yield return new WaitForSeconds(2);
        player.gameObject.GetComponent<PlayerMovementGrappling>().ManualJumpScare();

    }
}
