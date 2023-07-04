using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KeepTrackOfLives : MonoBehaviour
{
    private const string SceneLoadCountKey = "SceneLoadCount";
    public int sceneLoadCount = 0;
    public AudioSource[] audioSource;
    public Animator anim;

    private void Awake()
    {
        sceneLoadCount = PlayerPrefs.GetInt(SceneLoadCountKey, 0);

        if (SceneManager.GetActiveScene().name == "CabinScene")
        {
            sceneLoadCount++;
        }

        PlayerPrefs.SetInt(SceneLoadCountKey, sceneLoadCount);
        PlayerPrefs.Save();
        StartCoroutine(nameof(TimesDied));
        StartCoroutine(PlaySound(audioSource[sceneLoadCount-1]));
    }
    public IEnumerator TimesDied()
    {
        yield return new WaitForSeconds(2);
        anim.SetInteger("Died", sceneLoadCount);

    }
    public IEnumerator PlaySound(AudioSource source)
    {
        if(sceneLoadCount == 1)
        {
            yield return new WaitForSeconds(2.45f);
        }
        else
        {
            yield return new WaitForSeconds(3.6f);
        }
        source.Play();
        

    }
    private void OnApplicationQuit()
    {
        sceneLoadCount = 0;
        PlayerPrefs.SetInt(SceneLoadCountKey,sceneLoadCount);
    }

}
