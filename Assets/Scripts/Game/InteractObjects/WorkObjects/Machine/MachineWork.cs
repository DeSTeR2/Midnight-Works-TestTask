using CustomSystems;
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
                this.workingCharacter = character;
                workStatus.StartLoad(loadSystem);
                isWorking = true;

                Debug.Log("Start work");
            }
            else
            {
                isWorking = false;
                this.workingCharacter = null;
                workStatus.EndLoad();

                Debug.Log("End work");
            }
        }

        public override void AfterWork()
        {
            if (takePlace.AddObject(receipt.craftResult))
            {
                awailableItems -= receipt.craftFrom.count;
                putPlace.RemoveObjects(receipt.craftFrom.count);
            }
            base.AfterWork();
        }

        private void OnDestroy()
        {
            putPlace.OnObjectPlace -= PutObject;
        }
    }
}