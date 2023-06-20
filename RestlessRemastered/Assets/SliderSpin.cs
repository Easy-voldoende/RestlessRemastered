using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using static Unity.VisualScripting.Member;

public class SliderSpin : MonoBehaviour
{
    public float sliderValue;
    public float spinSpeed;
    public GameObject handle;
    private Slider slider;
    public bool isPlaying;
    public bool canPlay;
    public AudioMixer mixer;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider ambientSlider;
    public float volumeValue;
    float actualValue;
    [SerializeField] private GameObject volumeText;
    public AudioSource[] boogersounds;
    const string MIXER_MASTER = "Master";
    const string MIXER_AMBIENT = "Ambient";
    const string MIXER_SFX = "SFX";
    public int soundIndex;
    public int previousIndex;
    public float intCooldown = 3;

    private void Start()
    {
        slider = GetComponent<Slider>();
    }
    public void Awake()
    {
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        ambientSlider.onValueChanged.AddListener(SetAmbientVolume);
    }

    public void SetMusicVolume(float value)
    {
        mixer.SetFloat(MIXER_MASTER, Mathf.Log10(value) * 20);
        isPlaying = true;
    }
    public void SetAmbientVolume(float value)
    {
        mixer.SetFloat(MIXER_AMBIENT, Mathf.Log10(value) * 20);
        isPlaying = true;
    }
    public void SetSFXVolume(float value)
    {
        mixer.SetFloat(MIXER_SFX, Mathf.Log10(value) * 20);
        isPlaying = true;

    }
    private void Update()
    {        

        sliderValue = slider.value;
        volumeValue = slider.value;
        actualValue = volumeValue * 100;
        volumeText.GetComponent<TextMeshProUGUI>().text = actualValue.ToString("F0");
        Vector3 rot = new Vector3(0, 0, sliderValue * -spinSpeed);
        handle.GetComponent<RectTransform>().localEulerAngles = rot;
        
        if(isPlaying == true)
        {
            PlaySound(boogersounds[soundIndex]);
        }

        if (Input.GetMouseButtonUp(0))
        {
            StopSound();
        }
        NewInt();
    }
    public void NewInt()
    {
        intCooldown -= 1 * Time.deltaTime;
        if (intCooldown <= 0 && soundIndex ==0)
        {

            soundIndex = 1;
        }
        else if(intCooldown <=0 && soundIndex == 1)
        {
            soundIndex = 0;
        }
        
    }
    public void SaveVolumeButton()
    {
        float volumeValue = slider.value;
        PlayerPrefs.SetFloat("VolumeValue", volumeValue);
        LoadValues();
    }

    public void LoadValues()
    {
        float volumeValue = PlayerPrefs.GetFloat("VolumeValue");
        slider.value = volumeValue;
        AudioListener.volume = volumeValue;
    }


    public void PlaySound(AudioSource source)
    {
        if (boogersounds[previousIndex].isPlaying == true)
        {
            boogersounds[previousIndex].Stop();
        }

        if (source.isPlaying == false)
        {
            source.pitch = Random.Range(0.9f, 1.1f);
            source.Play();
        }
    }

    public void StopSound()
    {
        isPlaying = false;

        foreach(AudioSource source in boogersounds)
        {
            source.Stop();
        }
    }
}
