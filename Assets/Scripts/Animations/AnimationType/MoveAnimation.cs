using Animation.Tweener;
using CustomSystems;
using UnityEngine;

namespace Animation
{
    public class MoveAnimation : AnimationObject
    {
        [SerializeField] Transform startPosition;
        [SerializeField] Transform endPosition;

        public override async void Animate()
        {
            transform.position = startPosition.position;

            Vector3Tween tween = new(startPosition.position, endPosition.position, duration, OnUpgdate, ease);
            AddTween(tween);
        }

        private void OnUpgdate(Vector3 position)
        {
            transform.position = position;
        }
    }
}