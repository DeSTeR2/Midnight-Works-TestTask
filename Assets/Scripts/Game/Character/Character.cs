using CustomSystems;
using InteractObjects;
using System.Collections;
using System.ComponentModel;
using UnityEngine;

namespace Character
{
    public class Character : MonoBehaviour
    {
        [SerializeField] float moveSpeed = 5;
        [SerializeField] float rotationSpeed = 5;

        [Space]
        [SerializeField] Transform carryPoint;
        [SerializeField] CharacterInteractConfig interactConfig;

        private Animator animator;
        CharacterAnimationController animationController;

        Vector3 inputVec;
        bool isPaused;

        IInteractObject carryObject;

        void Awake()
        {
            animator = GetComponent<Animator>();
        }

        protected virtual void Start()
        {
            animationController = new(animator, this);
        }

        protected void SetMoveVector(Vector3 moveVector)
        {
            moveVector.y = 0;
            moveVector.Normalize();

            animationController.Move(moveVector);
            inputVec = moveVector;
        }

        void RotateTowardsMovementDir()
        {
            if (!animationController.IsPause)
            {
                if (inputVec != Vector3.zero)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(inputVec), Time.deltaTime * rotationSpeed);
                }
            }
        }

        public float UpdateMovement()
        {
            if (!animationController.IsPause)
            {
                RotateTowardsMovementDir();
            }

            return inputVec.magnitude * moveSpeed;
        }

        protected void OperateWithObject()
        {
            if (animationController.State == CharacterState.Idle && carryObject == null)
            {
                IInteractObject interact = FindObject<IInteractObject>();
                interact.PickUp();
                carryObject = interact;

                InteractObject obj = interact.GetObject<InteractObject>();

                animationController.PickUp(obj.isInFloor);
                DelaySystem.DelayFunction(delegate
                {
                    ShowItem(obj.gameObject);
                }, .5f);
            }
            else if (animationController.State == CharacterState.Carring && carryObject != null)
            {
                InteractObject obj = carryObject.GetObject<InteractObject>();

                IObjectPlace interact = FindObject<IObjectPlace>();
                animationController.PutDown(interact.IsOnFloor);

                DelaySystem.DelayFunction(delegate
                {
                    interact.PutObject(obj.gameObject);
                    carryObject.PutDown();
                    carryObject = null;
                }, .5f);
            }
        }

        private T FindObject<T>()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position + interactConfig.interactOffset, interactConfig.interactRadius);

            foreach (Collider collider in colliders)
            {
                GameObject obj = collider.gameObject;
                if (obj.TryGetComponent(out T component))
                {
                    Debug.Log($"Found object {obj}");
                    return component;
                }
            }

            throw new System.Exception("Didn't find any objects");
        }


        protected void ShowItem(GameObject item)
        {
            if (item != null)
            {
                item.transform.parent = carryPoint;
                item.transform.localPosition = Vector3.zero;
                item.transform.localRotation = Quaternion.identity;
            }
        }
    }

    public enum CharacterState
    {
        Idle,
        Carring
    };
}