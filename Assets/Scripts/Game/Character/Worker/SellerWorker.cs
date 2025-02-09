using CustomSystems;
using InteractObjects.Work;
using static UnityEditor.IMGUI.Controls.PrimitiveBoundsHandle;

namespace Character.Worker
{
    public class SellerWorker : StationaryWorker
    {
        protected async override void ReWork()
        {
            base.ReWork();
            work = TryWork<AcounterWork>();

            if (work != null && work.isWorking)
            {
                transform.forward = work.transform.position - transform.position;
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