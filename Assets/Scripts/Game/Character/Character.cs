using CustomSystems;
using InteractObjects;
using InteractObjects.Work;
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
        bool isWorking = false;

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

        private void PlaceObject()
        {
            if (animationController.State == CharacterState.Carring && carryObject != null && isWorking == false)
            {
                InteractObject obj = carryObject.GetObject<InteractObject>();
                try
                {
                    IObjectPlace interact = FindObject<IObjectPlace>();
                    if (interact.CanPlace(obj.ResourceType) == false) return;

                    animationController.PutDown(interact.IsOnFloor);

                    DelaySystem.DelayFunction(delegate
                    {
                        interact.PutObject(obj.gameObject);
                        carryObject.PutDown();
                        carryObject = null;
                    }, .5f);
                }
                catch { }
            }
        }

        private void TakeObject()
        {
            if (animationController.State == CharacterState.Idle && carryObject == null && isWorking == false)
            {
                try
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
                } catch { }
            }
        }
        
        protected void Work()
        {
            if (!(animationController.State == CharacterState.Idle && carryObject == null)) return;

            if (TryWork<ChopTreeWork>())
            {
                WorkAnimation(CharacterAnimations.CoppingTree);
                return;
            }
            if (TryWork<MachineWork>())
            {
                WorkAnimation(CharacterAnimations.MachineWorking); 
                return;
            }
        }

        private bool TryWork<T>() where T : IWorkPlace
        {
            try
            {
                T work = FindObject<T>();
                isWorking = !isWorking;
                Debug.Log($"Found work object. Working state: {isWorking}");
                work.Work(isWorking, this);
                return true;
            } catch
            {
                return false;
            }
        }

        public void EndWork()
        {
            isWorking = false;
            WorkAnimation(CharacterAnimations.IsWorking);
            WorkAnimation(CharacterAnimations.CoppingTree);
            WorkAnimation(CharacterAnimations.MachineWorking);
        }

        protected void WorkAnimation(string animName)
        {
            animationController.WorkAnimation(animName, isWorking);
        }

        protected void Interact()
        {
            PlaceObject();
            TakeObject();
            Work();
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