using Resources;
using System;
using UnityEngine;

namespace InteractObjects.Work
{
    public class MachineObjectPut : MachineObjectPlace
    {
        public Action OnObjectPlace;

        ResourceType requaredResource;

        public void SetResource(ResourceType requaredResource) => this.requaredResource = requaredResource;

        public override void PutObject(GameObject go)
        {
            base.PutObject(go);
            go.GetComponent<BoxCollider>().enabled = false;
            OnObjectPlace?.Invoke();
        }

        public override bool CanPlace(ResourceType type)
        {
            return base.CanPlace(type) && type == requaredResource;
        }

        public void RemoveObjects(int objectNumber)
        {
            placeStrategy.RemoveObjects(objectNumber);
        }
    }
}