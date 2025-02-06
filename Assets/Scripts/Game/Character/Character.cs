using CustomSystems;
using InteractObjects;
using InteractObjects.Work;
using UnityEngine;

namespace Character
{
    public class Character : MonoBehaviour
    {
        [SerializeField] float moveSpeed = 5;
        [SerializeField] float rotationSpeed = 5;

        [Space]
        [SerializeField] Transform carryPoint;
        [SerializeField] Transform leftHand;
        [SerializeField] Transform rightHand;
        [SerializeField] CharacterInteractConfig interactConfig;

        private Animator animator;
        protected CharacterAnimationController animationController;

        Vector3 inputVec;
        protected bool isWorking = false;

        protected IInteractObject carryObject;

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

        protected void RotateTowards(Vector3 dir)
        {
            //if (!animationController.IsPause)
            //{
            Debug.Log($"Rotate {dir}");
            if (dir != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * rotationSpeed);
            }
            //}
        }

        public float UpdateMovement()
        {
            if (!animationController.IsPause)
            {
                RotateTowards(inputVec);
            }

            return inputVec.magnitude * moveSpeed;
        }

        protected void PlaceObject()
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
                        carryObject.PutDown();
                        interact.PutObject(obj.gameObject);
                        carryObject = null;
                    }, .5f);
                }
                catch { }
            }
        }

        protected void TakeObject()
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
                }
                catch { }
            }
        }

        protected T TryWork<T>() where T : IWorkPlace
        {
            try
            {
                T work = FindObject<T>();
                isWorking = !isWorking;
                Debug.Log($"Found work object. Working state: {isWorking}");
                work.Work(isWorking, this);
                return work;
            }
            catch
            {
                return default;
            }
        }

        protected void DropObject()
        {
            Debug.Log(carryObject + " " + animationController.State);
            if (carryObject != null && animationController.State == CharacterState.Carring)
            {
                animationController.PutDown(true);

                DelaySystem.DelayFunction(delegate
                {
                    carryObject.PutDown();
                    carryObject = null;
                }, .55f);
            }
        }

        public virtual void EndWork()
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

        private void LateUpdate()
        {
            UpdateCarryPoint();
        }

        private void UpdateCarryPoint()
        {
            Vector3 position = (leftHand.transform.position + rightHand.transform.position) / 2;
            carryPoint.position = position;
        }
    }

    public enum CharacterState
    {
        Idle,
        Carring
    };
}