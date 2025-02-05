using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Character
{
    public class Player : Character
    {
        [SerializeField] Button interactBtn;

        protected override void Start()
        {
            base.Start();
            interactBtn.onClick.AddListener(delegate {
                OperateWithObject();
            });
        }

        private void Update()
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            SetMoveVector(new Vector3(x, 0, z));
        }
    }
}
