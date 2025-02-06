using Resources;
using UnityEngine;

namespace InteractObjects
{
    public class InteractObject : MonoBehaviour, IInteractObject
    {
        [SerializeField] ResourceConfig config;
        public bool isInFloor;

        Resource resource;

        public ResourceType ResourceType { get => config.resourceType; }

        private void OnEnable()
        {
            gameObject.GetComponent<BoxCollider>().enabled = true;   
        }

        private void Start()
        {
            resource = new Resource(config.resourceType, config);
        }

        public T GetObject<T>()
        {
            if (this is T t)
            {
                return t;
            }
            else
            {
                throw new System.Exception($"Cannot cast {nameof(InteractObject)} to type {typeof(T)}");
            }
        }

        public void PickUp()
        {
            EmitParticleSystem.instance.Play(ParticleType.PickObject,transform.position);
        }

        public void PutDown()
        {
            gameObject.transform.parent = null;
            gameObject.transform.rotation = Quaternion.Euler(0, gameObject.transform.rotation.eulerAngles.y, 0);
            EmitParticleSystem.instance.Play(ParticleType.PutDownObject, transform.position + new Vector3(0, 1,0));
        }
    }
}