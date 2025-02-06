using CustomSystems;
using Request;
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

        protected override void Start()
        {
            base.Start();
            putPlace.SetResource(receipt.craftFrom.craftItem.ResourceType);
            putPlace.OnObjectPlace += PutObject;
        }

        private void PutObject()
        {
            awailableItems++;
        }

        public override void Work(bool isWork, Character.Character character)
        {
            if (awailableItems >= receipt.craftFrom.count && isWork && workingCharacter == null)
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
                    DeliveryRequest(putPlace.RemaintObjectsToFull());
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
                TakeRequest(receipt.craftResult.count);
            }
            base.AfterWork();
        }

        private async void TakeRequest(int objectNumber)
        {
            for (int i = 0; i < objectNumber; i++)
            {
                await DelaySystem.DelayFunction(delegate {
                    new TakeRequest(takePlace.transform.position);
                }, .5f);
            }
        }

        private async void DeliveryRequest(int objectNumber)
        {
            if (!deliveryRequest)
            {
                deliveryRequest = true;
                for (int i = 0; i < objectNumber; i++)
                {
                    await DelaySystem.DelayFunction(delegate {
                        new DeliveryRequest(receipt.craftFrom.craftItem.ResourceType, putPlace.transform.position);
                    }, .5f);
                }
            }
        }

        private void OnDestroy()
        {
            putPlace.OnObjectPlace -= PutObject;
        }
    }
}