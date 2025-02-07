using Resources;
using UnityEngine;

namespace InteractObjects.Work
{
    public class MachineObjectPlace : ObjectPlace
    {
        [SerializeField] Transform interactPosition;

        public Vector3 GetInteractPosition() => interactPosition.position;
    }
}