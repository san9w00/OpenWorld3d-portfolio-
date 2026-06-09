using UnityEngine;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour
{
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider sfxSlider;

    private void Start()
    {
        masterSlider.value = SoundManager.Instance.MasterVolume;
        sfxSlider.value = SoundManager.Instance.SFXVolume;

        masterSlider.onValueChanged.AddListener(OnMasterChanged);
        sfxSlider.onValueChanged.AddListener(OnSFXChanged);
    }

    private void OnMasterChanged(float value)
    {
        SoundManager.Instance.SetMasterVolume(value);
    }

    private void OnSFXChanged(float value)
    {
        SoundManager.Instance.SetSFXVolume(value);
    }
}
