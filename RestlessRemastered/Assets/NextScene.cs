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
    }
    public IEnumerator Sound()
    {        
        PlayOnce(sounds[2], 1);
        yield return new WaitForSeconds(20);
        crashed = true;
        PlayOnce(sounds[3], 1);
        
        yield return new WaitForSeconds(2);
        PlayOnce(sounds[1], 1);


        yield return new WaitForSeconds(1f);
        PlayOnce(sounds[0], 1);
        yield return new WaitForSeconds(1);
    }
    private void Update()
    {
        if (sounds[2].volume < 0.5f && crashed == false)
        {
            sounds[2].volume += 0.25f * Time.deltaTime;
        }
        else if(crashed == true)
        {
            sounds[2].volume -= 0.4f * Time.deltaTime;
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
