using Resources;
using UnityEngine;

namespace InteractObjects
{
    interface IObjectPlace
    {
        bool IsOnFloor { get; }
        void PutObject(InteractObject go);
        bool CanPlace(ResourceType resourceType);
    }
}