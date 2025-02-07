using Character;
using CustomSystems;
using InteractObjects.Place;
using Resources;
using System.Resources;
using UnityEngine;

namespace InteractObjects.Work
{
    public class AcounterWork : WorkObject
    {
        [SerializeField] StorageObject storage;
        [SerializeField] Transform customerPositionsParent;

        CustomerQueue customerQueue;

        protected override void Start()
        {
            base.Start();
            customerQueue = new CustomerQueue(customerPositionsParent);
        }

        public override void AfterWork()
        {
            base.AfterWork();
            Customer customer = customerQueue.RemoveFirstCustomer();
            ResourceType order = customer.Order;

            try
            {
                InteractObject resource = storage.GetResourse(order);
                ResourceSystem.instance.BackObject(resource);
                customer.OrderComplete();
            }
            catch
            {
                customer.OrderCancel();
            }
            CustomerSystem.instance.SendCustomerToEnd(customer);
        }

        public bool AddCustomer(Customer customer)
        {
            return customerQueue.AddCustomer(customer);
        }

    }
}