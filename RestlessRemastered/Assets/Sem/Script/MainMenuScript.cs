using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuScript : MonoBehaviour
{
    public GameObject[] subMenus;
    private void Start()
    {
        subMenus[0].SetActive(true);
        subMenus[1].SetActive(false);
        subMenus[2].SetActive(false);
    }
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void Credits()
    {
        subMenus[0].SetActive(false);
        subMenus[1].SetActive(false);
        subMenus[2].SetActive(true);
    }
    public void Options()
    {
        subMenus[0].SetActive(false);
        subMenus[1].SetActive(true);
        subMenus[2].SetActive(false);
    }
    public void OptionsBack()
    {
        subMenus[0].SetActive(true);
        subMenus[1].SetActive(false);
        subMenus[2].SetActive(false);
    }
}
