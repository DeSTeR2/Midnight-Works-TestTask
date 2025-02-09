using System;
using UnityEngine;
using Utils;

namespace Data
{
    [CreateAssetMenu(fileName = "DeliveryWorkerConfig", menuName = "Work/DeliveryWorkerConfig")]
    public class DeliveryWorkerConfig : ScriptableObject, IFile, IChangable, IGetUpgradeObject
    {
        public DeliverConfig objectData;

        public Action OnConfigChanged;

        public virtual string FileName => $"DeliveryWorkerConfig{name}.json";

        public void Changed() => OnConfigChanged?.Invoke();

        public void Assign<T>(T data) where T : IData
        {
            objectData = new(Changed);
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

        public object GetUpgradeObject() => objectData;
    }

    [Serializable]
    public class DeliverConfig : IData, IBuyWorker
    {
        public int workerNumber = 0;

        private const int maxWorker = 20;

        public delegate void OnChange();
        public OnChange onChange;

        public DeliverConfig(OnChange onChange) => this.onChange = onChange;

        public virtual void Copy(IData data)
        {
            if (data != null)
            {
                DeliverConfig dat = (DeliverConfig)data;
                workerNumber = dat.workerNumber;
            }
        }

        public int BuyWorker(bool watch = false)
        {
            if (watch) return 1;

            if (workerNumber < maxWorker)
            {
                workerNumber++;
                onChange?.Invoke();
                return 1;
            }
            return 0;
        }

        public bool IsUpradeTimeFull()
        {
            return workerNumber == maxWorker;
        }
    }
}