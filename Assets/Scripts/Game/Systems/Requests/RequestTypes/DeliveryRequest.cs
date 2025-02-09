using Character.Worker;
using CustomSystems;
using Resources;
using UnityEngine;
using InteractObjects.Work.Actions;
using InteractObjects.Work;

namespace RequestManagment
{
    public class DeliveryRequest : Request
    {
        ResourceType resourceType;

        public DeliveryRequest(ResourceType resourceType, Vector3 position, int priority) : base(priority, position)
        {
            this.resourceType = resourceType;
        }

        public override async void PerfomRequest(DeliveryWorker worker)
        {
            Vector3 position = await ResourceSystem.instance.GetResourcePosition(resourceType);

            worker.AddAction(new Action(position, ActionType.Take, resourceType));
            worker.AddAction(new Action(requestPosition, ActionType.Place));
            worker.StartWork(this);
            isPerfoming = true;
        }
    }
}