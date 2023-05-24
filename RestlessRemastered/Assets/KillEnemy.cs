using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KillEnemy : MonoBehaviour
{
    Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void OnCollisionEnter(Collision collision)
    {
        
        if(collision.gameObject.tag == "Enemy")
        {
            Vector3 myVelocity = rb.velocity;
            Vector3 normal = collision.contacts[0].normal;
            Rigidbody rb2 = collision.gameObject.GetComponent<Rigidbody>();

            float collisionAngle = 90 - (Vector3.Angle(myVelocity, -normal));
            Debug.Log("Collision Angle:" + collisionAngle);
            rb2.gameObject.GetComponent<NavMeshAgent>().enabled = false;
            rb2.AddForce( normal* 20f, ForceMode.Impulse);
        }
        
    }
}
