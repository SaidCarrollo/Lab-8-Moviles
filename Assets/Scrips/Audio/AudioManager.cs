using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public static event Action<float> OnMusicVolumeChanged;
    public static event Action<float> OnSFXVolumeChanged;

    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer audioMixer;

    [Header("Mixer Parameters")]
    [SerializeField] private string musicVolumeParam = "MusicVolume";
    [SerializeField] private string sfxVolumeParam = "SFXVolume";

    private const string MUSIC_VOLUME_KEY = "MusicVolume";
    private const string SFX_VOLUME_KEY = "SFXVolume";
    private const float DEFAULT_VOLUME = 0.8f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAudio();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeAudio()
    {
        SetMusicVolume(PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, DEFAULT_VOLUME));
        SetSFXVolume(PlayerPrefs.GetFloat(SFX_VOLUME_KEY, DEFAULT_VOLUME));
    }

    public void SetMusicVolume(float volume)
    {
        float dbVolume = LinearToDecibel(volume);
        audioMixer.SetFloat(musicVolumeParam, dbVolume);
        PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, volume);
        OnMusicVolumeChanged?.Invoke(volume); // Notificar a los suscriptores
    }

    public void SetSFXVolume(float volume)
    {
        float dbVolume = LinearToDecibel(volume);
        audioMixer.SetFloat(sfxVolumeParam, dbVolume);
        PlayerPrefs.SetFloat(SFX_VOLUME_KEY, volume);
        OnSFXVolumeChanged?.Invoke(volume); // Notificar a los suscriptores
    }

    public float GetMusicVolume()
    {
        return PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, DEFAULT_VOLUME);
    }

    public float GetSFXVolume()
    {
        return PlayerPrefs.GetFloat(SFX_VOLUME_KEY, DEFAULT_VOLUME);
    }

    private float LinearToDecibel(float linear)
    {
        return linear != 0 ? 20f * Mathf.Log10(linear) : -144f;
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.Save();
    }
}