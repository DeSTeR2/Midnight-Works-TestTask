using Character;
using Character.Worker;
using InteractObjects.Work;
using System.Collections.Generic;
using UnityEngine;

namespace CustomSystems
{
    public class CustomerSystem : MonoBehaviour {
        [SerializeField] List<AcounterWork> acounters;
        [SerializeField] Customer customer;

        [Space]
        [SerializeField] Transform startPosition;
        [SerializeField] Transform endPosition;

        ObjectPool<Customer> pool;

        public static CustomerSystem instance;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            pool = new ObjectPool<Customer>();
            pool.OnPoolIsEmpty += SpawnCustomer;
            CustomerQueue.OnQueueNotFull += AssignCustomerToAcounter;
        }

        public async void SendCustomerToEnd(Customer customer) {
            await customer.AssignWalkTarget(endPosition.gameObject);
            customer.OnPathComplete += BackCustomer;
        }

        private void BackCustomer(AIWalkable customer) { 
            customer.OnPathComplete -= BackCustomer;
            customer.gameObject.SetActive(false);
            pool.AddObject((Customer)customer);
        }

        private void AssignCustomerToAcounter()
        {
            Customer customer = pool.GetObject();
            customer.transform.position = startPosition.position;
            for (int i = 0; i < acounters.Count; i++) { 
                if (acounters[i].gameObject.activeInHierarchy)
                {
                    if (acounters[i].AddCustomer(customer))
                    {
                        return;
                    }
                }
            }
            pool.AddObject(customer);
        }

        private void SpawnCustomer(ObjectPool<Customer> pool)
        {
            Customer customer = Instantiate(this.customer);
            customer.gameObject.SetActive(false);
            pool.AddObject(customer);
        }

        private void OnDestroy()
        {
            pool.OnPoolIsEmpty -= SpawnCustomer;
            CustomerQueue.OnQueueNotFull -= AssignCustomerToAcounter;
        }
    }
}