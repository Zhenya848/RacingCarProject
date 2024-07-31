using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioItem : MonoBehaviour
{
    [SerializeField] private AudioEffectsManager _audioEffectsManager;
    [SerializeField] private bool _loopMode;

    private AudioSource _audioSource;
    [SerializeField] private AudioClip _audioEffect;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        _audioSource.clip = _audioEffect;
        _audioSource.volume = _audioEffectsManager.Volume;

        if (_loopMode)
        {
            _audioSource.loop = true;
            _audioSource.Play();

            StartCoroutine(SetVolume());
        }
    }

    IEnumerator SetVolume()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f);
            _audioSource.volume = _audioEffectsManager.Volume;
        }
    }

    public void PlayAudio()
    {
        _audioSource.volume = _audioEffectsManager.Volume;
        _audioSource.Play();
    }
}