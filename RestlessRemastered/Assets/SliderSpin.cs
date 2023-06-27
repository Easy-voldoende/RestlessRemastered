using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using static Unity.VisualScripting.Member;
using static UnityEngine.Rendering.DebugUI;

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
    public float intCooldown = 3;
    public bool isMainGame;
    public int i;
    private int previousIndex = -1;

    private void Start()
    {
        slider = GetComponent<Slider>();
        LoadValues();
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
        SaveVolumeButton();
    }
    public void SetAmbientVolume(float value)
    {
        mixer.SetFloat(MIXER_AMBIENT, Mathf.Log10(value) * 20);
        isPlaying = true;
        SaveVolumeButton();
    }
    public void SetSFXVolume(float value)
    {
        mixer.SetFloat(MIXER_SFX, Mathf.Log10(value) * 20);
        isPlaying = true;
        SaveVolumeButton();

    }
    private void Update()
    {        

        sliderValue = slider.value;
        volumeValue = slider.value;
        actualValue = volumeValue * 100;
        volumeText.GetComponent<TextMeshProUGUI>().text = actualValue.ToString("F0");
        Vector3 rot = new Vector3(0, 0, sliderValue * -spinSpeed);
        handle.GetComponent<RectTransform>().localEulerAngles = rot;

        
        if (isPlaying == true && Input.GetMouseButton(0))
        {
            if (isPlaying && Input.GetMouseButton(0))
            {
                if (i != previousIndex)
                {
                    previousIndex = i;
                    PlaySound(boogersounds[i]);
                    i += 1;
                    if (i > boogersounds.Length - 1)    
                    {
                        i = 0;
                    }
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            StopSound();
        }
        
    }
    public void SaveVolumeButton()
    {
        if(gameObject.name == "AmbientSlider")
        {
            slider = GetComponent<Slider>();
            PlayerPrefs.SetFloat("AmbientSlider",slider.value);
            mixer.SetFloat(MIXER_AMBIENT, Mathf.Log10(slider.value) * 20);
        }
        if (gameObject.name == "MusicSlider")
        {
            slider = GetComponent<Slider>();
            PlayerPrefs.SetFloat("MusicValue", slider.value);
            mixer.SetFloat(MIXER_MASTER, Mathf.Log10(slider.value) * 20);
        }
        if (gameObject.name == "SFXSlider")
        {
            slider = GetComponent<Slider>();
            mixer.SetFloat(MIXER_SFX, Mathf.Log10(slider.value) * 20);
            PlayerPrefs.SetFloat("SFXSlider",slider.value);
        }
    }

    public void LoadValues()
    {
        if(isMainGame == true)
        {
            if (gameObject.name == "MusicSlider")
            {
                slider = GetComponent<Slider>();
                slider.value = PlayerPrefs.GetFloat("MusicValue");
                mixer.SetFloat(MIXER_MASTER, Mathf.Log10(slider.value) * 20);
                
            }
            if (gameObject.name == "AmbientSlider")
            {
                slider = GetComponent<Slider>();
                slider.value = PlayerPrefs.GetFloat("AmbientSlider");
                mixer.SetFloat(MIXER_AMBIENT, Mathf.Log10(slider.value) * 20);
                
            }
            if (gameObject.name == "SFXSlider")
            {
                slider = GetComponent<Slider>();
                slider.value = PlayerPrefs.GetFloat("SFXSlider");
                mixer.SetFloat(MIXER_SFX, Mathf.Log10(slider.value) * 20);
                

            }
        }
       
        
    }


    public void PlaySound(AudioSource source)
    {

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
