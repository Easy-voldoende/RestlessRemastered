using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuScript : MonoBehaviour
{
    public GameObject[] subMenus;
    public AudioSource aud;
    private void Start()
    {
        subMenus[0].SetActive(true);
        subMenus[1].SetActive(false);
        subMenus[2].SetActive(false);
    }
    public void StartGame()
    {
        PlayOnce(aud);
        SceneManager.LoadScene(1);

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
