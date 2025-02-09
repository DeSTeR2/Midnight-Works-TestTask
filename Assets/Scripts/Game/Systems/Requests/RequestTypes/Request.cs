﻿using Character.Worker;
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

        public Request(int priority, Vector3 position)
        {
            requestPosition = position;
            this.priority = priority;
            OnRequestCreate?.Invoke(this);
        }

        public abstract void PerfomRequest(DeliveryWorker worker);

        public void Dispose()
        {
            OnRequestDestroy?.Invoke(this);

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        ~Request()
        {
            Dispose();
        }
    }
}