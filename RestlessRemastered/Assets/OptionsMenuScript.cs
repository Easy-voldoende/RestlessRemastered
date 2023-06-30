using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsMenuScript : MonoBehaviour
{
    public GameObject[] subMenus;
    public AudioSource aud;
    public AudioSource aud2;
    public AudioSource aud3;
    public ParticleSystem particle1;
    public ParticleSystem particle2;
    public ActivateUI ui;
    public Light light;
    public Image image;
    public bool canFade;
    public bool canSmoke;
    float speed = 20;
    Color color;
    private void Start()
    {


    }
    public void StartGame()
    {
        PlayOnce(aud);
        StartCoroutine(SwitchScene());

    }

    public IEnumerator SwitchScene()
    {
        particle1.startLifetime = 0;
        particle2.emissionRate = 0;
        PlayOnce(aud2);
        canSmoke = true;
        canFade = true;
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene(1);



    }
    private void Update()
    {
        FadeImage();
        Smoke();
    }
    public void Smoke()
    {
        if (canSmoke == true)
        {
            particle2.playbackSpeed += speed * Time.deltaTime;
            particle1.playbackSpeed += 0.5f * Time.deltaTime;
            light.intensity -= 15 * Time.deltaTime;
        }
    }
    public void FadeImage()
    {
        if (canFade == true)
        {

            color.a += 0.5f * Time.deltaTime;
            image.color = color;
        }
    }
    public void Credits()
    {
        PlayOnce(aud);
        subMenus[0].SetActive(false);
        subMenus[1].SetActive(false);
        subMenus[2].SetActive(true);
    }
    public void Options()
    {
        PlayOnce(aud);
        subMenus[0].SetActive(false);
        subMenus[1].SetActive(true);
        subMenus[2].SetActive(false);
    }
    public void OptionsBack()
    {
        PlayOnce(aud2);
        ui.menuSound1.Play();
        ui.otherUI.SetActive(true);
        subMenus[0].SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        ui.headBob.GetComponent<HeadBob>().enabled = true;
        ui.cam.GetComponent<CustomizableCamera>().sensitivity = ui.previousSens;
        ui.mix.SetFloat("Lowpass Simple", 20000);
        ui.menuOpen = false;
        //ui.source1.Play();
        //ui.source2.Play();
        ui.biden.Play();
        Time.timeScale = 1.0f;
    }

    public void ReturnToMain()
    {
        Time.timeScale = 1;
        PlayOnce(aud3);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(0);
    }
    public void Quit()
    {
        PlayOnce(aud);
        Application.Quit();
        Debug.Log("quit");
    }
    public void PlayOnce(AudioSource source)
    {
        source.pitch = Random.Range(1f, 1.15f);
        source.Play();
    }
}

