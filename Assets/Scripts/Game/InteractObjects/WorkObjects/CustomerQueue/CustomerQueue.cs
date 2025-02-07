using Character;
using CustomSystems;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace InteractObjects.Work
{
    public class CustomerQueue
    {
        List<Transform> positions = new();
        List<Customer> customers = new();

        public static Action OnQueueNotFull;

        public CustomerQueue(Transform positionsParent)
        {
            for (int i = 0; i < positionsParent.childCount; i++) { 
                positions.Add(positionsParent.GetChild(i));
            }

            CheckIfQueueIfFull();
        }

        public bool AddCustomer(Customer customer)
        {
            if (customers.Count == positions.Count) return false;
            customers.Add(customer);
            customer.gameObject.SetActive(true);
            MoveCustomers();

            return true;
        }

        public Customer RemoveFirstCustomer() {
            Customer customer = customers[0];

            customers.RemoveAt(0);
            MoveCustomers();
            return customer;
        }

        public async void CheckIfQueueIfFull()
        {
            while (true)
            {
                await DelaySystem.DelayFunction(delegate { }, .5f);

                if (customers.Count < positions.Count)
                {
                    OnQueueNotFull?.Invoke();
                }
            }
        }
        private async void MoveCustomers()
        {
            for (int i = 0; i < customers.Count; i++) {
                await customers[i].AssignWalkTarget(positions[i].gameObject);
            }
        }
    }
}