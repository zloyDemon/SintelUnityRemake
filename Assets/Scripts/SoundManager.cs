using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SoundManager : MonoBehaviour
{
    private const float MaxMusicVolume = 0.5f;

    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource soundSource;

    [SerializeField] SoundsSO musics;
    [SerializeField] SoundsSO uiEffects;
    [SerializeField] SoundsSO sounds;

    private Tween fadeMusicTween;

    public AudioClip CurrentMusic => musicSource.clip;

    public enum SoundId
    {

    }

    public enum MusicId
    {
        Menu,
        Battle,
    }

    public static SoundManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == this)
            Destroy(gameObject);
        if (Instance == null)
            Instance = this;
        DontDestroyOnLoad(gameObject);

        musicSource.volume = MaxMusicVolume;
        soundSource.volume = MaxMusicVolume;
    }

    public void PlayNewMusic(string musicId)
    {
        var clip = musics.GetSound(musicId);
        musicSource.clip = clip;
        musicSource.Play();
        DoFadeOutMusic();
    }

    public void PlaySound(string soundId)
    {
        var clip = uiEffects.GetSound(soundId);
        soundSource.clip = clip;
        soundSource.Play();
    }

    public void StopMusic()
    {
        if (CurrentMusic != null)
            musicSource.Stop();
    }

    public void PlayMusic()
    {
        if (CurrentMusic != null)
            musicSource.Play();
    }

    public void DoFadeOutMusic()
    {
        if (musicSource.clip != null)
            musicSource.Play();
        DoFadeMusic(MaxMusicVolume);
    }

    public void PlayClipOnAudioSource(AudioSource source, AudioClip clip)
    {

    }

    public void DoFadeInMusic()
    {
        DoFadeMusic(0);
    }

    private void DoFadeMusic(float endValue)
    {
        TweenUtils.KillAndNull(ref fadeMusicTween);
        const float fadeMusicDuration = 1.5f;
        fadeMusicTween = musicSource.DOFade(endValue, fadeMusicDuration).OnComplete(() =>
        {
            fadeMusicTween = null;
            if (endValue < MaxMusicVolume)
                musicSource.Pause();
        });
    }
}
