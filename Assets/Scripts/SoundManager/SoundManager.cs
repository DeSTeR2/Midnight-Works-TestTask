using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] Sound[] sounds;

    [Header("Audio sourses")]
    [SerializeField] AudioSource _sound;
    [SerializeField] AudioSource _music;

    [Space]
    [SerializeField] float _playSoundIn;

    public static Action OnSoundVolumeChange;
    public static Action OnMusicVolumeChange;
    public static SoundManager instance;

    private static string _saveSound = "Sound";
    private static string _saveMusic = "Music";
    private static string _saveMuteSound = "MuteSound";
    private static string _saveMuteMusic = "MuteMusic";

    Dictionary<SoundType, Sound> _audios = new Dictionary<SoundType, Sound>();
    Dictionary<SoundType, float> _timer = new Dictionary<SoundType, float>();

    public static float GetSoundVolume() {
        return PlayerPrefs.GetFloat(_saveSound, 1f);
    }
    public static float GetMusicVolume() {
        return PlayerPrefs.GetFloat(_saveMusic, 1f);
    }
    public static bool GetMuteSound()
    {
        return PlayerPrefs.GetInt(_saveMuteSound, 1) == 1;
    }

    public static bool GetMuteMusic()
    {
        return PlayerPrefs.GetInt(_saveMuteMusic, 1) == 1;
    }

    private void Awake() {
        instance = this;
    }

    private void Start() {
        foreach (var sound in sounds) {
            _audios.Add(sound.type, sound);
        }

        if (GetMuteSound() == true)
        {
            MuteSound();
        }
        else
        {
            UnMuteSound();
        }

        if (GetMuteMusic() == true)
        {
            MuteMusic();
        }
        else
        {
            UnMuteMusic();
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
        PlayerPrefs.SetFloat(_saveSound, volume);
        PlayerPrefs.Save();

        OnSoundVolumeChange?.Invoke();
    }

    public void ChangeMusicVolume(float volume) {
        _music.volume = volume;
        PlayerPrefs.SetFloat(_saveMusic, volume);
        PlayerPrefs.Save();

        OnMusicVolumeChange?.Invoke();
    }

    private void MuteSound() {
        _sound.mute = true;

        PlayerPrefs.SetInt(_saveMuteSound, 1);
        PlayerPrefs.Save();
    }

    private void UnMuteSound() {
        _sound.mute = false;

        PlayerPrefs.SetInt(_saveMuteSound, 0);
        PlayerPrefs.Save();
    }

    public void ChangeMuteSound() {
        if (PlayerPrefs.GetInt(_saveMuteSound, 0) == 1) {
            UnMuteSound();
        } else {
            MuteSound();
        }
    }


    private void MuteMusic()
    {
        _music.mute = true;

        PlayerPrefs.SetInt(_saveMuteMusic, 1);
        PlayerPrefs.Save();
    }

    private void UnMuteMusic()
    {
        _music.mute = false;

        PlayerPrefs.SetInt(_saveMuteMusic, 0);
        PlayerPrefs.Save();
    }

    public void ChangeMuteMusic()
    {
        if (PlayerPrefs.GetInt(_saveMuteMusic, 0) == 1)
        {
            UnMuteMusic();
        }
        else
        {
            MuteMusic();
        }
    }
}
