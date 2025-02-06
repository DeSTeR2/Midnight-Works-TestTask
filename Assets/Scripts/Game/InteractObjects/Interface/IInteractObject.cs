using UnityEngine;

namespace InteractObjects
{
    public interface IInteractObject
    {
        T GetObject<T>();
        void PickUp();
        void PutDown();
    }
}