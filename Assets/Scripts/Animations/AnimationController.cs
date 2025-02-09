using CustomSystems;
using System.Collections.Generic;
using UnityEngine;

namespace Animation
{
    public class AnimationController : MonoBehaviour
    {
        [SerializeField] List<AnimationObject> animations;
        [SerializeField] bool playOnAwake = false;

        public delegate void Callback();
        Callback callback;

        int animationEndNumber = 0;
        bool subscribed = false;

        private void Start()
        {
            if (playOnAwake)
            {
                Animate();
            }
        }

        public AnimationController Animate()
        {
            animationEndNumber = 0;
            for (int i = 0; i < animations.Count; i++)
            {
                animations[i].Animate();

                if (!subscribed) animations[i].OnEnd += EndAnimation;
            }
            subscribed = true;
            return this;
        }

        private void EndAnimation()
        {
            animationEndNumber++;

            if (animationEndNumber == animations.Count)
            {
                callback?.Invoke();
            }
        }

        public AnimationController SetCallback(Callback callback) { 
            this.callback = callback;
            return this;
        }
    }
}