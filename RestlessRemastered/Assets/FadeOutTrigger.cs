using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class FadeOutTrigger : MonoBehaviour
{
    public Color color = Color.black;
    public Transform player;
    public GameObject start;
    public float fadeDistance;
    public float fadeAmount;
    public AudioMixer audioMixer;
    public bool passedTrigger;
    public float volume;
    public float maxvolume = 0.999f;
    public Image image;
    void Start()
    {
        color.a = 0;
    }

    void Update()
    {
        if(passedTrigger == true)
        {
            fadeAmount = Vector3.Distance(start.transform.position, player.transform.position) / fadeDistance;
            color.a = fadeAmount;
            image.color = color;
            volume = maxvolume - color.a;
            audioMixer.SetFloat("Master",Mathf.Log10(volume)*20);
        }
        if (color.a >= 0.99f)
        {
            SceneManager.LoadScene(0);
            

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.gameObject.transform;
            start.transform.position = other.gameObject.transform.position;
            passedTrigger = true;
        }
    }
}
