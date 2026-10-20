using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplashUiController : MonoBehaviour
{
    [SerializeField] private Button playButton;
    
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button creditsButton;
    
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject creditsPanel;
    
    [SerializeField] private AudioClip clickSound;

    private void Awake()
    {
        settingsPanel.SetActive(false);
        creditsPanel.SetActive(false);
        
        settingsButton.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlaySound(clickSound);
            settingsPanel.SetActive(true);
        });
        creditsButton.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlaySound(clickSound);
            creditsPanel.SetActive(true);
        });
        
        playButton.onClick.AddListener(Play);
    }

    private void OnDestroy()
    {
        playButton.onClick.RemoveAllListeners();
        settingsButton.onClick.RemoveAllListeners();
        creditsButton.onClick.RemoveAllListeners();
    }

    private void Play()
    {
        AudioManager.Instance.PlaySound(clickSound);
        SceneManager.LoadScene("Scenes/IntroCardScene");
    }
}
