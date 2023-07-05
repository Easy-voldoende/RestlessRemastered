using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public GameObject blackImage;
    public Light light;
    public Image image;
    public bool canFade;
    public bool canSmoke;
    float speed = 20;
    Color color;
    public TextMeshProUGUI[] texts;
    public bool canFadeout;
    Color colorText;
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
        if(canFadeout == true)
        {
            foreach(TextMeshProUGUI text in texts)
            {
                colorText.a -= 1 * Time.deltaTime;
                text.color = colorText;
            }
        }
        Fading();
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
        //PlayOnce(aud2);
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
        if (texts[0] != null)
        {
            canFadeout = true;
            colorText = texts[1].color;
        }
        GameObject.Find("LoadingScreenObj").GetComponent<LoadingScreen>().enabled = true;
        if(blackImage != null)
        {
            Color color2 = Color.black;
            blackImage.GetComponent<Image>().color = color2;
        }
        //GameObject.Find("OptionsMenu").SetActive(false);
        Time.timeScale = 1;
        PlayOnce(aud3);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        StartCoroutine(ReturnToMainMenu());
        
    }
    
    public void ReturnToMainFromFade()
    {
        GameObject.Find("LoadingScreenObj").GetComponent<LoadingScreen>().enabled = true;
        GameObject black = GameObject.Find("BlackImage");
        if (black != null)
        {
            black.GetComponent<Image>().color = Color.black;
        }
        //GameObject.Find("OptionsMenu").SetActive(false);
        Time.timeScale = 1;
        PlayOnce(aud3);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        StartCoroutine(ReturnToMainMenu());

    }
    bool fading = false;
    public void ReturnToMainFromFade2()
    {
        fading = true;
        GameObject.Find("LoadingScreenObj").GetComponent<LoadingScreen>().enabled = true;
        //GameObject.Find("OptionsMenu").SetActive(false);
        Time.timeScale = 1;
        PlayOnce(aud3);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        StartCoroutine(ReturnToMainMenu());

    }
    public void Fading()
    {
        if(fading == true)
        {
            GameObject black = GameObject.Find("BlackImage");
            if (black != null)
            {
                Color color = black.GetComponent<Image>().color;
                color.a += 1 * Time.deltaTime;
                black.GetComponent<Image>().color = color;
            }
        }
    }
    public IEnumerator ReturnToMainMenu()
    {
        yield return new WaitForSeconds(Random.Range(3, 6));
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

