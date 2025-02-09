using CustomSystems;
using Data;
using RequestManagment;
using Resources;
using System;
using UnityEngine;

namespace InteractObjects.Work
{
    public class MachineWork : WorkObject
    {
        [SerializeField] CraftingReceipt receipt;
        [SerializeField] WorkObjectConfig workObjectConfig;

        [Space]
        [SerializeField] MachineObjectPut putPlace;
        [SerializeField] MachineObjectTake takePlace;

        int awailableItems = 0;

        public static Action<ResourceType> OnAwailableResource;

        MachineRequestManager requestManager;

        private void Awake()
        {
            workObjectConfig.OnConfigChanged += UpdateObject;
        }

        protected override void Start()
        {
            requestManager = new MachineRequestManager(takePlace, putPlace, receipt);

            if (workObjectConfig.objectData.isActive || haveToBeActive)
            {
                OnAwailableResource?.Invoke(receipt.craftResult.craftItem.ResourceType);
                OnAwailableResource?.Invoke(receipt.craftFrom.craftItem.ResourceType);
            }

            base.Start();
            putPlace.SetResource(receipt.craftFrom.craftItem.ResourceType);
            UpdateObject();
        }

        protected override void UpdateObject()
        {
            workTime = workObjectConfig.objectData.workTime;
            UpdateWorkTime();

            if (workObjectConfig.objectData.isActive || haveToBeActive) Open();
            else
            {
                gameObject.SetActive(false);
            }

            character.gameObject.SetActive(workObjectConfig.objectData.isHaveWorker);

            putPlace.SetCapability(workObjectConfig.objectData.putCapasity);
            takePlace.SetCapability(workObjectConfig.objectData.takeCapasity);
        }

        protected override void Open()
        {
            if (gameObject.activeInHierarchy) return;
            gameObject.SetActive(true);
        }

        public override void Work(bool isWork, Character.Character character)
        {
            if (putPlace.ObjectNumber() >= receipt.craftFrom.count && isWork && workingCharacter == null)
            {
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

        public void SetPutPlace(int capasity) => putPlace.SetCapability(capasity);
        public void SetTakePlace(int capasity) => takePlace.SetCapability(capasity);

        private void OnDestroy()
        {
            workStatus.OnLoadEnd -= AfterWork;
            workObjectConfig.OnConfigChanged -= UpdateObject;
        }

    }
}