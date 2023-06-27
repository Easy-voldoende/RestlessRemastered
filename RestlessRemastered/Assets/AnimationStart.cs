using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStart : MonoBehaviour
{
    public Animator animator;
    public MeshRenderer meshRenderer;
    public CustomizableCamera cameraScript;
    public PlayerMovementGrappling pm;
    public bool b = true;

    // Update is called once per frame
    private void Awake()
    {
        meshRenderer.enabled = false;
        pm.enabled = false;
        cameraScript.enabled = false;
        meshRenderer.enabled = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        animator.SetBool("StartOfMain", b);
    }
    private void Start()
    {
        StartCoroutine(nameof(WaitForSet));
    }
    void Update()
    {
        
    }

    public IEnumerator WaitForSet()
    {
        yield return new WaitForSeconds(6.6f);
        cameraScript.enabled = true;
        meshRenderer.enabled = true;
        b = false;
        animator.SetBool("StartOfMain", b);
        pm.enabled = true;
    }
}
