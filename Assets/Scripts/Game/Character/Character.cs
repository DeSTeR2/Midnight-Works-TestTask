using CustomSystems;
using InteractObjects;
using InteractObjects.Work;
using System.Threading.Tasks;
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

        protected virtual void Awake()
        {
            animator = GetComponent<Animator>();
        }

        protected virtual void Start()
        {
            animationController = new(animator, this);
            UpdateCarryPoint();
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

        protected async Task<bool> PlaceObject()
        {
            if (animationController.State == CharacterState.Carring && carryObject != null && isWorking == false)
            {
                InteractObject obj = carryObject.GetObject<InteractObject>();
                try
                {
                    IObjectPlace interact = FindObject<IObjectPlace>();
                    if (interact.CanPlace(obj.ResourceType) == false) return false;

                    animationController.PutDown(interact.IsOnFloor);

                    await DelaySystem.DelayFunction(delegate
                    {
                        carryObject.PutDown(false);
                        interact.PutObject(obj);
                        carryObject = null;
                    }, .5f);
                    return true;
                }
                catch { }
            }
            return false;
        }

        protected async Task<bool> TakeObject()
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
                catch { }
            }
            return false;
        }

        protected T TryWork<T>() where T : IWorkPlace
        {
            try
            {
                T work = FindObject<T>();
                isWorking = !isWorking;
                work.Work(isWorking, this);
                return work;
            }
            catch
            {
                return default;
            }
        }

        protected async void DropObject()
        {
            if (carryObject != null && animationController.State == CharacterState.Carring)
            {
                animationController.PutDown(true);

                await DelaySystem.DelayFunction(delegate
                {
                    carryObject.PutDown(true);
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

        protected T FindObject<T>()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position + interactConfig.interactOffset, interactConfig.interactRadius);

            for (int i = 0; i < colliders.Length; i++)
            {
                Collider col = colliders[i];
                GameObject obj = col.gameObject;
                if (obj.TryGetComponent(out T component))
                {
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

        protected void UpdateCarryPoint()
        {
            if (leftHand == null || rightHand == null || carryObject == null) return;

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