using System;
using UnityEngine;
using Utils;

namespace Data
{
    [CreateAssetMenu(fileName = "BaseWorkConfig", menuName = "Work/BaseConfig")]
    public class BaseWorkConfig : ScriptableObject, IFile, IChangable
    {
        public BaseData objectData = new();

        public Action OnConfigChanged;

        public virtual string FileName => $"BaseWorkConfig{name}.json";

        public void Changed() => OnConfigChanged?.Invoke();

        public void Assign<T>(T data) where T : IData
        {
            objectData.Copy(data);
        }

        public bool Load()
        {
            BaseWorkConfig work = this;
            return FileSystem.Load<BaseData, BaseWorkConfig>(FileName, ref work);
        }

        public void Save()
        {
            FileSystem.Save(FileName, objectData);
        }
    }

    [Serializable]
    public class BaseData : IData
    {
        public float workTime = 20;

        public bool isActive = false;
        public bool isHaveWorker = false;

        public virtual void Copy(IData data)
        {
            if (data != null)
            {
                BaseData dat = (BaseData)data;
                workTime = dat.workTime;
                isActive = dat.isActive;
                isHaveWorker = dat.isHaveWorker;
            }
        }
    }
}