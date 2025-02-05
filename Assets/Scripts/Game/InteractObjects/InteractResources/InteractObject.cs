using UnityEngine;

namespace InteractObjects
{
    public class InteractObject : MonoBehaviour, IInteractObject
    {
        public bool isInFloor;

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
            EmitParticleSystem.instance.Play(ParticleType.PutDownObject, transform.position);
        }
    }
}