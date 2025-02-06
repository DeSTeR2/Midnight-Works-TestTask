using System;

namespace InteractObjects.Work
{
    public interface IWorkPlace
    {
        float WorkTime { get; }
        void Work(bool isWork, Character.Character character);
        void AfterWork();
    }
}