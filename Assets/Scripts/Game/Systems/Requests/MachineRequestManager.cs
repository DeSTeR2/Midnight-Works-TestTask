using CustomSystems;
using RequestManagment;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace InteractObjects.Work
{
    public class MachineRequestManager
    {
        List<TakeRequest> takeRequests = new();
        List<DeliveryRequest> deliveryRequests = new();

        MachineObjectPlace takePlace;
        MachineObjectPlace deliveryPlace;
        CraftingReceipt receipt;

        delegate void CreateFunction();

        public MachineRequestManager(MachineObjectPlace takePlace, MachineObjectPlace putPlace, CraftingReceipt receipt) {
            this.takePlace = takePlace;
            this.deliveryPlace = putPlace;
            this.receipt = receipt;

            RequesSystem.OnUpdateRequests += ManageAllRequests;
        }

        public void RequestCreated(Request request)
        {
            if (request is TakeRequest)
            {
                takeRequests.Add((TakeRequest)request);
            }
            else if (request is DeliveryRequest) {
                deliveryRequests.Add((DeliveryRequest)request);
            }

            request.OnRequestDestroy += RequestDeleted;
        }

        public void CreateDeliveryRequest()
        {
            int requestToCreate = deliveryPlace.RemaintObjectsToFull() - deliveryRequests.Count;
            if (requestToCreate > 0)
            {
                Request req = new DeliveryRequest(receipt.craftFrom.craftItem.ResourceType, deliveryPlace.GetInteractPosition(), receipt.craftFrom.priority);
                RequestCreated(req);
            }
        }

        public void CreateTakeRequest()
        {
            int requestToCreate = takePlace.ObjectNumber() - takeRequests.Count;
            if (requestToCreate > 0)
            {
                Request req = new TakeRequest(receipt.craftResult.priority, takePlace.GetInteractPosition());
                RequestCreated(req);
            }
        }

        private void RequestDeleted(Request request)
        {
            if (request is TakeRequest)
            {
                takeRequests.Remove((TakeRequest)request);
            }
            else if (request is DeliveryRequest)
            {
                deliveryRequests.Remove((DeliveryRequest)request);
            }

            ManageAllRequests();
            request.OnRequestDestroy -= RequestDeleted;
        }

        private void ManageAllRequests()
        {
            ManageRequests(takeRequests, takePlace.ObjectNumber(), CreateTakeRequest);
            ManageRequests(deliveryRequests, deliveryPlace.RemaintObjectsToFull(), CreateDeliveryRequest);
        }

        private void ManageRequests<T>(List<T> requests, int numberToCreate, CreateFunction createFunction) where T : Request
        {
            int reqCount = requests.Count;
            for (int i=0; i< reqCount; i++)
            {
                requests[0].Dispose();
                requests.RemoveAt(0);
            }

            for (int i=0; i<numberToCreate;i++) {
                createFunction.Invoke();
            }
        }

        private void TakeManagment(int remainObjects, List<TakeRequest> requests)
        {
            if (remainObjects > requests.Count)
            {
                CreateTakeRequest();
            }
            else if (remainObjects < requests.Count)
            {
                int removeRequests = requests.Count - remainObjects;
                for (int i = 0; i < Mathf.Min(removeRequests, requests.Count); i++)
                {
                    Request req = requests[0];
                    req.Dispose();
                }
            }
        }

        private void DeliveryManagment(int remainToFull, List<DeliveryRequest> requests)
        {
            if (remainToFull > requests.Count)
            {
                CreateDeliveryRequest();
            }
            else if (remainToFull < requests.Count)
            {
                int removeRequests = requests.Count - remainToFull;
                for (int i = 0; i < Mathf.Min(removeRequests, requests.Count); i++)
                {
                    Request req = requests[0];
                    req.Dispose();
                }
            }
        }

        ~MachineRequestManager()
        {
            RequesSystem.OnUpdateRequests -= ManageAllRequests;
        }
    }
}