using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionManager : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    private Resolution[] resolutions;
    private List<Resolution> filteredResolutions;
    private float currentRefreshRate;
    private int currentResIndex = 0;

    private void Start()
    {
        resolutions = Screen.resolutions;
        filteredResolutions = new List<Resolution>();

        resolutionDropdown.ClearOptions();
        //this first line is a reference of where to add it in the script
        currentRefreshRate = Screen.currentResolution.refreshRate;
        //this is the actual loop check
        for (int i = 0; i < resolutions.Length; i++)
        {
            if (!filteredResolutions.Any(x => x.width == resolutions[i].width && x.height == resolutions[i].height))  //check if resolution already exists in list
            {
                filteredResolutions.Add(resolutions[i]);  //add resolution to list if it doesn't exist yet
            }
        }

        List<string> options =new List<string>();
        for (int i = 0; i < filteredResolutions.Count; i++)
        {
            string resolutionOption = filteredResolutions[i].width + "x"+ filteredResolutions[i].height +" "+ filteredResolutions[i].refreshRate+ " Hz";
            options.Add(resolutionOption);
            if(filteredResolutions[i].width == Screen.width && filteredResolutions[i].height == Screen.height)
            {
                currentResIndex = i;
            }
        }


        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResIndex;
        resolutionDropdown.RefreshShownValue();

    }
    public void SetRes(int resolutionIndex)
    {
        Resolution resolution = filteredResolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, true);
    }
    
}
