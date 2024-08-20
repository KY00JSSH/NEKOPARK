using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System.Diagnostics;
using Mirror;

public class AudioManager : MonoBehaviour
{
    public AudioMixer audioMixer;

    public static AudioManager instance = null;        //싱글톤

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Init();
        }
        else
        {
            Destroy(gameObject);
        }
    }



    [Header("#BGM")]
    public AudioClip[] BgmClip;   
    AudioSource[] bgmPlayer;

    public enum Bgm
    {
        FlyingGameBGM,      //비행기 스테이지를 위한 브금
        GameBGM1,           //인게임브금1
        GameBGM2,           //인게임브금2
        MainMenuBGM,        //메인메뉴 브금
        MultiRoomBGM        //멀티플레이어 대기실 브금
    }



    [Header("#SFX")]
    public AudioClip[] SfxClip;
    AudioSource[] sfxPlayer;

    public enum Sfx
    {
        buttonDown,         //빨간 버튼 눌렀을 때 효과음
        coin,               //코인 획득 효과음
        getKeyDoorOpen,     //열쇠 획득, 문 열었을 때 효과음
        jump,               //점프 효과음
        nextStageLoading,   //다음 스테이지 로딩 효과음
        playerDie,          //플레이어 사망 효과음
        stageClear,         //스테이지 클리어 효과음
        timerBipBip_1sec,   //타이머 삡삡 효과음
        timeUp              //시간초과 실패 효과음
    }

    private void Init()
    {
        GameObject bgmObject = new GameObject("BGMPlayer");     //BGM플레이어 초기화
        bgmObject.transform.parent = transform;
        bgmPlayer = new AudioSource[BgmClip.Length];

        //Debug.Log("BGM Channels: " + bgmPlayer.Length);

        for (int index = 0; index < bgmPlayer.Length; index++)
        {
            bgmPlayer[index] = bgmObject.AddComponent<AudioSource>();
            bgmPlayer[index].playOnAwake = false;
            bgmPlayer[index].loop = true;
            bgmPlayer[index].outputAudioMixerGroup = audioMixer.FindMatchingGroups("BGM")[0];
        }
        //Debug.Log("BGM Player Array Length: " + bgmPlayer.Length);



        GameObject sfxObject = new GameObject("SFXPlayer");     //SFX플레이어 초기화
        sfxObject.transform.parent = transform;
        sfxPlayer = new AudioSource[SfxClip.Length];

        for (int index = 0; index < sfxPlayer.Length; index++)
        {
            sfxPlayer[index] = sfxObject.AddComponent<AudioSource>();
            sfxPlayer[index].playOnAwake = false;
            sfxPlayer[index].outputAudioMixerGroup = audioMixer.FindMatchingGroups("SFX")[0];
        }

        //Debug.Log("AudioManager Initialized.");
    }



    public void PlayBGM(Bgm bgm, int channel)           //브금 플레이 메서드
    {
        //사용시, AudioManager.instance.PlayBGM(AudioManager.Bgm.ClipName, 0); 사용
        //정지시, AudioManager.instance.StopBGM(0);

        if (channel < 0 || channel >= bgmPlayer.Length)
        {
            return;
        }

        AudioSource bgmSource = bgmPlayer[channel];

        bgmSource.clip = BgmClip[(int)bgm];
        bgmSource.Play();
    }

    public void StopBGM(int channel)                               //브금 정지 메서드
    {
        //foreach(AudioSource player in bgmPlayer)
        //{
        //    if(player.isPlaying)
        //    {
        //        player.Stop();
        //    }
        //}

        if (channel < 0 || channel >= bgmPlayer.Length)
        {
            return;
        }

        AudioSource bgmSource = bgmPlayer[channel];
        if (bgmSource.isPlaying)
        {
            bgmSource.Stop();
        }
    }



    public void PlaySFX(Sfx sfx)
    {
        //사용시, AudioManager.instance.PlaySFX(AudioManager.Sfx.ClipName); 사용

        //TODO: [김수주] 오디오 에러로 임시 주석처리
        AudioClip clipToPlay = SfxClip[(int)sfx];
                
        foreach(var sfxSource in sfxPlayer)
        {
            if(!sfxSource.isPlaying)
            {
                sfxSource.clip = clipToPlay;
                sfxSource.Play();
                break;
            }
        }
    }

    public void BGMVolume(float volume)
    {
        audioMixer.SetFloat("BGMVolume", Mathf.Log10(volume) * 20);
    }

    public void SFXVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);

    }
}
