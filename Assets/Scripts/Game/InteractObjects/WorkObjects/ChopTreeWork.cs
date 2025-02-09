﻿using CustomSystems;
using Data;
using Resources;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace InteractObjects.Work
{
    public class ChopTreeWork : WorkObject, IWorkPlace
    {
        [SerializeField] float spawnRadius;
        [SerializeField] protected ResourceType resourceType;
        [SerializeField] int maxObjectsNearTree = 15;

        public override void Work(bool isWork, Character.Character character)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, spawnRadius);

            if (colliders.Length <= maxObjectsNearTree)
            {
                base.Work(isWork, character);
                isWorking = true;
            } else
            {
                isWorking = false;
            }
        }

        public override void AfterWork()
        {
            base.AfterWork();
            Vector3 position = GetRngPosition();
            Quaternion rotation = GetRngRotation();
            InteractObject obj = ResourceSystem.instance.RequestSpawnResource(resourceType, position, rotation, true);
            obj.isInFloor = true;
        }

        private Vector3 GetRngPosition()
        {
            float deltaX = Random.Range(-spawnRadius, spawnRadius);
            float deltaZ = Random.Range(-spawnRadius, spawnRadius);

            float baseOffset = 0.5f;

            if (Mathf.Abs(deltaX) < baseOffset)
            {
                if (deltaX < 0)
                {
                    deltaX = -baseOffset;
                }
                else
                    deltaX = baseOffset;
            }
            if (Mathf.Abs(deltaZ) < baseOffset)
            {
                if (deltaZ < 0)
                {
                    deltaZ = -baseOffset;
                }
                else
                    deltaZ = baseOffset;
            }

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