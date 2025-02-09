using System;
using UnityEngine;
using Utils;

namespace Data
{
    [CreateAssetMenu(fileName = "SoundDataConfig", menuName = "SoundDataConfig/SoundDataConfig")]
    public class SoundDataConfig : ScriptableObject, IFile, IChangable
    {
        public SoundData objectData = new();

        public Action OnConfigChanged;

        public virtual string FileName => $"SoundDataConfig{name}.json";

        public void Changed() => OnConfigChanged?.Invoke();

        public void Assign<T>(T data) where T : IData
        {
            objectData.Copy(data);
        }

        public bool Load()
        {
            SoundDataConfig work = this;
            return FileSystem.Load<SoundData, SoundDataConfig>(FileName, ref work);
        }

        public void Save()
        {
            FileSystem.Save(FileName, objectData);
        }
    }

    [Serializable]
    public class SoundData : IData
    {
        public float soundVolume = 1f;
        public float musicVolume = 1f;

        public virtual void Copy(IData data)
        {
            if (data != null)
            {
                SoundData dat = (SoundData)data;
                soundVolume = dat.soundVolume;
                musicVolume = dat.musicVolume;
            }
        }
    }
}