using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Character
{
    public class Player : Character
    {
        [SerializeField] Button interactBtn;
        [SerializeField] 

        protected override void Start()
        {
            base.Start();
            interactBtn.onClick.AddListener(delegate {
                Interact();
            });
        }

        private void Update()
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            SetMoveVector(new Vector3(x, 0, z));
        }

        public void Work()
        {

        }
    }
}
