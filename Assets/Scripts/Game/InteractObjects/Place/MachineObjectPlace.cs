using UnityEngine;

namespace InteractObjects.Work
{
    public class MachineObjectPlace : ObjectPlace
    {
        protected override void Start()
        {
            base.Start();
            gameObject.GetComponent<BoxCollider>().enabled = false;

            Debug.LogWarning("TODO: crafting!");
        }
    }
}