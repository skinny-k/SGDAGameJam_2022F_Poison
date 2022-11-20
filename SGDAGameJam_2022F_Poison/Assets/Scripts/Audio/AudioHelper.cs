using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AudioHelper
{
    public static AudioSource PlayClip2D(AudioClip clip, float volume)
    {
        GameObject _audioObject = new GameObject("Audio2D");
        AudioSource _audioSource = _audioObject.AddComponent<AudioSource>();

        _audioSource.clip = clip;
        _audioSource.volume = volume;

        _audioSource.Play();
        Object.Destroy(_audioObject, clip.length);

        return _audioSource;
    }

    public static AudioSource PlayClip3D(AudioClip clip, float volume, Vector3 location)
    {
        GameObject _audioObject = new GameObject("Audio3D");
        AudioSource _audioSource = _audioObject.AddComponent<AudioSource>();

        _audioSource.clip = clip;
        _audioSource.volume = volume;
        _audioSource.transform.position = location;
        _audioSource.spatialBlend = 1.0f;

        _audioSource.Play();
        Object.Destroy(_audioObject, clip.length);

        return _audioSource;
    }
}
