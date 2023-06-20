using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullScreen : MonoBehaviour
{
    AudioSource audioSource;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void Change()
    {
        Screen.fullScreen = !Screen.fullScreen;
        PlayOnce(audioSource);
        Debug.Log("Changed fullscreen mode");
    }
    public void PlayOnce(AudioSource source)
    {
        source.pitch = Random.Range(0.9f, 1.1f);
        source.Play();
    }
}
