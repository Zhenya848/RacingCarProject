using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioEffectsManager : MonoBehaviour
{
    public float Volume;
    [SerializeField] private Slider _volumeSlider;
    private const string VOLUME_TAG = "EffectVolume";

    private void Awake()
    {
        Volume = GetVolume(VOLUME_TAG, 0.6f);
        _volumeSlider.value = Volume;
    }

    private void Update()
    {
        ChangeVolume();
    }

    private float GetVolume(in string key, float defaultValue)
    {
        if (PlayerPrefs.HasKey(key) == false)
            PlayerPrefs.SetFloat(key, defaultValue);

        return PlayerPrefs.GetFloat(key, defaultValue);
    }

    private void ChangeVolume()
    {
        if (Volume != _volumeSlider.value)
        {
            Volume = _volumeSlider.value;
            PlayerPrefs.SetFloat(VOLUME_TAG, Volume);
        }
    }
}