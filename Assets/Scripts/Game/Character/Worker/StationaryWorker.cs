using CustomSystems;
using InteractObjects.Work;
using System.Threading.Tasks;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.AI;

namespace Character.Worker
{
    public class StationaryWorker : AIWalkable
    {
        [SerializeField] WorkObject workOblect;

        protected WorkObject work;

        Vector3 workPosition;

        protected override void Start()
        {
            base.Start();
        }

        private void OnEnable()
        {
            AssignWalkTarget(workOblect.GetWorkPosition());
        }

        protected override void CompletePath()
        {
            base.CompletePath();
            ReWork();

            workPosition = transform.position;
            MakeItObstacle();
        }

        private async void UpdatePosition()
        {
            while (true)
            {
                await DelaySystem.DelayFunction(delegate { 
                    if (this != null) transform.position = workPosition;
                }, 0.5f);
            }
        }

        private void MakeItObstacle()
        {
            gameObject.GetComponent<NavMeshAgent>().enabled = false;
            NavMeshObstacle obstacle = gameObject.AddComponent<NavMeshObstacle>();
            obstacle.carving = true;
            obstacle.carveOnlyStationary = false;

            UpdatePosition();
        }

        protected virtual void ReWork() {
            isWorking = false;
            if (work != null)
            {
                work.onEndWork -= ReWork;
            }
        }

        protected async Task Waiter()
        {
            await DelaySystem.DelayFunction(delegate { }, .5f);
        }
    }
}