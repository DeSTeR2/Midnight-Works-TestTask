using CustomSystems;
using InteractObjects.Work;
using System;
using UnityEngine;

namespace Character.Worker
{
    public class MachineWorker : AIWalkable
    {
        [SerializeField] GameObject workObject;

        MachineWork work;

        protected override void Start()
        {
            base.Start();
            AssignWalkTarget(workObject);
        }

        protected override void CompletePath()
        {
            base.CompletePath();
            ReWork();
        }

        private async void ReWork()
        {
            await DelaySystem.DelayFunction(delegate { }, .5f);

            isWorking = false;
            if (work != null)
            {
                work.onEndWork -= ReWork;
            }

            work = TryWork<MachineWork>();

            if (work != null && work.isWorking)
            {
                transform.forward = work.transform.position - transform.position;
                work.onEndWork += ReWork;
                animationController.WorkAnimation(CharacterAnimations.MachineWorking, true);
            }
            else
            {
                DelaySystem.DelayFunction(ReWork, 3f);
            }
        }
    }
}