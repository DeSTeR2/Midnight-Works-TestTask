using System;
using UnityEngine;
using Utils;

namespace Data
{
    [CreateAssetMenu(fileName = "WorkObjectConfig", menuName = "Work/WorkObjectConfig")]
    public class WorkObjectConfig : ScriptableObject, IFile, IChangable, IGetUpgradeObject
    {
        public WorkObjectData objectData;
        public Action OnConfigChanged;

        public void Changed() => OnConfigChanged?.Invoke();

        public virtual string FileName => $"WorkObjectConfig{name}.json";

        public void Assign<T>(T data) where T : IData
        {
            objectData = new(Changed);
            objectData.Copy(data);
        }

        public bool Load()
        {
            WorkObjectConfig work = this;
            return FileSystem.Load<WorkObjectData, WorkObjectConfig>(FileName, ref work);
        }

        public void Save()
        {
            FileSystem.Save(FileName, objectData);
        }

        public object GetUpgradeObject() => objectData;
    }

    [Serializable]
    public class WorkObjectData : IData, IWorkObjectUpgrade
    {
        public int workTime = 20;
        public int upgradeNumber = 0;

        public int putCapasity = 4;
        public int takeCapasity = 4;

        public bool isActive = false;
        public bool isHaveWorker = false;

        private const int putMax = 20;
        private const int takeMax = 20;

        private const int upgradeTimeBy = 2;
        private const int upgradeCapasityBy = 4;

        public delegate void OnChange();
        public OnChange onChange;

        public WorkObjectData(OnChange onChange)
        {
            this.onChange = onChange;
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
                workTime -= upgradeTimeBy;
                onChange?.Invoke();
            }

            return workTime;
        }

        public void Copy(IData data)
        {
            if (data != null)
            {
                WorkObjectData dat = (WorkObjectData)data;

                workTime = dat.workTime;
                putCapasity = dat.putCapasity;
                takeCapasity = dat.takeCapasity;
                isActive = dat.isActive;
                isHaveWorker = dat.isHaveWorker;
            }
        }

        public int UpgradePutCapasity(bool watch = false)
        {
            if (watch) return putCapasity;

            if (putCapasity < putMax)
            {
                putCapasity += upgradeCapasityBy;
                onChange?.Invoke();
            }

            return putCapasity;
        }

        public int UpgradeTakeCapasity(bool watch = false)
        {
            if (watch) return takeCapasity;

            if (takeCapasity < putMax)
            {
                takeCapasity += upgradeCapasityBy;
                onChange?.Invoke();
            }
            return takeCapasity;
        }

        public bool IsUpgradeTakeCapasityFull()
        {
            return takeCapasity == takeMax;
        }

        public bool IsUpradeTimeFull()
        {
            return upgradeNumber == 5;
        }

        public bool IsUpgradePutCapasityFull()
        {
            return putCapasity == putMax;
        }
    }
}