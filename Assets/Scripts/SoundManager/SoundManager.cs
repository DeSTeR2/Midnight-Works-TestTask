using Data;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] Sound[] sounds;

    [Header("Audio sourses")]
    [SerializeField] AudioSource _sound;
    [SerializeField] AudioSource _music;

    [Space]
    [SerializeField] SoundDataConfig soundData;

    public static Action OnSoundVolumeChange;
    public static Action OnMusicVolumeChange;
    public static SoundManager instance;

    Dictionary<SoundType, Sound> _audios = new Dictionary<SoundType, Sound>();
    Dictionary<SoundType, float> _timer = new Dictionary<SoundType, float>();

    public float GetSoundVolume() {
        return soundData.objectData.soundVolume;
    }
    public float GetMusicVolume()
    {
        return soundData.objectData.musicVolume;
    }

    private void Awake() {
        if (instance == null) { 
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
    }

    private void Start() {
        foreach (var sound in sounds) {
            _audios.Add(sound.type, sound);
        }
    }

    public void PlaySound(SoundType type) {
        if (_audios.ContainsKey(type) == false) return;

        if (_timer.ContainsKey(type)) {
            float time = _timer[type];

            if (_audios[type].audio == null) {
                Debug.LogError($"Non audio. Type {type.ToString()}");
                return;
            }

            if (time + _audios[type].timeToPlayAgain <= Time.time) {
                _sound.PlayOneShot(_audios[type].audio);
                _timer[type] = Time.time;
            }
        } else {
            _sound.PlayOneShot(_audios[type].audio);
            _timer.Add(type, Time.time);
        }
    }

    public void ChangeSoundVolume(float volume) {
        _sound.volume = volume;
        soundData.objectData.soundVolume = volume;

        OnSoundVolumeChange?.Invoke();
    }

    public void ChangeMusicVolume(float volume) {
        _music.volume = volume;
        soundData.objectData.musicVolume = volume;

        OnMusicVolumeChange?.Invoke();
    }
}
