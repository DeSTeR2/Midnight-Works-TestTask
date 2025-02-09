using System;
using UnityEngine;
using Utils;

namespace Data
{
    [CreateAssetMenu(fileName = "StorageConfig", menuName = "Work/StorageConfig")]
    public class StorageConfig : ScriptableObject, IFile, IChangable, IGetUpgradeObject
    {
        public StorageData storageData;
        public Action OnConfigChanged;

        public string FileName => $"StorageConfig{name}.json";

        public void Assign<T>(T data) where T : IData
        {
            storageData = new(Changed);
            storageData.Copy(data);
        }

        public void Changed()
        {
            OnConfigChanged?.Invoke();
        }

        public object GetUpgradeObject() => storageData;

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
        public int maxCapability = 242;

        private const int upgradeBy = 20;

        public delegate void OnChange();
        public OnChange onChange;

        public StorageData(OnChange onChange)
        {
            this.onChange = onChange;
        }

        public int UpgradeCapasity(bool watch = false)
        {
            if (capability == maxCapability || watch) return capability;

            if (capability + upgradeBy > maxCapability)
            {
                capability = maxCapability;
            }
            else {
                capability += upgradeBy;
            }

            onChange?.Invoke();

            return capability;
        }

        public void Copy(IData data)
        {
            if (data != null)
            {
                capability = ((StorageData)data).capability;
            }
        }
    }
}