using CustomSystems;
using RequestManagment;
using Resources;
using System;
using UnityEngine;

namespace InteractObjects.Work
{
    public class MachineWork : WorkObject
    {
        [SerializeField] CraftingReceipt receipt;

        [Space]
        [SerializeField] MachineObjectPut putPlace;
        [SerializeField] MachineObjectTake takePlace;

        int awailableItems = 0;
        bool deliveryRequest = false;

        public static Action<ResourceType> OnAwailableResource;
        public int remainToFull;
        public int objectToTake;

        MachineRequestManager requestManager;

        protected override void Start()
        {
            requestManager = new MachineRequestManager(takePlace, putPlace, receipt);

            OnAwailableResource?.Invoke(receipt.craftResult.craftItem.ResourceType);
            OnAwailableResource?.Invoke(receipt.craftFrom.craftItem.ResourceType);

            base.Start();
            putPlace.SetResource(receipt.craftFrom.craftItem.ResourceType);
        }

        private void Update()
        {
            remainToFull = putPlace.RemaintObjectsToFull();
            objectToTake = takePlace.ObjectNumber();
        }

        public override void Work(bool isWork, Character.Character character)
        {
            if (putPlace.ObjectNumber() >= receipt.craftFrom.count && isWork && workingCharacter == null)
            {
                deliveryRequest = false;
                this.workingCharacter = character;
                workStatus.StartLoad(loadSystem);
                isWorking = true;
            }
            else
            {
                if (awailableItems < receipt.craftFrom.count)
                {
                    DeliveryRequest();
                }

                isWorking = false;
                this.workingCharacter = null;
                workStatus.EndLoad();
            }
        }

        public override void AfterWork()
        {
            if (takePlace.AddObject(receipt.craftResult))
            {
                awailableItems -= receipt.craftFrom.count;
                putPlace.RemoveObjects(receipt.craftFrom.count);
                TakeRequest();
            }
            base.AfterWork();
        }

        public async void TakeRequest()
        {
            await DelaySystem.DelayFunction(delegate
            {
                requestManager.CreateTakeRequest();
            }, .5f);

        }

        public async void DeliveryRequest()
        {
            await DelaySystem.DelayFunction(delegate
            {
                requestManager.CreateDeliveryRequest();
            }, .5f);
        }
    }
}