using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeSettings : MonoBehaviour
{
    public AudioMixer mixer;
    public string[] mixerParameters; // Array of mixer parameter names for each group

    private const float MinVolume = -80f;
    private const float MaxVolume = 0f;

    public void SetVolume(int index, float value)
    {
        float mappedValue = Mathf.Lerp(MinVolume, MaxVolume, value);
        mixer.SetFloat(mixerParameters[index], mappedValue);
    }

    public float GetVolume(int index)
    {
        mixer.GetFloat(mixerParameters[index], out float value);
        float mappedValue = Mathf.InverseLerp(MinVolume, MaxVolume, value);
        return mappedValue;
    }

    public void SaveVolumes()
    {
        for (int i = 0; i < mixerParameters.Length; i++)
        {
            float volumeValue = GetVolume(i);
            PlayerPrefs.SetFloat(mixerParameters[i], volumeValue);
        }
        PlayerPrefs.Save();
    }

    public void LoadVolumes()
    {
        for (int i = 0; i < mixerParameters.Length; i++)
        {
            if (PlayerPrefs.HasKey(mixerParameters[i]))
            {
                float volumeValue = PlayerPrefs.GetFloat(mixerParameters[i]);
                SetVolume(i, volumeValue);
            }
            else
            {
                SetVolume(i, 1f);
            }
        }
    }
}