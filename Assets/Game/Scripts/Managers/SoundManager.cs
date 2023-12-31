﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AudioConfig
{
    public enum AudioType
    {
        SFX,
        BGM
    };

    public AudioType Type;
    public AudioClip Clip;
}

public class SoundManager : MonoBehaviour
{
#if UNITY_IOS
    [DllImport("__Internal")]
    private static extern void _playVibrate();
#endif
    [Header("Sound Configs")]
    [SerializeField] protected AudioSource _sampleSoundSource;

    [Header("Common - SFXs")]
    [SerializeField] protected AudioClip _sfxTapButton;

    [Header("Gameplay - SFXs")]
    [SerializeField] protected AudioClip _sfxAddObject;
    [SerializeField] protected AudioClip _sfxThrowObject;
    [SerializeField] protected AudioClip _sfxLevelComplete;
    [SerializeField] protected AudioClip[] _sfxCombos;

    [Header("BGMs")]
    [SerializeField] protected AudioClip _bgmHome;
    [SerializeField] protected AudioClip[] _bgmGameplay;

    const string KEY_SFX = "SFX";
    const string KEY_BGM = "BGM";
    const string KEY_VIBRATE = "VIBRATE";

    bool _isSfxOn;
    bool _isBgmOn;
    bool _isVibrate;

    private float _sfxVolume = 1.0f;
    private float _bgmVolume = 1.0f;

    Sequence _seqChangeSound = null;

    ObjectPool<Transform> _poolSounds;
    List<AudioSource> _activeSources = new List<AudioSource>();

    List<AudioConfig> _soundConfigs = new List<AudioConfig>();

    public event System.Action<bool> OnEnableMusic;

    public static SoundManager Instance { get; private set; }

