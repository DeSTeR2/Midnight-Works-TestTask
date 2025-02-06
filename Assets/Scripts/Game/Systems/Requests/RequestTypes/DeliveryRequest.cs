using Character.Worker;
using CustomSystems;
using Resources;
using UnityEngine;

namespace Request
{
    public class DeliveryRequest : Request
    {
        ResourceType resourceType;

        public DeliveryRequest(ResourceType resourceType, Vector3 position) : base(position)
        {
            this.resourceType = resourceType;
        }

        public override async void PerfomRequest(DeliveryWorker worker)
        {
            Vector3 position = await ResourceSystem.instance.GetResourcePosition(resourceType);

            worker.AddPosition(position);
            worker.AddPosition(requestPosition);
            worker.StartWork();
        }

        public override string ToString()
        {
            return $"DeliverRequest: ResourceType = {resourceType}, Delivery Position = {requestPosition}";
        }
    }
}