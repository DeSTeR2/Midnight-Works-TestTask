using System;
using UnityEngine;
using Utils;

namespace Data
{
    [CreateAssetMenu(fileName = "StorageConfig", menuName = "Work/StorageConfig")]
    public class StorageConfig : ScriptableObject, IFile, IChangable
    {
        public StorageData storageData;
        public Action OnConfigChanged;

        public string FileName => $"StorageConfig{name}.json";

        public void Assign<T>(T data) where T : IData
        {
            storageData.Copy(data);
        }

        public void Changed()
        {
            OnConfigChanged?.Invoke();
        }

        public bool Load()
        {
            StorageConfig config = this;
            return FileSystem.Load<StorageData, StorageConfig>(FileName, ref config);  
        }

        public void Save()
        {
            FileSystem.Save(FileName, storageData);
        }
    }

    [Serializable] 
    public class StorageData : IData
    {
        public int capability = 20;

        public void Copy(IData data)
        {
            if (data != null)
            {
                capability = ((StorageData)data).capability;
            }
        }
    }
}