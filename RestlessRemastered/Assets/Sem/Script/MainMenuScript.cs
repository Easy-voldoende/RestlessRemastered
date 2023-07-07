using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    public GameObject[] subMenus;
    public AudioSource aud;
    public AudioSource aud2;
    public ParticleSystem particle1;
    public SliderSpin[] spins;
    public ParticleSystem particle2;
    public Light light;
    public Image image;
    public bool canFade;
    public bool canSmoke;
    public AudioSource[] sources;
    public bool canMuteSources;
    float speed =20;
    Color color;
    public LoadingScreen screen;
    private void Start()
    {
        subMenus[0].SetActive(true);
        subMenus[1].SetActive(false);
        subMenus[2].SetActive(false);
        color.a = 0;
        foreach(SliderSpin spin in spins)
        {
            spin.mixer.SetFloat("Master", Mathf.Log10(1) * 20);
        }
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
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
        canMuteSources = true;
        yield return new WaitForSeconds(2.5f);
        screen.enabled = true;
        foreach(SliderSpin spin in spins)
        {
            spin.SaveVolumeButton();
        }
        PlayerPrefs.DeleteAll();
        yield return new WaitForSeconds(Random.Range(3, 5));
        SceneManager.LoadSceneAsync(1);
        


    }
    private void Update()
    {
        FadeImage();
        Smoke();
        if(canMuteSources == true)
        {
            foreach(AudioSource source in sources)
            {
                source.volume -= 0.2f * Time.deltaTime;
            }
        }
    }
    public void Smoke()
    {
        if(canSmoke == true)
        {
            particle2.playbackSpeed += speed * Time.deltaTime;
            particle1.playbackSpeed += 0.75f * Time.deltaTime;
            light.intensity -= 20 * Time.deltaTime;
        }
    }
    public void FadeImage()
    {
        if(canFade == true)
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
        PlayOnce(aud);
        subMenus[0].SetActive(true);
        subMenus[1].SetActive(false);
        subMenus[2].SetActive(false);
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
