using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KeepTrackOfLives : MonoBehaviour
{
    private const string SceneLoadCountKey = "SceneLoadCount";
    public int sceneLoadCount = 0;
    public Animator anim;

    private void Start()
    {
        StartCoroutine(nameof(TimesDied)); 
        sceneLoadCount = PlayerPrefs.GetInt(SceneLoadCountKey, 0);

        if (SceneManager.GetActiveScene().name == "CabinScene")
        {
            sceneLoadCount++;
        }

        PlayerPrefs.SetInt(SceneLoadCountKey, sceneLoadCount);
        PlayerPrefs.Save();
    }
    public IEnumerator TimesDied()
    {
        yield return new WaitForSeconds(2);
        anim.SetInteger("Died", sceneLoadCount);

    }
    private void OnApplicationQuit()
    {
        sceneLoadCount = 0;
        PlayerPrefs.SetInt(SceneLoadCountKey,sceneLoadCount);
    }

}
