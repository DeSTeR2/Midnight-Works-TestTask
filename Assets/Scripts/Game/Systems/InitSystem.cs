using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace CustomSystems
{
    public class InitSystem : MonoBehaviour
    {
        [SerializeField] List<SoundType> sounds;
        [SerializeField] float minTimeToPlay;
        [SerializeField] float maxTimeToPlay;

        private void Awake()
        {
            SellSystem.InitSystem();
            RequesSystem.InitSystem();
        }

        private void Start()
        {
            PlaySound();
        }

        private void OnDestroy()
        {
            RequesSystem.Delete();
            SellSystem.Delete();
        }

        private async void PlaySound()
        {
            while (true)
            {
                float waitTime = Random.Range(minTimeToPlay, maxTimeToPlay);
                SoundType rng = sounds[Random.Range(0, sounds.Count)];
                await DelaySystem.DelayFunction(delegate {
                    if (this == null) return;
                    SoundManager.instance.PlaySound(rng);
                }, waitTime);
            }
        }
    }
}