using Character.Worker;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace CustomSystems
{
    public class WorkerSystem : MonoBehaviour
    {
        public List<DeliveryWorker> workerList;

        Queue<DeliveryWorker> freeWorkers = new();

        public static WorkerSystem instance;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            foreach (var worker in workerList) { 
                freeWorkers.Enqueue(worker);
            }

            DeliveryWorker.OnWorkerFree += BackWorker;
        }

        public async Task<DeliveryWorker> GetWorker()
        {
            while (freeWorkers.Count == 0) {
                await Task.Delay((int)(1000));
            }

            return freeWorkers.Dequeue();
        }

        private void BackWorker(DeliveryWorker worker)
        {
            freeWorkers.Enqueue(worker);
        }

        private void OnDestroy()
        {
            DeliveryWorker.OnWorkerFree -= BackWorker;
        }
    }
}