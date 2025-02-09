using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CustomSystems
{
    public class CameraMoveFromPointToPoint : MonoBehaviour
    {
        [SerializeField] List<MovePoint> points;
        [SerializeField] float speed;
        [SerializeField] float rotationSpeed;
        [SerializeField] float rotateFromDistance;

        int index = 0;
        MovePoint curMovePoint;
        MovePoint nextPoint;

        private MovePoint GetPoint()
        {
            if (index >= points.Count)
            {
                index = 0;
            }

            return points[index++];
        }

        private void Start()
        {
            curMovePoint = GetPoint();
        }

        private void Update()
        {
            if (curMovePoint == null) return;
            float dist = Vector3.Magnitude(transform.position - curMovePoint.position.position);
            if (dist < 0.1f)
            {
                if (nextPoint == null) { 
                    nextPoint = GetPoint();
                }

                curMovePoint = nextPoint;
            } else if (nextPoint != null && dist < rotateFromDistance)
            {
                if (curMovePoint == nextPoint)
                {
                    nextPoint = GetPoint();
                }

                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(nextPoint.rotation), rotationSpeed * Time.deltaTime);
            }

            Vector3 dir = curMovePoint.position.position - transform.position;
            dir.Normalize();

            transform.position += dir * speed * Time.deltaTime;
        }

    }

    [Serializable]
    public class MovePoint
    {
        public Transform position;
        public Vector3 rotation;
    }
}