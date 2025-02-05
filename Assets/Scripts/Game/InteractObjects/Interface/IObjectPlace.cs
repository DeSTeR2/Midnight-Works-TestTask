using UnityEngine;

namespace InteractObjects
{
    interface IObjectPlace
    {
        bool IsOnFloor { get; }
        void PutObject(GameObject go);
    }
}