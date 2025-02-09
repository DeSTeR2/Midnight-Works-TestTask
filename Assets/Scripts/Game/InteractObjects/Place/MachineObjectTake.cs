using CustomSystems;
using Resources;
using System;
using UnityEngine;

namespace InteractObjects.Work
{
    public class MachineObjectTake : MachineObjectPlace
    {
        public override bool CanPlace(ResourceType type)
        {
            return false;
        }

        public bool AddObject(CraftSetting settings)
        {
            int addNumber = settings.count;
            if (placeStrategy.CanPlace(addNumber) == false) return false;
            ResourceType type = settings.craftItem.ResourceType;

            for (int i = 0; i < addNumber; i++) {
                InteractObject resource = ResourceSystem.instance.RequestSpawnResource(type, Vector3.zero, Quaternion.identity, IsOnFloor);
                placeStrategy.Place(resource.gameObject);
                resource.isInFloor = true;
            }

            return true;
        }
    }
}