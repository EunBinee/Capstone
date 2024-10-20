using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance = null;
    public static SoundManager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }

    [Header("BGM")]
    public AudioClip[] bgmClip;
    public AudioSource bgmPlayer;
    public enum BGM
    {
        Title,
        Ingame,
        BossIngame
    }

    [Header("PlayerSound")]
    public AudioClip[] playerSoundClips;
    public AudioSource playerSoundPlayer;
    public enum PlayerSound
    {
        Hit,
        Shoot,
        SwordAttack,
        Dodge,
        Electronic,
        GetDamage,
        Death,
    }

    [Header("MonsterSound")]
    //몬스터 사운드 클립은 몬스터 스크립트로 따로 관리
    public AudioSource[] mosterSoundPlayer;
    public int monsterSound_channels;
    private int monsterSound_ChannelIndex;

    [Header("UI Sound")]
    public AudioSource UIPlayer;

    [Header("Other sfx Sound")] //기타. 효과음.
    public AudioClip[] sfxClips;
    public AudioSource[] sfxPlayer;
    public int sfx_channels;
    private int sfx_channelIndex;
    public enum SfxSound
    {
        UI,
        SoundObject
    }

    void Awake()
    {
        Init();
    }

    private void Init()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this.gameObject);

        //배경음 플레이어 초기화
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;
        bgmPlayer.volume = 1;

        //플레이어 사운드 플레이어 초기화
        GameObject playerSoundObject = new GameObject("playerSoundPlayer");
        playerSoundObject.transform.parent = transform;
        playerSoundPlayer = playerSoundObject.AddComponent<AudioSource>();
        playerSoundPlayer.playOnAwake = false;
        playerSoundPlayer.loop = true;
        playerSoundPlayer.volume = 1;

        //몬스터 사운드 플레이어 초기화
        GameObject monsterSoundObject = new GameObject("monsterSoundPlayer");
        monsterSoundObject.transform.parent = transform;
        mosterSoundPlayer = new AudioSource[monsterSound_channels];

        for (int i = 0; i < monsterSound_channels; i++)
        {
            mosterSoundPlayer[i] = monsterSoundObject.AddComponent<AudioSource>();
            mosterSoundPlayer[i].playOnAwake = false;
            mosterSoundPlayer[i].loop = true;
            mosterSoundPlayer[i].volume = 1;
        }

        //UI 플레이어
        GameObject uiobject = new GameObject("UIPlayer");
        uiobject.transform.parent = transform;
        UIPlayer = uiobject.AddComponent<AudioSource>();
        UIPlayer.playOnAwake = false;
        UIPlayer.loop = true;
        UIPlayer.volume = 1;

        //기타 효과음
        GameObject sfxObject = new GameObject("sfxPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayer = new AudioSource[sfx_channels];

        for (int i = 0; i < sfx_channels; i++)
        {
            sfxPlayer[i] = sfxObject.AddComponent<AudioSource>();
            sfxPlayer[i].playOnAwake = false;
            sfxPlayer[i].loop = true;
            sfxPlayer[i].volume = 1;
        }
    }

    public void Play_BGM(BGM bgm, bool useLoop = false)
    {
        playerSoundPlayer.loop = useLoop;
        bgmPlayer.clip = bgmClip[(int)bgm];
        bgmPlayer.Play();
    }

    public void Play_BGM_Delay(BGM bgm, float delay, bool useLoop = false)
    {
        StartCoroutine(delay_Play_co(bgm, delay, useLoop));

    }

    IEnumerator delay_Play_co(BGM bgm, float delay, bool useLoop = false)
    {
        yield return new WaitForSeconds(delay);

        playerSoundPlayer.loop = useLoop;
        bgmPlayer.clip = bgmClip[(int)bgm];
        bgmPlayer.Play();
    }


    public void Stop_BGM(BGM bgm)
    {
        bgmPlayer.clip = bgmClip[(int)bgm];
        bgmPlayer.Stop();
    }


    public void Play_PlayerSound(PlayerSound p_Sound, bool useLoop = false)
    {
        //플레이어 캐릭터 사운드
        playerSoundPlayer.loop = useLoop;
        playerSoundPlayer.clip = playerSoundClips[(int)p_Sound];
        playerSoundPlayer.Play();

    }

    public void Play_PlayerSound(PlayerSound p_Sound, float delay, bool useLoop = false)
    {
        StartCoroutine(delay_PlayerSound_co(p_Sound, delay, useLoop));

    }

    IEnumerator delay_PlayerSound_co(PlayerSound p_Sound, float delay, bool useLoop = false)
    {
        yield return new WaitForSeconds(delay);

        //플레이어 캐릭터 사운드
        playerSoundPlayer.loop = useLoop;
        playerSoundPlayer.clip = playerSoundClips[(int)p_Sound];
        playerSoundPlayer.Play();
    }


    public void Play_MonsterSound(AudioClip monsterSoundClip, bool useLoop = false)
    {
        for (int index = 0; index < mosterSoundPlayer.Length; index++)
        {
            int loopIndex = (index + monsterSound_ChannelIndex) % mosterSoundPlayer.Length;

            if (mosterSoundPlayer[loopIndex].isPlaying)
                continue;

            monsterSound_ChannelIndex = loopIndex;
            mosterSoundPlayer[monsterSound_ChannelIndex].loop = useLoop;
            mosterSoundPlayer[monsterSound_ChannelIndex].clip = monsterSoundClip;
            mosterSoundPlayer[monsterSound_ChannelIndex].Play();
            break;
        }
    }


    public void Play_MonsterSound(AudioClip monsterSoundClip, float delay, bool useLoop = false)
    {
        StartCoroutine(delay_PlayerSound_co(monsterSoundClip, delay, useLoop));

    }

    IEnumerator delay_PlayerSound_co(AudioClip monsterSoundClip, float delay, bool useLoop = false)
    {
        yield return new WaitForSeconds(delay);

        for (int index = 0; index < mosterSoundPlayer.Length; index++)
        {
            int loopIndex = (index + monsterSound_ChannelIndex) % mosterSoundPlayer.Length;

            if (mosterSoundPlayer[loopIndex].isPlaying)
                continue;

            monsterSound_ChannelIndex = loopIndex;
            mosterSoundPlayer[monsterSound_ChannelIndex].loop = useLoop;
            mosterSoundPlayer[monsterSound_ChannelIndex].clip = monsterSoundClip;
            mosterSoundPlayer[monsterSound_ChannelIndex].Play();
            break;
        }
    }

    public void Stop_MonsterSound(AudioClip monsterSoundClip)
    {
        for (int index = 0; index < mosterSoundPlayer.Length; index++)
        {
            int loopIndex = (index + monsterSound_ChannelIndex) % mosterSoundPlayer.Length;

            if (mosterSoundPlayer[loopIndex].isPlaying)
            {
                monsterSound_ChannelIndex = loopIndex;
                mosterSoundPlayer[monsterSound_ChannelIndex].Stop();
                break;
            }
        }
    }

    public void Play_SfxSound(SfxSound sfx_Sound, bool useLoop = false)
    {
        for (int index = 0; index < sfxPlayer.Length; index++)
        {
            int loopIndex = (index + sfx_channelIndex) % sfxPlayer.Length;

            if (sfxPlayer[loopIndex].isPlaying)
                continue;

            sfx_channelIndex = loopIndex;
            sfxPlayer[sfx_channelIndex].loop = useLoop;
            sfxPlayer[sfx_channelIndex].clip = sfxClips[(int)sfx_Sound];
            sfxPlayer[sfx_channelIndex].Play();
            break;
        }
    }
    public void Stop_SfxSound(SfxSound sfx_Sound)
    {
        for (int index = 0; index < sfxPlayer.Length; index++)
        {
            int loopIndex = (index + sfx_channelIndex) % sfxPlayer.Length;

            if (sfxPlayer[loopIndex].isPlaying)
                continue;

            sfx_channelIndex = loopIndex;
            sfxPlayer[sfx_channelIndex].Stop();
            break;
        }
    }

}
