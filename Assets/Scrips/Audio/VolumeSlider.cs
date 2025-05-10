using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class VolumeSlider : MonoBehaviour
{
    public enum VolumeType { Music, SFX }

    [SerializeField] private VolumeType volumeType;
    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        InitializeSlider();
    }

    private void InitializeSlider()
    {
        if (AudioManager.Instance == null) return;

        slider.minValue = 0.0001f;
        slider.maxValue = 1f;

        slider.value = volumeType == VolumeType.Music
            ? AudioManager.Instance.GetMusicVolume()
            : AudioManager.Instance.GetSFXVolume();

        if (volumeType == VolumeType.Music)
        {
            AudioManager.OnMusicVolumeChanged += UpdateSliderValue;
        }
        else
        {
            AudioManager.OnSFXVolumeChanged += UpdateSliderValue;
        }

        slider.onValueChanged.AddListener(HandleSliderChanged);
    }

    private void UpdateSliderValue(float newVolume)
    {
        slider.onValueChanged.RemoveListener(HandleSliderChanged);
        slider.value = newVolume;
        slider.onValueChanged.AddListener(HandleSliderChanged);
    }

    private void HandleSliderChanged(float value)
    {
        if (AudioManager.Instance == null) return;

        switch (volumeType)
        {
            case VolumeType.Music:
                AudioManager.Instance.SetMusicVolume(value);
                break;
            case VolumeType.SFX:
                AudioManager.Instance.SetSFXVolume(value);
                break;
        }
    }

    private void OnDestroy()
    {
        if (volumeType == VolumeType.Music)
        {
            AudioManager.OnMusicVolumeChanged -= UpdateSliderValue;
        }
        else
        {
            AudioManager.OnSFXVolumeChanged -= UpdateSliderValue;
        }

        slider.onValueChanged.RemoveListener(HandleSliderChanged);
    }
}