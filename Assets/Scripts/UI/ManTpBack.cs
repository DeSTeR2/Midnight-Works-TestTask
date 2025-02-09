using UnityEngine;

namespace UI
{
    public class ManTpBack : MonoBehaviour
    {
        [SerializeField] Transform startPosition;
        [SerializeField] Transform endPosition;

        private void FixedUpdate()
        {
            float dist = Vector3.Magnitude(transform.position - endPosition.position);
            if (dist < 0.1f)
            {
                transform.position = startPosition.position;
            }
        }
    }
}