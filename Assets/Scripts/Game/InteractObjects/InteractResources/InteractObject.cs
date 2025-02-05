﻿using Resources;
using UnityEngine;

namespace InteractObjects
{
    public class InteractObject : MonoBehaviour, IInteractObject
    {
        [SerializeField] ResourceConfig config;
        public bool isInFloor;

        Resource resource;

        public ResourceType ResourceType { get => config.resourceType; }

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
            EmitParticleSystem.instance.Play(ParticleType.PutDownObject, transform.position);
        }
    }
}