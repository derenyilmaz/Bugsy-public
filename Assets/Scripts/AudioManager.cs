
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager  : MonoBehaviour
{
    public static AudioManager Instance;
    
    [SerializeField] private AudioClip ambientMusicIntro;
    [SerializeField] private AudioClip ambientMusicLoop;
    [SerializeField] private AudioClip bossMusicIntro;
    [SerializeField] private AudioClip bossMusicLoop;
    
    [SerializeField] private AudioSource sfxAudioSource;
    [SerializeField] private AudioSource musicAudioSource;
    [SerializeField] private AudioSource musicLoopAudioSource;
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        PlayAmbientMusic();
        SceneManager.sceneLoaded += OnSceneLoaded;
        SetSfxVolume(50);
        SetMusicVolume(70);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene loadedScene, LoadSceneMode arg1)
    {
        if (loadedScene.name == "BossScene") PlayBossMusic();
        else if (loadedScene.name != "SplashScene") PlayAmbientMusic();
    }
    
    public void PlaySound(AudioClip clip)
    {
        if (sfxAudioSource.clip == clip && sfxAudioSource.isPlaying) return;
        sfxAudioSource.PlayOneShot(clip);
    }

    private void PlayBossMusic()
    {
        if (musicAudioSource.clip == bossMusicIntro || musicAudioSource.clip == bossMusicLoop) return;
        
        StartCoroutine(PlayBossMusicRoutine());
    }
    
    private IEnumerator PlayBossMusicRoutine()
    {
        musicAudioSource.clip = bossMusicIntro;
        musicAudioSource.loop = false;
        musicAudioSource.Play();
        
        musicLoopAudioSource.clip = bossMusicLoop;
        musicLoopAudioSource.loop = true;
        
        yield return new WaitForSeconds(bossMusicIntro.length-1f);

        musicLoopAudioSource.volume = 0;
        musicLoopAudioSource.Play();
        var endVol = musicAudioSource.volume;
        
        var elapsedTime = 0f;
        var currentVol = 0f;
        
        while (elapsedTime < 1)
        {
            currentVol = Mathf.Lerp(0f, endVol, elapsedTime / 1f);
            musicAudioSource.volume = (1-currentVol);
            musicLoopAudioSource.volume = currentVol;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
    
    private void PlayAmbientMusic()
    {
        if (musicAudioSource.clip == ambientMusicIntro || musicAudioSource.clip == ambientMusicLoop) return;
        
        StartCoroutine(PlayAmbientMusicRoutine());
    }

    private IEnumerator PlayAmbientMusicRoutine()
    {
        musicAudioSource.clip = ambientMusicIntro;
        musicAudioSource.loop = false;
        musicAudioSource.Play();
        
        musicLoopAudioSource.clip = ambientMusicLoop;
        musicLoopAudioSource.loop = true;
        
        yield return new WaitForSeconds(ambientMusicIntro.length-1f);

        musicLoopAudioSource.volume = 0;
        musicLoopAudioSource.Play();
        var endVol = musicAudioSource.volume;
        
        var elapsedTime = 0f;
        var currentVol = 0f;
        
        while (elapsedTime < 1)
        {
            currentVol = Mathf.Lerp(0f, endVol, elapsedTime / 1f);
            musicAudioSource.volume = (1-currentVol);
            musicLoopAudioSource.volume = currentVol;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    public void SetSfxVolume(float volume)
    {
        sfxAudioSource.volume = volume;
    }

    public void SetMusicVolume(float volume)
    {
        musicAudioSource.volume = volume;
        musicLoopAudioSource.volume = volume;
    }
}
