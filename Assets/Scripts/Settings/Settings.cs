using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour {
    [Header("Slider")]
    [SerializeField] Slider _soundSlider;
    [SerializeField] Slider _musicSlider;

    [Space]
    [SerializeField] Button _closeBtn;

    private void Start() {
        _soundSlider.value = SoundManager.instance.GetSoundVolume();
        _musicSlider.value = SoundManager.instance.GetMusicVolume();

        _soundSlider.onValueChanged.AddListener(SoundChange);
        _musicSlider.onValueChanged.AddListener(MusicChange);
    }

    private void SoundChange(float volume) {
        SoundManager.instance.ChangeSoundVolume(volume);
    }

    private void MusicChange(float volume) {
        SoundManager.instance.ChangeMusicVolume(volume);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void Open()
    {
        gameObject.SetActive(true);
    }
}
