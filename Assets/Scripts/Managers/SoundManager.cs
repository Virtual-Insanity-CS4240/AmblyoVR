using System;
using System.Collections;
using UnityEngine;

public class SoundManager : SimpleSingleton<SoundManager>
{
    [Header("AudioSource")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("AudioClip - Music")]
    public AudioClip bgmBossFightCleanIntro;
    public AudioClip bgmBossFightLoop;
    public AudioClip bgmBattleCleanIntro;
    public AudioClip bgmBattleLoop;
    public AudioClip bgmCasualCleanIntro;
    public AudioClip bgmCasualLoop;

    [Header("AudioClip - Sound Effects")]
    public AudioClip ballChanging;
    public AudioClip ballLanding;
    public AudioClip ballThrowing;
    public AudioClip doorOpening;
    public AudioClip hit;
    public AudioClip poof;
    public AudioClip sneeze;
    public AudioClip warning;

    //public AudioClip[] audioClips;

    protected override void HandleAwake()
    {
        base.HandleAwake();
    }

    void playBossFightMusic()
    {
        StartCoroutine(PlayBGMSequence(bgmBossFightCleanIntro, bgmBossFightLoop));
    }

    void playBattleMusic()
    {
        StartCoroutine(PlayBGMSequence(bgmBattleCleanIntro, bgmBattleLoop));
    }

    void playCasualMusic()
    {
        StartCoroutine(PlayBGMSequence(bgmCasualCleanIntro, bgmCasualLoop));
    }

    public void PlayBallChangingSound()
    {
        SFXSource.PlayOneShot(ballChanging);
    }

    public void PlayBallLandingSound()
    {
        SFXSource.PlayOneShot(ballLanding);
    }

    public void PlayBallThrowingSound()
    {
        SFXSource.PlayOneShot(ballThrowing);
    }

    public void PlayDoorOpeningSound()
    {
        SFXSource.PlayOneShot(doorOpening);
    }

    public void PlayHitSound()
    {
        SFXSource.PlayOneShot(hit);
    }

    public void PlayPoofSound()
    {
        SFXSource.PlayOneShot(poof);
    }

    public void PlaySneezeSound()
    {
        SFXSource.PlayOneShot(sneeze);
    }

    public void PlayWarningSound()
    {
        SFXSource.PlayOneShot(warning);
    }

    //void playSound(string sound)
    //{
    //    AudioClip audioClip = Array.Find(audioClips, x => x.name == sound);

    //    if (audioClip == null)
    //    {
    //        Debug.Log("Sound Not Found");
    //    } 

    //    else
    //    {
    //        SFXSource.PlayOneShot(audioClip);
    //    }
    //}

    private IEnumerator PlayBGMSequence(AudioClip bgm1, AudioClip bgm2)
    {
        // Play bgm1 once
        musicSource.clip = bgm1;
        musicSource.loop = false;
        musicSource.Play();

        // Wait for bgm1 to finish
        yield return new WaitForSeconds(bgm1.length);

        // Then switch to bgm2 and loop
        musicSource.clip = bgm2;
        musicSource.loop = true;
        musicSource.Play();
    }
}
