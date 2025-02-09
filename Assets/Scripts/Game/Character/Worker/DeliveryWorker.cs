using System;
using System.Collections.Generic;
using UnityEngine;
using InteractObjects.Work;
using Resources;
using CustomSystems;
using InteractObjects;
using System.Threading.Tasks;
using InteractObjects.Place;
using RequestManagment;
namespace Character.Worker
{
    public class DeliveryWorker : AIWalkable
    {
        [SerializeField] float timeToBeFree = 10;
        Queue<InteractObjects.Work.Actions.Action> actionsToGo = new();

        public string requestType;
        public bool isFree = true;

        public static Action<DeliveryWorker> OnWorkerFree;

        InteractObjects.Work.Actions.Action currentAction;
        Request currentRequest;

        int redoTimes = 0;
        float time = 0;

        protected override async void CompletePath()
        {
            base.CompletePath();
            if (currentAction.actionType == ActionType.Take && await TakeObject(currentAction.resourceType))
            {
                NextPosition();
                return;
            }
            else
            if (currentAction.actionType == ActionType.Place && await PlaceObject())
            {
                NextPosition();
                return;
            }

            ReDoAction();
        }

        protected override void OnTick()
        {
            base.OnTick();
            UpdateCarryPoint();

            if (isFree && currentAction == null)
            {
                time += Time.fixedDeltaTime;

                if (time >= timeToBeFree)
                {
                    WorkerFree();
                }
            } else
            {
                isFree = false;
            }
        }

        public void AddAction(InteractObjects.Work.Actions.Action action)
        {
            actionsToGo.Enqueue(action);
        }

        public void StartWork(RequestManagment.Request req)
        {
            DropObject();
            NextPosition();
            requestType = req.ToString();
            currentRequest = req;
            isFree = false;
        }

        private async void ReDoAction()
        {
            redoTimes++;
            if (redoTimes == 4)
            {
                currentRequest.Dispose();
                WorkerFree();
                return;
            }

            await AssignWalkTarget(currentAction.position);
        }

        private async void NextPosition()
        {
            if (actionsToGo.Count == 0)
            {
                currentAction = null;
                WorkerFree();
                return;
            }

            currentAction = actionsToGo.Dequeue();
            Vector3 position = currentAction.position;
            await AssignWalkTarget(position);
        }

        private void WorkerFree()
        {
            redoTimes = 0;
            time = 0;
            isFree = true;
            requestType = string.Empty;
            currentAction = null;
            actionsToGo.Clear();
            WorkerSystem.instance.BackWorker(this);
        }

        protected async Task<bool> TakeObject(ResourceType resourceType)
        {
            if (resourceType == ResourceType.None)
            {
                return await base.TakeObject();
            }
            else
            if (animationController.State == CharacterState.Idle && carryObject == null && isWorking == false)
            {
                try
                {
                    StorageObject interact = FindObject<StorageObject>();
                    InteractObject obj = interact.GetResourse(resourceType);
                    carryObject = obj;

                    animationController.PickUp(obj.isInFloor);
                    Vector3 rotate = obj.transform.position - transform.position;

                    transform.forward = rotate;
                    Quaternion rot = transform.rotation;
                    rot = Quaternion.Euler(0, rot.eulerAngles.y, 0);
                    transform.rotation = rot;


                    await DelaySystem.DelayFunction(delegate
                    {
                        ShowItem(obj.gameObject);
                    }, .5f);

                    return true;
                }
                catch
                {
                    return await base.TakeObject();
                }
            }
            return false;
        }
    }


}