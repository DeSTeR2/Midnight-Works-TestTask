using CustomSystems;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace Character.Worker
{
    public class AIWalkable : Character
    {
        [SerializeField] float distToStop = 0.15f;

        NavMeshAgent agent;

        bool isPathCompleted = false;
        bool isAllowedToMove = false;
        Vector3 moveTarget;

        public Action<AIWalkable> OnPathComplete; 

        protected override void Start()
        {
            base.Start();
            agent = GetComponent<NavMeshAgent>();

            GlobalTicker.OnTick += OnTick;
        }

        protected virtual void OnTick()
        {
            if (agent.destination == null || isPathCompleted || !isAllowedToMove || moveTarget == Vector3.zero)
            {
                SetMoveVector(Vector3.zero);
                return;
            }

            Vector3 moveDir = agent.steeringTarget - transform.position;
            SetMoveVector(moveDir);


            if ((transform.position - moveTarget).sqrMagnitude < distToStop)
            {
                CompletePath();
            }
        }

        protected virtual void CompletePath()
        {
            SetMoveVector(Vector3.zero);
            isPathCompleted = true;
            OnPathComplete?.Invoke(this);
        }

        public async Task AssignWalkTarget(Vector3 position)
        {
            if (this == null) return;

            if (agent == null)
            {
                Start();
            }

            agent.SetDestination(position);
            moveTarget = position;
            isPathCompleted = false;

            await DelaySystem.DelayFunction(delegate {
                isAllowedToMove = true;
            }, .5f);
        }

        public async Task AssignWalkTarget(GameObject target)
        {
            await AssignWalkTarget(target.transform.position);
        }

        private void OnDestroy()
        {
            GlobalTicker.OnTick += OnTick;
        }
    }
}