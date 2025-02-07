using InteractObjects;
using InteractObjects.Work;
using UnityEngine;
using UnityEngine.UI;

namespace Character
{
    public class Player : Character
    {
        [SerializeField] Button interactBtn;
        [SerializeField] Button dtopBtn;

        [SerializeField] GameObject axe;

        protected override void Start()
        {
            base.Start();
            interactBtn.onClick.AddListener(delegate {
                Interact();
            });

            dtopBtn.onClick.AddListener(delegate {
                DropObject();
            });
        }
        protected void Interact()
        {
            PlaceObject();
            TakeObject();
            Work();
        }
        private void Update()
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            SetMoveVector(new Vector3(x, 0, z));

            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        }

        protected void Work()
        {
            if (!(animationController.State == CharacterState.Idle && carryObject == null)) return;

            WorkObject obj = TryWork<ChopTreeWork>();
            if (obj != null)
            {
                axe.gameObject.SetActive(true);
                WorkAnimation(CharacterAnimations.CoppingTree);
                transform.forward = obj.transform.position;
                return;
            }

            obj = TryWork<MachineWork>();
            if (obj)
            {
                WorkAnimation(CharacterAnimations.MachineWorking);
                transform.forward = obj.transform.position;
                return;
            }
        }

        public override void EndWork()
        {
            base.EndWork();
            axe.gameObject.SetActive(false);
        }
    }
}
