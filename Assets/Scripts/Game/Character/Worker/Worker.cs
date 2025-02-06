using CustomSystems;
using UnityEngine;
using UnityEngine.AI;

namespace Character.Worker
{
    public class Worker : Character
    {
        [SerializeField] float distToStop = 0.1f;

        NavMeshAgent agent;
        public GameObject go;

        bool isPathCompleted = false;
        bool isAllowedToMove = false;

        protected override void Start()
        {
            base.Start();
            agent = GetComponent<NavMeshAgent>();

            AssignWalkTarget(go);
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

        public void AssignWalkTarget(GameObject target)
        {
            agent.SetDestination(target.transform.position);
            isPathCompleted = false;

            DelaySystem.DelayFunction(delegate { 
                isAllowedToMove = true;
            }, .5f);
        }
    }
}