using UnityEngine;

namespace InteractObjects
{
    interface IInteractObject
    {
        T GetObject<T>();
        void PickUp();
        void PutDown();
    }
}