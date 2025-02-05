using CustomSystems;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace InteractObjects.Work
{
    public class ChopTreeWork : WorkObject, IWorkPlace
    {
        [SerializeField] float spawnRadius;

        public override void AfterWork()
        {
            base.AfterWork();
            Vector3 position = GetRngPosition();
            Quaternion rotation = GetRngRotation();
            ResourceSystem.instance.RequestSpawnResource(resourceType, position, rotation, true);
        }

        private Vector3 GetRngPosition()
        {
            float deltaX = Random.Range(-spawnRadius, spawnRadius);
            float deltaZ = Random.Range(-spawnRadius, spawnRadius);
            
            Vector3 position = transform.position + new Vector3(deltaX, 0, deltaZ);
            return position;
        }
        private Quaternion GetRngRotation()
        {
            float yRotation = Random.Range(-180f, 180f);
            return Quaternion.Euler(0, yRotation, 0);
        }
    }
}