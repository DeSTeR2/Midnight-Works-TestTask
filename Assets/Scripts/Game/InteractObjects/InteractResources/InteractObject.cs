using CustomSystems;
using Resources;
using Unity.VisualScripting;
using UnityEngine;

namespace InteractObjects
{
    public class InteractObject : MonoBehaviour, IInteractObject
    {
        [SerializeField] ResourceConfig config;
        public bool isInFloor;

        Resource resource;
        new BoxCollider collider;

        public ResourceType ResourceType { get => config.resourceType; }
        public bool IsOnObjectPlace = false;

        ObjectPlace objectPlace;

        private void OnEnable()
        {
            gameObject.GetComponent<BoxCollider>().enabled = true;   
        }

        private void Start()
        {
            collider = GetComponent<BoxCollider>();
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

        public void PlaceOnObjectPlace(ObjectPlace objectPlace)
        {
            this.objectPlace = objectPlace;
            IsOnObjectPlace = true;
        }

        public void PickUp()
        {
            EmitParticleSystem.instance.Play(ParticleType.PickObject,transform.position);
            collider.enabled = false;
            collider.isTrigger = true;

            if (IsOnObjectPlace)
            {
                objectPlace.TookResource(ResourceType);
            }

            IsOnObjectPlace = false;
            objectPlace = null;
        }

        public void PutDown(bool backToQueue)
        {
            if (backToQueue)
            {
                ResourceSystem.instance.BackResourceToQueue(this);
            }

            collider.enabled = true;
            collider.isTrigger = false;
            gameObject.transform.parent = null;
            gameObject.transform.rotation = Quaternion.Euler(0, gameObject.transform.rotation.eulerAngles.y, 0);
            EmitParticleSystem.instance.Play(ParticleType.PutDownObject, transform.position + new Vector3(0, 1,0));
        }
    }
}