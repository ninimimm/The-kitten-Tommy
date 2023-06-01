using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    private Resolution[] _resolutions;
    public Dropdown resolutionDropdown;
    private void Start()
    {
        _resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        var options = new List<string>();
        var currentResolutionIndex = 0;
        for (var i = 0; i < _resolutions.Length; i++)
        {
            options.Add($"{_resolutions[i].width} x {_resolutions[i].height}");
            if (_resolutions[i].width == Screen.currentResolution.width &&
                _resolutions[i].height == Screen.currentResolution.height)
                currentResolutionIndex = i;
        }
            
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public AudioMixer audioMixer;
    public void SelVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }
    public void SetFullScreen(bool full)
    {
        Screen.fullScreen = full;
    }

    public void SetResolution(int resolutionIndex)
    {
        var resolution = _resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width,resolution.height,Screen.fullScreen);
    }
}
