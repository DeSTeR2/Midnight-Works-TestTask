using Character.Worker;
using CustomSystems;
using Resources;
using System;
using UnityEngine;

namespace Request
{
    public class TakeRequest : Request
    {
        public TakeRequest(Vector3 position) : base(position) {}

        public override void PerfomRequest(DeliveryWorker worker)
        {
            worker.AddPosition(requestPosition);
            worker.AddPosition(ResourceSystem.instance.GetStoragePosition());
            worker.StartWork();
        }

        public override string ToString()
        {
            return $"TakeRequest: Pickup at {requestPosition}";
        }
    }
}