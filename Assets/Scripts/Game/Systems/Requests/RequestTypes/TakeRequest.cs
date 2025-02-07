using Character.Worker;
using CustomSystems;
using InteractObjects.Work;
using InteractObjects.Work.Actions;
using UnityEngine;

namespace RequestManagment
{
    public class TakeRequest : Request
    {
        public TakeRequest(int priority, Vector3 position) : base(priority, position) {}

        public override void PerfomRequest(DeliveryWorker worker)
        {
            worker.AddAction(new Action(requestPosition, ActionType.Take));
            worker.AddAction(new Action(ResourceSystem.instance.GetStoragePosition(), ActionType.Place));
            worker.StartWork(this);
            isPerfoming = true;
        }

        public override string ToString()
        {
            return $"TakeRequest: Pickup at {requestPosition}";
        }
    }
}