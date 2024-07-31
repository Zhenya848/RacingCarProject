using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    private AudioSource _audioSource;
    public Slider MusicSlider;

    [SerializeField] private AudioClip[] _musics;
    private int _indexOfMusic;

    private const string VOLUME_TAG = "MusicVolume";

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();

        _indexOfMusic = Random.Range(0, _musics.Length);
        _audioSource.clip = _musics[_indexOfMusic];

        _audioSource.volume = GetVolume(VOLUME_TAG, 0.6f); ;
        MusicSlider.value = _audioSource.volume;

        _audioSource.Play();
    }

    private void Update()
    {
        if (_audioSource.isPlaying == false)
            PlayNextMusic();

        ChangeVolume();
    }

    private void PlayNextMusic()
    {
        if (_indexOfMusic == _musics.Length - 1)
            _indexOfMusic = 0;
        else
            _indexOfMusic++;

        _audioSource.clip = _musics[_indexOfMusic];
        _audioSource.Play();
    }

    private void ChangeVolume()
    {
        if (_audioSource.volume != MusicSlider.value)
        {
            _audioSource.volume = MusicSlider.value;
            PlayerPrefs.SetFloat(VOLUME_TAG, _audioSource.volume);
        }
    }

    private float GetVolume(in string key, float defaultValue)
    {
        return PlayerPrefs.HasKey(key) ? PlayerPrefs.GetFloat(key, defaultValue) : defaultValue;
    }
}