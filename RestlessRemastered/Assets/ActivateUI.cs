using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ActivateUI : MonoBehaviour
{
    public bool menuOpen;
    public GameObject pauseMenu;
    public GameObject volume;
    public GameObject cam;
    public GameObject headBob;
    public float previousSens;
    public float previousvolume1;
    public float previousvolume2;
    public AudioSource source1;
    public AudioSource source2;
    public AudioSource biden;
    public AudioSource menuSound1;
    public AudioSource menuSound2;
    public AudioMixer mix;
    public GameObject otherUI;
    public Volume v;
    DepthOfField depthOfField;
    void Start()
    {
        pauseMenu.SetActive(false);
        v.profile.TryGet<DepthOfField>(out depthOfField);
    }

    // Update is called once per frame
    void Update()
    {
        depthOfField.active = menuOpen;



        if (Input.GetKeyDown(KeyCode.Escape) && menuOpen == false)
        {
            menuSound2.Play();
            biden.Pause();
            otherUI.SetActive(false);
            pauseMenu.SetActive(true);
            Quaternion q = cam.transform.rotation;
            previousvolume1 = source1.volume;
            previousvolume2 = source2.volume;
            //source1.Pause();
            //source2.Pause();
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            //volume.GetComponent<DepthOfField>().active = true;
            mix.SetFloat("Lowpass Simple", 100f);
            previousSens = cam.GetComponent<CustomizableCamera>().sensitivity;  
            cam.GetComponent<CustomizableCamera>().sensitivity = 0;
            headBob.GetComponent<HeadBob>().enabled = false;
            pauseMenu.SetActive(true);
            cam.transform.rotation = q;
            StartCoroutine(nameof(SetBoolActive));
        }

        if (Input.GetKeyDown(KeyCode.Escape) && menuOpen == true)
        {
            menuSound1.Play();
            biden.Play();
            otherUI.SetActive(true);
            StartCoroutine(nameof(SetBoolInActive));
            pauseMenu.SetActive(false);
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            mix.SetFloat("Lowpass Simple", 20000f);
            Cursor.visible = false;
            cam.GetComponent<CustomizableCamera>().sensitivity = previousSens;
            headBob.GetComponent<HeadBob>().enabled = true;
            //volume.GetComponent<DepthOfField>().active = false;
        }
    }
    IEnumerator SetBoolActive()
    {
        yield return new WaitForEndOfFrame();
        menuOpen = true;

    }
    IEnumerator SetBoolInActive()
    {
        yield return new WaitForEndOfFrame();
        menuOpen = false;

    }
}
