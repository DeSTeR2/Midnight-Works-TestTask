using Animation.Ease;
using System;
using UnityEngine;

namespace Animation.Tweener
{
    public class Vector3Tween : Tween
    {
        private Vector3 startValue;
        private Vector3 endValue;
        private Action<Vector3> onUpdate;

        public Vector3Tween(Vector3 start, Vector3 end, float duration, Action<Vector3> onUpdate, EasingType easingType = EasingType.Linear)
            : base(duration, easingType)
        {
            startValue = start;
            endValue = end;
            this.onUpdate = onUpdate;
        }

        protected override void ApplyTween(float t)
        {
            Vector3 currentValue = Vector3.Lerp(startValue, endValue, t);
            onUpdate?.Invoke(currentValue);
        }
    }
}