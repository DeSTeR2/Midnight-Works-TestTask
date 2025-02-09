using CustomSystems;
using InteractObjects.Work;
using System;
using UnityEngine;

namespace Character.Worker
{
    public class LumberjackWorker : StationaryWorker
    {
        [SerializeField] GameObject axe;

        protected async override void ReWork()
        {
            await Waiter();
            base.ReWork();
            work = TryWork<ChopTreeWork>();

            if (work != null && work.isWorking)
            {
                transform.forward = work.transform.position - transform.position;
                axe.SetActive(true);
                work.onEndWork += ReWork;
                animationController.WorkAnimation(CharacterAnimations.CoppingTree, true);
            } else
            {
                axe.SetActive(true);
                await DelaySystem.DelayFunction(ReWork, 3f);
            }
        }

        public override void EndWork()
        {
            base.EndWork();
            axe.SetActive(false);
        }
    }
}