    public int CurrentBackground = 0;

    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);

        _poolSounds = new ObjectPool<Transform>(_sampleSoundSource.transform);
    }

    public void LoadSoundSettings()
    {
        IsSfxOn = PlayerPrefs.GetInt(KEY_SFX, 1) > 0 ? true : false;
        IsBgmOn = PlayerPrefs.GetInt(KEY_BGM, 1) > 0 ? true : false;
        IsVibrateOn = PlayerPrefs.GetInt(KEY_VIBRATE, 1) > 0 ? true : false;
    }

    public void ToggleSfx()
    {
        IsSfxOn = !_isSfxOn;
    }

    public void ToggleBGM()
    {
        IsBgmOn = !_isBgmOn;
    }

    public void ToggleVibrate()
    {
        IsVibrateOn = !_isVibrate;
    }

    public bool IsSfxOn
    {
        get
        {
            return _isSfxOn;
        }

        set
        {
            if (value != _isSfxOn)
            {
                _isSfxOn = value;
                PlayerPrefs.SetInt(KEY_SFX, _isSfxOn ? 1 : 0);
                PlayerPrefs.Save();
            }

#if USE_ANDROID_NATIVE_AUDIO && !UNITY_EDITOR
            AndroidNativeAudio.Off = !_isSfxOn;
#else
            setVolumeSFXs(_isSfxOn ? 1f : 0f);
#endif
        }
    }

    public bool IsBgmOn
    {
        get
        {
            return _isBgmOn;
        }

        set
        {
            if (value != _isBgmOn)
            {
                _isBgmOn = value;
                PlayerPrefs.SetInt(KEY_BGM, _isBgmOn ? 1 : 0);
                PlayerPrefs.Save();
            }

#if USE_ANDROID_NATIVE_AUDIO && !UNITY_EDITOR
            AndroidNativeAudio.Off = !_isBgmOn;
#else
            setVolumeBGMs(_isBgmOn ? 1f : 0f);
#endif
            OnEnableMusic?.Invoke(_isBgmOn);

            if (!value)
                StopBGMs();

            //GoogleMobileAds.Api.MobileAds.SetApplicationVolume(_isSfxOn ? 1f : 0f);
        }
    }

    public bool IsVibrateOn
    {
        get
        {
            return _isVibrate;
        }

        set
        {
            if (value != _isVibrate)
            {
                _isVibrate = value;
                PlayerPrefs.SetInt(KEY_VIBRATE, _isVibrate ? 1 : 0);
                PlayerPrefs.Save();
            }
            //Taptic.tapticOn = _isVibrate;
        }
    }

    void setVolumeBGMs(float volume)
    {
        _bgmVolume = volume;
        foreach (var source in _activeSources)
            if (source.loop)
                source.volume = _bgmVolume;
    }

    void setVolumeSFXs(float volume)
    {
        _sfxVolume = volume;
        foreach (var source in _activeSources)
            if (!source.loop)
                source.volume = _sfxVolume;
    }

    void playSound(AudioConfig soundConfig)
    {
        if (soundConfig.Type == AudioConfig.AudioType.BGM && isPlayingSameBGM(soundConfig))
            return;

        var audioSource = _poolSounds.Get();
        audioSource.parent = _sampleSoundSource.transform.parent;
        var audioCom = audioSource.GetComponent<AudioSource>();
        StartCoroutine(playSound(audioCom, soundConfig));
    }

    IEnumerator playSound(AudioSource source, AudioConfig config)
    {
        _soundConfigs.Add(config);
        source.clip = config.Clip;
        source.loop = config.Type == AudioConfig.AudioType.BGM;
        source.volume = (config.Type == AudioConfig.AudioType.SFX) ? _sfxVolume : _bgmVolume;

        if (config.Type == AudioConfig.AudioType.BGM)
        {
            StopBGMs();
        }
        _activeSources.Add(source);
        source.Play();

        yield return new WaitForSeconds(0.1f);
        while (source.isPlaying)
            yield return null;

        source.Stop();
        _soundConfigs.Remove(config);
        _activeSources.Remove(source);
        _poolSounds.Store(source.transform);
    }

    private bool isPlayingSameBGM(AudioConfig newConfig)
    {
        foreach (var source in _activeSources)
        {
            if (source.loop && source.isPlaying)
            {
                return source.clip == newConfig.Clip;
            }
        }
        return false;
    }

    public void StopBGMs()
    {
        foreach (var item in _activeSources)
        {
            if (item.loop && item.isPlaying)
            {
                item.Stop();
            }
        }
    }

    public void PauseBGMs()
    {
        foreach (var item in _activeSources)
        {
            if (item.loop)
            {
                item.volume = 0.0f;
            }
        }
    }

    public void ResumeBGMs()
    {
        foreach (var item in _activeSources)
        {
            if (item.loop)
            {
                item.volume = 1.0f;
            }
        }
    }

    public void StopSFXs()
    {
        foreach (var item in _activeSources)
        {
            if (!item.loop && item.isPlaying)
            {
                item.Stop();
            }
        }
    }

    public void PlayVibrate()
    {
        if(!IsVibrateOn) return;
        
#if UNITY_IOS && !UNITY_EDITOR
        _playVibrate();
#elif UNITY_ANDROID && !UNITY_EDITOR
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
        
        AndroidJavaClass vibrationEffectClass = new AndroidJavaClass("android.os.VibrationEffect");
        if (vibrationEffectClass != null)
        {
            long milliseconds = 40;
            int amplitude = 60;
            var parameters = new object[] { milliseconds, amplitude };
            AndroidJavaObject vibrationEffect = vibrationEffectClass.CallStatic<AndroidJavaObject>("createOneShot", parameters);
            vibrator.Call("vibrate", vibrationEffect);
        }
#else
        Debug.Log("Play Vibrate");
        Handheld.Vibrate();
#endif
    }

    void TestVibration()
    {
        Handheld.Vibrate();
    }

    public void PlaySfxTapButton()
    {
        playSound(new AudioConfig()
        {
            Clip = _sfxTapButton,
            Type = AudioConfig.AudioType.SFX
        });
    }

    /// <summary>
    /// GAMEPLAY
    /// </summary>

    public void PlaySfxAddedObject()
    {
        playSound(new AudioConfig()
        {
            Clip = _sfxAddObject,
            Type = AudioConfig.AudioType.SFX
        });
    }

    public void PlaySfxThrowObject()
    {
        playSound(new AudioConfig()
        {
            Clip = _sfxThrowObject,
            Type = AudioConfig.AudioType.SFX
        });
    }

    public void PlaySfxCombo(int index)
    {
        if (index < 0)
            index = 0;
        else if (index >= _sfxCombos.Length)
            index = _sfxCombos.Length - 1;

        playSound(new AudioConfig()
        {
            Clip = _sfxCombos[index],
            Type = AudioConfig.AudioType.SFX
        });
    }

    public void PlaySfxLevelComplete()
    {
        playSound(new AudioConfig()
        {
            Clip = _sfxLevelComplete,
            Type = AudioConfig.AudioType.SFX
        });
    }

    /// <summary>
    /// BGM
    /// </summary>
    public void PlayBgmHome()
    {
        // if (!IsBgmOn) return;
        // playSound(new AudioConfig()
        // {
        //     Clip = _bgmHome,
        //     Type = AudioConfig.AudioType.BGM
        // });
    }

    public void RandomeBGMGameplay()
    {
        // CurrentBackground = Random.Range(0, _bgmGameplay.Length);
    }

    public void PlayBgmGameplay()
    {
        // if (!IsBgmOn) return;

        // playSound(new AudioConfig()
        // {
        //     Clip = _bgmGameplay[CurrentBackground],
        //     Type = AudioConfig.AudioType.BGM
        // });
    }
}
