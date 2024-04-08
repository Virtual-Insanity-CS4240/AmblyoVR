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
    public AudioClip doorClosing;
    public AudioClip hit;
    public AudioClip poof;
    public AudioClip sneeze;
    public AudioClip warning;
    public AudioClip stepping;

    private float timer = 0f;
    private bool isInMusicSequence = false;
    private AudioClip secondPart = null;

    private void Update()
    {
        if (isInMusicSequence)
        {
            if (timer > 0f)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                isInMusicSequence = false;
                PlayerBGMLoop();
            }
        }
    }

    protected override void HandleAwake()
    {
        base.HandleAwake();
    }

    public void PlayBossFightMusic()
    {                
        PlayBGMSequence(bgmBossFightCleanIntro, bgmBossFightLoop);
    }

    public void PlayBattleMusic()
    {
        PlayBGMSequence(bgmBattleCleanIntro, bgmBattleLoop);
    }

    public void PlayCasualMusic()
    {
        PlayBGMSequence(bgmCasualCleanIntro, bgmCasualLoop);
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

    public void PlayDoorClosingSound()
    {
        SFXSource.PlayOneShot(doorClosing);
    }

    public void PlayHitSound()
    {
        SFXSource.PlayOneShot(hit);
    }

    public void PlayPoofSound()
    {
        SFXSource.PlayOneShot(poof, 2);
    }

    public void PlaySneezeSound()
    {
        SFXSource.PlayOneShot(sneeze);
    }

    public void PlayWarningSound()
    {
        SFXSource.PlayOneShot(warning);
    }

    public void PlaySteppingSound()
    {
        SFXSource.PlayOneShot(stepping, 0.3f);
    }

    private void PlayBGMSequence(AudioClip bgm1, AudioClip bgm2)
    {
        musicSource.Stop();

        secondPart = bgm2;

        // Play bgm1 once
        musicSource.clip = bgm1;
        musicSource.loop = false;
        musicSource.Play();

        // Wait for bgm1 to finish
        timer = bgm1.length;
        isInMusicSequence = true;
    }

    private void PlayerBGMLoop()
    {
        if (secondPart == null)
            Debug.LogError("No Second Part of BGM Found!");
        musicSource.clip = secondPart;
        musicSource.loop = true;
        musicSource.Play();
    }
}
