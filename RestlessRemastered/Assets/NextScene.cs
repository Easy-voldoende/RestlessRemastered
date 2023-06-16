using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Unity.VisualScripting.Member;

public class NextScene : MonoBehaviour
{
    public AudioSource[] sounds;
    public bool crashed;
    public float stopTime;
    void Start()
    {
        StartCoroutine(nameof(Nextscene));
        StartCoroutine(nameof(Sound));
        StartCoroutine(nameof(Sound2));
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public IEnumerator Sound()
    {        
        PlayOnce(sounds[2], 2.5f);
        yield return new WaitForSeconds(16.5f);
        crashed = true;
        yield return new WaitForSeconds(1.7f);
        PlayOnce(sounds[4], 1);                
        yield return new WaitForSeconds(1.8f);
        
        sounds[4].volume = 1;
        //PlayOnce(sounds[3], 1);
        
        yield return new WaitForSeconds(2);
        PlayOnce(sounds[1], 1);


        yield return new WaitForSeconds(1f);
        PlayOnce(sounds[0], 1);
        yield return new WaitForSeconds(1);
    }
    public IEnumerator Sound2()
    {
        yield return new WaitForSeconds(16f);
        PlayOnce(sounds[6], 1);
        yield return new WaitForSeconds(3.4f);
        PlayOnce(sounds[5], 1); 
    }
    private void Update()
    {
        if (sounds[2].volume < 1f && crashed == false)
        {
            sounds[2].volume += 0.5f * Time.deltaTime;
        }
        else if(crashed == true)
        {
            sounds[2].volume -= 0.5f * Time.deltaTime;
        }
        if (sounds[2].pitch < 2.5f && crashed == false)
        {
            sounds[2].pitch += 0.5f * Time.deltaTime;
        }


        if (sounds[7].volume < 0.1f && crashed == false)
        {
            sounds[7].volume += 0.05f * Time.deltaTime;
        }
        else if (crashed == true)
        {
            sounds[7].volume -= 0.5f * Time.deltaTime;
        }
        

    }
    public void PlayOnce(AudioSource source, float pitch)
    {
        source.pitch = pitch;
        source.Play();

    }

    public void StopSound(AudioSource source)
    {
        source.Stop();
    }
    public IEnumerator Nextscene()
    {

        yield return new WaitForSeconds(26f);
        SceneManager.LoadScene(1);
    }

}
