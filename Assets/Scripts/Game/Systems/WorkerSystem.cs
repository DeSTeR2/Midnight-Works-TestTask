using Character.Worker;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace CustomSystems
{
    public class WorkerSystem : MonoBehaviour
    {
        [SerializeField] Transform deliveryWorkerParent;

        List<DeliveryWorker> workerList;
        Queue<DeliveryWorker> freeWorkers = new();

        public static WorkerSystem instance;

        private void Awake()
        {
            instance = this;
            for (int i = 0; i < deliveryWorkerParent.childCount; i++)
            {
                DeliveryWorker worker = deliveryWorkerParent.GetChild(i).GetComponent<DeliveryWorker>();
                workerList.Add(worker);
                worker.gameObject.SetActive(false);
            }
        }

        private void Start()
        {
            DeliveryWorker.OnWorkerFree += BackWorker;
        }

        public void AssignWorkers(int workerCount)
        {
            for (int i = 0; i < workerList.Count; i++) { 
                if (workerList[i].gameObject.activeInHierarchy == false)
                {
                    workerList[i].gameObject.SetActive(true);
                    freeWorkers.Enqueue(workerList[i]);
                }
            }
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