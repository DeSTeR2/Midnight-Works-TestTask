using System;
using System.Collections.Generic;
using UnityEngine;

namespace Character.Worker
{
    public class DeliveryWorker : AIWalkable
    {
        Queue<Vector3> positionsToGo = new();

        public static Action<DeliveryWorker> OnWorkerFree;

        protected override async void CompletePath()
        {
            base.CompletePath();
            if (await TakeObject())
            {
                NextPosition();
                return;
            }

            if (await PlaceObject())
            {
                NextPosition();
                return;
            } else
            {
                if (animationController.State == CharacterState.Carring)
                {
                    DropObject();
                    NextPosition();
                }
            }
        }

        public void AddPosition(Vector3 position) { 
            positionsToGo.Enqueue(position);
        }

        public void StartWork()
        {
            NextPosition();
        }

        private void NextPosition()
        {
            if (positionsToGo.Count == 0)
            {
                OnWorkerFree?.Invoke(this);
                return;
            }

            Vector3 position = positionsToGo.Dequeue();
            AssignWalkTarget(position);
        }
    }
}