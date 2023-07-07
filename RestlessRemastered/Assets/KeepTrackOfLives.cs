using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KeepTrackOfLives : MonoBehaviour
{
    private const string SceneLoadCountKey = "Died";
    public int sceneLoadCount = 0;
    public AudioSource[] audioSource;
    public GameObject vignette1;
    public GameObject vignette2;
    public AudioSource clip;
    public Animator anim;

    private void Start()
    {
        sceneLoadCount = PlayerPrefs.GetInt(SceneLoadCountKey);
        if (sceneLoadCount == 1)
        {
            vignette1.SetActive(true);
        }
        if (sceneLoadCount == 2)
        {
            vignette1.SetActive(true);
            vignette2.SetActive(true);
        }
        StartCoroutine(nameof(TimesDied));
        StartCoroutine(PlaySound(audioSource[sceneLoadCount]));
    }
    public IEnumerator TimesDied()
    {
        yield return new WaitForSeconds(2);
        anim.SetInteger("Died", sceneLoadCount);

    }
    public IEnumerator PlaySound(AudioSource source)
    {
        if(sceneLoadCount == 0)
        {
            yield return new WaitForSeconds(2.45f);
        }
        else
        {
            yield return new WaitForSeconds(3.6f);
        }
        if (sceneLoadCount > 0)
        {
            clip.Play();
        }
        source.Play();
        

    }
    private void OnApplicationQuit()
    {
        sceneLoadCount = 0;
        PlayerPrefs.SetInt(SceneLoadCountKey,sceneLoadCount);
    }

}
