using CustomSystems;
using InteractObjects.Work;
using System;
using UnityEngine;

namespace Character.Worker
{
    public class MachineWorker : StationaryWorker
    {
        protected override async void ReWork()
        {
            await Waiter();
            base.ReWork();

            isWorking = false;
            if (work != null)
            {
                work.onEndWork -= ReWork;
            }

            work = TryWork<MachineWork>();

            if (work != null && work.isWorking)
            {
                transform.LookAt(work.transform);
                Quaternion rotation = transform.rotation;
                rotation = Quaternion.Euler(0, rotation.y, 0);
                transform.rotation = rotation;  

                work.onEndWork += ReWork;
                animationController.WorkAnimation(CharacterAnimations.MachineWorking, true);
            }
            else
            {
                await DelaySystem.DelayFunction(ReWork, 3f);
            }
        }
    }
}