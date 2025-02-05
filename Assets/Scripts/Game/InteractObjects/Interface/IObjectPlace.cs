using Resources;
using UnityEngine;

namespace InteractObjects
{
    interface IObjectPlace
    {
        bool IsOnFloor { get; }
        void PutObject(GameObject go);
        bool CanPlace(ResourceType resourceType);
    }
}