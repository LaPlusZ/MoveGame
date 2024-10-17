using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using DG.Tweening;

public class SettingsMenu : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown;
    public Toggle fullscreenToggle;
    public Slider volumeSlider;

    private Resolution[] resolutions;
    private bool isOpen;

    void Start()
    {
        // Initialize Resolutions
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);

        // Load saved resolution
        currentResolutionIndex = PlayerPrefs.GetInt("resolutionIndex", currentResolutionIndex);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        // Set the resolution at start
        SetResolution(currentResolutionIndex);

        // Load saved fullscreen state
        bool isFullscreen = PlayerPrefs.GetInt("isFullscreen", Screen.fullScreen ? 1 : 0) == 1;
        fullscreenToggle.isOn = isFullscreen;
        SetFullscreen(isFullscreen);

        // Load saved volume setting
        float savedVolume = PlayerPrefs.GetFloat("volume", 1f);
        volumeSlider.value = savedVolume;
        SetVolume(savedVolume);
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        
        // Save resolution setting
        PlayerPrefs.SetInt("resolutionIndex", resolutionIndex);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        
        // Save fullscreen setting
        PlayerPrefs.SetInt("isFullscreen", isFullscreen ? 1 : 0);
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        
        // Save volume setting
        PlayerPrefs.SetFloat("volume", volume);
    }

    public async void CloseMenu()
    {
        if (isOpen == true)
        {
            transform.localScale = new Vector3(1,1,1);
            GetComponent<CanvasGroup>().alpha = 1;
            transform.DOScale(new Vector3(1.2f,1.2f,1.2f), 0.5f).SetEase(Ease.InQuart).SetUpdate(true);
            await GetComponent<CanvasGroup>().DOFade(0, 0.5f).SetUpdate(true).AsyncWaitForCompletion();
            gameObject.SetActive(false);
            isOpen = false;
        }
    }

    public async void OpenMenu()
    {
        if (isOpen == false)
        {
            gameObject.SetActive(true);
            GetComponent<CanvasGroup>().alpha = 0;
            transform.localScale = new Vector3(1.2f,1.2f,1.2f);
            transform.DOScale(new Vector3(1,1,1), 0.5f).SetEase(Ease.OutQuart).SetUpdate(true);
            await GetComponent<CanvasGroup>().DOFade(1, 0.5f).SetUpdate(true).AsyncWaitForCompletion();
            isOpen = true;
        }
    }
}
