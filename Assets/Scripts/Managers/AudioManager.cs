using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : BaseManager
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    
    [Header("Audio Clips")]
    [SerializeField] private AudioClip battleBGM;
    
    [Header("Audio Settings")]
    [SerializeField] private float defaultMusicVolume = 0.5f;
    [SerializeField] private float defaultSFXVolume = 1f;
    
    private void Start()
    {
        InitializeAudioSources();
        PlayBattleBGM();
    }
    
    private void InitializeAudioSources()
    {
        // If the music source is not set, add a new one
        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.loop = true;
            musicSource.volume = defaultMusicVolume;
        }
        
        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.volume = defaultSFXVolume;
        }
    }
    
    public void PlayBattleBGM()
    {
        if (battleBGM != null && musicSource != null)
        {
            musicSource.clip = battleBGM;
            musicSource.Play();
        }
    }
    
    public void StopBGM()
    {
        if (musicSource != null)
        {
            musicSource.Stop();
        }
    }
    
    public void PlaySFX(AudioClip clip)
    {
        if (clip != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }
    
    public void SetMusicVolume(float volume)
    {
        if (musicSource != null)
        {
            musicSource.volume = Mathf.Clamp01(volume);
        }
    }
    
    public void SetSFXVolume(float volume)
    {
        if (sfxSource != null)
        {
            sfxSource.volume = Mathf.Clamp01(volume);
        }
    }
}
