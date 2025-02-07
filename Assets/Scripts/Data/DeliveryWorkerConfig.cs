using System;
using UnityEngine;
using Utils;

namespace Data
{
    [CreateAssetMenu(fileName = "DeliveryWorkerConfig", menuName = "Work/DeliveryWorkerConfig")]
    public class DeliveryWorkerConfig : ScriptableObject, IFile, IChangable
    {
        public DeliverConfig objectData = new();

        public Action OnConfigChanged;

        public virtual string FileName => $"DeliveryWorkerConfig{name}.json";

        public void Changed() => OnConfigChanged?.Invoke();

        public void Assign<T>(T data) where T : IData
        {
            objectData.Copy(data);
        }

        public bool Load()
        {
            DeliveryWorkerConfig work = this;
            return FileSystem.Load<DeliverConfig, DeliveryWorkerConfig>(FileName, ref work);
        }

        public void Save()
        {
            FileSystem.Save(FileName, objectData);
        }
    }

    [Serializable]
    public class DeliverConfig : IData
    {
        public int workerNumber = 20;

        public virtual void Copy(IData data)
        {
            if (data != null)
            {
                DeliverConfig dat = (DeliverConfig)data;
                workerNumber = dat.workerNumber;
            }
        }
    }
}