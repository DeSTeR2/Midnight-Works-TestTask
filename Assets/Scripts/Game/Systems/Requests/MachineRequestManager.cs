using CustomSystems;
using RequestManagment;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
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
            if (deliveryPlace.gameObject.activeInHierarchy == false) return;

            int requestToCreate = deliveryPlace.RemaintObjectsToFull() - deliveryRequests.Count;
            if (requestToCreate > 0)
            {
                Request req = new DeliveryRequest(receipt.craftFrom.craftItem.ResourceType, deliveryPlace.GetInteractPosition(), receipt.craftFrom.priority);
                RequestCreated(req);
            }
        }

        public void CreateTakeRequest()
        {
            if (takePlace.gameObject.activeInHierarchy == false) return;

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

            request.OnRequestDestroy -= RequestDeleted;
        }

        private async void ManageAllRequests()
        {
            int targetTakeCount = takePlace.ObjectNumber();
            int targetDeliveryCount = deliveryPlace.RemaintObjectsToFull();

            Task takeTask = ManageRequestsAsync(takeRequests, targetTakeCount, CreateTakeRequest);
            Task deliveryTask = ManageRequestsAsync(deliveryRequests, targetDeliveryCount, CreateDeliveryRequest);

            await Task.WhenAll(takeTask, deliveryTask);
        }

        private async Task ManageRequestsAsync<T>(List<T> requests, int numberToCreate, CreateFunction createFunction) where T : Request
        {
            await Task.Factory.StartNew(() =>
            {
                lock (requests)
                {
                    int reqCount = requests.Count;
                    for (int i = 0; i < reqCount; i++)
                    {
                        if (requests.Count > 0 && requests[0] != null)
                        {
                            requests[0].Dispose();
                        }
                    }
                }
            }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());

            await Task.Factory.StartNew(() =>
            {
                for (int i = 0; i < numberToCreate; i++)
                {
                    createFunction.Invoke();
                }
            }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
        }

        ~MachineRequestManager()
        {
            RequesSystem.OnUpdateRequests -= ManageAllRequests;
        }
    }
}