using Animation.Ease;
using Animation.Tweener;
using CustomSystems;
using System;
using UnityEngine;

namespace Animation
{
    public abstract class AnimationObject : MonoBehaviour
    {
        [SerializeField] protected float duration;
        [SerializeField] protected EasingType ease;
        [SerializeField] protected float delay;

        public Action OnEnd;
        Tween tween;

        public abstract void Animate();

        public void EndAnimation()
        {
            OnEnd?.Invoke();
            tween.OnComplete -= EndAnimation;
        }

        protected async void AddTween(Tween tween)
        {
            this.tween = tween;
            tween.OnComplete += EndAnimation;
            await DelaySystem.DelayFunction(delegate {
                TweenManager.Instance.AddTween(tween);
            }, delay);
        }
    }
}