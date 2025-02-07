using Character.Worker;
using CustomSystems;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace RequestManagment
{
    public abstract class Request : IDisposable
    {
        public static Action<Request> OnRequestCreate;
        public Action<Request> OnRequestDestroy;
        protected Vector3 requestPosition;

        public int priority;

        protected bool isPerfoming = false;
        private float lifeTime = 20f;

        public Request(int priority, Vector3 position)
        {
            requestPosition = position;
            this.priority = priority;
            OnRequestCreate?.Invoke(this);
        }

        public abstract void PerfomRequest(DeliveryWorker worker);

        public void Dispose()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();

            OnRequestDestroy?.Invoke(this);
        }

        private void Delete()
        {
            if (isPerfoming) return;
            Dispose();
        }

        ~Request()
        {
            Dispose();
        }
    }
}