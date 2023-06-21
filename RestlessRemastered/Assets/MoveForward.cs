using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    Transform t;
    public float a;
    public float b;
    public Animator animator;
    bool canMove;
    private void Start()
    {
        animator.SetInteger("State",-9);
        
        t = GetComponent<Transform>();
        StartCoroutine(nameof(Scene));
    }
    IEnumerator Scene()
    {
        yield return new WaitForSeconds(Random.Range(a, b));
        canMove = true;
        animator.SetTrigger("CutScene");


    }
    void Update()
    {

        if(canMove == true)
        {
            transform.Translate(Vector3.forward * 10 * Time.deltaTime);
        }
    }
}
