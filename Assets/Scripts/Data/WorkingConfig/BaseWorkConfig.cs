using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements.Experimental;
using Utils;

namespace Data
{
    [CreateAssetMenu(fileName = "BaseWorkConfig", menuName = "Work/BaseConfig")]
    public class BaseWorkConfig : ScriptableObject, IFile, IChangable, IGetUpgradeObject
    {
        public BaseData objectData;

        public Action OnConfigChanged;

        public virtual string FileName => $"BaseWorkConfig{name}.json";

        public void Changed() => OnConfigChanged?.Invoke();

        public void Assign<T>(T data) where T : IData
        {
            objectData = new(Changed);
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

        public object GetUpgradeObject() => objectData;
    }

    [Serializable]
    public class BaseData : IData, IBaseDataUpgrade
    {
        public int workTime = 20;
        public int upgradeNumber = 0;

        public bool isActive = false;
        public bool isHaveWorker = false;

        private const int upgradeBy = 2;

        public delegate void OnChange();
        public OnChange onChange;

        public BaseData(OnChange onChange)
        {
            this.onChange = onChange;
        }

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

        public int BuyWorker(bool watch = false)
        {
            if (watch) return (isHaveWorker == true ? 1 : 0);

            isHaveWorker = true;
            onChange?.Invoke();

            return 1;
        }

        public bool BuyStation(bool watch = false)
        {
            if (watch) return isActive;

            isActive = true;
            onChange?.Invoke();

            return true;
        }

        public int UpgradeTime(bool watch = false)
        {
            if (watch) return workTime;

            if (upgradeNumber < 5)
            {
                upgradeNumber++;
                workTime -= upgradeBy;
                onChange?.Invoke();
            }

            return workTime;
        }

        public bool IsUpradeTimeFull()
        {
            return upgradeNumber == 5;
        }
    }
}