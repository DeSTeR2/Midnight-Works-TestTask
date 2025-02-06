using CustomSystems;
using InteractObjects.Work;
using System;
using UnityEngine;

namespace Character.Worker
{
    public class LumberjackWorker : Worker
    {
        [SerializeField] GameObject axe;

        ChopTreeWork work;

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
                DelaySystem.DelayFunction(ReWork, 3f);
            }
        }

        public override void EndWork()
        {
            base.EndWork();
            axe.SetActive(false);
        }
    }
}