using CustomSystems;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

namespace Character.Worker
{
    public class AIWalkable : Character
    {
        [SerializeField] float distToStop = 0.15f;

        NavMeshAgent agent;

        bool isPathCompleted = false;
        bool isAllowedToMove = false;

        protected override void Start()
        {
            base.Start();
            agent = GetComponent<NavMeshAgent>();
        }

        private void FixedUpdate()
        {
            if (agent.destination == null || isPathCompleted || !isAllowedToMove)
            {
                SetMoveVector(Vector3.zero);
                return;
            }

            Vector3 moveDir = agent.steeringTarget - transform.position;
            SetMoveVector(moveDir);


            if (agent.remainingDistance < distToStop)
            {
                CompletePath();
            }
        }

        protected virtual void CompletePath()
        {
            SetMoveVector(Vector3.zero);
            isPathCompleted = true;
        }

        public void AssignWalkTarget(Vector3 position)
        {
            agent.SetDestination(position);
            isPathCompleted = false;

            DelaySystem.DelayFunction(delegate {
                isAllowedToMove = true;
            }, .5f);
        }

        public void AssignWalkTarget(GameObject target)
        {
            AssignWalkTarget(target.transform.position);
        }
    }
}