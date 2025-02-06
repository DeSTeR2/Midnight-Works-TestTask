using CustomSystems;
using InteractObjects.Work;

namespace Character.Worker
{
    public class MachineWorker : Worker
    {
        MachineWork work;

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