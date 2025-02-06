using Character.Worker;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Request
{
    public abstract class Request
    {
        public static Action<Request> OnRequestCreate;
        protected Vector3 requestPosition;

        public Request(Vector3 position)
        {
            requestPosition = position;
            OnRequestCreate?.Invoke(this);
        }

        public abstract void PerfomRequest(DeliveryWorker worker);
    }
}