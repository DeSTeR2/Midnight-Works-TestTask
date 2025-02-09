using Animation.Tweener;
using UnityEngine;

namespace Animation
{
    public class ScaleAnimation : AnimationObject
    {
        [SerializeField] Vector3 start;
        [SerializeField] Vector3 end;

        public override void Animate()
        {
            transform.localScale = start;

            Vector3Tween tween = new(start, end, duration, OnUpgdate, ease);
            AddTween(tween);
        }

        private void OnUpgdate(Vector3 scale)
        {
            transform.localScale = scale;
        }
    }
}