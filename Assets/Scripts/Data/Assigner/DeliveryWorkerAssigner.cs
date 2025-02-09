using Character.Worker;
using CustomSystems;
using InteractObjects.Work;
using UnityEngine;

namespace Data.Assigner
{
    public class DeliveryWorkerAssigner : MonoBehaviour
    {
        [SerializeField] WorkerSystem workerSystem;
        [SerializeField] DeliveryWorkerConfig workConfig;

        private void Start()
        {
            Assign();
            workConfig.OnConfigChanged += Assign;
        }

        private void Assign()
        {
            workerSystem.AssignWorkers(workConfig.objectData.workerNumber);
        }

        private void OnDestroy()
        {
            workConfig.OnConfigChanged -= Assign;
        }
    }
}