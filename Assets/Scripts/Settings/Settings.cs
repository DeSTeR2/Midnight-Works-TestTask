using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour {
    [Header("Slider")]
    [SerializeField] Slider _soundSlider;
    [SerializeField] Slider _musicSlider;

    [Header("Mute")]
    [SerializeField] Button _muteButtonSound;
    [SerializeField] GameObject _offSound;
    [SerializeField] GameObject _onSound;

    [SerializeField] Button _muteButtonMusic;
    [SerializeField] GameObject _offMusic;
    [SerializeField] GameObject _onMusic;

    private void Start() {
        _soundSlider.value = SoundManager.GetSoundVolume();
        _musicSlider.value = SoundManager.GetMusicVolume();

        CheckMuteSound(true);
        CheckMuteMusic(true);

        _soundSlider.onValueChanged.AddListener(SoundChange);
        _musicSlider.onValueChanged.AddListener(MusicChange);

        _muteButtonSound.onClick.AddListener(MuteSound);
        _muteButtonMusic.onClick.AddListener(MuteMusic);
    }

    private void SoundChange(float volume) {
        SoundManager.instance.ChangeSoundVolume(volume);
    }

    private void MusicChange(float volume) {
        SoundManager.instance.ChangeMusicVolume(volume);
    }

    private void MuteSound() {
        SoundManager.instance.ChangeMuteSound();
        CheckMuteSound();
    }

    private void CheckMuteSound(bool start = false) {
        if (start) {
            if (SoundManager.GetMuteSound() == true) {
                _onSound.SetActive(false);
                _offSound.SetActive(true);
            } else {
                _onSound.SetActive(true);
                _offSound.SetActive(false);
            }
        } else {
            if (SoundManager.GetMuteSound() == true) {
                _onSound.SetActive(false);
                _offSound.SetActive(true);
            } else {
                _onSound.SetActive(true);
                _offSound.SetActive(false);
            }
        }
    }

    private void MuteMusic()
    {
        SoundManager.instance.ChangeMuteSound();
        CheckMuteMusic();
    }

    private void CheckMuteMusic(bool start = false)
    {
        if (start)
        {
            if (SoundManager.GetMuteMusic() == true)
            {
                _onMusic.SetActive(false);
                _offMusic.SetActive(true);
            }
            else
            {
                _onMusic.SetActive(true);
                _offMusic.SetActive(false);
            }
        }
        else
        {
            if (SoundManager.GetMuteMusic() == true)
            {
                _onMusic.SetActive(false);
                _offMusic.SetActive(true);
            }
            else
            {
                _onMusic.SetActive(true);
                _offMusic.SetActive(false);
            }
        }
    }
}
