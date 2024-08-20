using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System.Diagnostics;
using Mirror;

public class AudioManager : MonoBehaviour
{
    public AudioMixer audioMixer;

    public static AudioManager instance = null;        //�̱���

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
        FlyingGameBGM,      //����� ���������� ���� ���
        GameBGM1,           //�ΰ��Ӻ��1
        GameBGM2,           //�ΰ��Ӻ��2
        MainMenuBGM,        //���θ޴� ���
        MultiRoomBGM        //��Ƽ�÷��̾� ���� ���
    }



    [Header("#SFX")]
    public AudioClip[] SfxClip;
    AudioSource[] sfxPlayer;

    public enum Sfx
    {
        buttonDown,         //���� ��ư ������ �� ȿ����
        coin,               //���� ȹ�� ȿ����
        getKeyDoorOpen,     //���� ȹ��, �� ������ �� ȿ����
        jump,               //���� ȿ����
        nextStageLoading,   //���� �������� �ε� ȿ����
        playerDie,          //�÷��̾� ��� ȿ����
        stageClear,         //�������� Ŭ���� ȿ����
        timerBipBip_1sec,   //Ÿ�̸� ��� ȿ����
        timeUp              //�ð��ʰ� ���� ȿ����
    }

    private void Init()
    {
        GameObject bgmObject = new GameObject("BGMPlayer");     //BGM�÷��̾� �ʱ�ȭ
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



        GameObject sfxObject = new GameObject("SFXPlayer");     //SFX�÷��̾� �ʱ�ȭ
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



    public void PlayBGM(Bgm bgm, int channel)           //��� �÷��� �޼���
    {
        //����, AudioManager.instance.PlayBGM(AudioManager.Bgm.ClipName, 0); ���
        //������, AudioManager.instance.StopBGM(0);

        if (channel < 0 || channel >= bgmPlayer.Length)
        {
            return;
        }

        AudioSource bgmSource = bgmPlayer[channel];

        bgmSource.clip = BgmClip[(int)bgm];
        bgmSource.Play();
    }

    public void StopBGM(int channel)                               //��� ���� �޼���
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
        //����, AudioManager.instance.PlaySFX(AudioManager.Sfx.ClipName); ���

        //TODO: [�����] ����� ������ �ӽ� �ּ�ó��
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
