using System;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UIElements;
using Utils;

namespace Data
{
    [CreateAssetMenu(fileName = "WorkObjectConfig", menuName = "Work/WorkObjectConfig")]
    public class WorkObjectConfig : ScriptableObject, IFile, IChangable
    {
        public WorkObjectData objectData = new();
        public Action OnConfigChanged;

        public void Changed() => OnConfigChanged?.Invoke();

        public virtual string FileName => $"WorkObjectConfig{name}.json";

        public void Assign<T>(T data) where T : IData
        {
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
    }

    [Serializable]
    public class WorkObjectData : IData
    {
        public float workTime = 20;

        public int putCapasity = 4;
        public int takeCapasity = 4;

        public bool isActive = false;
        public bool isHaveWorker = false;

        public int putMax = 20;
        public int takeMax = 20;

        public void Copy(IData data)
        {
            if (data != null)
            {
                WorkObjectData dat = (WorkObjectData)data;

                workTime = dat.workTime;
                putCapasity = dat.putCapasity;
                takeCapasity = dat.takeCapasity;
                putMax = dat.putMax;
                takeMax = dat.takeMax;
                isActive = dat.isActive;
                isHaveWorker = dat.isHaveWorker;
            }
        }
    }
}