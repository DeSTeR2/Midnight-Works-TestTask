using Character.Worker;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace CustomSystems
{
    public class WorkerSystem : MonoBehaviour
    {
        [SerializeField] Transform deliveryWorkerParent;

        List<DeliveryWorker> workerList = new();
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
            for (int i = 0; i < workerList.Count; i++)
            {
                workerList[i].gameObject.SetActive(false);
            }
        }

        public void AssignWorkers(int workerCount)
        {
            for (int i = 0; i < workerCount; i++) { 
                if (workerList[i].gameObject.activeSelf == false)
                {
                    workerList[i].gameObject.SetActive(true);
                    freeWorkers.Enqueue(workerList[i]);
                }
            }
        }

        public async Task<DeliveryWorker> GetWorker()
        {
            DeliveryWorker worker = null;
            while (freeWorkers.Count == 0 || worker == null) {
                await Task.Delay((int)(1000));
                if (freeWorkers.Count != 0) { 
                    worker = freeWorkers.Dequeue();
                    if (worker.requestType == string.Empty)
                    {
                        break;
                    }
                    else worker = null;
                }
            }

            return worker;
        }

        public void BackWorker(DeliveryWorker worker)
        {
            freeWorkers.Enqueue(worker);
        }
    }
}