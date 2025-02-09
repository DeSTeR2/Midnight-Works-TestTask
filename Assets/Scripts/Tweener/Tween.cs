using UnityEngine;
using System;
using UnityEngine.UIElements.Experimental;
using Animation.Ease;

namespace Animation.Tweener
{

    public abstract class Tween
    {
        public float Duration { get; protected set; }
        public float Elapsed { get; protected set; }
        public bool IsCompleted { get; protected set; }
        public Action OnComplete;

        protected Func<float, float> easingFunction;

        public Tween(float duration, EasingType ease = EasingType.Linear)
        {
            Duration = duration;
            Elapsed = 0f;
            IsCompleted = false;
            this.easingFunction = EasingConverter.GetEasingFunction(ease);
        }

        public void Update(float deltaTime)
        {
            if (IsCompleted)
                return;

            Elapsed += deltaTime;
            float time = Mathf.Clamp01(Elapsed / Duration);
            time = easingFunction(time);

            ApplyTween(time);

            if (Elapsed >= Duration)
            {
                Complete();
            }
        }

        protected void Complete()
        {
            IsCompleted = true;
            OnComplete?.Invoke();
        }

        protected abstract void ApplyTween(float t);
    }
}