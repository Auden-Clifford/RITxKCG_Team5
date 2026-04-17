using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AudioClips
{
    public string Name;
    public AudioClip Clip;
}

[RequireComponent(typeof(AudioSource))]
public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private AudioSource _bgmAS;
    [SerializeField] private AudioSource _sfxAS;
    [SerializeField] private List<AudioClips> _bgmAudioClips = new();
    [SerializeField] private List<AudioClips> _sfxAudioClips = new();

    private AudioClips _currentBGM;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    private void Start() => PlayBGM("TITLE", .1f);

    public void PlayBGM(string name, float fadeTime = 1f)
    {
        if (_currentBGM != null && _currentBGM.Name == name) return;

        StartCoroutine(FadeOut(_bgmAS, fadeTime, () =>
        {
            _currentBGM = _bgmAudioClips.Find(x => x.Name == name);
            _bgmAS.clip = _currentBGM?.Clip;
            _bgmAS.Play();
        }));
    }

    public void PlaySFX(string name)
    {
        _sfxAS.clip = _sfxAudioClips.Find(x => x.Name == name)?.Clip;
        _sfxAS.Play();
    }

    private IEnumerator FadeOut(AudioSource audioSource, float fadeTime, Action callback)
    {
        float startVolume = audioSource.volume;
        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.unscaledDeltaTime / fadeTime;
            yield return null;
        }
        audioSource.Stop();
        audioSource.volume = startVolume;
        callback?.Invoke();
    }
}