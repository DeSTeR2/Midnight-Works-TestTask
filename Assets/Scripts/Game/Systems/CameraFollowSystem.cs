using UnityEngine;

namespace CustomSystems
{
    public class CameraFollowSystem : MonoBehaviour
    {
        [SerializeField] Transform followObject;

        [Header("Movement Settings")]
        [SerializeField] float startMoveFromDistance = 5f;
        [SerializeField] float moveSpeed = 5f;
        [SerializeField] Vector3 offset = Vector3.zero;

        bool isMoving = false;
        float offsetSqrMagnitude;

        private void Awake()
        {
            offsetSqrMagnitude = offset.sqrMagnitude;
        }

        private void Update()
        {
            offsetSqrMagnitude = offset.sqrMagnitude;
            CheckIfCanMove();
        }

        private void LateUpdate()
        {
            if (!isMoving) return;

            Vector3 targetPosition = followObject.position + offset;
            transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }

        private void CheckIfCanMove()
        {
            float sqrDistance = (transform.position - followObject.position).sqrMagnitude - offsetSqrMagnitude;
            float thresholdSqr = startMoveFromDistance * startMoveFromDistance;

            if (sqrDistance >= thresholdSqr)
            {
                isMoving = true;
            }
            else if (sqrDistance < 0.04f && sqrDistance >= 0)
            {
                isMoving = false;
            }
            else if (sqrDistance < 0)
            {
                isMoving = true;
            }
        }
    }
}
