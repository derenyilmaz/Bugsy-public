using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    [SerializeField] private Slider sfxVolume;
    [SerializeField] private TextMeshProUGUI sfxVolumeText;
    
    [SerializeField] private Slider musicVolume;
    [SerializeField] private TextMeshProUGUI musicVolumeText;
    [SerializeField] private AudioClip sfxSetSound;
    
    
    private const float SetSoundPlayThreshold = 1f;
    private float _lastPlayTime = 0f;

    private void Start()
    {
        sfxVolume.onValueChanged.AddListener(UpdateSfxVolume);
        musicVolume.onValueChanged.AddListener(UpdateMusicVolume);
    }

    private void OnDestroy()
    {
        sfxVolume.onValueChanged.RemoveListener(UpdateSfxVolume);
        musicVolume.onValueChanged.RemoveListener(UpdateMusicVolume);
    }

    private void UpdateMusicVolume(float volume)
    {
        musicVolumeText.text = $"music volume: {volume}";
        AudioManager.Instance.SetMusicVolume(volume/100f);
    }

    private void UpdateSfxVolume(float volume)
    {
        sfxVolumeText.text = $"sfx volume: {volume}";
        TryPlay();
        AudioManager.Instance.SetSfxVolume(volume/100f);
    }

    
    private void TryPlay()
    {
        if (Time.realtimeSinceStartup - _lastPlayTime > SetSoundPlayThreshold)
        {
            AudioManager.Instance.PlaySound(sfxSetSound);
            _lastPlayTime = Time.realtimeSinceStartup;
        }
    }
}
